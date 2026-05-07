namespace StudentLibrary.Domain.Models;


public class Student : Person
{
    /// <summary>
    /// Академічна група студента.
    /// </summary>
    public string AcademicGroup { get; private set; }

    /// <summary>
    /// Ініціалізує нового студента із заданими ім'ям, прізвищем та академічною групою.
    /// </summary>
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
    public override string ToPrintString()
    {
        return $"[Студент] {GetFullName()}, група: {AcademicGroup}, ID: {Id}";
    }

    /// <summary>
    /// Перевизначений метод пошуку, що додатково шукає по академічній групі.
    /// </summary>
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
               && AcademicGroup.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}