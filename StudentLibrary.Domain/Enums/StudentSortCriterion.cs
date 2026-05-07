namespace StudentLibrary.Domain.Enums;

/// Визначає критерій сортування для списку студентів-користувачів бібліотеки.
public enum StudentSortCriterion
{
    ByFirstName,    /// Сортування за іменем (вимога 1.5.1).
    ByLastName,    /// Сортування за прізвищем (вимога 1.5.2).
    ByAcademicGroup    /// Сортування за академічною групою (вимога 1.5.3).
}
