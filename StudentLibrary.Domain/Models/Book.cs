namespace StudentLibrary.Domain.Models;

/// <summary>
/// Клас, що представляє книгу як конкретний тип документа.
/// Демонструє відношення "Узагальнення" (наслідування) з абстрактним класом <see cref="Document"/>.
/// </summary>
public class Book : Document
{
    /// <summary>
    /// Кількість сторінок у книзі.
    /// </summary>
    public int PageCount { get; private set; }

    /// <summary>
    /// Видавництво.
    /// </summary>
    public string Publisher { get; private set; }

    /// <summary>
    /// Ініціалізує нову книгу.
    /// </summary>
    /// <param name="title">Назва книги.</param>
    /// <param name="author">Автор книги.</param>
    /// <param name="year">Рік видання.</param>
    /// <param name="pageCount">Кількість сторінок.</param>
    /// <param name="publisher">Видавництво.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо кількість сторінок не додатна.</exception>
    /// <exception cref="ArgumentException">Виникає, якщо видавництво порожнє.</exception>
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
    /// <returns>Рядок "Книга".</returns>
    public override string GetDocumentType() => "Книга";

    /// <summary>
    /// Оновлює кількість сторінок.
    /// </summary>
    /// <param name="newPageCount">Нова кількість сторінок.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо значення не додатне.</exception>
    public void UpdatePageCount(int newPageCount)
    {
        if (newPageCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(newPageCount), "Кількість сторінок має бути додатною.");
        PageCount = newPageCount;
    }

    /// <summary>
    /// Оновлює видавництво.
    /// </summary>
    /// <param name="newPublisher">Нове видавництво.</param>
    /// <exception cref="ArgumentException">Виникає, якщо видавництво порожнє.</exception>
    public void UpdatePublisher(string newPublisher)
    {
        if (string.IsNullOrWhiteSpace(newPublisher))
            throw new ArgumentException("Видавництво не може бути порожнім.", nameof(newPublisher));
        Publisher = newPublisher.Trim();
    }

    /// <summary>
    /// Розширене рядкове представлення з врахуванням специфічних атрибутів книги.
    /// </summary>
    /// <returns>Детальна інформація про книгу.</returns>
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, видавництво: {Publisher}, сторінок: {PageCount}";
    }

    /// <summary>
    /// Розширений пошук, що додатково шукає по назві видавництва.
    /// </summary>
    /// <param name="keyword">Ключове слово.</param>
    /// <returns>true, якщо є збіг по будь-якому атрибуту.</returns>
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
            && Publisher.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
