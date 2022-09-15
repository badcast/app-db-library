using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicLib
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : UserControl, IWindowBase
    {
        public Home()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow { get; set; }

        public void Created(MainWindow main)
        {
            MainWindow = main;
        }

        public UIElement GetElement()
        {
            return this;
        }

        public void SetStyle(DesignManager design)
        {
            line.Source = design.GetBitmapSourceFromId("linedes");
            Background = design.GetImageBrushFromId("background_00");
        }

        public void Showed()
        {
        }

        private void Click_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindow(MainWindow.Windows.W_COLLEGE);
        }
    }
}
