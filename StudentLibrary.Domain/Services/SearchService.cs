using StudentLibrary.Domain.Interfaces;
using StudentLibrary.Domain.Models;

namespace StudentLibrary.Domain.Services;


public class SearchService
{
    // Шукає документи в бібліотеці за ключовим словом (вимога 4.1).
    public IReadOnlyList<Document> SearchDocuments(Library library, string keyword)
    {
        if (library is null)
            throw new ArgumentNullException(nameof(library));

        if (string.IsNullOrWhiteSpace(keyword))
            return Array.Empty<Document>(); //повертає порожній масив (не створює новий об'єкт при кожному виклику).

        return library.GetDocumentsForSearch()
            .Where(d => ((ISearchable)d).MatchesKeyword(keyword)) //фільтрує документи
            .ToList();
    }
    
    // Шукає студентів у бібліотеці за ключовим словом (вимога 4.2).
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
