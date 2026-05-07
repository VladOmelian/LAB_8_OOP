namespace StudentLibrary.Domain.Exceptions;

public class DocumentNotAvailableException(string documentTitle)
    : LibraryException($"Документ \"{documentTitle}\" наразі видано іншому користувачеві та недоступний для видачі.");
