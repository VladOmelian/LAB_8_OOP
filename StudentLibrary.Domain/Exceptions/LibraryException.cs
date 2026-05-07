namespace StudentLibrary.Domain.Exceptions;

// Базовий клас для всіх винятків предметної області бібліотеки.
// Демонструє наслідування на рівні системи виключень.
public class LibraryException(string message) : Exception(message);
