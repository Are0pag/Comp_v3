using System.Windows;
using Infrastructure;
using WPF.Templates.TableWindow.v1.Entities;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Ui;

public class CreateRawCommand<TWindow, T> : BaseCommand<ModuleContext<TWindow, T>>
    where TWindow : Window
    where T : class, new()
{
    protected readonly ModuleContext<TWindow, T> _context;
    public CreateRawCommand(ModuleContext<TWindow, T> parameter) : base(parameter) {
        _context = parameter;
    }
    public T Item { get; protected set; }

    public override Task ExecuteAsync() {
        try {
            Item = new T();
            _context.DataGridViewModel.Items.Add(Item);
            _context.DataGridViewModel.SelectedItem = Item;
        }
        catch (Exception e) {
            e.Log(this);
            throw;
        }
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.DataGridViewModel.Items.Remove(Item);
        return Task.CompletedTask;
    }
}