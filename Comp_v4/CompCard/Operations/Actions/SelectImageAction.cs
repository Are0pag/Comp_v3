using System.Windows.Media.Imaging;
using Comp_v4.CompCard.Vm;
using Utils.WPF;

namespace Comp_v4.CompCard.Operations.Actions;

public class SelectImageAction : ImageActionBase
{
    public SelectImageAction(ImageFieldVm imageFieldVm) : base(imageFieldVm) {
        imageFieldVm.SelectAction = PerformAsync;
    }

    public override void PerformAsync(object? parameter) {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog {
            Title = "Выберите изображение",
            Filter = "Все поддерживаемые изображения|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.webp|" +
                     "JPEG изображения|*.jpg;*.jpeg|" +
                     "PNG изображения|*.png|" +
                     "GIF изображения|*.gif|" +
                     "Bitmap изображения|*.bmp|" +
                     "TIFF изображения|*.tiff|" +
                     "WebP изображения|*.webp|" +
                     "Все файлы|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == true) {
            try {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze(); // Важно для потокобезопасности

                // Обновляем Image через UI поток, если необходимо
                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    _imageFieldVm.Image = bitmap;
                });
            }
            catch (Exception ex) {
                // Обработка ошибок загрузки изображения
                System.Windows.MessageBox.Show(
                                               $"Ошибка при загрузке изображения: {ex.Message}", 
                                               "Ошибка", 
                                               System.Windows.MessageBoxButton.OK, 
                                               System.Windows.MessageBoxImage.Error
                                              );
            }
        }
    }

    public override bool CanPerform() {
        return true;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}

public abstract class ImageActionBase : BaseAction
{
    protected readonly ImageFieldVm _imageFieldVm;

    protected ImageActionBase(ImageFieldVm imageFieldVm) {
        _imageFieldVm = imageFieldVm;
    }
}