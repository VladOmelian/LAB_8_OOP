namespace StudentLibrary.Domain.Models;


public class Book : Document
{
    public int PageCount { get; private set; }
    public string Publisher { get; private set; }

    /// <summary>
    /// Ініціалізує нову книгу.
    /// </summary>
    public Book(string title, string author, int year, int pageCount, string publisher)
        : base(title, author, year)
    {
        if (pageCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageCount), "Кількість сторінок має бути додатною.");
        if (string.IsNullOrWhiteSpace(publisher))
            throw new ArgumentException("Видавництво не може бути порожнім.", nameof(publisher));

        PageCount = pageCount;
        Publisher = publisher.Trim();
    }

    /// <summary>
    /// Повертає тип документа.
    /// </summary>
    public override string GetDocumentType() => "Книга";

    /// <summary>
    /// Оновлює кількість сторінок.
    /// </summary>
    public void UpdatePageCount(int newPageCount)
    {
        if (newPageCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(newPageCount), "Кількість сторінок має бути додатною.");
        PageCount = newPageCount;
    }

    /// <summary>
    /// Оновлює видавництво.
    /// </summary>
    public void UpdatePublisher(string newPublisher)
    {
        if (string.IsNullOrWhiteSpace(newPublisher))
            throw new ArgumentException("Видавництво не може бути порожнім.", nameof(newPublisher));
        Publisher = newPublisher.Trim();
    }

    /// <summary>
    /// Розширене рядкове представлення з врахуванням специфічних атрибутів книги.
    /// </summary>
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, видавництво: {Publisher}, сторінок: {PageCount}";
    }

    /// <summary>
    /// Розширений пошук, що додатково шукає по назві видавництва.
    /// </summary>
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
            && Publisher.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}