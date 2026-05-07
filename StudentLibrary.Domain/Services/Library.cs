using StudentLibrary.Domain.Enums;
using StudentLibrary.Domain.Exceptions;
using StudentLibrary.Domain.Models;

namespace StudentLibrary.Domain.Services;

public class Library
{
    public const int MaxActiveLoansPerStudent = 4;

    /// <summary>
    /// Колекція зареєстрованих студентів-користувачів бібліотеки (агрегація).
    /// </summary>
    private readonly List<Student> students;

    /// <summary>
    /// Колекція документів бібліотеки (агрегація).
    /// </summary>
    private readonly List<Document> documents;

    /// <summary>
    /// Колекція видач.
    /// </summary>
    private readonly List<Loan> loans;

    /// <summary>
    /// Ініціалізує нову бібліотеку із заданою назвою.
    /// </summary>
    public Library(string name)
    {
        students = new List<Student>();
        documents = new List<Document>();
        loans = new List<Loan>();
    }


    /// <summary>
    /// Додає нового студента до бібліотеки (вимога 1.1).
    /// </summary>
    public void AddStudent(Student student)
    {
        if (student is null)
            throw new ArgumentNullException(nameof(student));

        if (students.Any(s => s.Id == student.Id))
            throw new LibraryException($"Студент з ідентифікатором {student.Id} вже зареєстрований.");

        students.Add(student);
    }

    /// <summary>
    /// Видаляє студента з бібліотеки за ідентифікатором (вимога 1.2).
    /// </summary>
    public void RemoveStudent(Guid studentId)
    {
        var student = FindStudentOrThrow(studentId);

        if (loans.Any(l => l.Student.Id == studentId && l.IsActive))
            throw new EntityInUseException("студента", "у нього є активні (неповернуті) видачі.");

        students.Remove(student);
    }

    /// <summary>
    /// Повертає список всіх студентів бібліотеки з можливістю сортування (вимоги 1.5, 1.5.1 - 1.5.3).
    /// </summary>
    public IReadOnlyList<Student> GetAllStudents(StudentSortCriterion sortCriterion = StudentSortCriterion.ByLastName)
    {
        return sortCriterion switch
        {
            StudentSortCriterion.ByFirstName => students.OrderBy(s => s.FirstName).ToList(),
            StudentSortCriterion.ByLastName => students.OrderBy(s => s.LastName).ToList(),
            StudentSortCriterion.ByAcademicGroup => students.OrderBy(s => s.AcademicGroup).ToList(),
            _ => students.ToList()
        };
    }

    /// <summary>
    /// Додає новий документ до бібліотеки (вимога 2.1).
    /// </summary>
    public void AddDocument(Document document)
    {
        if (document is null)
            throw new ArgumentNullException(nameof(document));

        if (documents.Any(d => d.Id == document.Id))
            throw new LibraryException($"Документ з ідентифікатором {document.Id} вже існує в бібліотеці.");

        documents.Add(document);
    }

    /// <summary>
    /// Видаляє документ з бібліотеки (вимога 2.2).
    /// </summary>
    public void RemoveDocument(Guid documentId)
    {
        var document = FindDocumentOrThrow(documentId);

        if (loans.Any(l => l.Document.Id == documentId && l.IsActive))
            throw new EntityInUseException("документ", "наразі він виданий читачеві.");

        documents.Remove(document);
    }

    /// <summary>
    /// Повертає список всіх документів з можливістю сортування (вимоги 2.5, 2.5.1, 2.5.2).
    /// </summary>
    public IReadOnlyList<Document> GetAllDocuments(DocumentSortCriterion sortCriterion = DocumentSortCriterion.ByTitle)
    {
        return sortCriterion switch
        {
            DocumentSortCriterion.ByTitle => documents.OrderBy(d => d.Title).ToList(),
            DocumentSortCriterion.ByAuthor => documents.OrderBy(d => d.Author).ToList(),
            _ => documents.ToList()
        };
    }


    /// <summary>
    /// Видає документ студенту (вимога 3.1).
    /// </summary>
    public Loan IssueDocument(Guid studentId, Guid documentId)
    {
        var student = FindStudentOrThrow(studentId);
        var document = FindDocumentOrThrow(documentId);

        var activeLoansForStudent = loans.Count(l => l.Student.Id == studentId && l.IsActive);
        if (activeLoansForStudent >= MaxActiveLoansPerStudent)
            throw new LoanLimitExceededException(student.GetFullName(), MaxActiveLoansPerStudent);

        if (loans.Any(l => l.Document.Id == documentId && l.IsActive))
            throw new DocumentNotAvailableException(document.Title);

        var loan = new Loan(student, document);
        loans.Add(loan);
        return loan;
    }

    /// <summary>
    /// Повертає список документів, наразі взятих конкретним студентом (вимога 3.2).
    /// </summary>
    public IReadOnlyList<Document> GetDocumentsHeldByStudent(Guid studentId)
    {
        FindStudentOrThrow(studentId);

        return loans
            .Where(l => l.Student.Id == studentId && l.IsActive)
            .Select(l => l.Document)
            .ToList();
    }

    /// <summary>
    /// Визначає, чи знаходиться документ у бібліотеці, та хто його взяв, якщо виданий (вимога 3.3).
    /// </summary>
    public (bool IsInLibrary, Student? CurrentHolder) GetDocumentStatus(Guid documentId)
    {
        FindDocumentOrThrow(documentId);

        var activeLoan = loans.FirstOrDefault(l => l.Document.Id == documentId && l.IsActive);
        return activeLoan is null
            ? (true, null)
            : (false, activeLoan.Student);
    }

    /// <summary>
    /// Повертає документ до бібліотеки (вимога 3.4).
    /// </summary>
    public void ReturnDocument(Guid studentId, Guid documentId)
    {
        var loan = loans.FirstOrDefault(l =>
            l.Student.Id == studentId
            && l.Document.Id == documentId
            && l.IsActive);

        if (loan is null)
            throw new LibraryException(
                "Активна видача для зазначеного студента та документа не знайдена.");

        loan.MarkAsReturned();
    }

    /// <summary>
    /// Повертає всі активні видачі.
    /// </summary>
    public IReadOnlyList<Loan> GetActiveLoans() =>
        loans.Where(l => l.IsActive).ToList();

    /// <summary>
    /// Знаходить студента за ID або кидає виняток.
    /// </summary>
    private Student FindStudentOrThrow(Guid studentId)
    {
        return students.FirstOrDefault(s => s.Id == studentId)
            ?? throw new EntityNotFoundException("Студент", studentId);
    }

    /// <summary>
    /// Знаходить документ за ID або кидає виняток.
    /// </summary>
    private Document FindDocumentOrThrow(Guid documentId)
    {
        return documents.FirstOrDefault(d => d.Id == documentId)
            ?? throw new EntityNotFoundException("Документ", documentId);
    }

    /// <summary>
    /// Внутрішній доступ до колекції документів для сервісів-залежностей.
    /// </summary>
    internal IEnumerable<Document> GetDocumentsForSearch() => documents;

    /// <summary>
    /// Внутрішній доступ до колекції студентів для сервісів-залежностей.
    /// </summary>
    internal IEnumerable<Student> GetStudentsForSearch() => students;
}