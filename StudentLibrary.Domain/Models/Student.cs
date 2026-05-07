namespace StudentLibrary.Domain.Models;


public class Student : Person
{
    public string AcademicGroup { get; private set; }
    
    // Ініціалізує нового студента із заданими ім'ям, прізвищем та академічною групою.
    public Student(string firstName, string lastName, string academicGroup)
        : base(firstName, lastName)
    {
        if (string.IsNullOrWhiteSpace(academicGroup))
            throw new ArgumentException("Академічна група не може бути порожньою.", nameof(academicGroup));

        AcademicGroup = academicGroup.Trim();
    }

    
    public void UpdateAcademicGroup(string newAcademicGroup)
    {
        if (string.IsNullOrWhiteSpace(newAcademicGroup))
            throw new ArgumentException("Академічна група не може бути порожньою.", nameof(newAcademicGroup));

        AcademicGroup = newAcademicGroup.Trim();
    }
    
    // Повертає детальне рядкове представлення студента.
    public override string ToPrintString()
    {
        return $"[Студент] {GetFullName()}, група: {AcademicGroup}, ID: {Id}";
    }


    // Перевизначений метод пошуку, що додатково шукає по академічній групі.
    public override bool MatchesKeyword(string keyword)
    {
        if (base.MatchesKeyword(keyword))
            return true;

        return !string.IsNullOrWhiteSpace(keyword)
            && AcademicGroup.Contains(keyword.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
