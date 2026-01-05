using System.Diagnostics;
using System.Numerics;
using System.Text;
using AFFKit.Core.Enumerators;
using AFFKit.Core.Utility;
using AFFKit.Core.Utility.Numerics;
using AFFKit.Core.Utility.Text;

namespace AFFKit.Core.Data.Chart;

public sealed class ArcChartReader : IDisposable, IAsyncDisposable
{
	private readonly Stream _cs;
	private ArcChart? _chart;
	private bool isDisposed = false;
	
	public ArcChartReader(Stream chartStream)
	{
		_cs = chartStream;
		_chart = null;
	}
	
	public ArcChartReader(ReadOnlySpan<byte> chartData)
	{
		_cs = new MemoryStream(chartData.ToArray());
		_chart = null;
	}
	
	public ArcChartReader(ReadOnlyMemory<byte> chartData)
	{
		_cs = new MemoryStream(chartData.ToArray());
		_chart = null;
	}
	
	public void Dispose()
	{
		if (isDisposed) return;
		_cs.Dispose();
		isDisposed = true;
	}
	
	public async ValueTask DisposeAsync()
	{
		if (isDisposed) return;
		await _cs.DisposeAsync();
		isDisposed = true;
	}
	
	public void ParseChart(AFFEventSortType sort = AFFEventSortType.ByTiming)
	{
		_chart = ParseChartInternal(sort);
	}

	public ArcChart ToChart()
	{
		if (_chart == null)
		{
			throw new InvalidOperationException("Chart has not been parsed yet. Call ParseChart() before accessing the chart data.");
		}
		return _chart;
	}

