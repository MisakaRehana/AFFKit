using System.ComponentModel;
using AFFKit.Core.Data.Chart;

namespace AFFKit.Core.Enumerators;

/// <summary>
/// Represents the type for <see cref="Camera"/> events.
/// </summary>
public enum CameraType
{
	/// <summary>
	/// The camera event resets all previous transformations and set back to default state.
	/// </summary>
	[Description("reset")] Reset = -1,
	
	/// <summary>
	/// The camera event applies translation with linear (<c>l</c>) easing.<br />
	/// Camera events with unrecognized types will also be treated as this type.
	/// </summary>
	[Description("l")] Linear = 0,
	
	/// <summary>
	/// The camera event applies translation with sine in-out (<c>s</c>) easing.
	/// </summary>
	[Description("s")] SineInOut = 1,
	
	/// <summary>
	/// The camera event applies translation with cubic in (<c>qi</c>) easing.
	/// </summary>
	[Description("qi")] CubicIn = 2,
	
	/// <summary>
	/// The camera event applies translation with cubic out (<c>qo</c>) easing.
	/// </summary>
	[Description("qo")] CubicOut = 3
}