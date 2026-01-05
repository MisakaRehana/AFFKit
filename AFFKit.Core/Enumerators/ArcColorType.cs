using AFFKit.Core.Data.Chart;

namespace AFFKit.Core.Enumerators;

public enum ArcColorType
{
	/// <summary>
	/// Blue color.
	/// </summary>
	Blue = 0,
	
	/// <summary>
	/// Red color.
	/// </summary>
	Red = 1,
	
	/// <summary>
	/// Green color.<br />
	/// The game will render it as <see cref="Red"/> unless <c>allowsMemes</c> flag is enabled or in the Special Scene of <c>Your Best Nightmare</c> (<c>SpecialSceneYourBestNightmareChallenge</c>).
	/// </summary>
	Green = 2,
	
	/// <summary>
	/// Gray (or white) color.<br />
	/// The game will render it as variant-length <see cref="ArcTap" /> instead of a normal arc if its duration is zero and <c>allowsMemes</c> flag is enabled.
	/// </summary>
	Gray = 3
}