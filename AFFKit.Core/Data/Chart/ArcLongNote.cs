using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Note)]
public abstract class ArcLongNote : AFFNote
{
	public int EndTiming { get; set; }
}