using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Comp_v4.CompCard.Vm;
using Comp.ModelData;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Operations.Actions;

public class SelectImageAction : ImageActionBase
{
    public SelectImageAction(ImageFieldVmBase imageFieldVm, IImageOwner item) : base(imageFieldVm, item) {
        imageFieldVm.SelectAction = PerformAsync;
        LoadImageFromPath();
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

                System.Windows.Application.Current.Dispatcher.Invoke(() => {
                    _imageFieldVm.Image = bitmap;
                    _imageFieldVm.ImagePath = openFileDialog.FileName;
                    _item.ImagePath = openFileDialog.FileName;
                });
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", 
                                               "Ошибка", 
                                               System.Windows.MessageBoxButton.OK, 
                                               System.Windows.MessageBoxImage.Error);
            }
        }
    }

    public override bool CanPerform() {
        return true;
    }

    public override void CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }

    public void LoadImageFromPath() {
        if (string.IsNullOrWhiteSpace(_item.ImagePath) || !File.Exists(_item.ImagePath))
            return;

        try {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(_item.ImagePath, UriKind.Absolute);
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
}