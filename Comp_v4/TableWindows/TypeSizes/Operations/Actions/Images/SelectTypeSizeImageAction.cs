using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Comp_v4._Installers;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp.ModelData.TechnicalItems;
using Templates.Common.Actions.Images;
using Utils.EventBus;

namespace Comp_v4.TableWindows.TypeSizes;

/// <summary>
/// Имеется в виду назначение изображения
/// </summary>
public class SelectTypeSizeImageAction : ImageActionBase, IRuntimeParamsContainer<TypeSize>
{
    protected TypeSize _targetItem;

    public SelectTypeSizeImageAction(TsImageFieldVm imageFieldVm) : base(imageFieldVm) {
        imageFieldVm.SelectAction = PerformAsync;
        LoadImageFromPath();
    }

    public void LoadImageFromPath() {
        if (string.IsNullOrWhiteSpace(RuntimeParam.ImagePath))
            return;

        string absolutePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RuntimeParam.ImagePath));
        if (!File.Exists(absolutePath))
            return;
        
        try {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(absolutePath, UriKind.Absolute);
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
                    
                    // _imageFieldVm.ImagePath = openFileDialog.FileName;
                    // RuntimeParam.ImagePath = openFileDialog.FileName;
                    
                    string relativePath = Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, openFileDialog.FileName);

                    _imageFieldVm.ImagePath = relativePath;
                    RuntimeParam.ImagePath = relativePath;
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
            return _targetItem;
        }
        set => _targetItem = value;
    }
}