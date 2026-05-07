namespace StudentLibrary.Domain.Exceptions;

// Виняток, що виникає у випадку, коли студент намагається отримати документ
// при наявності у нього вже максимально дозволеної кількості видач (n &lt; 5).
public class LoanLimitExceededException(string studentFullName, int limit) : LibraryException(
    $"Студент \"{studentFullName}\" вже має {limit} активних видач. Перевищення максимально дозволеного ліміту неможливе.");
