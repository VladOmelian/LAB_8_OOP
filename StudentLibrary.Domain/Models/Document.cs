using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public abstract class Document : IPrintable, ISearchable
{
    /// <summary>
    /// Унікальний ідентифікатор документа.
    /// </summary>
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Author { get; private set; }

    /// <summary>
    /// Рік видання документа.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Ініціалізує новий документ із заданими атрибутами.
    /// </summary>
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
    /// Повертає тип документа у вигляді рядка.
    /// </summary>
    public abstract string GetDocumentType();

    /// <summary>
    /// Оновлює назву документа.
    /// </summary>
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Нова назва не може бути порожньою.", nameof(newTitle));
        Title = newTitle.Trim();
    }

    /// <summary>
    /// Оновлює автора документа.
    /// </summary>
    public void UpdateAuthor(string newAuthor)
    {
        if (string.IsNullOrWhiteSpace(newAuthor))
            throw new ArgumentException("Новий автор не може бути порожнім.", nameof(newAuthor));
        Author = newAuthor.Trim();
    }

    /// <summary>
    /// Оновлює рік видання документа.
    /// </summary>
    public void UpdateYear(int newYear)
    {
        ValidateYear(newYear);
        Year = newYear;
    }

    /// <summary>
    /// Реалізація формування рядкового представлення документа.
    /// </summary>
    public virtual string ToPrintString()
    {
        return $"[{GetDocumentType()}] \"{Title}\" - {Author}, {Year} р., ID: {Id}";
    }

    /// <summary>
    /// Перевіряє, чи містить документ ключове слово в назві або імені автора.
    /// </summary>
    public virtual bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        var pattern = keyword.Trim();
        return Title.Contains(pattern, StringComparison.OrdinalIgnoreCase)
            || Author.Contains(pattern, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Метод валідації року видання.
    /// </summary>
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