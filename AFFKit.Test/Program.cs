using AFFKit.Core.Data.Chart;
using AFFKit.Core.Enumerators;

using var fs = new FileStream(@"path\to\your\chart.aff", FileMode.Open, FileAccess.Read);
using var reader = new ArcChartReader(fs);
reader.ParseChart(AFFEventSortType.ByType);
var chart = reader.ToChart();

// fs and reader can be disposed safely since here

Console.WriteLine($"AudioOffset: {chart.AudioOffset}");
Console.WriteLine($"Timing Point Density Factor (TPDF): {chart.TimingPointDensityFactor}");
Console.WriteLine($"Number of Timing Groups (including default group): {chart.Groups.Count}");
Console.WriteLine($"Number of Notes (each long note counts as one): {chart.Groups.Sum(g => g.Events.Count(e => e is AFFNote))}");
Console.WriteLine($"Number of Judgable Notes (each long note counts as one): {chart.Groups.Where(g => !g.Params.Contains("noinput")).Sum(g => g.Events.Count(e => e is AFFNote))}");