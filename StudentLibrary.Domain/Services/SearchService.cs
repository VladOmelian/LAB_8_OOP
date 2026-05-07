using StudentLibrary.Domain.Models;

namespace StudentLibrary.Domain.Services;

/// Сервіс пошуку по бібліотеці.
public class SearchService
{
    /// Шукає документи в бібліотеці за ключовим словом (вимога 4.1).
    public IReadOnlyList<Document> SearchDocuments(Library library, string keyword)
    {
        if (library is null)
            throw new ArgumentNullException(nameof(library));

        if (string.IsNullOrWhiteSpace(keyword))
            return Array.Empty<Document>();

        return library.GetDocumentsForSearch()
            .Where(d => d.MatchesKeyword(keyword))
            .ToList();
    }
    
    /// Шукає студентів у бібліотеці за ключовим словом (вимога 4.2).
    public IReadOnlyList<Student> SearchStudents(Library library, string keyword)
    {
        if (library is null)
            throw new ArgumentNullException(nameof(library));

        if (string.IsNullOrWhiteSpace(keyword))
            return Array.Empty<Student>();

        return library.GetStudentsForSearch()
            .Where(s => s.MatchesKeyword(keyword))
            .ToList();
    }
}
