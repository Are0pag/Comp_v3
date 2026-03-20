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
        if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
        {
            try 
            {
                // Создаем новый BitmapImage
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(ImagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze(); // Рекомендуется для потокобезопасности

                // Устанавливаем изображение
                Image = bitmap;
            }
            catch (Exception ex)
            {
                // Обработка ошибок загрузки изображения
                Image = null;
                // Можно добавить логирование ошибки
                // _logger.LogError($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
        else 
        {
            // Если путь пустой или файл не существует
            Image = null;
        }
    }
}