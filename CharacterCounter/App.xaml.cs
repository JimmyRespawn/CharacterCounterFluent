using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CharacterCounter
{
    sealed partial class App : Application
    {
        public App()
        {
            //Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
            //    Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
            //    Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            DetermineAppTheme();
            this.Suspending += OnSuspending;
        }
        
        private void DetermineAppTheme()
        {
            //AutoDetectTheme
            UISettings uiSettings = new UISettings();
            var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);
            if (backgroundColor == Windows.UI.Colors.Black)
                this.RequestedTheme = ApplicationTheme.Dark;
            else
                this.RequestedTheme = ApplicationTheme.Light;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    this.DebugSettings.EnableFrameRateCounter = true;
            //}
#endif

            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                //back button handled
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                rootFrame.Navigated += RootFrame_Navigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Note, on device families that have no title bar, setting AppViewBackButtonVisibility can safely execute 
            // but it will have no effect. Such device families provide back button UI for you.
            if (rootFrame.CanGoBack)
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
