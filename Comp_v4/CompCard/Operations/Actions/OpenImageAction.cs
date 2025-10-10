using System.Diagnostics;
using System.IO;
using System.Windows;
using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;
using WPF.UCL;

namespace Comp_v4.CompCard.Operations.Actions;

public class OpenImageAction : ImageActionBase
{
    protected readonly Component _component;
    
    public OpenImageAction(ImageFieldVm imageFieldVm, Component component) : base(imageFieldVm) {
        _component = component;
        _imageFieldVm.OpenAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        if (string.IsNullOrWhiteSpace(_component.ImagePath) || !File.Exists(_component.ImagePath)) {
            MessageBox.Show("Изображение не выбрано или файл не существует",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            return;
        }

        try {
            // Метод 1: Открытие через системный обработчик файлов
            var imageWindow = new OpenImageWindow(_component.ImagePath);
            imageWindow.Show();
        }
        catch (Exception ex) {
            // Альтернативный метод: создание собственного окна просмотра
            try {
                Process.Start(new ProcessStartInfo {
                    FileName = _component.ImagePath,
                    UseShellExecute = true
                });
            }
            catch (Exception innerEx) {
                MessageBox.Show(
                                $"Не удалось открыть изображение: {innerEx.Message}",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                               );
            }
        }
    }

    public override bool CanPerform() {
        return !string.IsNullOrWhiteSpace(_component.ImagePath) && File.Exists(_component.ImagePath);
    }

    public override void CancelAsync(object? parameter = null) {
        // В данном случае операция не требует отмены
    }
}