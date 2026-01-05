using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Note)]
public sealed class Hold : ArcLongNote
{
	public int Track { get; set; }
	public float? TrackF { get; set; } = null;

	public override int CompareTo(AFFBaseEvent? other)
	{
		int @base = base.CompareTo(other);
		if (@base != 0) return @base;
		if (other is not Hold otherHold) return 1;
		@base = EndTiming.CompareTo(otherHold.EndTiming);
		if (@base != 0) return @base;
		if (TrackF.HasValue && otherHold.TrackF.HasValue)
		{
			@base = TrackF.Value.CompareTo(otherHold.TrackF.Value);
		}
		else
		{
			@base = Track.CompareTo(otherHold.Track);
		}
		return @base;
	}
}