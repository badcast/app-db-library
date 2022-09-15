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
    /// Логика взаимодействия для WSpecialites.xaml
    /// </summary>
    public partial class WSpecialites : UserControl, IWindowBase
    {
        public WSpecialites()
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
            Background = design.GetImageBrushFromId("background_00");
            center.Source = design.GetBitmapSourceFromId("linedes");
            asanbaiBut.Text = "Меркенский многопрофильный колледж\n                   им.А.Аскарова\n               (все специальности)";
            courceBut.Text = "Краткосрочные-профессиональные курсы\n                  (все специальности)";
        }

        public void Showed()
        {
         
        }

        private void LeftButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindow(MainWindow.Windows.W_Home);
        }

        private void asanbaiBut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_SPECIALS, SpecialitiesType.AsanbaiCollege);
        }
    }
}
