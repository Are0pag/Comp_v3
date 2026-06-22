using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Comp_v4._Installers;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Comp.ModelData.SortingItems;
using Templates.Common.Events.Input;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;
using Utils.WPF.Windows;
using Component = Comp.ModelData.Comp.Component;

namespace Comp_v4.NomDict.View;

public partial class NomDictWindow : ColumnsVisibilityTableWindowBase, IDisposable, IGridSelectingStateHandler, IRuntimeParamsResolver<NomDictWindow>, IGetResultOfSelectionHanlder
{
    private readonly MoveCategoryAction _moveCategoryAction;
    private readonly TreeViewVm _treeViewVm;
    protected readonly EditCompButVm _editCompButVm;
    private TreeViewItem? _draggedItem;
    private Point _startPoint;
    private TaskCompletionSource<Component>? _selectingTcs;

    public NomDictWindow(TreeViewVm treeViewVm, DataGridVm dataGridVm,
                         AddNewCategoryButtonVm addNewCategoryButtonVm, DeleteCategoryButtonVm deleteCategoryButtonVm,
                         UpdateCategoryNameButtonVm updateCategoryNameButtonVm, MoveCategoryAction moveCategoryAction,
                         AddCompButtonVm addCompButtonVm, EditCompButVm editCompButVm) {
        InitializeComponent();
        
        _treeViewVm = treeViewVm;
        _moveCategoryAction = moveCategoryAction;
        _editCompButVm = editCompButVm;
        CategoryTreeView.DataContext = treeViewVm;
        MainDataGrid.DataContext = dataGridVm;
        ViewSubcategoriesContentCheckBox.DataContext = _treeViewVm;
        
        TreeView_Button_Add.DataContext = addNewCategoryButtonVm;
        TreeView_Button_Delete.DataContext = deleteCategoryButtonVm;
        TreeView_Button_UpdateName.DataContext = updateCategoryNameButtonVm;
        
        AddComponentButton.DataContext = addCompButtonVm;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (MainDataGrid.SelectedItem is not Component component)
            throw new InvalidDataException($"No component selected in {nameof(NomDictWindow)}");

        //if (_editCompButVm.IsEnabled) {
        if (_selectingTcs == null) {
            _editCompButVm.OnClickAsync();
        }
        else {
            if (_selectingTcs is { Task.IsCompleted: false }) {
                EventBus<INomDictWindowSubscriber>
                   .RaiseEvent<ICommitSelectionHandler>(h => {
                        h?.OnCommitSelection(_selectingTcs!);
                    });
            }
        }
    }

    void IGridSelectingStateHandler.OnSelecting(TaskCompletionSource<Component> tcs, Type requesterType) {
        _selectingTcs = tcs;
    }

