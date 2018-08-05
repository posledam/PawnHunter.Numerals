using System;
using System.Text;

namespace PawnHunter.Numerals.Languages
{
    public class RussianLanguage : Language
    {
        private static readonly CountableForm[] Ranks =
        {
            new CountableForm(" квинтильон", " квинтильона", " квинтильонов", Gender.Masculine),
            new CountableForm(" квадрильон", " квадрильона", " квадрильонов", Gender.Masculine),
            new CountableForm(" триллион", " триллиона", " триллионов", Gender.Masculine),
            new CountableForm(" миллиард", " миллиарда", " миллиардов", Gender.Masculine),
            new CountableForm(" миллион", " миллиона", " миллионов", Gender.Masculine),
            new CountableForm(" тысяча", " тысячи", " тысяч", Gender.Feminine),
            new CountableForm("", "", "")
        };

        private static readonly string[] Hundreds =
        {
            "", "сто", "двести", "триста", "четыреста", "пятьсот",
            "шестьсот", "семьсот", "восемьсот", "девятьсот"
        };

        private static readonly string[] Tens =
        {
            "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят",
            "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
        };

        private static readonly string[] Units =
        {
            "ноль", "один", "два", "три", "четыре", "пять",
            "шесть", "семь", "восемь", "девять", "десять", "одиннадцать",
            "двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
            "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
        };

        private static readonly string[,] Gendered =
        {
            {"один", "одна", "одно"},
            {"два", "две", "два"}
        };


        #region Overrides of Language

        /// <inheritdoc />
        public override string NumberInWords(long number, Gender gender, bool uppercaseFirstLetter)
        {
            if (number == 0)
            {
                if (uppercaseFirstLetter) return char.ToUpper(Units[0][0]) + Units[0].Remove(0, 1);
                return Units[0];
            }

            var isNegative = false;
            if (number < 0)
            {
                isNegative = true;
                number = Math.Abs(number);
            }

            var sb = new StringBuilder();
            for (var i = 0; i < RankValues.Length; i++)
            {
                var rankValue = (int) (number / RankValues[i]);
                number %= RankValues[i];

                if (rankValue <= 0) continue;
                var hundreds = rankValue / 100;
                var tens = rankValue / 10 % 10;
                var units = rankValue % 10;

                if (tens == 1)
                {
                    tens = 0;
                    units = rankValue % 100;
                }

                if (sb.Length > 0) sb.Append(" ");

                if (hundreds > 0)
                {
                    sb.Append(Hundreds[hundreds]);
                    if (tens + units > 0) sb.Append(" ");
                }

                if (tens > 0)
                {
                    sb.Append(Tens[tens]);
                    if (units > 0) sb.Append(" ");
                }

                if (units > 0)
                {
                    // mind the gender
                    if (units < 3)
                    {
                        var j = i == RankValues.Length - 1 ? (int) gender : (int) Ranks[i].Gender;
                        sb.Append(Gendered[units - 1, j]);
                    }
                    else
                    {
                        sb.Append(Units[units]);
                    }
                }

                sb.Append(Ranks[i].ForNumber(units));
            }

            if (isNegative) sb.Insert(0, "минус ");

            if (uppercaseFirstLetter) sb[0] = char.ToUpper(sb[0]);

            return sb.ToString();
        }

        #endregion
    }
}
