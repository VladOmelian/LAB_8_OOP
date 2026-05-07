using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public abstract class Person : IPrintable, ISearchable
{
    /// <summary>
    /// Унікальний ідентифікатор особи.
    /// </summary>
    public Guid Id { get; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    /// <summary>
    /// Ініціалізує нову особу із заданими ім'ям та прізвищем.
    /// </summary>
    protected Person(string firstName, string lastName)
    {
        ValidateName(firstName, nameof(firstName));
        ValidateName(lastName, nameof(lastName));

        Id = Guid.NewGuid();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    /// <summary>
    /// Повертає повне ім'я особи у форматі "Прізвище Ім'я".
    /// </summary>
    public string GetFullName() => $"{LastName} {FirstName}";

    /// <summary>
    /// Оновлює ім'я особи з валідацією вхідних даних.
    /// </summary>
    public void UpdateFirstName(string newFirstName)
    {
        ValidateName(newFirstName, nameof(newFirstName));
        FirstName = newFirstName.Trim();
    }

    /// <summary>
    /// Оновлює прізвище особи з валідацією вхідних даних.
    /// </summary>
    public void UpdateLastName(string newLastName)
    {
        ValidateName(newLastName, nameof(newLastName));
        LastName = newLastName.Trim();
    }

    /// <summary>
    /// Абстрактний метод формування рядкового представлення особи.
    /// </summary>
    public abstract string ToPrintString();

    /// <summary>
    /// Перевіряє, чи відповідає особа заданому ключовому слову.
    /// Реалізація може бути доповнена у класах-нащадках.
    /// </summary>
    public virtual bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        var pattern = keyword.Trim();
        return FirstName.Contains(pattern, StringComparison.OrdinalIgnoreCase)
            || LastName.Contains(pattern, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Допоміжний метод валідації імені/прізвища.
    /// </summary>
    private static void ValidateName(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значення не може бути порожнім.", parameterName);
    }
}