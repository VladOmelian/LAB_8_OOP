using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public abstract class Document : IPrintable, ISearchable
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    
    // Рік видання документа.
    public int Year { get; private set; }


    // Ініціалізує новий документ із заданими атрибутами.
    protected Document(string title, string author, int year) //protected — конструктор можна викликати лише з класів-нащадків (через base()), але не ззовні.
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


    // Повертає тип документа у вигляді рядка.
    // Має бути реалізований класами-нащадками (поліморфізм).
    public abstract string GetDocumentType();
    
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Нова назва не може бути порожньою.", nameof(newTitle));
        Title = newTitle.Trim();
    }
    
    
    public void UpdateAuthor(string newAuthor)
    {
        if (string.IsNullOrWhiteSpace(newAuthor))
            throw new ArgumentException("Новий автор не може бути порожнім.", nameof(newAuthor));
        Author = newAuthor.Trim();
    }
    
    
    public void UpdateYear(int newYear)
    {
        ValidateYear(newYear);
        Year = newYear;
    }

    // Реалізація формування рядкового представлення документа.
    public virtual string ToPrintString()
    {
        return $"[{GetDocumentType()}] \"{Title}\" - {Author}, {Year} р., ID: {Id}";
    }


    // Перевіряє, чи містить документ ключове слово в назві або імені автора.
    public virtual bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        var pattern = keyword.Trim();
        return Title.Contains(pattern, StringComparison.OrdinalIgnoreCase) //StringComparison.OrdinalIgnoreCase робить пошук нечутливим до регістру
            || Author.Contains(pattern, StringComparison.OrdinalIgnoreCase);
    }

    
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
