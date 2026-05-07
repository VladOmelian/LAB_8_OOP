namespace StudentLibrary.Domain.Exceptions;

/// Виняток, що виникає у випадку, коли запитувана сутність (студент, документ, видача)
/// не знайдена в бібліотеці.
public class EntityNotFoundException : LibraryException
{
    public EntityNotFoundException(string entityType, Guid identifier)
        : base($"{entityType} з ідентифікатором {identifier} не знайдено в бібліотеці.")
    {
    }
}
