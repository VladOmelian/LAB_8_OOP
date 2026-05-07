namespace StudentLibrary.Domain.Exceptions;

/// Виняток, що виникає у випадку спроби видачі документа, який вже видано іншому користувачеві.
public class DocumentNotAvailableException : LibraryException
{
    public DocumentNotAvailableException(string documentTitle)
        : base($"Документ \"{documentTitle}\" наразі видано іншому користувачеві та недоступний для видачі.")
    {
    }
}
