using System;
using System.Globalization;
using Xunit;

namespace PawnHunter.Numerals.Tests
{
    public class ReadmeBasedTests
    {
        [Fact]
		public void Readme_example_1()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "Inbox: {0} {0:W;новое,новых} {0:W;сообщение,сообщения,сообщений}";

			Assert.Equal("Inbox: 1 новое сообщение", string.Format(formatter, format, 1));
	        Assert.Equal("Inbox: 2 новых сообщения", string.Format(formatter, format, 2));
	        Assert.Equal("Inbox: 5 новых сообщений", string.Format(formatter, format, 5));
		}

	    [Fact]
	    public void Readme_example_2()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "Inbox: {0} {0:W;нов(ое,ых)} {0:W;сообщени(е,я,й)}";

		    Assert.Equal("Inbox: 1 новое сообщение", string.Format(formatter, format, 1));
		    Assert.Equal("Inbox: 2 новых сообщения", string.Format(formatter, format, 2));
		    Assert.Equal("Inbox: 5 новых сообщений", string.Format(formatter, format, 5));
		}

	    [Fact]
	    public void Readme_example_3()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "{0:W;Найден(а,о)} {0} {0:W;запис(ь,и,ей)}, {0:W;удовлетворяющ(ая,ие,их)} запросу.";

		    Assert.Equal("Найдена 1 запись, удовлетворяющая запросу.", string.Format(formatter, format, 1));
		    Assert.Equal("Найдено 2 записи, удовлетворяющие запросу.", string.Format(formatter, format, 2));
			Assert.Equal("Найдено 5 записей, удовлетворяющих запросу.", string.Format(formatter, format, 5));
		}

	    [Fact]
	    public void Readme_example_4()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "{0:T}";

		    Assert.Equal("Одно", string.Format(formatter, format, 1));
		    Assert.Equal("Тринадцать", string.Format(formatter, format, 13));
		}

	    [Fact]
	    public void Readme_example_5()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "{0:t;f}";

		    Assert.Equal("одна", string.Format(formatter, format, 1));
		    Assert.Equal("тринадцать", string.Format(formatter, format, 13));
		}

	    [Fact]
	    public void Readme_example_6()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "{0:T;M} {0:W;час(,а,ов)}.";

		    Assert.Equal("Один час.", string.Format(formatter, format, 1));
		    Assert.Equal("Два часа.", string.Format(formatter, format, 2));
		    Assert.Equal("Десять часов.", string.Format(formatter, format, 10));
		}

	    [Fact]
	    public void Readme_example_7()
		{
			var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("ru-RU") };

			var format = "{0:T;M} {0:W;час(,а,ов)} {1:t;F} {1:W;минут(а,ы,)}.";

		    Assert.Equal("Два часа десять минут.", string.Format(formatter, format, 2, 10));
		    Assert.Equal("Двадцать один час одна минута.", string.Format(formatter, format, 21, 1));
		    Assert.Equal("Ноль часов ноль минут.", string.Format(formatter, format, 0, 0));
		}

	    [Fact]
	    public void Readme_example_8()
	    {
		    var formatter = new NumeralsFormatter {CultureInfo = new CultureInfo("ru-RU")};

		    var format = "{0:T;M} {0:W;рубл(ь,я,ей)} {1:00} коп.";

		    Assert.Equal("Один рубль 12 коп.", string.Format(formatter, format, 1, 12));
		    Assert.Equal("Три рубля 05 коп.", string.Format(formatter, format, 3, 5));
		    Assert.Equal("Минус три рубля -05 коп.", string.Format(formatter, format, -3, -5));
		}

	    [Fact]
	    public void Readme_example_9()
	    {
		    var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("en-US") };

		    var format = "{0:T} {0:W;dollar(,s)} and {1:t} {1:W;cent(,s)}.";

		    Assert.Equal("One dollar and two cents.", string.Format(formatter, format, 1, 2));
		    Assert.Equal("One hundred and twenty-three thousand, four hundred and fifty-six dollars and seven cents.", string.Format(formatter, format, 123456, 7));
		}

	    [Fact]
	    public void Readme_example_10()
	    {
		    var formatter = new NumeralsFormatter { CultureInfo = new CultureInfo("en-US") };

		    var format = "{0:T} {0:W;hour(,s)} and {1:t} {1:W;minute(,s)}.";
		    var dateTime = new DateTime(2018, 8, 1, 4, 1, 0);

		    Assert.Equal("Four hours and one minute.", string.Format(formatter, format, dateTime.Hour, dateTime.Minute));
	    }
	}
}
