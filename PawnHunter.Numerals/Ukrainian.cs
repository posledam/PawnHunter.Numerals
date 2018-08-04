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
				new Countable(" квінтильйон", " квінтильйона", " квінтильйонів", Gender.Masculine),
				new Countable(" квадрильйон", " квадрильйона", " квадрильйонів", Gender.Masculine),
				new Countable(" трильйон", " трильйона", " трильйонів", Gender.Masculine),
				new Countable(" мільярд", " мільярда", " мільярдів", Gender.Masculine),
				new Countable(" мільйон", " мільйона", " мільйонів", Gender.Masculine),
				new Countable(" тисяча", " тисячі", " тисяч", Gender.Feminine),
				new Countable("", "", "")
			};

		private static readonly string[] Hundreds = new[]
			{
				"", "сто", "двісті", "триста", "чотириста", "п'ятсот",
				"шістсот", "сімсот", "вісімсот", "дев'ятсот"
			};

		private static readonly string[] Tens = new[]
			{
				"", "десять", "двадцять", "тридцять", "сорок", "п'ятдесят",
				"шістдесят", "сімдесят", "вісімдесят", "дев'яносто"
			};

		private static readonly string[] Units = new[]
			{
				"нуль", "один", "два", "три", "чотири", "п'ять",
				"шість", "сім", "вісім", "дев'ять", "десять", "одинадцять",
				"дванадцять", "тринадцять", "чотирнадцять", "п'ятнадцять",
				"шістнадцять", "сімнадцять", "вісімнадцять", "дев'ятнадцять"
			};

		private static readonly string[,] Gendered = new[,]
			{
				{ "один", "одна", "одне" },
				{ "два", "дві", "двоє" }
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
				sb.Insert(0, "мінус ");
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
