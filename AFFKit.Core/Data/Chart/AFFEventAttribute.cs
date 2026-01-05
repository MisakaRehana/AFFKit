using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AFFEventAttribute : Attribute
{
	public AFFEventType EventType { get; }

	/// <summary>
	/// Whether the event can be nested inside a custom <see cref="TimingGroup"/> event.<br />
	/// This check will be ignored for events inside the default timing group.
	/// </summary>
	public bool IsNestable { get; }

	public AFFEventAttribute(AFFEventType eventType, bool nestable = true)
	{
		EventType = eventType;
		IsNestable = nestable;
	}
}