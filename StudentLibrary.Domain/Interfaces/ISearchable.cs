namespace StudentLibrary.Domain.Interfaces;

// Інтерфейс для об'єктів, які підтримують пошук за ключовим словом.
// Використовується для уніфікованого пошуку серед документів та користувачів.
public interface ISearchable
{
    bool MatchesKeyword(string keyword);
}
