namespace StudentLibrary.Domain.Exceptions;

public class EntityNotFoundException(string entityType, Guid identifier)
    : LibraryException($"{entityType} з ідентифікатором {identifier} не знайдено в бібліотеці.");
