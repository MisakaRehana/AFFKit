using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.TimingGroup, nestable: false)]
public sealed class TimingGroup : AFFBaseEvent
{
	private readonly static Dictionary<Type, int> EventTypeOrder = new()
	{
		{ typeof(Timing), 0 },
		{ typeof(Tap), 1 },
		{ typeof(Hold), 2 },
		{ typeof(Arc), 3 },
		{ typeof(Camera), 4 },
		{ typeof(SceneControl), 5 }
	};
	
	public int Index { get; set; }
	public List<string> Params { get; set; } = new();
	public List<AFFBaseEvent> Events { get; set; } = new();
	
	public void Sort(AFFEventSortType sort = AFFEventSortType.ByTiming)
	{
		if (sort == AFFEventSortType.ByType)
		{
			// first type then timing.
			Events = Events
				.OrderBy(e => EventTypeOrder.ContainsKey(e.GetType()) ? EventTypeOrder[e.GetType()] : int.MaxValue)
				.ToList();
		}
		Events.Sort();
	}
}