    private void CategoriesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        _treeViewVm.SelectedCategory = e.NewValue as Category;
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<ISelectedCategoryChangedHandler>(h => h?.OnSelectedCategoryChanged(CategoryTreeView));
    }

    private void TreeViewItem_DragEnter(object sender, DragEventArgs e) {
        if (e.Data.GetData(typeof(Category)) is not Category) 
            return;
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        _startPoint = e.GetPosition(null);
        _draggedItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);
    }


    // Обработчик движения мыши - отслеживает, когда начинается перетаскивание
    private void TreeViewItem_PreviewMouseMove(object sender, MouseEventArgs e) {
        if (e.LeftButton != MouseButtonState.Pressed || _draggedItem == null) 
            return;

        var currentPoint = e.GetPosition(null);

        // Проверяем, достаточно ли переместили мышь для начала drag&drop
        // SystemParameters.MinimumHorizontalDragDistance - минимальное расстояние (обычно 2-4 пикселя)
        // Это нужно чтобы случайные мелкие движения мыши не запускали перетаскивание
        if (!(Math.Abs(currentPoint.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) &&
            !(Math.Abs(currentPoint.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
            return;

        // ★ ЗАПУСК ПЕРЕТАСКИВАНИЯ ★
        // DragDrop.DoDragDrop - системный вызов, который начинает операцию перетаскивания
        // _draggedItem.DataContext - наши данные (Category)
        // DragDropEffects.Move - эффект "перемещение" (будет показывать соответствующий курсор)
        DragDrop.DoDragDrop(_draggedItem, _draggedItem.DataContext, DragDropEffects.Move);

        _draggedItem = null;
    }


    // Обработчик события "броска" элемента
    private async void TreeViewItem_Drop(object sender, DragEventArgs e) {
        // FindAncestor<TreeViewItem> - ищем TreeViewItem, НА который бросаем
        // e.OriginalSource - это конкретный элемент внутри TreeViewItem (TextBlock, Border и т.д.)
        // Нам нужен сам TreeViewItem, поэтому поднимаемся по дереву элементов
        var targetItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

        // Проверяем: targetItem существует И его DataContext - это Category (целевая категория)
        // И из данных перетаскивания извлекаем Category (исходная категория)
        if (targetItem?.DataContext is Category targetCategory &&
            e.Data.GetData(typeof(Category)) is Category sourceCategory) {
            // Выполняем перемещение
            await _moveCategoryAction.PerformAsync(sourceCategory, targetCategory);

            // Помечаем событие как обработанное, чтобы другие обработчики не сработали
            e.Handled = true;
        }
    }


    // Обработчик события "перетаскивания над элементом"
    private void TreeViewItem_DragOver(object sender, DragEventArgs e) {
        // Проверяем: перетаскиваем ли мы Category?
        if (e.Data.GetData(typeof(Category)) is Category) {
            // Устанавливаем эффект "перемещение" - курсор изменится на разрешающий
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
    }


    // ★ ВАЖНО: Поиск предка (Ancestor) в визуальном дереве ★
    private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject {
        // Дерево элементов WPF:
        // TreeView
        //   → TreeViewItem (корневая категория) 
        //       → TreeViewItem (подкатегория)
        //           → Border
        //               → StackPanel  
        //                   → TextBlock (на него кликаем!)

        // Поднимаемся от TextBlock вверх до TreeViewItem
        while (current != null) {
            // Если текущий элемент - нужного типа (TreeViewItem), возвращаем его
            if (current is T ancestor) return ancestor;

            // Переходим к родительскому элементу
            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    public void OnGetResultOfSelection(Component component, Type requesterType) {
        _selectingTcs = null;
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<NomDictWindow> container) {
        container.RuntimeParam = this;
    }

    protected override ComboBox ColumnsVisibilityComboBox => ThisColumnsVisibilityComboBox;

    private void ComboBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DependencyObject visualTarget = e.OriginalSource as DependencyObject;
    
        while (visualTarget != null && !(visualTarget is ComboBoxItem) && !(visualTarget is CheckBox) && !(visualTarget is Button))
        {
            visualTarget = VisualTreeHelper.GetParent(visualTarget);
        }

        // Если кликнули по кнопкам "Выбрать/Снять всё", разрешаем клик, но запрещаем ComboBox закрываться
        if (visualTarget is Button)
        {
            // Позволяем кнопке выполнить ее стандартный Click, но гасим событие для ComboBox
            e.Handled = true; 
        
            // Вручную вызываем событие клика на кнопке, так как e.Handled его перехватит
            var button = (Button)visualTarget;
            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
        // Если клик пришелся на CheckBox
        else if (visualTarget is CheckBox checkBox)
        {
            checkBox.IsChecked = !checkBox.IsChecked;
            CheckBox_Click(checkBox, new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, checkBox));
            e.Handled = true; 
        }
    }

    private void SelectAll_Click(object sender, RoutedEventArgs e)
    {
        SetAllCheckBoxesState(true);
    }

    private void UnselectAll_Click(object sender, RoutedEventArgs e)
    {
        SetAllCheckBoxesState(false);
    }
    
    private void SetAllCheckBoxesState(bool isChecked)
    {
        // Проходим по всем элементам внутри ComboBox
        foreach (var item in ThisColumnsVisibilityComboBox.Items)
        {
            // Ищем именно CheckBox (кнопки и сепаратор программа пропустит)
            if (item is CheckBox checkBox)
            {
                // Меняем состояние только если оно отличается (чтобы не спамить событиями)
                if (checkBox.IsChecked != isChecked)
                {
                    checkBox.IsChecked = isChecked;
                
                    // Вызываем ваш рабочий метод, чтобы применились изменения к DataGrid
                    CheckBox_Click(checkBox, new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, checkBox));
                }
            }
        }
    }
}