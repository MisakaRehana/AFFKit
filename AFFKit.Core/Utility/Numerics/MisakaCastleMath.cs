using System.Numerics;

namespace AFFKit.Core.Utility.Numerics
{
	public static class MisakaCastleMath // ported from Misaka Castle's project-afs repository
	{
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static float InverseLerp(float a, float b, float value)
		{
			if (!Approximately(a, b))
			{
				return (value - a) / (b - a);
			}
			else
			{
				return 0f;
			}
		}

		public static float InverseLerpClamped(float a, float b, float value)
		{
			return Clamp01(InverseLerp(a, b, value));
		}

		public static float Clamp(float value, float min, float max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		public static float Clamp01(float value)
		{
			return Clamp(value, 0f, 1f);
		}

		public static float ProjectionClamped(float x, float srcA, float srcB, float dstA, float dstB)
		{
			float t = InverseLerpClamped(srcA, srcB, x);
			return Lerp(dstA, dstB, t);
		}

		public static bool Approximately(float a, float b, float tolerance = float.Epsilon)
		{
			return MathF.Abs(b - a) <= tolerance;
		}

		public static Quaternion Euler(float x, float y, float z)
		{
			float pitch = x * MathF.PI / 180f;
			float yaw = y * MathF.PI / 180f;
			float roll = z * MathF.PI / 180f;

			float cy = MathF.Cos(yaw * 0.5f);
			float sy = MathF.Sin(yaw * 0.5f);
			float cp = MathF.Cos(pitch * 0.5f);
			float sp = MathF.Sin(pitch * 0.5f);
			float cr = MathF.Cos(roll * 0.5f);
			float sr = MathF.Sin(roll * 0.5f);

			var qx = new Quaternion(sp, 0f, 0f, cp);
			var qy = new Quaternion(0f, sy, 0f, cy);
			var qz = new Quaternion(0f, 0f, sr, cr);

			return Quaternion.Normalize(qy * qx * qz);
		}
	}
}