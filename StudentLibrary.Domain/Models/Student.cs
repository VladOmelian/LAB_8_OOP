namespace StudentLibrary.Domain.Models;

/// <summary>
/// Клас, що представляє студента-користувача бібліотеки.
/// Є нащадком абстрактного класу <see cref="Person"/> (відношення "Узагальнення/Наслідування" - "is-a").
/// Студент є особою з додатковою характеристикою "академічна група".
/// </summary>
public class Student : Person
{
    /// <summary>
    /// Академічна група студента.
    /// </summary>
    public string AcademicGroup { get; private set; }

    /// <summary>
    /// Ініціалізує нового студента із заданими ім'ям, прізвищем та академічною групою.
    /// </summary>
    /// <param name="firstName">Ім'я студента.</param>
    /// <param name="lastName">Прізвище студента.</param>
    /// <param name="academicGroup">Академічна група студента.</param>
    /// <exception cref="ArgumentException">Виникає, якщо будь-який параметр порожній.</exception>
    public Student(string firstName, string lastName, string academicGroup)
        : base(firstName, lastName)
    {
        if (string.IsNullOrWhiteSpace(academicGroup))
            throw new ArgumentException("Академічна група не може бути порожньою.", nameof(academicGroup));

        AcademicGroup = academicGroup.Trim();
    }

    /// <summary>
    /// Оновлює академічну групу студента.
    /// </summary>
    /// <param name="newAcademicGroup">Нова академічна група.</param>
    /// <exception cref="ArgumentException">Виникає, якщо нова академічна група порожня.</exception>
    public void UpdateAcademicGroup(string newAcademicGroup)
    {
        if (string.IsNullOrWhiteSpace(newAcademicGroup))
            throw new ArgumentException("Академічна група не може бути порожньою.", nameof(newAcademicGroup));

        AcademicGroup = newAcademicGroup.Trim();
    }

    /// <summary>
    /// Повертає детальне рядкове представлення студента.
    /// Перевизначає абстрактний метод базового класу (демонстрація поліморфізму).
    /// </summary>
    /// <returns>Рядок з повною інформацією про студента.</returns>
    public override string ToPrintString()
    {
        return $"[Студент] {GetFullName()}, група: {AcademicGroup}, ID: {Id}";
    }

    /// <summary>
    /// Перевизначений метод пошуку, що додатково шукає по академічній групі.
    /// </summary>
    /// <param name="keyword">Ключове слово.</param>
    /// <returns>true, якщо ключове слово знайдене у будь-якому атрибуті студента.</returns>
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
            && AcademicGroup.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
