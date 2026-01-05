namespace AFFKit.Core.Enumerators;

public enum AFFEventSortType
{
	ByTiming = 0, // Events are sorted by their timing only
	ByType = 1 // Timing -> Tap -> Hold -> Arc -> ArcTap -> Camera -> SceneControl
}