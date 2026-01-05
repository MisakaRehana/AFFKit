using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

public static class ArcAlgorithm
{
	// Straight line
	public static float LineStraight(float start, float end, float t)
	{
		return (1 - t) * start + end * t;
	}
	
	// Bezier curve
	public static float LineBezier(float start, float end, float t)
	{
		float o = 1 - t;
		return MathF.Pow(o, 3) * start + 3 * MathF.Pow(o, 2) * t * start + 3 * o * MathF.Pow(t, 2) * end + MathF.Pow(t, 3) * end;
	}

	// Sine In (lit. sine out, but Sine In in Arc's design)
	public static float LineSineIn(float start, float end, float t)
	{
		return start + (end - start) * MathF.Sin(1.5707963f * t);
	}

	// Sine Out (lit. sine in, but Sine Out in Arc's design)
	public static float LineSineOut(float start, float end, float t)
	{
		return start + (end - start) * (1 - MathF.Cos(1.5707963f * t));
	}

	public static float ArcX(float start, float end, float t, ArcLineType lineType)
	{
		return lineType switch
		{
			ArcLineType.Straight => LineStraight(start, end, t),
			ArcLineType.Bezier => LineBezier(start, end, t),
			ArcLineType.Si or ArcLineType.SiSi or ArcLineType.SiSo => LineSineIn(start, end, t),
			ArcLineType.So or ArcLineType.SoSi or ArcLineType.SoSo => LineSineOut(start, end, t),
			_ => LineStraight(start, end, t)
		};
	}
	
	public static float ArcY(float start, float end, float t, ArcLineType lineType)
	{
		return lineType switch
		{
			ArcLineType.Straight or ArcLineType.Si or ArcLineType.So => LineStraight(start, end, t),
			ArcLineType.Bezier => LineBezier(start, end, t),
			ArcLineType.SiSi or ArcLineType.SoSi => LineSineIn(start, end, t),
			ArcLineType.SiSo or ArcLineType.SoSo => LineSineOut(start, end, t),
			_ => LineStraight(start, end, t)
		};
	}
}