	private ArcChart ParseChartInternal(AFFEventSortType sort)
	{
		var chart = new ArcChart();
		chart.Groups.Add(new TimingGroup { Index = 0 });
		using var sr = new StreamReader(_cs, Encoding.UTF8, false, 1024, true);
		int lineCount = 1;
		ParseAudioOffset(chart, sr, ref lineCount);
		ParseTimingPointDensityFactor(chart, sr, ref lineCount);
		lineCount++;
		string line = sr.ReadLine() ?? string.Empty;
		int groupIndex = 0;
		int groupMax = 0;
		{
			int firstTiming;
			if (line.StartsWith("timing(", StringComparison.OrdinalIgnoreCase))
			{
				firstTiming = ParseTiming(chart, line, 0, ref lineCount);
			}
			else
			{
				throw new ArcChartException("The chart must start with a Timing event.", lineCount);
			}

			if (firstTiming != 0)
			{
				throw new ArcChartException("Invalid initial Timing event. The first Timimng event must start at timing 0.", lineCount);
			}
		}
		while (!sr.EndOfStream)
		{
			line = sr.ReadLine()?.Replace(" ", string.Empty).Trim() ?? string.Empty;
			if (line.StartsWith("timing(", StringComparison.OrdinalIgnoreCase))
			{
				ParseTiming(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith('('))
			{
				ParseTap(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith("hold(", StringComparison.OrdinalIgnoreCase))
			{
				ParseHold(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith("arc(", StringComparison.OrdinalIgnoreCase))
			{
				ParseArc(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith("camera(", StringComparison.OrdinalIgnoreCase))
			{
				ParseCamera(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith("scenecontrol(", StringComparison.OrdinalIgnoreCase))
			{
				ParseSceneControl(chart, line, groupIndex, ref lineCount);
			}
			else if (line.StartsWith("timinggroup(", StringComparison.OrdinalIgnoreCase))
			{
				groupMax++;
				groupIndex = groupMax;
				ParseTimingGroup(chart, line, ref lineCount);
			}
			else if (line == "};")
			{
				groupIndex = 0; // Return to default group
			}
#if DEBUG
			else if (line.Length >= 1 && Rune.GetRuneAt(line, 0).Value != '-')
			{
				Debug.WriteLine($"ArcChartReader warning: Unrecognized line at {lineCount - 1}: {line}");
			}
#endif
			chart.GroupSize = groupMax + 1;
			foreach (var group in chart.Groups)
			{
				group.Sort(sort);
			}
		}
		
		return chart;
	}

	private static void ParseAudioOffset(ArcChart chart, StreamReader sr, ref int lineCount)
	{
		string line = sr.ReadLine() ?? string.Empty;
		if (!line.StartsWith("AudioOffset:"))
		{
			throw new ArcChartException("Parsing error in AudioOffset: The first line doesn't start with \"AudioOffset:\".", 1);
		}
		
		bool ok = int.TryParse(line["AudioOffset:".Length..].Trim(), out int offset);
		if (ok)
		{
			chart.AudioOffset = offset;
			lineCount += 1;
		}
		else
		{
			throw new ArcChartException("Parsing error in AudioOffset: Invalid integer format.", lineCount);
		}
	}

	private static void ParseTimingPointDensityFactor(ArcChart chart, StreamReader sr, ref int lineCount)
	{
		string line = sr.ReadLine() ?? string.Empty;
		if (line.StartsWith("TimingPointDensityFactor:"))
		{
			bool ok = float.TryParse(line["TimingPointDensityFactor:".Length..].Trim(), out float factor);
			if (ok)
			{
				chart.TimingPointDensityFactor = factor;
				lineCount += 1;
			}
			else
			{
				throw new ArcChartException("Parsing error in TimingPointDensityFactor: Invalid float format.", lineCount);
			}
		}
	}

	private static int ParseTiming(ArcChart chart, string line, int group, ref int lineCount)
	{
		// timing(0,120.00,4.00);
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(7); // skip "timing("
			int timing = parser.ReadInt();
			float bpm = parser.ReadFloat();
			float beatsPerLine = parser.ReadFloat();
			if (beatsPerLine == 0)
			{
				if (bpm == 0)
				{
					beatsPerLine = 4;
				}
				else
				{
					throw new ArcChartException("Parsing error in Timing: BeatsPerLine cannot be zero when BPM is not zero.", lineCount);
				}
			}
			else if (beatsPerLine < 0)
			{
				bpm = -bpm;
				beatsPerLine = -beatsPerLine;
			}

			chart.Groups[group].Events.Add(new Timing()
			{
				Timing = timing,
				BPM = bpm,
				BeatsPerLine = beatsPerLine
			});
			lineCount += 1;
			return timing;
		}
		catch (ArcChartException ex)
		{
			throw;
		}
		catch (Exception)
		{
			throw new ArcChartException("Parsing error in Timing: Invalid event format.", lineCount);
		}
	}

	private static int ParseTap(ArcChart chart, string line, int group, ref int lineCount)
	{
		// (1500,2);    -- regular tap
		// (1500,0.75); -- float track tap
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(1); // skip "("
			int timing = parser.ReadInt();
			if (parser.TryReadInt(out int track))
			{
				if (track is < 0 or > 5)
				{
					throw new ArcChartException("Parsing error in Tap: Track index out of range (must be between 0 and 5).", lineCount);
				}
				chart.Groups[group].Events.Add(new Tap()
				{
					Timing = timing,
					Track = track
				});
				lineCount += 1;
			}
			else
			{
				float trackF = parser.ReadFloat();
				if (float.IsNaN(trackF) || float.IsInfinity(trackF))
				{
					throw new ArcChartException("Parsing error in Tap: Invalid float format for Track.", lineCount);
				}
				chart.Groups[group].Events.Add(new Tap()
				{
					Timing = timing,
					TrackF = trackF
				});
				lineCount += 1;
			}
			return timing;
		}
		catch (ArcChartException ex)
		{
			throw;
		}
		catch (Exception)
		{
			throw new ArcChartException("Parsing error in Tap: Invalid event format.", lineCount);
		}
	}

	private static int ParseHold(ArcChart chart, string line, int group, ref int lineCount)
	{
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(5); // skip "hold("
			int timing = parser.ReadInt();
			int endTiming = parser.ReadInt();
			if (parser.TryReadInt(out int track))
			{
				if (track is < 0 or > 5)
				{
					throw new ArcChartException("Parsing error in Hold: Track index out of range (must be between 0 and 5).", lineCount);
				}
				chart.Groups[group].Events.Add(new Hold()
				{
					Timing = timing,
					EndTiming = endTiming,
					Track = track
				});
				lineCount += 1;
			}
			else
			{
				float trackF = parser.ReadFloat();
				if (float.IsNaN(trackF) || float.IsInfinity(trackF))
				{
					throw new ArcChartException("Parsing error in Hold: Invalid float format for Track.", lineCount);
				}
				chart.Groups[group].Events.Add(new Hold()
				{
					Timing = timing,
					EndTiming = endTiming,
					TrackF = trackF
				});
				lineCount += 1;
			}
			return timing;
		}
		catch (ArcChartException)
		{
			throw;
		}
		catch
		{
			throw new ArcChartException("Parsing error in Hold: Invalid event format.", lineCount);
		}
	}
	
	private static int ParseArc(ArcChart chart, string line, int group, ref int lineCount)
	{
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(4); // skip "arc("
			int timing = parser.ReadInt();
			int endTiming = parser.ReadInt();
			float xStart = parser.ReadFloat();
			float xEnd = parser.ReadFloat();
			var lineType = EnumExtension.ParseByDescription<ArcLineType>(parser.ReadString(), strict: true);
			float yStart = parser.ReadFloat();
			float yEnd = parser.ReadFloat();
			var colorType = (ArcColorType)parser.ReadInt();
			string sfx = parser.ReadString();
			var baseType = EnumExtension.ParseByDescription<ArcType>(parser.ReadString(out bool hasSmoothness), strict: true);
			var arctaps = new List<ArcTap>();
			float smoothness = 1f;
			if (hasSmoothness)
			{
				smoothness = parser.ReadFloat();
			}
			if (parser.Current.Value != ';')
			{
				int duration = endTiming - timing;
				do
				{
					parser.SkipRunes(8); // skip "[arctap(" or ",arctap("
					int arcTapTiming = parser.ReadInt();
					float r = duration > 0 ? MisakaCastleMath.InverseLerpClamped(timing, endTiming, arcTapTiming) : 0f;
					arctaps.Add(new ArcTap()
					{
						Timing = arcTapTiming,
						ActualPosition = new Vector2(
							ArcAlgorithm.ArcX(xStart, xEnd, r, lineType),
							ArcAlgorithm.ArcY(yStart, yEnd, r, lineType))
					});
				}
				while (parser.Current.Value == ',');
			}

			if (arctaps.Count > 0 && baseType == ArcType.Solid)
			{
				Debug.WriteLine($"ArcChartReader warning: Solid Arc at line {lineCount} contains ArcTaps, which will be forcibly treated as Trace Arc.");
				baseType = ArcType.TraceVoid;
			}

			var arc = new Arc()
			{
				Timing = timing, EndTiming = endTiming,
				StartPosition = new Vector2(xStart, yStart),
				EndPosition = new Vector2(xEnd, yEnd),
				Type = baseType,
				LineType = lineType,
				ColorType = colorType,
				Sfx = !string.IsNullOrWhiteSpace(sfx) ? sfx : "none",
				Smoothness = smoothness,
				ArcTaps = arctaps
			};

			if (baseType >= ArcType.TraceVoid || colorType is >= ArcColorType.Blue and <= ArcColorType.Gray)
			{
				chart.Groups[group].Events.Add(arc);
			}
			else
			{
				throw new ArcChartException("Parsing error in Arc: Invalid color for a solid arc. Accepted colors are Blue(0), Red(1), Green(2), Gray(3) or the arc must be trace(void).", lineCount);
			}
			lineCount += 1;
			return timing;
		}
		catch (ArcChartException)
		{
			throw;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			throw new ArcChartException("Parsing error in Arc: Invalid event format.", lineCount);
		}
	}
	
	private static void ParseCamera(ArcChart chart, string line, int group, ref int lineCount)
	{
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(7); // skip "camera("
			int timing = parser.ReadInt();
			var move = new Vector3(parser.ReadFloat(), parser.ReadFloat(), parser.ReadFloat());
			var rotate = MisakaCastleMath.Euler(parser.ReadFloat(), parser.ReadFloat(), parser.ReadFloat());
			var type = EnumExtension.ParseByDescription<CameraType>(parser.ReadString(), strict: true);
			int duration = parser.ReadInt();
			if (duration < 0)
			{
				throw new ArcChartException("Parsing error in Camera: Duration cannot be negative.", lineCount);
			}
			
			chart.Groups[group].Events.Add(new Camera()
			{
				Timing = timing,
				Type = type,
				Duration = duration,
				DeltaMove = move,
				DeltaRotate = rotate
			});
			lineCount += 1;
		}
		catch (ArcChartException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new ArcChartException("Parsing error in Camera: Invalid event format.", lineCount);
		}
	}
	
