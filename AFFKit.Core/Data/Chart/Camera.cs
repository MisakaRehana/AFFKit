using System.Numerics;
using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Camera)]
public sealed class Camera : AFFBaseEvent
{
	public Vector3 DeltaMove { get; set; }
	public Quaternion DeltaRotate { get; set; }
	public CameraType Type { get; set; } = CameraType.Linear;
	public int Duration { get; set; } // in milliseconds
}