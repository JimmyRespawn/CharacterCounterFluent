using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT.Interop;
using LetterCounterWinUI3.Views;

namespace LetterCounterWinUI3
{
    public sealed partial class MainWindow : Window
    {
        private double MinWidth = 360;
        private double MinHeight = 500;

        public MainWindow()
        {
            this.InitializeComponent();
            try
            {
                //Set Header Title
                this.ExtendsContentIntoTitleBar = true;
                this.SetTitleBar(CustomTitleBar);

                //Set app icon
                SetWindowIcon("Assets/icon.ico");

                //Set Background
                ApplyBackdrop();
                //Navigation
                NavigateToHomePage();

                //Get the Window Scale
                IntPtr hwnd = WindowNative.GetWindowHandle(this);
                double dpi = (GetDpiForWindow(hwnd) / 96d);
                MinWidth = MinWidth * dpi;
                MinHeight = MinHeight * dpi;

                //Set Window Min Size
                hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
                SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
                bool bReturn = SetWindowSubclass(hWnd, SubClassDelegate, 0, 0);
            }
            catch
            {

            }
        }


        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            switch (selectedItem.Tag)
            {
                case "home":
                    ContentFrame.Navigate(typeof(LetterCounterPage));
                    break;
            }
        }
        private void ApplyBackdrop()
        {
            SystemBackdrop = new MicaBackdrop()
            { Kind = MicaKind.BaseAlt };
        }

        private void NavigateToHomePage()
        {
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(Views.LetterCounterPage));
        }



        // P/Invoke µ÷ÓÃ GetDpiForWindow º¯Êý
        [DllImport("user32.dll")]
        public static extern int GetDpiForWindow(IntPtr hWnd);

        //Set Minimum Size
        IntPtr hWnd = IntPtr.Zero;
        private SUBCLASSPROC SubClassDelegate;
        private int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
        {
            switch (uMsg)
            {
                case WM_GETMINMAXINFO:
                    {
                        MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                        mmi.ptMinTrackSize.X = (int)MinWidth;
                        mmi.ptMinTrackSize.Y = (int)MinHeight;
                        Marshal.StructureToPtr(mmi, lParam, false);
                        return 0;
                    }
                    break;
            }
            return DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }


        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public const int WM_GETMINMAXINFO = 0x0024;

        public struct MINMAXINFO
        {
            public System.Drawing.Point ptReserved;
            public System.Drawing.Point ptMaxSize;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Point ptMinTrackSize;
            public System.Drawing.Point ptMaxTrackSize;
        }

        // Set Window Icon
        private void SetWindowIcon(string iconPath)
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            IntPtr hIcon = LoadImage(IntPtr.Zero, iconPath, IMAGE_ICON, 0, 0, LR_LOADFROMFILE | LR_DEFAULTSIZE);
            SendMessage(hwnd, WM_SETICON, ICON_BIG, hIcon);
            SendMessage(hwnd, WM_SETICON, ICON_SMALL, hIcon);
        }
        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;
        private const int IMAGE_ICON = 1;
        private const int LR_LOADFROMFILE = 0x10;
        private const int LR_DEFAULTSIZE = 0x40;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
    }
}
