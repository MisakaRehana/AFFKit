using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AFFKit.Core.Utility
{
    internal class Chiruno
    {
        private static ulong[] temp_binomial_coefficient = new ulong[1024];
        public static void Init(uint level_of_binomial_coefficient, uint max_factorial)
        {
        }
        public static void swap<T>(T a, T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public static int ArrayTriangleIndex(int level, int offset)
        {
            return (level * (level + 1)) >> 1 + offset;
        }
        public static int Summation(int n)
        {
            return n * (n + 1) >> 1;
        }
        public static ulong Summation(ulong n)
        {
            return n * (n + 1) >> 1;
        }
    }
}
