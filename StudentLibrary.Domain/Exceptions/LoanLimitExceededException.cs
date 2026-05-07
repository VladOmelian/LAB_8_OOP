namespace StudentLibrary.Domain.Exceptions;

/// Виняток, що виникає у випадку, коли студент намагається отримати документ
/// при наявності у нього вже максимально дозволеної кількості видач (n &lt; 5).
public class LoanLimitExceededException : LibraryException
{
    public LoanLimitExceededException(string studentFullName, int limit)
        : base($"Студент \"{studentFullName}\" вже має {limit} активних видач. Перевищення максимально дозволеного ліміту неможливе.") { }
}
