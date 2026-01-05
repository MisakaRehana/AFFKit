using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.SceneControl)]
public sealed class SceneControl : AFFBaseEvent
{
	public SceneControlType Type { get; set; }
	public float ParamFloat { get; set; } = 0f; // usually used for intensity or duration
	public int ParamInt { get; set; } = 0; // usually used for switch flags (0 == off, 1 == on)
}