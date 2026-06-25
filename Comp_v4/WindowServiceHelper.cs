using System.Windows;
using Castle.Core;

namespace Comp_v4;

public static class WindowServiceHelper
{
    public static void Register<TArea, TOwner>(Window window)
        where TArea : Window
        where TOwner : Window
    {
        if (new InstanceContainer<TArea>().RuntimeParam is not TArea area) 
            throw new ArgumentException();
        
        if (new InstanceContainer<TOwner>().RuntimeParam is not TOwner owner) 
            throw new ArgumentException();
        
        WindowService.SetMovingAreaInsideParent(area, window);
        try {
            window.Owner = owner;
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            throw;
        }
        WindowService.SetAlwaysOnTop(owner, window);
    }
}