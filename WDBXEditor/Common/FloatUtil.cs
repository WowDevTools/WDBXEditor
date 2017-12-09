using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Common
{
	/// <summary>
	/// android-apktool
	/// </summary>
	public class FloatUtil
	{
		private readonly static int canonicalFloatNaN = FloatToRawIntBits(float.NaN);
		private readonly static int maxFloat = FloatToRawIntBits(float.MaxValue);
		private readonly static int minFloat = FloatToRawIntBits(float.MinValue);
		private readonly static int piFloat = FloatToRawIntBits((float)Math.PI);
		private readonly static int eFloat = FloatToRawIntBits((float)Math.E);
		private readonly static int piNegFloat = FloatToRawIntBits(-(float)Math.PI);
		private readonly static int eNegFloat = FloatToRawIntBits(-(float)Math.E);
		private readonly static int pInfinity = FloatToRawIntBits(float.PositiveInfinity);
		private readonly static int nInfinity = FloatToRawIntBits(float.NegativeInfinity);

		private static unsafe int FloatToRawIntBits(float f) => *((int*)&f);
		private static unsafe float IntBitsToFloat(int i) => *((float*)&i);

		public static bool IsLikelyFloat(int value, int minexp = -7, int maxexp = 6)
		{
			// unwanted common named float values
			if (value == canonicalFloatNaN || value == maxFloat || value == minFloat || value == pInfinity || value == nInfinity)
			{
				return false;
			}

			// wanted common named float values
			if (value == eFloat || value == piFloat || value == eNegFloat || value == piNegFloat)
			{
				return true;
			}

			// check for some named integer values
			if (value == int.MaxValue || value == int.MinValue)
			{
				return false;
			}

			// check for likely resource id
			int packageId = value >> 24;
			int resourceType = value >> 16 & 0xff;
			int resourceId = value & 0xffff;
			if ((packageId == 0x7f || packageId == 1) && resourceType < 0x1f && resourceId < 0xfff)
			{
				return false;
			}

			// a non-canonical NaN is more likely to be an integer
			float floatValue = IntBitsToFloat(value);
			if (float.IsNaN(floatValue))
			{
				return false;
			}

			// exponent range check, range is taken from the Alpha client
			double exp = Math.Floor(Math.Log10(Math.Abs(floatValue)));
			if (exp <= minexp || exp >= maxexp)
				return false;

			// try to strip off any small imprecision near the end of the mantissa, remove 0 exponent
			string asInt = StripImprecision(string.Format("{0:0.00000000000000000000E0}", value));
			string asFloat = StripImprecision(string.Format("{0:0.00000000000000000000E0}", floatValue));

			return asFloat.TrimEnd('0').Length < asInt.Length;
		}

		private static string StripImprecision(string value)
		{
			int dp = value.IndexOf('.');
			int exp = value.IndexOf("E");
			int zeros = value.IndexOf("000");
			int nines = value.IndexOf("999");

			if (zeros > dp && zeros < exp)
			{
				value = value.Substring(0, zeros) + value.Substring(exp);
			}
			else if (nines > dp && nines < exp)
			{
				value = value.Substring(0, nines) + value.Substring(exp);
			}

			return value;
		}
	}
}
