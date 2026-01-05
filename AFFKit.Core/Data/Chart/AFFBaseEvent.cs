namespace AFFKit.Core.Data.Chart;

public abstract class AFFBaseEvent : IComparable<AFFBaseEvent>
{
	public int Timing { get; set; }
	
	public virtual int CompareTo(AFFBaseEvent? other)
	{
		if (other == null) return 1;
		return Timing.CompareTo(other.Timing);
	}
}