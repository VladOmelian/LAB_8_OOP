using StudentLibrary.Domain.Enums;
using StudentLibrary.Domain.Exceptions;
using StudentLibrary.Domain.Models;

namespace StudentLibrary.Domain.Services;

public class Library
{
    /// Максимально допустима кількість одночасних видач документів одному студенту (n &lt; 5, тобто n = 4).
    public const int MaxActiveLoansPerStudent = 4;
    
    /// Колекція зареєстрованих студентів-користувачів бібліотеки (агрегація).
    private readonly List<Student> students;
    
    /// Колекція документів (літератури) бібліотеки (агрегація).
    private readonly List<Document> documents;
    
    /// Колекція видач (читацьких формулярів). Композиція - бібліотека контролює їхнє створення та знищення.
    private readonly List<Loan> loans;
    
    /// Назва бібліотеки.
    public string Name { get; }


    /// Ініціалізує нову бібліотеку із заданою назвою.
    public Library(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва бібліотеки не може бути порожньою.", nameof(name));

        Name = name.Trim();
        students = new List<Student>();
        documents = new List<Document>();
        loans = new List<Loan>();
    }
    
    
    /// Додає нового студента до бібліотеки (вимога 1.1).
    public void AddStudent(Student student)
    {
        if (student is null)
            throw new ArgumentNullException(nameof(student));

        if (students.Any(s => s.Id == student.Id))
            throw new LibraryException($"Студент з ідентифікатором {student.Id} вже зареєстрований.");

        students.Add(student);
    }
    
    /// Видаляє студента з бібліотеки за ідентифікатором (вимога 1.2).
    public void RemoveStudent(Guid studentId)
    {
        var student = FindStudentOrThrow(studentId);

        if (loans.Any(l => l.Student.Id == studentId && l.IsActive))
            throw new EntityInUseException("студента", "у нього є активні (неповернуті) видачі.");

        students.Remove(student);
    }
    
    /// Оновлює дані студента: ім'я, прізвище та академічну групу (вимога 1.3).
    public void UpdateStudent(Guid studentId, string newFirstName, string newLastName, string newAcademicGroup)
    {
        var student = FindStudentOrThrow(studentId);
        student.UpdateFirstName(newFirstName);
        student.UpdateLastName(newLastName);
        student.UpdateAcademicGroup(newAcademicGroup);
    }
    
    /// Повертає інформацію про конкретного студента (вимога 1.4).
    public Student GetStudent(Guid studentId) => FindStudentOrThrow(studentId);
    
    /// Повертає список всіх студентів бібліотеки з можливістю сортування (вимоги 1.5, 1.5.1 - 1.5.3).
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
    
    /// Додає новий документ до бібліотеки (вимога 2.1).
    public void AddDocument(Document document)
    {
        if (document is null)
            throw new ArgumentNullException(nameof(document));

        if (documents.Any(d => d.Id == document.Id))
            throw new LibraryException($"Документ з ідентифікатором {document.Id} вже існує в бібліотеці.");

        documents.Add(document);
    }
    
    /// Видаляє документ з бібліотеки (вимога 2.2).
    public void RemoveDocument(Guid documentId)
    {
        var document = FindDocumentOrThrow(documentId);

        if (loans.Any(l => l.Document.Id == documentId && l.IsActive))
            throw new EntityInUseException("документ", "наразі він виданий читачеві.");

        documents.Remove(document);
    }
    
    /// Оновлює основні дані документа (вимога 2.3).
    public void UpdateDocument(Guid documentId, string newTitle, string newAuthor, int newYear)
    {
        var document = FindDocumentOrThrow(documentId);
        document.UpdateTitle(newTitle);
        document.UpdateAuthor(newAuthor);
        document.UpdateYear(newYear);
    }
    
    /// Повертає інформацію про конкретний документ (вимога 2.4).
    public Document GetDocument(Guid documentId) => FindDocumentOrThrow(documentId);
    
    /// Повертає список всіх документів з можливістю сортування (вимоги 2.5, 2.5.1, 2.5.2).
    public IReadOnlyList<Document> GetAllDocuments(DocumentSortCriterion sortCriterion = DocumentSortCriterion.ByTitle)
    {
        return sortCriterion switch
        {
            DocumentSortCriterion.ByTitle => documents.OrderBy(d => d.Title).ToList(),
            DocumentSortCriterion.ByAuthor => documents.OrderBy(d => d.Author).ToList(),
            _ => documents.ToList()
        };
    }

    
    /// Видає документ студенту (вимога 3.1).
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
    
    /// Повертає список документів, наразі взятих конкретним студентом (вимога 3.2).
    public IReadOnlyList<Document> GetDocumentsHeldByStudent(Guid studentId)
    {
        FindStudentOrThrow(studentId);

        return loans
            .Where(l => l.Student.Id == studentId && l.IsActive)
            .Select(l => l.Document)
            .ToList();
    }
    
    /// Визначає, чи знаходиться документ у бібліотеці, та хто його взяв, якщо виданий (вимога 3.3).
    public (bool IsInLibrary, Student? CurrentHolder) GetDocumentStatus(Guid documentId)
    {
        FindDocumentOrThrow(documentId);

        var activeLoan = loans.FirstOrDefault(l => l.Document.Id == documentId && l.IsActive);
        return activeLoan is null
            ? (true, null)
            : (false, activeLoan.Student);
    }


    /// Повертає документ до бібліотеки (вимога 3.4).
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


    /// Повертає всі активні видачі (для звітності або відображення).
    public IReadOnlyList<Loan> GetActiveLoans() =>
        loans.Where(l => l.IsActive).ToList();
    
    
    /// Знаходить студента за ID або кидає виняток.
    private Student FindStudentOrThrow(Guid studentId)
    {
        return students.FirstOrDefault(s => s.Id == studentId)
            ?? throw new EntityNotFoundException("Студент", studentId);
    }
    
    /// Знаходить документ за ID або кидає виняток.
    private Document FindDocumentOrThrow(Guid documentId)
    {
        return documents.FirstOrDefault(d => d.Id == documentId)
            ?? throw new EntityNotFoundException("Документ", documentId);
    }
    
    /// Внутрішній доступ до колекції документів для сервісів-залежностей.
    internal IEnumerable<Document> GetDocumentsForSearch() => documents;
    
    /// Внутрішній доступ до колекції студентів для сервісів-залежностей.
    internal IEnumerable<Student> GetStudentsForSearch() => students;
}
