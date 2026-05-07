using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public class Loan : IPrintable
{
    public Student Student { get; }

    /// <summary>
    /// Документ, який видано.
    /// </summary>
    public Document Document { get; }

    /// <summary>
    /// Дата та час видачі документа.
    /// </summary>
    public DateTime IssueDate { get; }

    /// <summary>
    /// Дата та час повернення документа. null - якщо документ ще не повернутий.
    /// </summary>
    public DateTime? ReturnDate { get; private set; }

    /// <summary>
    /// Ознака, що видача активна (документ ще не повернений).
    /// </summary>
    public bool IsActive => !ReturnDate.HasValue;

    /// <summary>
    /// Ініціалізує нову видачу. Конструктор internal, оскільки видачі мають створюватися тільки в межах класу.
    /// </summary>
    internal Loan(Student student, Document document)
    {
        Guid.NewGuid();
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Document = document ?? throw new ArgumentNullException(nameof(document));
        IssueDate = DateTime.Now;
        ReturnDate = null;
    }

    /// <summary>
    /// Фіксує повернення документа, встановлюючи дату повернення.
    /// </summary>
    internal void MarkAsReturned()
    {
        if (ReturnDate.HasValue)
            throw new InvalidOperationException("Документ вже був повернутий раніше.");
        ReturnDate = DateTime.Now;
    }

    /// <summary>
    /// Формує рядкове представлення видачі.
    /// </summary>
    public string ToPrintString()
    {
        var status = IsActive
            ? $"активна (видано {IssueDate:dd.MM.yyyy})"
            : $"повернуто {ReturnDate:dd.MM.yyyy}";

        return $"Видача [{status}]: \"{Document.Title}\" -> {Student.GetFullName()} (гр. {Student.AcademicGroup})";
    }
}