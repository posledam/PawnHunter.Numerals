using System;
using System.Collections.Generic;
using System.Globalization;

namespace PawnHunter.Numerals
{
    /// <summary>
    ///     Provides formatting of numerals and countable nouns.
    ///     Russian and english languages are supported.
    /// </summary>
    /// <remarks>
    ///     For english numerals the Thousands system of notation is used regardless of the subculture.
    ///     See http://www.quinion.com/words/articles/numbers.htm for more information.
    /// </remarks>
    public sealed class NumeralsFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the NumeralsFormatter class using the current thread's culture.
        /// </summary>
        public NumeralsFormatter()
        {
            // the current thread's culture will be used
            CultureInfo = null;
        }

        /// <summary>
        ///     Initializes a new instance of the NumeralsFormatter class using the specified culture.
        /// </summary>
        /// <param name="cultureInfo">CultureInfo representing the culture to be used by the formatter.</param>
        public NumeralsFormatter(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        /// <summary>
        ///     Gets or sets the culture for the formatter.
        /// </summary>
        public CultureInfo CultureInfo { get; set; }


        #region ICustomFormatter implementation

        /// <summary>
        ///     Formats arg using the specified format.
        /// </summary>
        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            // NumeralsFormatter processes integral types only.
            if (format == null || !IsIntegralType(arg.GetType())) return FormatUnknown(format, arg, formatProvider);

            // The base type for NumeralsFormatter is long,
            // so check whether arg is within long's range
            if (arg is ulong && Convert.ToUInt64(arg) > long.MaxValue)
                throw new FormatException("Argument is too large.",
                    new ArgumentOutOfRangeException(nameof(arg), arg,
                        string.Format("arg must be within -{0} ... {0} range.", long.MaxValue)));

            var formatStringParts = format.Split(';');

            // NumeralsFormatter understands only 'T'('t') and 'W'('w') format specifiers.
            switch (formatStringParts[0].ToUpper())
            {
                case "T":
                    return FormatNumeral(formatStringParts, Convert.ToInt64(arg));
                case "W":
                    return FormatCountable(formatStringParts, Convert.ToInt64(arg));
                default:
                    return FormatUnknown(format, arg, formatProvider);
            }
        }

        #endregion ICustomFormatter


        #region IFormatProvider implementation

        object IFormatProvider.GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter)
                ? this
                : CultureInfo.CurrentCulture.GetFormat(formatType);
        }

        #endregion IFormatProvider implementation


        #region Private routines

        /// <summary>
        ///     Delegates numeral construction to language specific class
        /// </summary>
        private string FormatNumeral(string[] stringFormatParts, long arg)
        {
            var uppercase = char.IsUpper(stringFormatParts[0], 0);

            Gender gender;
            if (stringFormatParts.Length > 1)
                try
                {
                    gender = GetGender(stringFormatParts[1]);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new FormatException("Unknown gender.", ex);
                }
            else
                gender = Gender.Neutral;

            var cultureInfo = CultureInfo ?? CultureInfo.CurrentCulture;

            var language = Language.GetLanguage(cultureInfo);
            if (language == null)
                throw new FormatException(
                    $"The '{cultureInfo.DisplayName}' culture is not supported.",
                    new NotSupportedException());
            return language.NumberInWords(arg, gender, uppercase);
        }

        /// <summary>
        ///     Parses formatting expression,
        ///     extracts and builds plural forms of a given word,
        ///     delegates choise of the correct form to a CountableForm class instance.
        /// </summary>
        private static string FormatCountable(IReadOnlyList<string> stringFormatParts, long arg)
        {
            var wordFormsString = stringFormatParts[stringFormatParts.Count - 1];
            var wordForms = wordFormsString.Split('(');

            string one, two, five;

            if (wordForms.Length == 2)
            {
                // looks like "<stem>(<flexions>)" format is used

                var stem = wordForms[0];
                var flectionsString = wordForms[1].Remove(wordForms[1].Length - 1, 1);
                var flexions = flectionsString.Split(',');

                one = stem + flexions[0];
                two = flexions.Length > 1 ? stem + flexions[1] : one;
                five = flexions.Length > 2 ? stem + flexions[2] : two;
            }
            else
            {
                // looks like "<singular>, <plural 2>, <plural 5>" format is used

                wordForms = wordFormsString.Split(',');
                one = wordForms[0];
                two = wordForms.Length > 1 ? wordForms[1] : one;
                five = wordForms.Length > 2 ? wordForms[2] : two;
            }

            // choose correct form for arg.
            var countable = new CountableForm(one, two, five);
            return countable.ForNumber(Math.Abs(arg));
        }

        /// <summary>
        ///     Performs default formatting.
        /// </summary>
        private static string FormatUnknown(string format, object arg, IFormatProvider formatProvider)
        {
            return !(arg is IFormattable formattable)
                ? arg.ToString()
                : formattable.ToString(format, formatProvider);
        }

        #endregion Private routines


        #region Static helpers

        /// <summary>
        ///     Gets a value indicating whether the type is an integral type.
        /// </summary>
        private static bool IsIntegralType(Type type)
        {
            return type == typeof(int)
                   || type == typeof(uint)
                   || type == typeof(long)
                   || type == typeof(ulong)
                   || type == typeof(short)
                   || type == typeof(ushort)
                   || type == typeof(byte)
                   || type == typeof(sbyte)
                   || type == typeof(char);
        }

        private static Gender GetGender(string genderString)
        {
            if (genderString == null) throw new ArgumentNullException(nameof(genderString));
            if (genderString.Length == 1)
                switch (char.ToUpperInvariant(genderString[0]))
                {
                    case 'F':
                        return Gender.Feminine;
                    case 'M':
                        return Gender.Masculine;
                    case 'N':
                        return Gender.Neutral;
                }
            throw new ArgumentOutOfRangeException(nameof(genderString), genderString, "Unexpected gender string.");
        }

        #endregion Static helpers
    }
}
