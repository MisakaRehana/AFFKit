namespace AFFKit.Core.Enumerators;

/// <summary>
/// Represents the type of AFF event.
/// </summary>
public enum AFFEventType
{
	/// <summary>
	/// The event is a timing event.
	/// </summary>
	Timing = 0,
	
	/// <summary>
	/// The event is a note event.
	/// </summary>
	Note = 1,
	
	/// <summary>
	/// The event is a camera event.
	/// </summary>
	Camera = 2,
	
	/// <summary>
	/// The event is a scene control event.
	/// </summary>
	SceneControl = 3,
	
	/// <summary>
	/// The event is a timing group event, which contains all other events within a specific timing group.
	/// </summary>
	TimingGroup = 4
}