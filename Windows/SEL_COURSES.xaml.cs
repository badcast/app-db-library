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
using ElectronicLib.Controls;

namespace ElectronicLib.Windows
{
    /// <summary>
    /// Логика взаимодействия для SEL_COURSES.xaml
    /// </summary>
    public partial class SEL_COURSES : UserControl, IWindowBaseWithParam
    {
        private List<ScopeButton> btns = new List<ScopeButton>();
        public SEL_COURSES()
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
            Background = design.GetImageBrushFromId("background_01");
        }

        public void Showed(SpecialitiesType specialitiesType)
        {
            List_Courses.Items.Clear();
            AllSpecialities a_spl = LectionManager.GetSpecialities(specialitiesType);
            var spl = a_spl.Specialities[a_spl.SpecialValue];
            Course[] courses = spl.Courses;
            this.lbl_01.Text = spl.Name;
            for (int i = 0; i < courses.Length; i++)
            {
                int param = i;
                ScopeButton b = null;
                if (btns.Count <= i)
                {
                    btns.Add(b = new ScopeButton());
                    b.Margin = new Thickness(3);
                    b.Width = 640D;
                    b.Height = 80;
                    b.MouseDown += (o, e) => ShowWndCmd(spl.SpecialValue = param);
                }
                else
                {
                    b = btns[i];
                }

                b.Text = courses[i].Name;

                List_Courses.Items.Add(b);
            }
        }

        void ShowWndCmd(int param)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_PREDMETS);
        }

        private void backBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_SPECIALS);
        }

        public void Closed()
        {
        }
    }
}
