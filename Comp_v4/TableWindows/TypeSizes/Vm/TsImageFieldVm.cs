using System.IO;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using Comp_v4.CompCard.Vm;
using Comp.ModelData;

namespace Comp_v4.TableWindows.TypeSizes.Vm;

public partial class TsImageFieldVm : ImageFieldVmBase
{
    [RelayCommand]
    public override void Clear() {
        base.Clear();
    }

    [RelayCommand]
    public override void Open() {
        base.Open();
    }

    [RelayCommand]
    public override void Select() {
        base.Select();
    }
    
    public void OnLoaded(IImageOwner imageOwner) {
        ImagePath = imageOwner.ImagePath;
    
        if (string.IsNullOrEmpty(ImagePath)) {
            Image = null;
            return;
        }

        try {
            // Объединяем папку приложения и относительный путь
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.GetFullPath(Path.Combine(basePath, ImagePath));

            if (File.Exists(absolutePath)) {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(absolutePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze(); 

                Image = bitmap;
            }
            else {
                Image = null;
            }
        }
        catch (Exception ex) {
            Image = null;
            // _logger.LogError($"Ошибка загрузки изображения: {ex.Message}");
        }
    }}