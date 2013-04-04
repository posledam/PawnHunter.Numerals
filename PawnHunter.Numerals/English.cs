using System;
using System.Text;

namespace PawnHunter.Numerals
{
	public sealed class English
	{
		#region Fields

		private static readonly string[] Ranks = new[]
			{
				" quintillion", " quadrillion", " trillion", 
				" billion", " million", " thousand", ""
			};

		private static readonly string[] Tens = new[]
			{
				"", "ten", "twenty", "thirty", "forty", "fifty", 
				"sixty", "seventy", "eighty", "ninety" 
			};

		private static readonly string[] Units = new[]
			{
				"zero", "one", "two", "three", "four", "five", 
				"six", "seven", "eight", "nine", "ten", 
				"eleven", "twelve", "thirteen", "fourteen", "fifteen",
				"sixteen", "seventeen", "eighteen", "nineteen"
			};

		#endregion Fields

		#region Methods

		public static string ToString(long number)
		{
			return ToString(number, false);
		}

		public static string ToString(long number, bool uppercase)
		{
			if (number == 0)
			{
				if (uppercase)
				{
					return Char.ToUpper(Units[0][0]) + Units[0].Remove(0, 1);
				}
				return Units[0];
			}

			var isNegative = false;
			if (number < 0)
			{
				isNegative = true;
				number = Math.Abs(number);
			}

			var sb = new StringBuilder();
			for (var i = 0; i < Neutral.Ranks.Length; i++)
			{
				var rankValue = (int)(number / Neutral.Ranks[i]);
				number %= Neutral.Ranks[i];

				if (rankValue <= 0) continue;
				var hundreds = rankValue / 100;
				var tens = rankValue / 10 % 10;
				var units = rankValue % 10;

				if (tens == 1)
				{
					tens = 0;
					units = rankValue % 100;
				}

				if (sb.Length > 0)
				{
					if (hundreds > 0)
						sb.Append(", ");
					else if (tens + units > 0)
						sb.Append(" and ");
				}

				if (hundreds > 0)
				{
					sb.Append(Units[hundreds]);
					sb.Append(" hundred");
					if (tens + units > 0)
						sb.Append(" and ");
				}

				if (tens > 0)
				{
					sb.Append(Tens[tens]);
					if (units > 0)
					{
						sb.Append("-");
					}
				}

				if (units > 0)
					sb.Append(Units[units]);

				sb.Append(Ranks[i]);
			}

			if (isNegative)
			{
				sb.Insert(0, "minus ");
			}

			if (uppercase)
			{
				sb[0] = Char.ToUpper(sb[0]);
			}

			return sb.ToString();
		}

		#endregion Methods
	}
}
