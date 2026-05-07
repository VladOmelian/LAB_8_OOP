namespace StudentLibrary.Domain.Models;

/// <summary>
/// Клас, що представляє науковий журнал як конкретний тип документа.
/// Демонструє відношення "Узагальнення" (наслідування) з абстрактним класом <see cref="Document"/>.
/// </summary>
public class Journal : Document
{
    /// <summary>
    /// Номер випуску журналу.
    /// </summary>
    public int IssueNumber { get; private set; }

    /// <summary>
    /// Том (volume) журналу.
    /// </summary>
    public int Volume { get; private set; }

    /// <summary>
    /// Ініціалізує новий журнал.
    /// </summary>
    /// <param name="title">Назва журналу.</param>
    /// <param name="author">Головний редактор або відповідальний автор.</param>
    /// <param name="year">Рік випуску.</param>
    /// <param name="volume">Том.</param>
    /// <param name="issueNumber">Номер випуску.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо том або номер не додатні.</exception>
    public Journal(string title, string author, int year, int volume, int issueNumber)
        : base(title, author, year)
    {
        if (volume <= 0)
            throw new ArgumentOutOfRangeException(nameof(volume), "Том має бути додатним числом.");
        if (issueNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(issueNumber), "Номер випуску має бути додатним числом.");

        Volume = volume;
        IssueNumber = issueNumber;
    }

    /// <summary>
    /// Повертає тип документа.
    /// </summary>
    /// <returns>Рядок "Журнал".</returns>
    public override string GetDocumentType() => "Журнал";

    /// <summary>
    /// Оновлює том журналу.
    /// </summary>
    /// <param name="newVolume">Новий том.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо значення не додатне.</exception>
    public void UpdateVolume(int newVolume)
    {
        if (newVolume <= 0)
            throw new ArgumentOutOfRangeException(nameof(newVolume), "Том має бути додатним числом.");
        Volume = newVolume;
    }

    /// <summary>
    /// Оновлює номер випуску журналу.
    /// </summary>
    /// <param name="newIssueNumber">Новий номер випуску.</param>
    /// <exception cref="ArgumentOutOfRangeException">Виникає, якщо значення не додатне.</exception>
    public void UpdateIssueNumber(int newIssueNumber)
    {
        if (newIssueNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(newIssueNumber), "Номер випуску має бути додатним числом.");
        IssueNumber = newIssueNumber;
    }

    /// <summary>
    /// Розширене рядкове представлення з врахуванням специфічних атрибутів журналу.
    /// </summary>
    /// <returns>Детальна інформація про журнал.</returns>
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, том {Volume}, випуск №{IssueNumber}";
    }
}
