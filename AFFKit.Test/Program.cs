using AFFKit.Core.Data.Chart;
using AFFKit.Core.Enumerators;
using AFFKit.Core.Utility;
using System.Diagnostics;

// fs and reader can be disposed safely since here

namespace AFFKit.Test
{
    class Program
    {
        private static void a()
        {
            /*
            using var fs = new FileStream(@"path\to\your\chart.aff", FileMode.Open, FileAccess.Read);
            using var reader = new ArcChartReader(fs);
            reader.ParseChart(AFFEventSortType.ByType);
            var chart = reader.ToChart();
            Console.WriteLine($"AudioOffset: {chart.AudioOffset}");
            Console.WriteLine($"Timing Point Density Factor (TPDF): {chart.TimingPointDensityFactor}");
            Console.WriteLine($"Number of Timing Groups (including default group): {chart.Groups.Count}");
            Console.WriteLine($"Number of Notes (each long note counts as one): {chart.Groups.Sum(g => g.Events.Count(e => e is AFFNote))}");
            Console.WriteLine($"Number of Judgable Notes (each long note counts as one): {chart.Groups.Where(g => !g.Params.Contains("noinput")).Sum(g => g.Events.Count(e => e is AFFNote))}");
            */
        }
        private static void SOATester()
        {
            StructOfArray<int> test = new(3, 50);
            Console.WriteLine(test.ToString());
            int[] test_a = { 1, 2, 3 };
            int[] test_b = { 4, 5, 6 };
            int[] test_c = { 7, 8, 9 };
            // 假设 number_of_array = 3
            // 插入三次
            test.add(id: 10, value: test_a);
            test.add(id: 15, value: test_b);
            test.add(id: 2, value: test_c);
            Console.WriteLine(test.ToString());

            Console.WriteLine(test.remove(15));
            Console.WriteLine(test.add(id: 7, value: test_b));
            Console.WriteLine(test.add(id: 11, value: [100, 100, 101]));
            Console.WriteLine(test.modify(id: 2, value: []));
            Console.WriteLine(test.modify(id: 11, value: [-200, -300, -500]));
            
            Console.WriteLine(test.ToString());

            int[] query_result = test.query(7);
            for (int i = 0; i < query_result.Length; i++)
            {
                Console.Write(query_result[i] + " ");
            }
            Console.WriteLine();
        }

        private static void BezierCurveTester()
        {
            
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello AFFKit Test Program .");
            SOATester();
        }
    }
}

