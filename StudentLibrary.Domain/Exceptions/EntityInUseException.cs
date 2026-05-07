namespace StudentLibrary.Domain.Exceptions;

// Виняток, що виникає у випадку спроби видалення сутності (студента або документа),
// яка пов'язана з активними видачами.
public class EntityInUseException(string entityType, string reason)
    : LibraryException($"Неможливо видалити {entityType}: {reason}");
