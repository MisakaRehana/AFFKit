using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Note)]
public sealed class Tap : AFFNote
{
	public int Track { get; set; }
	public float? TrackF { get; set; } = null;

	public override int CompareTo(AFFBaseEvent? other)
	{
		int @base = base.CompareTo(other);
		if (@base != 0) return @base;
		if (other is not Tap otherTap) return 1;
		if (TrackF.HasValue && otherTap.TrackF.HasValue)
		{
			@base = TrackF.Value.CompareTo(otherTap.TrackF.Value);
		}
		else
		{
			@base = Track.CompareTo(otherTap.Track);
		}
		return @base;
	}
}