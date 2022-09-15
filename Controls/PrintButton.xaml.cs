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
    public partial class PrintButton : UserControl, IStyle
    {
        public enum MouseStay
        {
            Normal,
            Hover
        }

        static ImageBrush normalImage;
        static ImageBrush hoverImage;
        private MouseStay ms_stay;
        public MouseStay mouseStay { get { return ms_stay; } set{ ms_stay = value; Draw();  } }
        public PrintButton()
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
               normalImage = design.GetImageBrushFromId("print_btn_normal");
               hoverImage = design.GetImageBrushFromId("print_btn_hover");
            }

            Background = normalImage;
        }


        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseStay = MouseStay.Hover;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseStay = MouseStay.Normal;
        }

        void Draw()
        {
            Background = mouseStay == MouseStay.Normal ? normalImage : hoverImage;
        }
    }
}
