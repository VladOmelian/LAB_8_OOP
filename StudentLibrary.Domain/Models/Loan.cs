using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;

/// <summary>
/// Клас, що представляє запис у читацькому формулярі - факт видачі конкретного документа конкретному студенту.
/// Демонструє відношення "Асоціація" зі студентом (<see cref="Models.Student"/>) та документом (<see cref="Models.Document"/>),
/// а також знаходиться в композиційному відношенні з бібліотекою (<see cref="Services.Library"/>):
/// видачі створюються та контролюються виключно бібліотекою і не можуть існувати поза нею.
/// </summary>
public class Loan : IPrintable
{
    /// <summary>
    /// Унікальний ідентифікатор видачі.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Студент, якому видано документ.
    /// Демонструє відношення асоціації між <see cref="Loan"/> та <see cref="Models.Student"/>.
    /// </summary>
    public Student Student { get; }

    /// <summary>
    /// Документ, який видано.
    /// Демонструє відношення асоціації між <see cref="Loan"/> та <see cref="Models.Document"/>.
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
    /// Ініціалізує нову видачу. Конструктор internal, оскільки видачі мають створюватися
    /// тільки в межах класу <see cref="Services.Library"/> (контроль композиції).
    /// </summary>
    /// <param name="student">Студент-отримувач.</param>
    /// <param name="document">Документ, що видається.</param>
    /// <exception cref="ArgumentNullException">Виникає, якщо студент або документ є null.</exception>
    internal Loan(Student student, Document document)
    {
        Id = Guid.NewGuid();
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Document = document ?? throw new ArgumentNullException(nameof(document));
        IssueDate = DateTime.Now;
        ReturnDate = null;
    }

    /// <summary>
    /// Фіксує повернення документа, встановлюючи дату повернення.
    /// Викликається тільки бібліотекою-власником.
    /// </summary>
    /// <exception cref="InvalidOperationException">Виникає, якщо документ вже був повернений.</exception>
    internal void MarkAsReturned()
    {
        if (ReturnDate.HasValue)
            throw new InvalidOperationException("Документ вже був повернутий раніше.");
        ReturnDate = DateTime.Now;
    }

    /// <summary>
    /// Формує рядкове представлення видачі.
    /// </summary>
    /// <returns>Опис видачі з усіма атрибутами.</returns>
    public string ToPrintString()
    {
        var status = IsActive
            ? $"активна (видано {IssueDate:dd.MM.yyyy})"
            : $"повернуто {ReturnDate:dd.MM.yyyy}";

        return $"Видача [{status}]: \"{Document.Title}\" -> {Student.GetFullName()} (гр. {Student.AcademicGroup})";
    }
}
