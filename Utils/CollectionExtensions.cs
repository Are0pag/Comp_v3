namespace Utils;

/// <summary>
///  var searchResult = categories.FindAllRecursive(
///      c => c.SubCategories,               // Как получить детей
///      c => c.Name.Contains("Смартфоны")   // Условие поиска
///      ).ToList();                         // Собираем всё в один плоский список
/// </summary>
public static class CollectionExtensions
{
    public static IEnumerable<T> FindAllRecursive<T>(
        this IEnumerable<T> source,
        Func<T, IEnumerable<T>> childrenSelector,
        Func<T, bool> condition) 
    {
        if (source == null)
            yield break;

        foreach (var item in source) {
            if (condition(item)) {
                yield return item;
            }

            // Получаем вложенную коллекцию
            var children = childrenSelector(item);

            if (children != null) {
                // Рекурсивно вызываем этот же метод для детей
                foreach (var child in children.FindAllRecursive(childrenSelector, condition)) {
                    yield return child;
                }
            }
        }
    }
}