namespace AFFKit.Core.Data.Chart;

public class ArcChartException : Exception
{
	public ArcChartException(string message, int lineNumber)
		: base($"Line {lineNumber}: {message}")
	{
	}
}