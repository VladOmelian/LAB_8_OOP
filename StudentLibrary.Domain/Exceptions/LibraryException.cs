namespace StudentLibrary.Domain.Exceptions;

/// Базовий клас для всіх винятків предметної області бібліотеки.
/// Демонструє узагальнення (наслідування) на рівні системи виключень.
public class LibraryException : Exception
{
    public LibraryException(string message) : base(message) { }
    
    public LibraryException(string message, Exception innerException) : base(message, innerException) { }
}
