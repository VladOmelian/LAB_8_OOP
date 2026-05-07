namespace StudentLibrary.Domain.Exceptions;

/// Виняток, що виникає у випадку спроби видалення сутності (студента або документа),
/// яка пов'язана з активними видачами.
public class EntityInUseException : LibraryException
{
    public EntityInUseException(string entityType, string reason)
        : base($"Неможливо видалити {entityType}: {reason}")
    {
    }
}
