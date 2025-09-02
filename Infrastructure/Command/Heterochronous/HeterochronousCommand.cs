using Infrastructure.Command.Classic;

namespace Infrastructure.Command.Heterochronous;

public class HeterochronousCommand : ICommand
{
    private readonly IModelCommand _modelCommand;
    private readonly IUiCommand _uiCommand;
    private bool _modelExecuted;

    public HeterochronousCommand(IModelCommand modelCommand, IUiCommand uiCommand) {
        _modelCommand = modelCommand;
        _uiCommand = uiCommand;
    }

    public async Task ExecuteAsync() {
        // Сначала UI - мгновенная реакция
        _uiCommand.ExecuteUi();
        
        // Затем Model - асинхронно, может быть отложено
        if (_modelCommand.Validate()) {
            await _modelCommand.ExecuteAsync().ConfigureAwait(false);
            _modelExecuted = true;
        }
        else {
            _uiCommand.UndoUi(); // Откатываем UI если валидация провалилась
        }
    }

    public async Task UndoAsync() {
        _uiCommand.UndoUi(); // Синхронный откат UI
        if (_modelExecuted) 
            await _modelCommand.UndoAsync().ConfigureAwait(false); // Асинхронный откат Model
    }
}