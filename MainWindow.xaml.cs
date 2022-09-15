using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Automation;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using ElectronicLib.Windows;
using ElectronicLib.Controls;
using System.Diagnostics;

namespace ElectronicLib
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Windows
        {
            W_NONE,
            W_Home,
            W_COLLEGE
        }

        public enum WindowsParam
        {
            NONE = -1,
            WIND_SPECIALS = 0,
            WIND_COURSES = 1,
            WIND_PREDMETS = 2,
            WIND_LECTIONS = 3,
            WIND_VIEW_LECTION = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        private static List<IStyle> stls;

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins pMarInset);

        public static void AddStyle(IStyle style)
        {
            if (stls == null)
                stls = new List<IStyle>();
            stls.Add(style);
        }

        public static MainWindow current { get; set; }
        public static bool DesignMode { get { return current == null; } }


        private Dictionary<Windows, IWindowBase> windows;
        private Dictionary<WindowsParam, IWindowBaseWithParam> windowsParam;
        private bool isMouseCaptured;
        private Point ms_startPoint;
        private Point ms_windowStartPoint;
        private System.Windows.Media.Effects.BlurEffect ld_blur;
        private Brush ld_prevBackgroundWindow;
        private SpecialitiesType ld_specialititesType;
        private Point MousePosition { get { var cnt = System.Windows.Forms.Control.MousePosition; return new Point(cnt.X, cnt.Y); } }
        private Windows showedWindow = Windows.W_NONE;
        private WindowsParam lastShowedWindowParam = WindowsParam.NONE;
        public IWindowBase currentWindow { get { return windows[showedWindow]; } }
        public DesignManager designManager { get; set; }

        public event Action WindowChanged;

        public MainWindow()
        {
            current = this;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = true;
            ms_startPoint = MousePosition;
            ms_windowStartPoint = new Point(Left, Top);
            (sender as UIElement).CaptureMouse();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseCaptured)
                return;

            Point p = MousePosition;
            this.Left = p.X - ms_startPoint.X + ms_windowStartPoint.X;
            this.Top = p.Y - ms_startPoint.Y + ms_windowStartPoint.Y;
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseCaptured)
                (sender as UIElement).ReleaseMouseCapture();
            isMouseCaptured = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            designManager = new DesignManager("design.xml");
            appIconSet.Background = designManager.GetImageBrushFromId("app_icon");

            SetAnimaiton();

            void setWind(List<IWindow> windows)
            {
                foreach (var w in windows)
                {
                    if (w == null)
                        continue;
                    w.Created(this);
                    w.SetStyle(designManager);
                }
            }


            windows = new Dictionary<Windows, IWindowBase>();
            windows.Add(Windows.W_NONE, null);
            windows.Add(Windows.W_Home, new Home());
            windows.Add(Windows.W_COLLEGE, new WSpecialites());
            setWind(windows.Values.Select((f) => (IWindow)f).ToList());

            windowsParam = new Dictionary<WindowsParam, IWindowBaseWithParam>();
            windowsParam.Add(WindowsParam.NONE, null);
            windowsParam.Add(WindowsParam.WIND_SPECIALS, new SEL_SPECIALITIES());
            windowsParam.Add(WindowsParam.WIND_COURSES, new SEL_COURSES());
            windowsParam.Add(WindowsParam.WIND_PREDMETS, new SEL_PREDMETS());
            windowsParam.Add(WindowsParam.WIND_LECTIONS, new SEL_LECTIONS());
            windowsParam.Add(WindowsParam.WIND_VIEW_LECTION, new SEL_LECTION());
            setWind(windowsParam.Values.Select((f) => (IWindow)f).ToList());

            foreach (var item in stls)
            {
                item.SetStyle(designManager);
            }

            SolidColorBrush solid = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Background = solid;
            ImageBrush logo = designManager.GetImageBrushFromId("logo");
            logo_draw.Background = logo;

            this.vis.Visibility = Visibility.Hidden;
            this.logo_draw.Visibility = Visibility.Visible;
            void showDef()
            {
                ShowWindow(Windows.W_Home);
                solid.Color = Color.FromRgb(255, 255, 255);
            }

            void setBackColor(byte vl)
            {
                solid.Color = Color.FromRgb(vl, vl, vl);
            }

            void setLogoOpacity(double value)
            {
                logo.Opacity = value;
            }

            DateTime startTime = DateTime.Now;
            double getProgressTime()
            {
                return (DateTime.Now - startTime).TotalSeconds;
            }

            DispatcherTimer tmr = new DispatcherTimer();

            //   this.Topmost = true;

            setLogoOpacity(0);

            tmr.Tick += (o, ef) =>
            {

                double remainedSeconds = getProgressTime();
                double endTime = 5;

                double progress = remainedSeconds / endTime;


                if (progress <= 0.25)
                {
                    double prog = progress / 0.25;
                    byte clr = (byte)Math.Min(prog * 217, 217);

                    setBackColor(clr);
                }
                else if (progress >= 0.75)
                {
                    double prog = 1 - (progress - 0.75) / 0.25;
                    byte clr = (byte)Math.Min(217 * prog, 217);

                    setBackColor(clr);
                }
                else
                {
                    double center = 0.5;
                    double p = (progress - 0.25) / center;

                    bool inversed = p > center;

                    Thickness mrg = logo_draw.Margin;
                    mrg.Left = p * 350;
                    logo_draw.Margin = mrg;

                    setLogoOpacity(!inversed ? p : 1 - p);
                }

                if (progress > 1)
                {
                    tmr.Stop();
                    tmr = null;
                    this.Topmost = false;
                    this.vis.Visibility = Visibility.Visible;
                    this.logo_draw.Visibility = Visibility.Hidden;
                    showDef();
                }

            };

            tmr.Interval = TimeSpan.FromMilliseconds(10);
            tmr.Start();

        }

        private void SetAnimaiton()
        {
            bool isAnimated = Configs.GetBool("Animated");

            if (!isAnimated)
                return;

            SolidColorBrush borderBrush = (SolidColorBrush)border_center.BorderBrush;
            Configs.WriteColor("borderColorStart", borderBrush.Color);

            Duration duration = new Duration(TimeSpan.FromMilliseconds(Configs.GetDouble("animateDuration")));

            Color borderColorStart = Configs.GetColor("borderColorStart");
            Color borderColorEnd = Configs.GetColor("borderColorEnd");

            LinearGradientBrush linearBrsh = new LinearGradientBrush(borderColorStart, borderColorEnd, 0);
            AnimationTimeline pntAnimStart = new PointAnimation(new Point(0, 0), new Point(1, 1), duration);
            pntAnimStart.AutoReverse = true;

            AnimationTimeline pntAnimEnd = new PointAnimation(new Point(1, 1), new Point(0, 0), duration);
            pntAnimEnd.AutoReverse = true;

            linearBrsh.BeginAnimation(LinearGradientBrush.StartPointProperty, pntAnimStart);
            linearBrsh.BeginAnimation(LinearGradientBrush.EndPointProperty, pntAnimEnd);

            border_center.BorderBrush = linearBrsh;

        }

        private void ViewAnimationChangeWindow(UIElement windowObject, Action showed)
        {
            WindowChanged?.Invoke();

            if (ld_prevBackgroundWindow == null)
            {
                ld_prevBackgroundWindow = parent.Background;
            }

            Brush backgroundNewWindow = (windowObject as UserControl).Background;

            bool added = false;
            void show()
            {
                if (added)
                    return;
                added = true;
                showed();
                parent.Children.Add(windowObject);
            }

            if (ld_blur == null)
                ld_blur = new System.Windows.Media.Effects.BlurEffect();

            bool start = true;
            parent.Effect = ld_blur;
            double pos = 0;
            double speed = 2;
            void AsyncLoad(Action stop)
            {
                if (!start)
                    pos -= speed;
                else
                    pos += speed;
                ld_blur.Radius = pos;

                if (start && pos >= 50)
                {
                    ld_prevBackgroundWindow = parent.Background = backgroundNewWindow;
                    start = false;
                }
                else
                if (!start && pos <= 0)
                {
                    show();
                    stop();
                }
            }

            parent.Children.Clear();
            parent.Background = ld_prevBackgroundWindow;

            DispatcherTimer fus = new DispatcherTimer(DispatcherPriority.Send);
            fus.Interval = TimeSpan.FromMilliseconds(1);
            fus.Tick += (o, e) => AsyncLoad(fus.Stop);
            fus.Start();

        }

        public void ShowWindow(Windows window)
        {
            if (!windows.ContainsKey(window))
                return;


            IWindowBase w = windows[window];

            showedWindow = window;

            ViewAnimationChangeWindow(w.GetElement(), w.Showed);


        }

        public void ShowWindowParam(WindowsParam window, SpecialitiesType specialitiesType = SpecialitiesType.DefaultCollege)
        {
            if (window == WindowsParam.NONE)
                return;




            IWindowBaseWithParam wnd = windowsParam[window];

            if(lastShowedWindowParam != WindowsParam.NONE)
            {
                windowsParam[lastShowedWindowParam].Closed();
            }

            lastShowedWindowParam = window;
            void showed()
            {
                if (specialitiesType == SpecialitiesType.DefaultCollege && ld_specialititesType != SpecialitiesType.DefaultCollege)
                {
                    specialitiesType = ld_specialititesType;
                }

                ld_specialititesType = specialitiesType;

                wnd.Showed(specialitiesType);
            }

            ViewAnimationChangeWindow(wnd.GetElement(), showed);
        }
    }
}
