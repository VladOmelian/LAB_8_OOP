namespace StudentLibrary.Domain.Exceptions;

public class LoanLimitExceededException(string studentFullName, int limit) : LibraryException(
    $"Студент \"{studentFullName}\" вже має {limit} активних видач. Перевищення максимально дозволеного ліміту неможливе.");
