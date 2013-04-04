namespace PawnHunter.Numerals
{
	public enum Gender
	{
		Masculine,
		Feminine,
		Neutral
	}

	public class Countable
	{
		#region Fields

		private readonly string[] _forms = new string[3];
		private readonly Gender _gender;

		#endregion Fields

		#region Properties

		public Gender Gender
		{
			get { return _gender; }
		}

		#endregion Properties

		#region Constructors

		public Countable(string one, string two, string five) :
			this(one, two, five, Gender.Neutral)
		{
		}

		public Countable(string one, string two, string five, Gender gender)
		{
			_forms[0] = one;
			_forms[1] = two;
			_forms[2] = five;

			_gender = gender;
		}

		#endregion Constructors

		#region Indexers

		public string this[long number]
		{
			get
			{
				number %= 100;

				if (number > 20)
				{
					number %= 10;
				}

				if (number == 1)
				{
					return _forms[0];
				}

				if (number > 1 && number < 5)
				{
					return _forms[1];
				}

				return _forms[2];
			}
		}

		#endregion Indexers
	}
}
