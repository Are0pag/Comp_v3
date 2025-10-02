using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp.ModelData.SortingItems;
using Templates.Common.Events.Input;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.View;

public partial class NomDictWindow : Window, IDisposable
{
    private readonly MoveCategoryAction _moveCategoryAction;
    private readonly TreeViewVm _treeViewVm;
    private TreeViewItem? _draggedItem;
    private Point _startPoint;

    public NomDictWindow(TreeViewVm treeViewVm, DataGridVm dataGridVm,
                         AddNewCategoryButtonVm addNewCategoryButtonVm, DeleteCategoryButtonVm deleteCategoryButtonVm,
                         UpdateCategoryNameButtonVm updateCategoryNameButtonVm, MoveCategoryAction moveCategoryAction) {
        InitializeComponent();
        _treeViewVm = treeViewVm;
        _moveCategoryAction = moveCategoryAction;
        CategoryTreeView.DataContext = treeViewVm;
        MainDataGrid.DataContext = dataGridVm;
        TreeView_Button_Add.DataContext = addNewCategoryButtonVm;
        TreeView_Button_Delete.DataContext = deleteCategoryButtonVm;
        TreeView_Button_UpdateName.DataContext = updateCategoryNameButtonVm;
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalMouseSubscriber>.RaiseEvent<IMouseDoubleClickHandler>(h => h?.OnMouseDoubleClick(sender, e));
    }

    public void Dispose() {
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
}