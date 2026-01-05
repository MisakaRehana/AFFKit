using System.Numerics;
using AFFKit.Core.Enumerators;

namespace AFFKit.Core.Data.Chart;

[AFFEvent(AFFEventType.Note)]
public sealed class ArcTap : AFFNote
{
	public Vector2 ActualPosition { get; set; } = Vector2.Zero;
}