using System.Numerics;
using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Note)]
public sealed class Arc : ArcLongNote
{
	public ArcType Type { get; set; } = ArcType.Solid;
	public ArcLineType LineType { get; set; } = ArcLineType.Straight;
	public ArcColorType ColorType { get; set; } = ArcColorType.Blue;
	public Vector2 StartPosition { get; set; } = Vector2.Zero;
	public Vector2 EndPosition { get; set; } = Vector2.Zero;
	public float Smoothness { get; set; } = 1f; // 1.0 (default); a.k.a. "RenderSplitCount" in some tools
	public string Sfx { get; set; } = "none"; // if not none (filename_wav), play the specified SFX (filename.wav) when hit all the ArcTaps on this Arc (only for Trace Arcs)
	public List<ArcTap> ArcTaps { get; set; } = new();

	public override int CompareTo(AFFBaseEvent? other)
	{
		int @base = base.CompareTo(other);
		if (@base != 0) return @base;
		if (other is not Arc otherArc) return 1;
		return EndTiming.CompareTo(otherArc.EndTiming);
	}
}