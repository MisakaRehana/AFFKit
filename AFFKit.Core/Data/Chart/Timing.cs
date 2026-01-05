using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Timing)]
public sealed class Timing : AFFBaseEvent
{
	public float BPM { get; set; }
	public float BeatsPerLine { get; set; } = 4f;
}