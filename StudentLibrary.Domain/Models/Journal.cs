namespace StudentLibrary.Domain.Models;


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


    public override string GetDocumentType() => "Журнал";

    /// <summary>
    /// Оновлює том журналу.
    /// </summary>
    public void UpdateVolume(int newVolume)
    {
        if (newVolume <= 0)
            throw new ArgumentOutOfRangeException(nameof(newVolume), "Том має бути додатним числом.");
        Volume = newVolume;
    }

    /// <summary>
    /// Оновлює номер випуску журналу.
    /// </summary>
    public void UpdateIssueNumber(int newIssueNumber)
    {
        if (newIssueNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(newIssueNumber), "Номер випуску має бути додатним числом.");
        IssueNumber = newIssueNumber;
    }

    /// <summary>
    /// Розширене рядкове представлення з врахуванням специфічних атрибутів журналу.
    /// </summary>
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, том {Volume}, випуск №{IssueNumber}";
    }
}