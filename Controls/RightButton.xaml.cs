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

namespace ElectronicLib.Controls
{
    /// <summary>
    /// Логика взаимодействия для LeftButton.xaml
    /// </summary>
    public partial class RightButton : UserControl, IStyle
    {
        static ImageBrush normalImage;
        static ImageBrush hoverImage;

        public RightButton()
        {
            InitializeComponent();

            if (MainWindow.DesignMode)
                return;

            MainWindow.AddStyle(this);
        }

        public void SetStyle(DesignManager design)
        {
            if (normalImage == null)
            {
                normalImage = design.GetImageBrushFromId("button_right_normal");
                hoverImage = design.GetImageBrushFromId("button_right_hover");
            }
            Background = normalImage;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = hoverImage;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = normalImage;
        }
    }
}
