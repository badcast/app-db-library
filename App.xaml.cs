using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows;
using Farbox.Packer;

namespace ElectronicLib
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() 
        {

            Configs.ConfigInitialize();

            DataBase.DataBaseInitialize();

            LectionManager.LectionInitialize();
            this.Exit += AppClosed;
            
        }

        public void AppClosed(object o, ExitEventArgs e)
        {
            Configs.Save();
        }
    }
}
