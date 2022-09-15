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
using System.Xml;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace ElectronicLib.Windows
{
    /// <summary>
    /// Логика взаимодействия для SEL_LECTION.xaml
    /// </summary>
    public partial class SEL_LECTION : UserControl, IWindowBaseWithParam
    {
        private Farbox.Packer.PackFile selected;
        private Lection lastLection;
        private bool canLoadDocument = false;
        private bool isDocumentLoaded { get; set; }

        public SEL_LECTION()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow { get; set; }

        public void Created(MainWindow main)
        {
            MainWindow = main;

            prt_but.MouseDown += (o, e) =>
            {
                if (selected == null)
                    return;

                PrintDialog pf = new PrintDialog();
                if(pf.ShowDialog() == true)
                {

                    FlowDocument doc = document.Document;
                    // Сохраняем все существующие параметры.
                    double pageHeight = doc.PageHeight;
                    double pageWidth = doc.PageWidth;
                    Thickness pagePadding = doc.PagePadding;
                    double columnGap = doc.ColumnGap;
                    double columnWidth = doc.ColumnWidth;
                    // Делаем так, чтобы страница FlowDocument соответствовала печатной странице.
                    doc.PageHeight = pf.PrintableAreaHeight;
                    doc.PageWidth = pf.PrintableAreaWidth;
                    doc.PagePadding = new Thickness(50);
                    // Используем два столбца.
                    doc.ColumnGap = 25;
                    doc.ColumnWidth = (doc.PageWidth - doc.ColumnGap
                    - doc.PagePadding.Left - doc.PagePadding.Right) / 2;
                    pf.PrintDocument(
                    ((IDocumentPaginatorSource)doc).DocumentPaginator, "A Flow Document");
                    // Снова применяем прежние параметры.
                    doc.PageHeight = pageHeight;
                    doc.PageWidth = pageWidth;
                    doc.PagePadding = pagePadding;
                    doc.ColumnGap = columnGap;
                    doc.ColumnWidth = columnWidth;
                }
            };

            sv_but.MouseDown += (o, e) =>
            {
                if (selected == null)
                    return;

                System.Windows.Forms.SaveFileDialog op = new System.Windows.Forms.SaveFileDialog();
                try
                {
                    op.Filter = DataFormats.Rtf + "(*.rtf)|*.rtf";
                    op.FileName = LectionManager.GetSpecialValue_Lection().Name;
                    if(op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        MemoryStream str = selected.stream as MemoryStream;
                        byte[] bytes = str.ToArray();
                        str.Read(bytes, 0, bytes.Length);
                        File.WriteAllBytes(op.FileName, bytes);

                        object buf = bytes;
                        bytes = null;
                        G_G.destroy(ref buf);
                    }
                }
                finally
                {
                    op.Dispose();
                }
            };
        }

        public UIElement GetElement()
        {
            return this;
        }

        public void SetStyle(DesignManager design)
        {
            Background = design.GetImageBrushFromId("background_01");

            MainWindow.WindowChanged += () => canLoadDocument = false;
        }

        public void Showed(SpecialitiesType specialitiesType)
        {

            Lection lect = LectionManager.GetSpecialValue_Lection(SpecialitiesType.DefaultCollege);

            label.Text = lect.Name;

            StartLoadDocument();

            if (lastLection == lect)
                return;

            lastLection = lect;
        }

        void TemplateCode()
        {
            FlowDocument newDoc = new FlowDocument();
            FlowDocument lastDoc = document.Document;

            if (selected != null)
                selected.AutoDestruct();

            document.Document = newDoc;

            new WeakReference(lastDoc);
            lastDoc = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void StartLoadDocument()
        {
            ShowProgressBar();


            System.Collections.IEnumerator startLoad()
            {

                canLoadDocument = true;
                TemplateCode();

                selected = null;


                DateTime now = DateTime.Now.AddSeconds(1.5);
                while (true)
                {
                    if (!canLoadDocument)
                        yield break;

                    if (DateTime.Now > now)
                    {
                        Lection lect = LectionManager.GetSpecialValue_Lection();
                        var info = selected = LectionManager.GetFile(lect);
                        if (info != null)
                        {
                            document.Selection.Load(info.stream, DataFormats.Rtf);
                        }

                        isDocumentLoaded = true;
                        HideProgressBar();
                        yield break;
                    }

                    yield return 0;
                }
            }

            AsyncOperation op = AsyncOperation.CreateAsync(startLoad());

        }

        void ShowProgressBar()
        {
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;
            document.Visibility = Visibility.Hidden;
        }

        void HideProgressBar()
        {
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = Visibility.Hidden;
            document.Visibility = Visibility.Visible;
        }
        private void backBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.ShowWindowParam(MainWindow.WindowsParam.WIND_LECTIONS);
        }

        public void Closed()
        {
            TemplateCode();
        }
    }
}
