using System.ComponentModel;
using AFFKit.Core.Data.Chart;

namespace AFFKit.Core.Enumerators;

/// <summary>
/// Represents the type for <see cref="SceneControl"/> events.
/// </summary>
public enum SceneControlType
{
	/// <summary>
	/// The scene control event modifies track display opacity (alpha).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,trackdisplay,&lt;duration&gt;,&lt;alpha&gt;);</c><br />
	/// Example: <c>scenecontrol(15000,trackdisplay,1.25,255);</c><br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>duration</c> (<see cref="float"/>): The duration of the transition in seconds. If set to 0, the change is instantaneous.</item>
	/// <item><c>alpha</c> (<see cref="int"/>): The target opacity value (0-255), where 0 is fully transparent and 255 is fully opaque.</item>
	/// </list>
	/// </summary>
	[Description("trackdisplay")] TrackDisplay = 0,
	
	/// <summary>
	/// The scene control event hides the track.<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,trackhide);</c><br />
	/// Example: <c>scenecontrol(20000,trackhide);</c><br />
	/// It works the same as <c>scenecontrol(&lt;time&gt;,trackdisplay,0.50,0);</c> with default duration 0.5 seconds.
	/// </summary>
	[Description("trackhide")] TrackHide = 1,
	
	/// <summary>
	/// The scene control event shows the track.<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,trackshow);</c><br />
	/// Example: <c>scenecontrol(25000,trackshow);</c><br />
	/// It works the same as <c>scenecontrol(&lt;time&gt;,trackdisplay,0.50,255);</c> with default duration 0.5 seconds.
	/// </summary>
	[Description("trackshow")] TrackShow = 2,
	
	/// <summary>
	/// Hides this timing group and all notes within it <b>instantly</b>.<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,hidegroup,0.00,&lt;switch&gt;);</c><br />
	/// Examples:<br />
	/// <c>scenecontrol(30000,hidegroup,0.00,1);</c> (to <b>hide</b> the group -- switch <b>ON</b>)<br />
	/// <c>scenecontrol(35000,hidegroup,0.00,0);</c> (to <b>show</b> the group -- switch <b>OFF</b>)<br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>switch</c> (<see cref="int"/>): Use 1 to hide the group (ON the hidegroup switch), or 0 to show the group (OFF the hidegroup switch).</item>
	/// </list>
	/// Note that the switch parameter is grammatically inversed compared to regular human logic -- use 1 to hide (ON) and 0 to show (OFF).
	/// </summary>
	[Description("hidegroup")] HideGroup = 10,
	
	/// <summary>
	/// Enwidens the camera view (from 4-Key suitable to 6-Key suitable).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,enwidencamera,&lt;duration&gt;,&lt;switch&gt;);</c><br />
	/// Examples:<br />
	/// <c>scenecontrol(40000,enwidencamera,325.00,1);</c> (to <b>enwiden</b> the camera -- switch <b>ON</b>)<br />
	/// <c>scenecontrol(45000,enwidencamera,325.00,0);</c> (to <b>reset</b> the camera -- switch <b>OFF</b>)<br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>duration</c> (<see cref="float"/>): The duration of the transition <b>in milliseconds but with decimal point to fit correct format</b>. For example, use 325.00 for 325 milliseconds (0.325s).</item>
	/// <item><c>switch</c> (<see cref="int"/>): Use 1 to enwiden the camera (ON the enwiden switch), or 0 to reset the camera (OFF the enwiden switch).</item>
	/// </list>
	/// </summary>
	[Description("enwidencamera")] EnwidenCamera = 20,
	
	/// <summary>
	/// Enwidens the lanes (Show/hide the extra lanes) (from 4-Key lanes to 6-Key lanes).<br />
	/// Notes on extra 2 lanes will ONLY being judged correctly when the extra lanes are shown (during enwidened state).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,enwidenlanes,&lt;duration&gt;,&lt;switch&gt;);</c><br />
	/// Examples:<br />
	/// <c>scenecontrol(50000,enwidenlanes,325.00,1);</c> (to <b>enwiden</b> the lanes (<b>show</b> extra lanes) -- switch <b>ON</b>)<br />
	/// <c>scenecontrol(55000,enwidenlanes,325.00,0);</c> (to <b>reset</b> the lanes (<b>hide</b> extra lanes) -- switch <b>OFF</b>)<br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>duration</c> (<see cref="float"/>): The duration of the transition <b>in milliseconds but with decimal point to fit correct format</b>. For example, use 325.00 for 325 milliseconds (0.325s).</item>
	/// <item><c>switch</c> (<see cref="int"/>): Use 1 to enwiden the lanes (ON the enwiden switch), or 0 to reset the lanes (OFF the enwiden switch).</item>
	/// </list>
	/// </summary>
	[Description("enwidenlanes")] EnwidenLanes = 21,
	
	/// <summary>
	/// Displays distortion effect for the background (used in <c>Arcahv</c> anomaly challenge).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,arcahvdistort,&lt;duration&gt;,&lt;opacity&gt;);</c><br />
	/// Example: <c>scenecontrol(60000,arcahvdistort,0.50,255);</c><br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>duration</c> (<see cref="float"/>): The duration of the transition in seconds. If set to 0, the change is instantaneous.</item>
	/// <item><c>opacity</c> (<see cref="int"/>): The target opacity value (0-255), where 0 is fully transparent and 255 is fully opaque.</item>
	/// </list>
	/// </summary>
	[Description("arcahvdistort")] ArcahvDistort = 30,
	
	/// <summary>
	/// Displays debris effect for the background (used in <c>Arcahv</c> anomaly challenge).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,arcahvdebris,&lt;duration&gt;,&lt;opacity&gt;);</c><br />
	/// Example: <c>scenecontrol(65000,arcahvdebris,0.50,255);</c><br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>duration</c> (<see cref="float"/>): The duration of the transition in seconds. If set to 0, the change is instantaneous.</item>
	/// <item><c>opacity</c> (<see cref="int"/>): The target opacity value (0-255), where 0 is fully transparent and 255 is fully opaque.</item>
	/// </list>
	/// </summary>
	[Description("arcahvdebris")] ArcahvDebris = 31,
	
	/// <summary>
	/// Displays redline effect for the background (used in <c>Arcahv</c> anomaly challenge and <c>Désive</c> normal play).<br />
	/// Standard event code: <c>scenecontrol(&lt;time&gt;,redline,&lt;existDuration&gt;,0);</c><br />
	/// Example: <c>scenecontrol(70000,redline,10.75,0);</c><br />
	/// <list type="bullet">
	/// <item><c>time</c>: The timing of the event in milliseconds.</item>
	/// <item><c>existDuration</c> (<see cref="float"/>): The duration for which the redline effect exists, in seconds.</item>
	/// <item>The last parameter is unused and should be set to 0.</item>
	/// </list>
	/// </summary>
	[Description("redline")] RedLine = 32
}