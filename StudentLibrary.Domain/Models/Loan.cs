using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public class Loan : IPrintable
{
    public Student Student { get; }
    public Document Document { get; }
    
    // Дата та час видачі документа.
    public DateTime IssueDate { get; }
    
    // Дата та час повернення документа. null - якщо документ ще не повернутий.
    public DateTime? ReturnDate { get; private set; }
    
    // Ознака, що видача активна (документ ще не повернений).
    public bool IsActive => !ReturnDate.HasValue;
    
    // Ініціалізує нову видачу. Конструктор internal, оскільки видачі мають створюватися тільки в межах класу.
    internal Loan(Student student, Document document)
    {
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Document = document ?? throw new ArgumentNullException(nameof(document));
        IssueDate = DateTime.Now;
        ReturnDate = null;
    }
    
    // Фіксує повернення документа, встановлюючи дату повернення.
    internal void MarkAsReturned()
    {
        if (ReturnDate.HasValue)
            throw new InvalidOperationException("Документ вже був повернутий раніше.");
        ReturnDate = DateTime.Now;
    }
    
    
    // Формує рядкове представлення видачі.
    public string ToPrintString()
    {
        var status = IsActive
            ? $"активна (видано {IssueDate:dd.MM.yyyy})"
            : $"повернуто {ReturnDate:dd.MM.yyyy}";

        return $"Видача [{status}]: \"{Document.Title}\" -> {Student.GetFullName()} (гр. {Student.AcademicGroup})";
    }
}