	private static void ParseSceneControl(ArcChart chart, string line, int group, ref int lineCount)
	{
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(13); // skip "scenecontrol("
			int timing = parser.ReadInt();
			var scType = EnumExtension.ParseByDescription<SceneControlType>(parser.ReadString(out bool hasArgs), strict: true);
			float paramFloat = hasArgs ? parser.ReadFloat() : 0f;
			int paramInt = hasArgs ? parser.ReadInt() : 0;
			chart.Groups[group].Events.Add(new SceneControl()
			{
				Timing = timing,
				Type = scType,
				ParamFloat = paramFloat,
				ParamInt = paramInt
			});
			lineCount += 1;
		}
		catch (ArcChartException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new ArcChartException("Parsing error in SceneControl: Invalid event format.", lineCount);
		}
	}
	
	private static void ParseTimingGroup(ArcChart chart, string line, ref int lineCount)
	{
		try
		{
			var parser = new StackStringParser(line);
			parser.SkipRunes(12);
			string arg = parser.ReadString(); // may be empty for no-argument timing groups
			chart.Groups.Add(new TimingGroup()
			{
				Index = chart.Groups.Count,
				Params = arg.Split('_', StringSplitOptions.RemoveEmptyEntries).ToList()
			});
		}
		catch (Exception)
		{
			throw new ArcChartException("Parsing error in TimingGroup: Invalid format.", lineCount);
		}
	}
}