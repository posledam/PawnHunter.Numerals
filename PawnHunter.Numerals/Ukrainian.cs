//
// Thanks to Andre (http://www.rsdn.ru/Users/Profile.aspx?uid=6921)
//

using System;
using System.Text;

namespace PawnHunter.Numerals
{
	public sealed class Ukrainian
	{
		#region Fields

		private static readonly Countable[] Ranks = new[]
			{
				new Countable(" кв≥нтильйон", " кв≥нтильйона", " кв≥нтильйон≥в", Gender.Masculine),
				new Countable(" квадрильйон", " квадрильйона", " квадрильйон≥в", Gender.Masculine),
				new Countable(" трильйон", " трильйона", " трильйон≥в", Gender.Masculine),
				new Countable(" м≥ль€рд", " м≥ль€рда", " м≥ль€рд≥в", Gender.Masculine),
				new Countable(" м≥льйон", " м≥льйона", " м≥льйон≥в", Gender.Masculine),
				new Countable(" тис€ча", " тис€ч≥", " тис€ч", Gender.Feminine),
				new Countable("", "", "")
			};

		private static readonly string[] Hundreds = new[]
			{
				"", "сто", "дв≥ст≥", "триста", "чотириста", "п'€тсот", 
				"ш≥стсот", "с≥мсот", "в≥с≥мсот", "дев'€тсот"
			};

		private static readonly string[] Tens = new[]
			{
				"", "дес€ть",  "двадц€ть", "тридц€ть", "сорок", "п'€тдес€т", 
				"ш≥стдес€т", "с≥мдес€т", "в≥с≥мдес€т", "дев'€носто"
			};

		private static readonly string[] Units = new[]
			{
				"нуль", "один", "два", "три", "чотири", "п'€ть", 
				"ш≥сть", "с≥м", "в≥с≥м", "дев'€ть", "дес€ть", "одинадц€ть", 
				"дванадц€ть", "тринадц€ть", "чотирнадц€ть", "п'€тнадц€ть",
				"ш≥стнадц€ть", "с≥мнадц€ть", "в≥с≥мнадц€ть", "дев'€тнадц€ть"
			};

		private static readonly string[,] Gendered = new[,]
			{
				{ "один", "одна", "одне" },
				{ "два",  "дв≥",  "двоЇ" }
			};

		#endregion Fields

		#region Methods

		public static string ToString(long number, Gender gender)
		{
			return ToString(number, gender, false);
		}

		public static string ToString(long number, Gender gender, bool uppercase)
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
					sb.Append(" ");
				}

				if (hundreds > 0)
				{
					sb.Append(Hundreds[hundreds]);
					if (tens + units > 0)
					{
						sb.Append(" ");
					}
				}

				if (tens > 0)
				{
					sb.Append(Tens[tens]);
					if (units > 0)
					{
						sb.Append(" ");
					}
				}

				if (units > 0)
				{
					// mind the gender
					if (units < 3)
					{
						var j = i == Neutral.Ranks.Length - 1 ? (int)gender : (int)Ranks[i].Gender;
						sb.Append(Gendered[units - 1, j]);
					}
					else
					{
						sb.Append(Units[units]);
					}
				}
				sb.Append(Ranks[i][units]);
			}

			if (isNegative)
			{
				sb.Insert(0, "м≥нус ");
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
