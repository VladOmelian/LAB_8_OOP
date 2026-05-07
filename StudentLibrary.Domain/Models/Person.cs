using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;


public abstract class Person : IPrintable, ISearchable
{
    // Унікальний ідентифікатор особи. Незмінний після створення (інкапсуляція).
    public Guid Id { get; }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    // Ініціалізує нову особу із заданими ім'ям та прізвищем.
    protected Person(string firstName, string lastName)
    {
        ValidateName(firstName, nameof(firstName));
        ValidateName(lastName, nameof(lastName));

        Id = Guid.NewGuid();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }
    
    // Повертає повне ім'я особи
    public string GetFullName() => $"{LastName} {FirstName}";


    // Оновлює ім'я особи з валідацією вхідних даних.
    public void UpdateFirstName(string newFirstName)
    {
        ValidateName(newFirstName, nameof(newFirstName));
        FirstName = newFirstName.Trim();
    }


    // Оновлює прізвище особи з валідацією вхідних даних.
    public void UpdateLastName(string newLastName)
    {
        ValidateName(newLastName, nameof(newLastName));
        LastName = newLastName.Trim();
    }


    // Абстрактний метод формування рядкового представлення особи.
    public abstract string ToPrintString();
    
    // Перевіряє, чи відповідає особа заданому ключовому слову.
    // Реалізація може бути доповнена у класах-нащадках.
    public virtual bool MatchesKeyword(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        var pattern = keyword.Trim();
        return FirstName.Contains(pattern, StringComparison.OrdinalIgnoreCase)
            || LastName.Contains(pattern, StringComparison.OrdinalIgnoreCase);
    }


    // Метод валідації імені/прізвища.
    private static void ValidateName(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значення не може бути порожнім.", parameterName);
    }
}
