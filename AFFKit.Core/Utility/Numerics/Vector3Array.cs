using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AFFKit.Core.Utility.Numerics
{
    /// <summary>
    /// Designed for prefetching efficiency
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct Vector3Array<T>
    {
        private int capacity = 1;
        public int size = 1;
        public T[] X = new T[1];
        public T[] Y = new T[1];
        public T[] Z = new T[1];
        public Vector3Array()
        {
        }
        public void Set(int index, T x, T y, T z)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException("Index out of range in Vector3Array Set.");
            }
            X[index] = x;
            Y[index] = y;
            Z[index] = z;
        }
        public void Resize(int new_size)
        {
            // error
            if (new_size < 0)
            {
                throw new ArgumentOutOfRangeException("new_size", "new_size must be non-negative in Vector3Array Resize.");
            }
            // init
            if (capacity == 0)
            {
                capacity = new_size;
                size = new_size;
                X = new T[new_size];
                Y = new T[new_size];
                Z = new T[new_size];
                return;
            }
            // expand
            if (new_size > capacity)
            {
                size = new_size;
                capacity = new_size;
                T[] new_x = new T[new_size];
                T[] new_y = new T[new_size];
                T[] new_z = new T[new_size];
                Array.Copy(X, new_x, X.Length);
                Array.Copy(Y, new_y, Y.Length);
                Array.Copy(Z, new_z, Z.Length);
                X = new_x;
                Y = new_y;
                Z = new_z;
                return;
            }
            // shrink
            if (new_size <= capacity)
            {
                size = new_size;
                T[] new_x = new T[new_size];
                T[] new_y = new T[new_size];
                T[] new_z = new T[new_size];
                Array.Copy(X, new_x, new_size);
                Array.Copy(Y, new_y, new_size);
                Array.Copy(Z, new_z, new_size);
                X = new_x;
                Y = new_y;
                Z = new_z;
                return;
            }
        }
        public void Swap(int index_a, int index_b)
        {
            if (index_a < 0 || index_a >= size || index_b < 0 || index_b >= size)
            {
                throw new IndexOutOfRangeException("Index out of range in Vector3Array Swap.");
            }
            if (index_a == index_b)
            {
                return;
            }
            T temp_x = X[index_a];
            T temp_y = Y[index_a];
            T temp_z = Z[index_a];
            X[index_a] = X[index_b];
            Y[index_a] = Y[index_b];
            Z[index_a] = Z[index_b];
            X[index_b] = temp_x;
            Y[index_b] = temp_y;
            Z[index_b] = temp_z;
        }
    }
}
