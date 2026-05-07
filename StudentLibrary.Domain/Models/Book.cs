namespace StudentLibrary.Domain.Models;


public class Book : Document
{
    public int PageCount { get; private set; }
    
    // Видавництво.
    public string Publisher { get; private set; }


    // Ініціалізує нову книгу.
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
    
    // Повертає тип документа.
    public override string GetDocumentType() => "Книга";
    
    
    public void UpdatePageCount(int newPageCount)
    {
        if (newPageCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(newPageCount), "Кількість сторінок має бути додатною.");
        PageCount = newPageCount;
    }

    
    public void UpdatePublisher(string newPublisher)
    {
        if (string.IsNullOrWhiteSpace(newPublisher))
            throw new ArgumentException("Видавництво не може бути порожнім.", nameof(newPublisher));
        Publisher = newPublisher.Trim();
    }
    
    // Викликає батьківську реалізацію (з типом, назвою, автором, роком, ID), а потім дописує специфічні для книги дані.
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, видавництво: {Publisher}, сторінок: {PageCount}";
    }


    // Розширений пошук, що додатково шукає по назві видавництва.
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
            && Publisher.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
