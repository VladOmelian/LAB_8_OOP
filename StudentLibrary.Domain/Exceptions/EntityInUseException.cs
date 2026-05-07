namespace StudentLibrary.Domain.Exceptions;

public class EntityInUseException(string entityType, string reason)
    : LibraryException($"Неможливо видалити {entityType}: {reason}");
