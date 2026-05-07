using StudentLibrary.Domain.Enums;
using StudentLibrary.Domain.Interfaces;
using StudentLibrary.Domain.Models;
using StudentLibrary.Domain.Services;

namespace StudentLibrary.App;

/// Консольний застосунок з інтерактивним меню для системи обліку
/// літератури та читацьких формулярів студентської бібліотеки.
internal static class Program
{
    private static readonly Library Lib = new("Студентська бібліотека");
    private static readonly SearchService Search = new();


    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        SeedData();

        var running = true;
        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("СТУДЕНТСЬКА БІБЛІОТЕКА ");
            Console.WriteLine("  1. Управління користувачами");
            Console.WriteLine("  2. Управління документами (літературою)");
            Console.WriteLine("  3. Управління видачами документів");
            Console.WriteLine("  4. Пошук");
            Console.WriteLine("  0. Вихід");
            Console.WriteLine("------------------------------------------");
            Console.Write("Оберіть розділ: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": UserMenu(); break;
                case "2": DocumentMenu(); break;
                case "3": LoanMenu(); break;
                case "4": SearchMenu(); break;
                case "0": running = false; break;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }

        Console.WriteLine("До побачення!");
    }

    // ========================================================================
    //  1. УПРАВЛІННЯ КОРИСТУВАЧАМИ (вимоги 1.1 – 1.5.3)
    // ========================================================================

    /// <summary>
    /// Підменю управління користувачами бібліотеки.
    /// </summary>
    private static void UserMenu()
    {
        var back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("--- Управління користувачами ---");
            Console.WriteLine("  1. Додати користувача            (1.1)");
            Console.WriteLine("  2. Видалити користувача           (1.2)");
            Console.WriteLine("  3. Змінити дані користувача       (1.3)");
            Console.WriteLine("  4. Переглянути конкретного        (1.4)");
            Console.WriteLine("  5. Список всіх користувачів       (1.5)");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть дію: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddStudent(); break;
                case "2": RemoveStudent(); break;
                case "3": UpdateStudentData(); break;
                case "4": ViewStudent(); break;
                case "5": ListStudents(); break;
                case "0": back = true; break;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }

    /// <summary>
    /// Додає нового студента (вимога 1.1).
    /// </summary>
    private static void AddStudent()
    {
        try
        {
            Console.Write("Ім'я: ");
            var firstName = Console.ReadLine()!;
            Console.Write("Прізвище: ");
            var lastName = Console.ReadLine()!;
            Console.Write("Академічна група: ");
            var group = Console.ReadLine()!;

            var student = new Student(firstName, lastName, group);
            Lib.AddStudent(student);
            Console.WriteLine($"Студента додано: {student.ToPrintString()}");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Видаляє студента за номером зі списку (вимога 1.2).
    /// </summary>
    private static void RemoveStudent()
    {
        try
        {
            var student = SelectStudent("Оберіть студента для видалення");
            if (student is null) return;

            Lib.RemoveStudent(student.Id);
            Console.WriteLine($"Студента \"{student.GetFullName()}\" видалено.");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Змінює дані обраного студента з вибором конкретного поля (вимога 1.3).
    /// </summary>
    private static void UpdateStudentData()
    {
        try
        {
            var student = SelectStudent("Оберіть студента для редагування");
            if (student is null) return;

            var editing = true;
            while (editing)
            {
                Console.WriteLine();
                Console.WriteLine($"Поточні дані: {student.ToPrintString()}");
                Console.WriteLine("Що змінити?");
                Console.WriteLine($"  1. Ім'я         ({student.FirstName})");
                Console.WriteLine($"  2. Прізвище     ({student.LastName})");
                Console.WriteLine($"  3. Група        ({student.AcademicGroup})");
                Console.WriteLine("  0. Назад");
                Console.Write("Вибір: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Write("Нове ім'я: ");
                        student.UpdateFirstName(Console.ReadLine()!);
                        Console.WriteLine("Ім'я оновлено.");
                        break;
                    case "2":
                        Console.Write("Нове прізвище: ");
                        student.UpdateLastName(Console.ReadLine()!);
                        Console.WriteLine("Прізвище оновлено.");
                        break;
                    case "3":
                        Console.Write("Нова група: ");
                        student.UpdateAcademicGroup(Console.ReadLine()!);
                        Console.WriteLine("Групу оновлено.");
                        break;
                    case "0":
                        editing = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Переглядає дані конкретного студента (вимога 1.4).
    /// </summary>
    private static void ViewStudent()
    {
        var student = SelectStudent("Оберіть студента для перегляду");
        if (student is null) return;

        Console.WriteLine(student.ToPrintString());

        var docs = Lib.GetDocumentsHeldByStudent(student.Id);
        if (docs.Count > 0)
        {
            Console.WriteLine($"  Документи на руках ({docs.Count}):");
            foreach (var d in docs)
                Console.WriteLine($"    - {d.ToPrintString()}");
        }
        else
        {
            Console.WriteLine("  Документів на руках немає.");
        }
    }

    /// <summary>
    /// Виводить список усіх студентів з вибором сортування (вимоги 1.5, 1.5.1–1.5.3).
    /// </summary>
    private static void ListStudents()
    {
        Console.WriteLine("Сортувати за: 1-Ім'я  2-Прізвище  3-Група");
        Console.Write("Вибір [2]: ");
        var criterion = Console.ReadLine()?.Trim() switch
        {
            "1" => StudentSortCriterion.ByFirstName,
            "3" => StudentSortCriterion.ByAcademicGroup,
            _ => StudentSortCriterion.ByLastName
        };

        var list = Lib.GetAllStudents(criterion);
        if (list.Count == 0)
        {
            Console.WriteLine("Список студентів порожній.");
            return;
        }

        PrintNumberedList(list);
    }

    // ========================================================================
    //  2. УПРАВЛІННЯ ДОКУМЕНТАМИ (вимоги 2.1 – 2.5.2)
    // ========================================================================

    /// <summary>
    /// Підменю управління документами бібліотеки.
    /// </summary>
    private static void DocumentMenu()
    {
        var back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("--- Управління документами ---");
            Console.WriteLine("  1. Додати документ               (2.1)");
            Console.WriteLine("  2. Видалити документ              (2.2)");
            Console.WriteLine("  3. Змінити дані документа         (2.3)");
            Console.WriteLine("  4. Переглянути конкретний         (2.4)");
            Console.WriteLine("  5. Список всіх документів         (2.5)");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть дію: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddDocument(); break;
                case "2": RemoveDocument(); break;
                case "3": UpdateDocumentData(); break;
                case "4": ViewDocument(); break;
                case "5": ListDocuments(); break;
                case "0": back = true; break;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }

    /// <summary>
    /// Додає новий документ — книгу або журнал (вимога 2.1).
    /// </summary>
    private static void AddDocument()
    {
        try
        {
            Console.WriteLine("Тип: 1-Книга  2-Журнал");
            Console.Write("Вибір: ");
            var type = Console.ReadLine()?.Trim();

            Console.Write("Назва: ");
            var title = Console.ReadLine()!;
            Console.Write("Автор: ");
            var author = Console.ReadLine()!;
            Console.Write("Рік видання: ");
            var year = int.Parse(Console.ReadLine()!);

            Document document;
            if (type == "2")
            {
                Console.Write("Том: ");
                var volume = int.Parse(Console.ReadLine()!);
                Console.Write("Номер випуску: ");
                var issue = int.Parse(Console.ReadLine()!);
                document = new Journal(title, author, year, volume, issue);
            }
            else
            {
                Console.Write("Кількість сторінок: ");
                var pages = int.Parse(Console.ReadLine()!);
                Console.Write("Видавництво: ");
                var publisher = Console.ReadLine()!;
                document = new Book(title, author, year, pages, publisher);
            }

            Lib.AddDocument(document);
            Console.WriteLine($"Документ додано: {document.ToPrintString()}");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Видаляє документ (вимога 2.2).
    /// </summary>
    private static void RemoveDocument()
    {
        try
        {
            var doc = SelectDocument("Оберіть документ для видалення");
            if (doc is null) return;

            Lib.RemoveDocument(doc.Id);
            Console.WriteLine($"Документ \"{doc.Title}\" видалено.");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Змінює дані документа з вибором конкретного поля (вимога 2.3).
    /// </summary>
    private static void UpdateDocumentData()
    {
        try
        {
            var doc = SelectDocument("Оберіть документ для редагування");
            if (doc is null) return;

            var editing = true;
            while (editing)
            {
                Console.WriteLine();
                Console.WriteLine($"Поточні дані: {doc.ToPrintString()}");
                Console.WriteLine("Що змінити?");
                Console.WriteLine($"  1. Назва        ({doc.Title})");
                Console.WriteLine($"  2. Автор        ({doc.Author})");
                Console.WriteLine($"  3. Рік видання  ({doc.Year})");

                switch (doc)
                {
                    case Book book:
                        Console.WriteLine($"  4. Видавництво  ({book.Publisher})");
                        Console.WriteLine($"  5. Сторінки     ({book.PageCount})");
                        break;
                    case Journal journal:
                        Console.WriteLine($"  4. Том          ({journal.Volume})");
                        Console.WriteLine($"  5. Випуск №     ({journal.IssueNumber})");
                        break;
                }

                Console.WriteLine("  0. Назад");
                Console.Write("Вибір: ");

                var choice = Console.ReadLine()?.Trim();
                switch (choice)
                {
                    case "1":
                        Console.Write("Нова назва: ");
                        doc.UpdateTitle(Console.ReadLine()!);
                        Console.WriteLine("Назву оновлено.");
                        break;
                    case "2":
                        Console.Write("Новий автор: ");
                        doc.UpdateAuthor(Console.ReadLine()!);
                        Console.WriteLine("Автора оновлено.");
                        break;
                    case "3":
                        Console.Write("Новий рік: ");
                        doc.UpdateYear(int.Parse(Console.ReadLine()!));
                        Console.WriteLine("Рік оновлено.");
                        break;
                    case "4":
                        if (doc is Book bookToEdit)
                        {
                            Console.Write("Нове видавництво: ");
                            bookToEdit.UpdatePublisher(Console.ReadLine()!);
                            Console.WriteLine("Видавництво оновлено.");
                        }
                        else if (doc is Journal journalToEdit)
                        {
                            Console.Write("Новий том: ");
                            journalToEdit.UpdateVolume(int.Parse(Console.ReadLine()!));
                            Console.WriteLine("Том оновлено.");
                        }
                        break;
                    case "5":
                        if (doc is Book bookPages)
                        {
                            Console.Write("Нова кількість сторінок: ");
                            bookPages.UpdatePageCount(int.Parse(Console.ReadLine()!));
                            Console.WriteLine("Кількість сторінок оновлено.");
                        }
                        else if (doc is Journal journalIssue)
                        {
                            Console.Write("Новий номер випуску: ");
                            journalIssue.UpdateIssueNumber(int.Parse(Console.ReadLine()!));
                            Console.WriteLine("Номер випуску оновлено.");
                        }
                        break;
                    case "0":
                        editing = false;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Переглядає дані конкретного документа (вимога 2.4).
    /// </summary>
    private static void ViewDocument()
    {
        var doc = SelectDocument("Оберіть документ для перегляду");
        if (doc is null) return;

        Console.WriteLine(doc.ToPrintString());

        var (isInLibrary, holder) = Lib.GetDocumentStatus(doc.Id);
        Console.WriteLine(isInLibrary
            ? "  Статус: у бібліотеці (доступний)."
            : $"  Статус: видано — {holder!.GetFullName()} (гр. {holder.AcademicGroup})");
    }

    /// <summary>
    /// Виводить список усіх документів з вибором сортування (вимоги 2.5, 2.5.1, 2.5.2).
    /// </summary>
    private static void ListDocuments()
    {
        Console.WriteLine("Сортувати за: 1-Назвою  2-Автором");
        Console.Write("Вибір [1]: ");
        var criterion = Console.ReadLine()?.Trim() switch
        {
            "2" => DocumentSortCriterion.ByAuthor,
            _ => DocumentSortCriterion.ByTitle
        };

        var list = Lib.GetAllDocuments(criterion);
        if (list.Count == 0)
        {
            Console.WriteLine("Список документів порожній.");
            return;
        }

        PrintNumberedList(list);
    }

    // ========================================================================
    //  3. УПРАВЛІННЯ ВИДАЧАМИ (вимоги 3.1 – 3.4)
    // ========================================================================

    /// <summary>
    /// Підменю управління видачами документів.
    /// </summary>
    private static void LoanMenu()
    {
        var back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("--- Управління видачами ---");
            Console.WriteLine("  1. Видати документ студенту       (3.1)");
            Console.WriteLine("  2. Документи конкретного студента (3.2)");
            Console.WriteLine("  3. Перевірити статус документа    (3.3)");
            Console.WriteLine("  4. Повернути документ             (3.4)");
            Console.WriteLine("  5. Всі активні видачі");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть дію: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": IssueDocument(); break;
                case "2": ViewStudentDocuments(); break;
                case "3": CheckDocumentStatus(); break;
                case "4": ReturnDocument(); break;
                case "5": ListActiveLoans(); break;
                case "0": back = true; break;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }

    /// <summary>
    /// Видає документ студенту (вимога 3.1, обмеження n менше 5).
    /// </summary>
    private static void IssueDocument()
    {
        try
        {
            var student = SelectStudent("Оберіть студента-отримувача");
            if (student is null) return;

            var doc = SelectDocument("Оберіть документ для видачі");
            if (doc is null) return;

            var loan = Lib.IssueDocument(student.Id, doc.Id);
            Console.WriteLine($"Видачу оформлено: {loan.ToPrintString()}");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Показує, які документи взяв конкретний студент (вимога 3.2).
    /// </summary>
    private static void ViewStudentDocuments()
    {
        try
        {
            var student = SelectStudent("Оберіть студента");
            if (student is null) return;

            var docs = Lib.GetDocumentsHeldByStudent(student.Id);
            if (docs.Count == 0)
            {
                Console.WriteLine($"У студента {student.GetFullName()} немає активних видач.");
                return;
            }

            Console.WriteLine($"Документи на руках у {student.GetFullName()} ({docs.Count}):");
            PrintNumberedList(docs);
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Визначає, чи документ у бібліотеці, а якщо виданий — кому (вимога 3.3).
    /// </summary>
    private static void CheckDocumentStatus()
    {
        try
        {
            var doc = SelectDocument("Оберіть документ для перевірки");
            if (doc is null) return;

            var (isInLibrary, holder) = Lib.GetDocumentStatus(doc.Id);
            if (isInLibrary)
            {
                Console.WriteLine($"Документ \"{doc.Title}\" знаходиться у бібліотеці (доступний).");
            }
            else
            {
                Console.WriteLine($"Документ \"{doc.Title}\" видано студенту: {holder!.GetFullName()} (гр. {holder.AcademicGroup}).");
            }
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Повертає документ до бібліотеки (вимога 3.4).
    /// </summary>
    private static void ReturnDocument()
    {
        try
        {
            var student = SelectStudent("Оберіть студента, який повертає");
            if (student is null) return;

            var docs = Lib.GetDocumentsHeldByStudent(student.Id);
            if (docs.Count == 0)
            {
                Console.WriteLine($"У студента {student.GetFullName()} немає документів для повернення.");
                return;
            }

            Console.WriteLine("Документи на руках:");
            PrintNumberedList(docs);
            Console.Write("Номер документа: ");
            var idx = int.Parse(Console.ReadLine()!) - 1;
            if (idx < 0 || idx >= docs.Count)
            {
                Console.WriteLine("Невірний номер.");
                return;
            }

            var doc = docs[idx];
            Lib.ReturnDocument(student.Id, doc.Id);
            Console.WriteLine($"Документ \"{doc.Title}\" повернуто до бібліотеки.");
        }
        catch (Exception ex) { PrintError(ex); }
    }

    /// <summary>
    /// Виводить список усіх активних видач.
    /// </summary>
    private static void ListActiveLoans()
    {
        var loans = Lib.GetActiveLoans();
        if (loans.Count == 0)
        {
            Console.WriteLine("Активних видач немає.");
            return;
        }

        Console.WriteLine($"Активні видачі ({loans.Count}):");
        for (var i = 0; i < loans.Count; i++)
            Console.WriteLine($"  {i + 1}. {loans[i].ToPrintString()}");
    }

    // ========================================================================
    //  4. ПОШУК (вимоги 4.1, 4.2)
    // ========================================================================

    /// <summary>
    /// Підменю пошуку.
    /// </summary>
    private static void SearchMenu()
    {
        var back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("--- Пошук ---");
            Console.WriteLine("  1. Пошук серед документів         (4.1)");
            Console.WriteLine("  2. Пошук серед користувачів       (4.2)");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть дію: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": SearchDocuments(); break;
                case "2": SearchStudents(); break;
                case "0": back = true; break;
                default: Console.WriteLine("Невірний вибір."); break;
            }
        }
    }

    /// <summary>
    /// Шукає документи за ключовим словом (вимога 4.1).
    /// </summary>
    private static void SearchDocuments()
    {
        Console.Write("Введіть ключове слово для пошуку серед документів: ");
        var keyword = Console.ReadLine()!;

        var results = Search.SearchDocuments(Lib, keyword);
        if (results.Count == 0)
        {
            Console.WriteLine("Документів за запитом не знайдено.");
            return;
        }

        Console.WriteLine($"Знайдено документів: {results.Count}");
        PrintNumberedList(results);
    }

    /// <summary>
    /// Шукає студентів за ключовим словом (вимога 4.2).
    /// </summary>
    private static void SearchStudents()
    {
        Console.Write("Введіть ключове слово для пошуку серед користувачів: ");
        var keyword = Console.ReadLine()!;

        var results = Search.SearchStudents(Lib, keyword);
        if (results.Count == 0)
        {
            Console.WriteLine("Користувачів за запитом не знайдено.");
            return;
        }

        Console.WriteLine($"Знайдено користувачів: {results.Count}");
        PrintNumberedList(results);
    }

    // ========================================================================
    //  ДОПОМІЖНІ МЕТОДИ
    // ========================================================================

    /// <summary>
    /// Заповнює бібліотеку демонстраційними даними.
    /// </summary>
    private static void SeedData()
    {
        var students = new Student[]
        {
            new("Владислав", "Омелян", "ІП-56"),
            new("Артем", "Шаповал", "ІП-56"),
            new("Олексій", "Богомолов", "ІК-51"),
            new("Роман", "Мельник", "ІП-51"),
            new("Ігор", "Петренко", "ІМ-51")
        };

        var documents = new Document[]
        {
            new Book("Чистий код", "Роберт Мартін", 2008, 464, "Prentice Hall"),
            new Book("Шаблони проектування", "Е. Гамма", 1994, 395, "Addison-Wesley"),
            new Book("Прагматичний програміст", "Е. Хант", 1999, 320, "Addison-Wesley"),
            new Journal("Computer Science Review", "Elsevier Editorial", 2024, 52, 3),
            new Journal("ACM Computing Surveys", "ACM", 2024, 56, 7)
        };

        foreach (var s in students) Lib.AddStudent(s);
        foreach (var d in documents) Lib.AddDocument(d);

        Console.WriteLine($"Бібліотеку ініціалізовано: {students.Length} студентів, {documents.Length} документів.");
    }

    /// <summary>
    /// Виводить пронумерований список та дозволяє обрати студента за номером.
    /// </summary>
    /// <param name="prompt">Текст підказки.</param>
    /// <returns>Обраний студент або null, якщо скасовано.</returns>
    private static Student? SelectStudent(string prompt)
    {
        var list = Lib.GetAllStudents();
        if (list.Count == 0)
        {
            Console.WriteLine("Список студентів порожній.");
            return null;
        }

        Console.WriteLine($"{prompt}:");
        PrintNumberedList(list);
        Console.Write("Номер (0 — скасувати): ");
        if (!int.TryParse(Console.ReadLine(), out var num)) return null;
        var idx = num - 1;

        if (idx < 0 || idx >= list.Count) return null;
        return list[idx];
    }

    /// <summary>
    /// Виводить пронумерований список документів та дозволяє обрати один.
    /// </summary>
    /// <param name="prompt">Текст підказки.</param>
    /// <returns>Обраний документ або null, якщо скасовано.</returns>
    private static Document? SelectDocument(string prompt)
    {
        var list = Lib.GetAllDocuments();
        if (list.Count == 0)
        {
            Console.WriteLine("Список документів порожній.");
            return null;
        }

        Console.WriteLine($"{prompt}:");
        PrintNumberedList(list);
        Console.Write("Номер (0 — скасувати): ");
        if (!int.TryParse(Console.ReadLine(), out var num)) return null;
        var idx = num - 1;

        if (idx < 0 || idx >= list.Count) return null;
        return list[idx];
    }

    /// <summary>
    /// Виводить пронумерований список об'єктів, що підтримують IPrintable.
    /// </summary>
    private static void PrintNumberedList<T>(IReadOnlyList<T> items) where T : IPrintable
    {
        for (var i = 0; i < items.Count; i++)
            Console.WriteLine($"  {i + 1}. {items[i].ToPrintString()}");
    }

    /// <summary>
    /// Зчитує рядок; якщо порожній — повертає значення за замовчуванням.
    /// </summary>
    private static string ReadOrDefault(string defaultValue)
    {
        var input = Console.ReadLine()?.Trim();
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }

    /// <summary>
    /// Виводить повідомлення помилки.
    /// </summary>
    private static void PrintError(Exception ex)
    {
        Console.WriteLine($"[Помилка] {ex.Message}");
    }
}
