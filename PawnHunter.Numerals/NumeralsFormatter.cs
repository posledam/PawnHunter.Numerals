using System;
using System.Globalization;

namespace PawnHunter.Numerals
{
	/// <summary>
	/// Provides formatting of numerals and countable nouns.
	/// Russian and english languages are supported.
	/// </summary>
	/// <remarks>
	/// For english numerals the Thousands system of notation is used regardless of the subculture.
	/// See http://www.quinion.com/words/articles/numbers.htm for more information.
	/// </remarks>
	public sealed class NumeralsFormatter : IFormatProvider, ICustomFormatter
	{
		#region Fields

		private CultureInfo _cultureInfo;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets or sets the culture for the formatter.
		/// </summary>
		public CultureInfo CultureInfo
		{
			get => _cultureInfo;
			set => _cultureInfo = value;
		}

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the NumeralsFormatter class using the current thread's culture.
		/// </summary>
		public NumeralsFormatter()
		{
			// the current thread's culture will be used
			_cultureInfo = null;
		}

		/// <summary>
		/// Initializes a new instance of the NumeralsFormatter class using the specified culture.
		/// </summary>
		/// <param name="cultureInfo">CultureInfo representing the culture to be used by the formatter.</param>
		public NumeralsFormatter(CultureInfo cultureInfo)
		{
			_cultureInfo = cultureInfo;
		}

		#endregion Constructors

		#region Private methods

		/// <summary>
		/// Delegates numeral construction to language specific class
		/// </summary>
		private string FormatNumeral(string[] stringFormatParts, long arg)
		{
			var uppercase = Char.IsUpper(stringFormatParts[0], 0);

			Gender gender;
			if (stringFormatParts.Length > 1)
			{
				try
				{
					gender = GetGender(stringFormatParts[1]);
				}
				catch (ArgumentOutOfRangeException ex)
				{
					throw new FormatException("Unknown gender.", ex);
				}

			}
			else
			{
				gender = Gender.Neutral;
			}

			var cultureInfo = _cultureInfo ?? CultureInfo.CurrentCulture;

			switch (cultureInfo.LCID & 0x00ff)
			{
				//
				// Add more languages here. ;)
				//

				case 0x09:
					return English.ToString(arg, uppercase);

				case 0x19:
					return Russian.ToString(arg, gender, uppercase);

				case 0x22:
					// Thanks to Andre (http://www.rsdn.ru/Users/Profile.aspx?uid=6921)
					return Ukrainian.ToString(arg, gender, uppercase);

				default:
					throw new FormatException(
						$"The '{cultureInfo.DisplayName}' culture is not supported.",
						new NotSupportedException());
			}
		}

		/// <summary>
		/// Parses formatting expression, 
		/// extracts and builds plural forms of a given word,
		/// delegates choise of the correct form to a Countable class instance.
		/// </summary>
		private string FormatCountable(string[] stringFormatParts, long arg)
		{
			var wordFormsString = stringFormatParts[stringFormatParts.Length - 1];
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
			var countable = new Countable(one, two, five);
			return countable[Math.Abs(arg)];
		}

		/// <summary>
		/// Performs default formatting.
		/// </summary>
		private static string FormatUnknown(string format, object arg, IFormatProvider formatProvider)
		{
			var formattable = arg as IFormattable;

			return formattable == null
				? arg.ToString()
				: formattable.ToString(format, formatProvider);
		}

		#region Helpers

		/// <summary>
		/// Gets a value indicating whether the type is an integral type.
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
			switch (genderString.ToUpper())
			{
				case "F":
					return Gender.Feminine;

				case "M":
					return Gender.Masculine;

				case "N":
					return Gender.Neutral;

				default:
					throw new ArgumentOutOfRangeException("genderString", genderString, "Unexpected gender string.");
			}
		}

		#endregion Helpers

		#endregion Private methods

		#region IFormatProvider implementation

		object IFormatProvider.GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter)
				? this
				: CultureInfo.CurrentCulture.GetFormat(formatType);
		}

		#endregion IFormatProvider implementation

		#region ICustomFormatter implementation

		/// <summary>
		/// Formats arg using the specified format.
		/// </summary>
		string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
		{
			// NumeralsFormatter processes integral types only.
			if (format == null || !IsIntegralType(arg.GetType()))
			{
				return FormatUnknown(format, arg, formatProvider);
			}

			// The base type for NumeralsFormatter is long,
			// so check whether arg is within long's range
			if (arg is ulong && Convert.ToUInt64(arg) > long.MaxValue)
			{
				throw new FormatException("Argument is too large.",
					new ArgumentOutOfRangeException(nameof(arg), arg,
						String.Format("arg must be within -{0} ... {0} range.", long.MaxValue)));
			}

			string[] formatStringParts = format.Split(';');

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
	}
}
