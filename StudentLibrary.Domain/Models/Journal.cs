namespace StudentLibrary.Domain.Models;


public class Journal : Document
{
    // Номер випуску журналу.
    public int IssueNumber { get; private set; }
    
    // Том (volume) журналу.
    public int Volume { get; private set; }
    
    // Ініціалізує новий журнал.
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

    
    public void UpdateVolume(int newVolume)
    {
        if (newVolume <= 0)
            throw new ArgumentOutOfRangeException(nameof(newVolume), "Том має бути додатним числом.");
        Volume = newVolume;
    }

    
    public void UpdateIssueNumber(int newIssueNumber)
    {
        if (newIssueNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(newIssueNumber), "Номер випуску має бути додатним числом.");
        IssueNumber = newIssueNumber;
    }


    // Розширене рядкове представлення з врахуванням специфічних атрибутів журналу.
    public override string ToPrintString()
    {
        return $"{base.ToPrintString()}, том {Volume}, випуск №{IssueNumber}";
    }
}
