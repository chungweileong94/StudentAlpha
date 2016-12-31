using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StudentAlpha.Views;
using Windows.UI;
using Windows.Storage;
using StudentAlpha.ViewModels;

namespace StudentAlpha
{
    sealed partial class App : Application
    {
        public static ApplicationDataContainer _LocalSettings;
        public const string THEME_SETTING = "theme_setting";
        public const string ASSIGNMENTS_JSONFILENAME = "Assignments.json";
        public const string TIMETABLE_JSONFILENAME = "Timetable.json";
        public static Type PreviousPageType = null;
        public static AssignmentsViewModel _AssignmentsViewModel_Share { get; set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _LocalSettings = ApplicationData.Current.LocalSettings;

            if (!_LocalSettings.Values.ContainsKey(THEME_SETTING)) { _LocalSettings.Values[THEME_SETTING] = 0; }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();

                setupWindowBarColor();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async void setupWindowBarColor()
        {
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            //title bar
            view.TitleBar.BackgroundColor = (Color)Resources["SystemAccentColor"];
            view.TitleBar.ForegroundColor = Colors.White;
            view.TitleBar.InactiveBackgroundColor = (Color)Resources["SystemAccentColor"];
            view.TitleBar.InactiveForegroundColor = Colors.LightGray;

            //title bar button
            view.TitleBar.ButtonBackgroundColor = (Color)Resources["SystemAccentColor"];
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = (Color)Resources["SystemAccentColor"];
            view.TitleBar.ButtonInactiveForegroundColor = Colors.LightGray;

            //mobile specific
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = (Color)Resources["SystemAccentColor"];
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 1;
                await statusBar.ShowAsync();
            }
        }
    }
}
