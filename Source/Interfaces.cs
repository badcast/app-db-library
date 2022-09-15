using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
namespace ElectronicLib
{
    public interface IWindow : IStyle
    {
        MainWindow MainWindow { get; set; }
        void Created(MainWindow main);
        UIElement GetElement();
    }

    public interface IWindowBase : IWindow
    {
        void Showed();
    }

    public interface IWindowBaseWithParam : IWindow
    {
        void Showed(SpecialitiesType specialitiesType);
        void Closed();
    }


    public interface IStyle
    {
        void SetStyle(DesignManager design);
    }
}
