using Infrastructure.Command.Classic;

namespace Infrastructure.Command.Heterochronous;

public class а
{
    
}

public interface IModelCommand : ICommand 
{
    bool Validate(); // Валидация для БД
}

public interface IUiCommand : ICommand
{
    void ExecuteUi(); // Синхронное выполнение для UI
    void UndoUi();    // Синхронная отмена для UI
}

// Композитная команда с разделением слоёв

// Пример реализации для вашего случая