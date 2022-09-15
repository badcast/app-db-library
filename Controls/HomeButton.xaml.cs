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
    public partial class HomeButton : UserControl, IStyle
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
        public HomeButton()
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
               normalImage = design.GetImageBrushFromId("homebutton_normal");
               hoverImage = design.GetImageBrushFromId("homebutton_hover");
            }

            Background = normalImage;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            MainWindow.current.ShowWindow(MainWindow.Windows.W_Home);
            base.OnMouseDown(e);
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
