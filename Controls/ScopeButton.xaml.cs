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
    /// Логика взаимодействия для ScopeButton.xaml
    /// </summary>
    public partial class ScopeButton : UserControl
    {
        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get { return label.Content.ToString(); } set { label.Content = value; } }
        public new object Content { get { return Text; } set { Text = value.ToString(); } }
        public ScopeButton()
        {
            InitializeComponent();
        }
    }
}
