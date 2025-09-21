#if DEBUG
using System.Reflection;

namespace WPF.Services;

public class ReferenceTracker
{
    public void TrackObjectReferences(object obj) {
        // Получаем все домены приложения
        var domains = AppDomain.CurrentDomain;

        // Принудительная сборка мусора
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Поиск ссылок на объект
        var references = domains
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.GetFields(
                                          BindingFlags.Static |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic)
                                     .Any(f => f.GetValue(null) == obj))
                        .ToList();

        if (references.Any()) {
            Console.WriteLine("Найдены ссылки в следующих типах:");
            foreach (var type in references) Console.WriteLine(type.FullName);
        }
        else
            Console.WriteLine("Ссылок на объект не найдено");
    }
}
#endif