namespace AFFKit.Core.Data.Chart;

public sealed class ArcChart
{
	/// <summary>
	/// The audio offset of the chart in milliseconds.
	/// </summary>
	public int AudioOffset { get; set; } = 0;
	
	/// <summary>
	/// The global timing point density factor (TPDF) of the chart.<br />
	/// This factor affects the timing points density of long notes (<see cref="Hold"/>s and <see cref="Arc"/>s) in the chart.<br />
	/// A higher value results in more timing points and with more combos (note counts) being generated for long notes.<br />
	/// Default is 1.0 (normal density).<br />
	/// The value of density factor must be greater than 0.0.
	/// </summary>
	public float TimingPointDensityFactor { get; set; } = 1f;
	
	/// <summary>
	/// The list of timing groups in the chart.<br />
	/// Every chart has at least one default timing group with index 0, which contains all events that are not nested inside any custom timing groups.
	/// </summary>
	public List<TimingGroup> Groups { get; set; } = new();

	public int GroupSize { get; set; } = 1;
}