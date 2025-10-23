using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db;

public class DatabaseInitializer
{
    private readonly AppDbContext _context;
    public DatabaseInitializer(AppDbContext context) {
        _context = context;
    }

    public const string ROOT_CATEGORY_NAME = "Компоненты";

    public async Task InitializeAsync() {
        await _context.Database.MigrateAsync(); // применяет существующие миграции, которые уже есть в проекте
        
    #if DEBUG
        try {
            var tableExists = await _context.SupplierOrders.AnyAsync();
            Console.WriteLine("SupplierOrders table exists and accessible");
        }
        catch (Exception ex) {
            Console.WriteLine($"SupplierOrders table problem: {ex.Message}");
            // Если таблицы нет - пересоздаем БД (ТОЛЬКО ДЛЯ РАЗРАБОТКИ!)
            await _context.Database.MigrateAsync();
        }
    
        await AddSomeTestData();
    #endif
        
        await EnsureRootCategoryExistsAsync();
    }

    private async Task EnsureRootCategoryExistsAsync() {
        var rootCategoryExists = await _context.Categories
                                               .AnyAsync(c => c.ParentCategoryId == null);

        if (!rootCategoryExists) {
            var rootCategory = new Category(ROOT_CATEGORY_NAME, null);
            _context.Categories.Add(rootCategory);
            await _context.SaveChangesAsync();
        }
    }

#if DEBUG
private async Task AddSomeTestData() {
    // Проверяем, есть ли уже записи в GenericParametersSets
    var existingParametersSets = await _context.GenericParametersSets.ToListAsync();

    // Список новых наборов параметров
    var newParametersSets = new List<GenericParametersSet> {
        new GenericParametersSet() {
            Name = "Расширенная конфигурация",
            GpMain = "Расширенный",
            Gp1 = "Высокая точность",
            Gp2 = "Многозадачность", 
            Gp3 = "Оптимизация",
            Gp4 = "Производительность",
            Gp5 = "Надежность"
        },
        new GenericParametersSet() {
            Name = "Научный эксперимент",
            GpMain = "Исследовательский режим",
            Gp1 = "Температура: 25°C",
            Gp2 = "Давление: 1 атм",
            Gp3 = "Влажность: 60%",
            Gp4 = "Освещенность: 500 лк",
            Gp5 = "Точность измерений: 0.01"
        },
        new GenericParametersSet() {
            Name = "Промышленный робот",
            GpMain = "Производственная линия",
            Gp1 = "Грузоподъемность: 50 кг",
            Gp2 = "Точность позиционирования: ±0.1 мм",
            Gp3 = "Скорость перемещения: 2 м/сек",
            Gp4 = "Радиус действия: 1.5 м",
            Gp5 = "Энергопотребление: 2.5 кВт"
        },
        new GenericParametersSet() {
            Name = "Конфигурация ПО",
            GpMain = "Серверная среда",
            Gp1 = "Версия: 2.5.1",
            Gp2 = "Кэширование: Включено",
            Gp3 = "Максимальные потоки: 16",
            Gp4 = "Лимит памяти: 8 ГБ",
            Gp5 = "Режим отладки: Выключен"
        }
    };

    // Фильтруем новые наборы, исключая уже существующие
    var setsToAdd = newParametersSets
        .Where(newSet => existingParametersSets.All(existing => existing.Name != newSet.Name))
        .ToList();

    // Добавляем только новые наборы
    if (setsToAdd.Count != 0) {
        _context.GenericParametersSets.AddRange(setsToAdd);
        await _context.SaveChangesAsync();
    }
}
#endif
}