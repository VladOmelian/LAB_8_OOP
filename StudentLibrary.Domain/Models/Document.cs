using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;

/// <summary>
/// Абстрактний клас, що представляє документ (одиницю літератури) в бібліотеці.
/// Демонструє абстракцію та виступає базовим класом для відношення "Узагальнення"
/// з конкретними типами документів (<see cref="Book"/>, <see cref="Journal"/>).
/// </summary>
public abstract class Document : IPrintable, ISearchable
{
    /// <summary>
    /// Унікальний ідентифікатор документа.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Назва документа.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Автор документа.
    /// </summary>
    public string Author { get; private set; }

    /// <summary>
    /// Рік видання документа.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Ініціалізує новий документ із заданими атрибутами.
    /// </summary>
    /// <param name="title">Назва документа.</param>
    /// <param name="author">Автор документа.</param>
    /// <param name="year">Рік видання (від 1450 до поточного року).</param>
    /// <exception cref="ArgumentException">Виникає, якщо назва або автор порожні.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо рік поза припустимим діапазоном.</exception>
    protected Document(string title, string author, int year)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва документа не може бути порожньою.", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор документа не може бути порожнім.", nameof(author));
        ValidateYear(year);

        Id = Guid.NewGuid();
        Title = title.Trim();
        Author = author.Trim();
        Year = year;
    }

    /// <summary>
    /// Повертає тип документа у вигляді рядка (наприклад, "Книга", "Журнал").
    /// Має бути реалізований класами-нащадками (поліморфізм).
    /// </summary>
    /// <returns>Тип документа.</returns>
    public abstract string GetDocumentType();

    /// <summary>
    /// Оновлює назву документа.
    /// </summary>
    /// <param name="newTitle">Нова назва.</param>
    /// <exception cref="ArgumentException">Виникає, якщо нова назва порожня.</exception>
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Нова назва не може бути порожньою.", nameof(newTitle));
        Title = newTitle.Trim();
    }

    /// <summary>
    /// Оновлює автора документа.
    /// </summary>
    /// <param name="newAuthor">Новий автор.</param>
    /// <exception cref="ArgumentException">Виникає, якщо новий автор порожній.</exception>
    public void UpdateAuthor(string newAuthor)
    {
        if (string.IsNullOrWhiteSpace(newAuthor))
            throw new ArgumentException("Новий автор не може бути порожнім.", nameof(newAuthor));
        Author = newAuthor.Trim();
    }

    /// <summary>
    /// Оновлює рік видання документа.
    /// </summary>
    /// <param name="newYear">Новий рік.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо рік поза допустимим діапазоном.</exception>
    public void UpdateYear(int newYear)
    {
        ValidateYear(newYear);
        Year = newYear;
    }

    /// <summary>
    /// Базова реалізація формування рядкового представлення документа.
    /// Може бути розширена в класах-нащадках.
    /// </summary>
    /// <returns>Рядок з основними характеристиками документа.</returns>
    public virtual string ToPrintString()
    {
        return $"[{GetDocumentType()}] \"{Title}\" - {Author}, {Year} р., ID: {Id}";
    }

    /// <summary>
    /// Перевіряє, чи містить документ ключове слово в назві або імені автора.
    /// </summary>
    /// <param name="keyword">Ключове слово для пошуку.</param>
    /// <returns>true, якщо знайдено збіг.</returns>
    public virtual bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        var pattern = keyword.Trim();
        return Title.Contains(pattern, StringComparison.OrdinalIgnoreCase)
            || Author.Contains(pattern, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Допоміжний метод валідації року видання.
    /// </summary>
    /// <param name="year">Рік для перевірки.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо рік поза припустимим діапазоном.</exception>
    private static void ValidateYear(int year)
    {
        const int minYear = 1450;
        var maxYear = DateTime.Now.Year;

        if (year < minYear || year > maxYear)
            throw new ArgumentOutOfRangeException(
                nameof(year),
                $"Рік видання має бути в діапазоні від {minYear} до {maxYear}.");
    }
}
