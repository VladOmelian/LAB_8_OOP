using StudentLibrary.Domain.Interfaces;

namespace StudentLibrary.Domain.Models;

/// <summary>
/// Абстрактний клас, що представляє особу.
/// Демонструє принципи абстракції та інкапсуляції, а також є базою для відношення
/// "Узагальнення" (наслідування) з класом <see cref="Student"/>.
/// </summary>
public abstract class Person : IPrintable, ISearchable
{
    /// <summary>
    /// Унікальний ідентифікатор особи. Незмінний після створення (інкапсуляція).
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Ім'я особи.
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Прізвище особи.
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Ініціалізує нову особу із заданими ім'ям та прізвищем.
    /// </summary>
    /// <param name="firstName">Ім'я особи.</param>
    /// <param name="lastName">Прізвище особи.</param>
    /// <exception cref="ArgumentException">Виникає, якщо ім'я або прізвище є null чи порожнім рядком.</exception>
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
    /// <returns>Рядок з повним ім'ям.</returns>
    public string GetFullName() => $"{LastName} {FirstName}";

    /// <summary>
    /// Оновлює ім'я особи з валідацією вхідних даних.
    /// </summary>
    /// <param name="newFirstName">Нове ім'я.</param>
    /// <exception cref="ArgumentException">Виникає, якщо нове ім'я порожнє.</exception>
    public void UpdateFirstName(string newFirstName)
    {
        ValidateName(newFirstName, nameof(newFirstName));
        FirstName = newFirstName.Trim();
    }

    /// <summary>
    /// Оновлює прізвище особи з валідацією вхідних даних.
    /// </summary>
    /// <param name="newLastName">Нове прізвище.</param>
    /// <exception cref="ArgumentException">Виникає, якщо нове прізвище порожнє.</exception>
    public void UpdateLastName(string newLastName)
    {
        ValidateName(newLastName, nameof(newLastName));
        LastName = newLastName.Trim();
    }

    /// <summary>
    /// Абстрактний метод формування рядкового представлення особи.
    /// Має бути перевизначений у класах-нащадках (поліморфізм).
    /// </summary>
    /// <returns>Детальний опис особи.</returns>
    public abstract string ToPrintString();

    /// <summary>
    /// Перевіряє, чи відповідає особа заданому ключовому слову.
    /// Реалізація може бути доповнена у класах-нащадках.
    /// </summary>
    /// <param name="keyword">Ключове слово для пошуку.</param>
    /// <returns>true, якщо ім'я або прізвище містять ключове слово.</returns>
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
    /// <param name="value">Значення, що перевіряється.</param>
    /// <param name="parameterName">Ім'я параметра для повідомлення про помилку.</param>
    /// <exception cref="ArgumentException">Виникає, якщо значення null або порожнє.</exception>
    private static void ValidateName(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значення не може бути порожнім.", parameterName);
    }
}
