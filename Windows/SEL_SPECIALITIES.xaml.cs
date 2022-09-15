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
    /// Логика взаимодействия для W_AsanbaiSpecialites.xaml
    /// </summary>
    public partial class SEL_SPECIALITIES : UserControl, IWindowBaseWithParam
    {
        public SEL_SPECIALITIES()
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
            label.Text = "Специальности";
        }

        public void Showed(SpecialitiesType specialitiesType)
        {
            List_Specialities.Items.Clear();
            AllSpecialities a_spec = LectionManager.GetSpecialities(specialitiesType);
            Specialities[] specs = a_spec.Specialities;
            for (int i = 0; i < specs.Length; i++)
            {
                Specialities spec = specs[i];
                
                var scBt = new ScopeButton();
                scBt.Width = 640D;
                scBt.Height = 60;
                scBt.Text = spec.Name;
                scBt.Margin = new Thickness(3);
                int param = i;
                scBt.MouseDown += (o, e) => ShowWndCmd(a_spec, a_spec.SpecialValue = param);
                List_Specialities.Items.Add(scBt);
            }
        }

        private void ShowWndCmd(AllSpecialities spec, int param)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_COURSES);
        }

        private void LeftButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindow(MainWindow.Windows.W_COLLEGE);
        }

        public void Closed()
        {
        }
    }
}
