using System;
using System.Text;

namespace PawnHunter.Numerals
{
	public sealed class Russian
	{
		#region Fields

		private static readonly Countable[] Ranks = new[]
			{
				new Countable(" квинтильон", " квинтильона", " квинтильонов", Gender.Masculine),
				new Countable(" квадрильон", " квадрильона", " квадрильонов", Gender.Masculine),
				new Countable(" триллион", " триллиона", " триллионов", Gender.Masculine),
				new Countable(" миллиард", " миллиарда", " миллиардов", Gender.Masculine),
				new Countable(" миллион", " миллиона", " миллионов", Gender.Masculine),
				new Countable(" тысяча", " тысячи", " тысяч", Gender.Feminine),
				new Countable("", "", "")
			};

		private static readonly string[] Hundreds = new[]
			{
				"", "сто", "двести", "триста", "четыреста", "пятьсот",
				"шестьсот", "семьсот", "восемьсот", "девятьсот"
			};

		private static readonly string[] Tens = new[]
			{
				"", "десять", "двадцать", "тридцать", "сорок", "пятьдесят",
				"шестьдесят", "семьдесят", "восемьдесят", "девяносто"
			};

		private static readonly string[] Units = new[]
			{
				"ноль", "один", "два", "три", "четыре", "пять",
				"шесть", "семь", "восемь", "девять", "десять", "одиннадцать",
				"двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
				"шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
			};

		private static readonly string[,] Gendered = new[,]
			{
				{ "один", "одна", "одно" },
				{ "два", "две", "два" }
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
				sb.Insert(0, "минус ");
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
