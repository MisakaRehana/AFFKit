using System.Numerics;

namespace AFFKit.Core.Utility.Numerics
{
    /*
    public class BezierCurve
    {
        private readonly bool[] control_points_visibility;
        private readonly Vector3Array<double> control_points;
        private readonly Vector3Array<double>[] temp_de_Casteljau_triangle;
        // private double[,] weight;
        public int precision = 3;
        public BezierCurve(int control_points_count = 3)
        {
            this.control_points_visibility = new bool[control_points_count];
            this.control_points = new Vector3Array<double>();
            this.temp_de_Casteljau_triangle = new Vector3Array<double>[this.precision];
        }
        ~BezierCurve()
        {
        }
        /// <summary>
        /// Space Manager(WIP)
        /// </summary>
        private void Init()
        {
            for (int i = 0; i < this.precision; i++)
            {
                this.temp_de_Casteljau_triangle[i].Resize(Chiruno.Summation(this.control_points.size));
            }
        }
        private void Lerp_de_Casteljau_X(double t_k, int k)
        {
            for (int i = 0; i < this.control_points.size; i++)
            {
                temp_de_Casteljau_triangle[k].X[i] = control_points.X[i];
            }
            int index_offset_previous = 0;
            int index_offset_next = control_points.size;
            for (int i = 1; i < this.control_points.size; i++)
            {
                for (int j = 0; j < this.control_points.size - i; j++)
                {
                    this.temp_de_Casteljau_triangle[k].X[Chiruno.ArrayTriangleIndex(i, j)] =
                        this.temp_de_Casteljau_triangle[k].X[Chiruno.ArrayTriangleIndex(i - 1, j)] +
                        (this.temp_de_Casteljau_triangle[k].X[Chiruno.ArrayTriangleIndex(i - 1, j + 1)] -
                        this.temp_de_Casteljau_triangle[k].X[Chiruno.ArrayTriangleIndex(i - 1, j)]) * t_k;
                }
                index_offset_previous = index_offset_next;
                index_offset_next += this.control_points.size - i;
            }
        }
        private void CalculateBernsteinCoefficient()
        {
            
        }
        public void SetControlPoint(int index, double x, double y, double z)
        {
            if (index < 0 || index >= this.control_points.size)
                throw new IndexOutOfRangeException("Index out of range in BezierCurve SetControlPoint.");
            this.control_points.X[index] = x;
            this.control_points.Y[index] = y;
            this.control_points.Z[index] = z;
        }
        public void SwapControlPoint(int index_a, int index_b)
        {
            this.control_points.Swap(index_a, index_b);
        }
        public void Interpolation_ALL(ref List<Vector3> points)
        {
            this.Validation();
            double t = 1.0 / (double)(this.precision - 1);
            for (int k = 0; k < this.precision; k++)
            {
                Lerp_de_Casteljau_X(t * k, k);
                Lerp_de_Casteljau_Y(t * k, k);
                Lerp_de_Casteljau_Z(t * k, k);
            }
            int result_index = Chiruno.Summation(this.control_points.size - 1) - 1;
            for (int i = 0; i < precision; i++)
            {
                points.Add(new Vector3(
                    (float)this.temp_de_Casteljau_triangle[i].X[result_index],
                    (float)this.temp_de_Casteljau_triangle[i].Y[result_index],
                    (float)this.temp_de_Casteljau_triangle[i].Z[result_index]
                ));
            }
        }
        public void Validation()
        {
            if (this.control_points.size < 3)
                throw new Exception("Invalid Bezier Curve: defined_points.Capacity < 3");
            if (this.precision < 1)
                throw new Exception("Invalid Bezier Curve: precision < 1");
        }
    }
    */
}
