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
    /// Логика взаимодействия для SEL_PREDMETS.xaml
    /// </summary>
    public partial class SEL_PREDMETS : UserControl, IWindowBaseWithParam
    {
        private SolidColorBrush backBrh = new SolidColorBrush(Color.FromRgb(61, 61, 61));
        private SolidColorBrush foreBrh = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public SEL_PREDMETS()
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
            label.Text = LectionManager.GetSpecialValue_Cource(specialitiesType).Name;

            List_Predmets.Items.Clear();
            Predmets[] predmets = LectionManager.GetSpecialValue_Cource(specialitiesType).Predmets;

            for (int i = 0; i < predmets.Length; i++)
            {
                Predmets predmet = predmets[i];

                LabelButton btn = new LabelButton();
                btn.Width = 640;
                btn.Background = backBrh;
                btn.Foreground = foreBrh;
                btn.Content = predmet.Name;
                int index = i;
                btn.Click += (o, e) => ShowCmdWnd(index);
                List_Predmets.Items.Add(btn);
            }
            
        }

        private void ShowCmdWnd(int param)
        {
            LectionManager.GetSpecialValue_Cource().SpecialValue = param;
            Predmets p = LectionManager.GetSpecialValue_Predmets(SpecialitiesType.DefaultCollege);

            p.SpecialValue = param;

            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_LECTIONS);
        }

        private void backBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_COURSES);
        }

        public void Closed()
        {
        }
    }
}
