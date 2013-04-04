PawnHunter.Numerals
===================

Provides formatting of numerals and countable nouns. Russian and english languages are supported.

Example 1
---------

```CSharp
Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

NumeralsFormatter formatter = new NumeralsFormatter();
string format;

format = "Inbox: {0} {0:W;новое,новых} {0:W;сообщение,сообщения,сообщений}";
Console.WriteLine(String.Format(formatter, format, 1));
Console.WriteLine(String.Format(formatter, format, 2));
Console.WriteLine(String.Format(formatter, format, 5));
Console.WriteLine();
```

Will be:

    Inbox: 1 новое сообщение
    Inbox: 2 новых сообщения
    Inbox: 5 новых сообщений

Example 2
---------
```CSharp
format = "Inbox: {0} {0:W;нов(ое,ых)} {0:W;сообщени(е,я,й)}";
Console.WriteLine(String.Format(formatter, format, 1));
Console.WriteLine(String.Format(formatter, format, 2));
Console.WriteLine(String.Format(formatter, format, 5));
Console.WriteLine();
```

Will be:

    Inbox: 1 новое сообщение
    Inbox: 2 новых сообщения
    Inbox: 5 новых сообщений
    
Example 3
---------
```CSharp
format = "{0:W;Найден(а,о)} {0} {0:W;запис(ь,и,ей)}, {0:W;удовлетворяющ(ая,их)} запросу.";
Console.WriteLine(String.Format(formatter, format, 1));
Console.WriteLine(String.Format(formatter, format, 5));
Console.WriteLine();
```

Will be:

    Найдена 1 запись, удовлетворяющая запросу.
    Найдено 5 записей, удовлетворяющих запросу.
    
Example 4
---------
```CSharp
format = "{0:T}";
Console.WriteLine(String.Format(formatter, format, 1));
Console.WriteLine(String.Format(formatter, format, 13));
Console.WriteLine();
```

Will be:

    Одно
    Тринадцать
    
Example 5
---------
```CSharp
format = "{0:t;f}";
Console.WriteLine(String.Format(formatter, format, 1));
Console.WriteLine(String.Format(formatter, format, 13));
Console.WriteLine();
```

Will be:

    одна
    тринадцать
    
Example 6
---------
```CSharp
format = "{0:T;M} {0:W;час(,а,ов)}.";
Console.WriteLine(String.Format(formatter, format, 2));
Console.WriteLine(String.Format(formatter, format, 10));
Console.WriteLine();
```

Will be:

    Два часа.
    Десять часов.

Example 7
---------
```CSharp
format = "{0:T;M} {0:W;час(,а,ов)} {1:t;F} {1:W;минут(а,ы,)}.";
Console.WriteLine(String.Format(formatter, format, 2, 10));
Console.WriteLine(String.Format(formatter, format, 21, 1));
Console.WriteLine(String.Format(formatter, format, 0, 0));
Console.WriteLine();
```

Will be:

    Два часа десять минут.
    Двадцать один час одна минута.
    Ноль часов ноль минут.

Example 8
---------
```CSharp
format = "{0:T;M} {0:W;рубл(ь,я,ей)} {1:00} коп.";
Console.WriteLine(String.Format(formatter, format, 1, 12));
Console.WriteLine(String.Format(formatter, format, 3, 5));
Console.WriteLine(String.Format(formatter, format, -3, -5));
Console.WriteLine();
```

Will be:

    Один рубль 12 коп.
    Три рубля 05 коп.
    Минус три рубля -05 коп.

Example 9
---------
```CSharp
formatter.CultureInfo = new CultureInfo("en-US");
 
format = "{0:T} {0:W;dollar(,s)} and {1:t} {1:W;cent(,s)}.";
Console.WriteLine(String.Format(formatter, format, 1, 2));
Console.WriteLine(String.Format(formatter, format, 123456, 7));
Console.WriteLine();
```

Will be:

    One dollar and two cents.
    One hundred and twenty-three thousand, four hundred and fifty-six dollars and seven cents.

Example 10
----------
```CSharp
format = "{0:T} {0:W;hour(,s)} and {1:t} {1:W;minute(,s)}.";
string text = String.Format(formatter, format, DateTime.Now.Hour, DateTime.Now.Minute);
```

Will be:

    Four hours and one minute.
