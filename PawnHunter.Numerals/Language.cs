using System.Collections.Generic;
using System.Globalization;
using PawnHunter.Numerals.Languages;

namespace PawnHunter.Numerals
{
    public abstract class Language
    {
        protected static long[] RankValues =
        {
            1000000000000000000,
            1000000000000000,
            1000000000000,
            1000000000,
            1000000,
            1000,
            1
        };

        private static readonly Language
            Ru = new RussianLanguage(),
            En = new EnglishLanguage(),
            Ua = new UkrainianLanguage();

        public abstract string NumberInWords(long number, Gender gender, bool uppercaseFirstLetter);

        public static Language GetLanguage(CultureInfo cultureInfo)
        {
            switch (cultureInfo.LCID & 0x00ff)
            {
                //
                // Add more languages here. ;)
                //
                case 0x09:
                    return En;
                case 0x19:
                    return Ru;
                case 0x22:
                    return Ua;
                default:
                    return null;
            }
        }
    }
}
