using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Comp_v4._Installers;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp.ModelData.TechnicalItems;
using Templates.Common.Actions.Images;
using Utils.EventBus;
using WPF.UCL;

namespace Comp_v4.TableWindows.TypeSizes;

public class OpenTsImageAction : ImageActionBase, IRuntimeParamsContainer<TypeSize>
{
    protected TypeSize _typeSize;
    public OpenTsImageAction(TsImageFieldVm imageFieldVm) : base(imageFieldVm) {
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
        throw new NotImplementedException();
    }

    public void LoadImageFromPath() {
        if (string.IsNullOrWhiteSpace(RuntimeParam.ImagePath) || !File.Exists(RuntimeParam.ImagePath))
            return;

        try {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(RuntimeParam.ImagePath, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze(); // Важно для потокобезопасности

            Application.Current.Dispatcher.Invoke(() => {
                _imageFieldVm.Image = bitmap;
            });
        }
        catch (Exception ex) {
            Debug.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
        }
    }
    
    public TypeSize RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<TypeSize>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return  _typeSize;
        }
        set => _typeSize = value;
    }
}