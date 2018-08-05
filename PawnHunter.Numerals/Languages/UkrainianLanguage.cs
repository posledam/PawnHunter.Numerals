//
// Thanks to Andre (http://www.rsdn.ru/Users/Profile.aspx?uid=6921)
//

using System;
using System.Text;

namespace PawnHunter.Numerals.Languages
{
    public sealed class UkrainianLanguage : Language
    {
        private static readonly CountableForm[] Ranks =
        {
            new CountableForm(" квінтильйон", " квінтильйона", " квінтильйонів", Gender.Masculine),
            new CountableForm(" квадрильйон", " квадрильйона", " квадрильйонів", Gender.Masculine),
            new CountableForm(" трильйон", " трильйона", " трильйонів", Gender.Masculine),
            new CountableForm(" мільярд", " мільярда", " мільярдів", Gender.Masculine),
            new CountableForm(" мільйон", " мільйона", " мільйонів", Gender.Masculine),
            new CountableForm(" тисяча", " тисячі", " тисяч", Gender.Feminine),
            new CountableForm("", "", "")
        };

        private static readonly string[] Hundreds =
        {
            "", "сто", "двісті", "триста", "чотириста", "п'ятсот",
            "шістсот", "сімсот", "вісімсот", "дев'ятсот"
        };

        private static readonly string[] Tens =
        {
            "", "десять", "двадцять", "тридцять", "сорок", "п'ятдесят",
            "шістдесят", "сімдесят", "вісімдесят", "дев'яносто"
        };

        private static readonly string[] Units =
        {
            "нуль", "один", "два", "три", "чотири", "п'ять",
            "шість", "сім", "вісім", "дев'ять", "десять", "одинадцять",
            "дванадцять", "тринадцять", "чотирнадцять", "п'ятнадцять",
            "шістнадцять", "сімнадцять", "вісімнадцять", "дев'ятнадцять"
        };

        private static readonly string[,] Gendered =
        {
            {"один", "одна", "одне"},
            {"два", "дві", "двоє"}
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

            if (isNegative) sb.Insert(0, "мінус ");

            if (uppercaseFirstLetter) sb[0] = char.ToUpper(sb[0]);

            return sb.ToString();
        }

        #endregion
    }
}
