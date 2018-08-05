namespace PawnHunter.Numerals
{
    public class CountableForm
    {
        private readonly string _one, _two, _five;

        public CountableForm(string one, string two, string five) :
            this(one, two, five, Gender.Neutral)
        {
        }

        public CountableForm(string one, string two, string five, Gender gender)
        {
            _one = one;
            _two = two;
            _five = five;
            Gender = gender;
        }

        public Gender Gender { get; }

        public string ForNumber(long number)
        {
            number %= 100;
            if (number > 20) number %= 10;
            if (number == 1) return _one;
            if (number > 1 && number < 5) return _two;
            return _five;
        }
    }
}
