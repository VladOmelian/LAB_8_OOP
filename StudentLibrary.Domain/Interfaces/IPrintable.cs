namespace StudentLibrary.Domain.Interfaces;

/// Інтерфейс для об'єктів, які можуть виводити інформацію про себе у строковому вигляді.
/// Використовується для демонстрації відношення "Реалізація" між інтерфейсом та класами.
public interface IPrintable
{
    string ToPrintString();
}
