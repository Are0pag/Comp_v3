using System.Diagnostics;
using System.IO;
using System.Windows;
using Comp_v4._Installers;
using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;
using WPF.UCL;

namespace Comp_v4.CompCard.Operations.Actions;

public class OpenImageAction : ImageActionBase, IRuntimeParamsContainer<IImageOwner>
{ 
    public OpenImageAction(ImageFieldVmBase imageFieldVm) : base(imageFieldVm) {
        _imageFieldVm.OpenAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        if (string.IsNullOrWhiteSpace(RuntimeParam.ImagePath) || !File.Exists(RuntimeParam.ImagePath)) {
            MessageBox.Show("Изображение не выбрано или файл не существует",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            return;
        }

        try {
            // Метод 1: Открытие через системный обработчик файлов
            var imageWindow = new OpenImageWindow(RuntimeParam.ImagePath);
            imageWindow.Show();
        }
        catch (Exception ex) {
            // Альтернативный метод: создание собственного окна просмотра
            try {
                Process.Start(new ProcessStartInfo {
                    FileName = RuntimeParam.ImagePath,
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
        return !string.IsNullOrWhiteSpace(RuntimeParam.ImagePath) && File.Exists(RuntimeParam.ImagePath);
    }

    public override void CancelAsync(object? parameter = null) {
        // В данном случае операция не требует отмены
    }
    
    public IImageOwner RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<IImageOwner>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _item;
        }
        set => _item = value;
    }
}