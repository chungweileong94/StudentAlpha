using static StudentAlpha.App;
using StudentAlpha.Views.SubViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace StudentAlpha.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MainFrame.Navigated += (s, e) =>
            {
                var sender = s as Frame;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = sender.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

                foreach (var rb in allRadioButtons(this))
                {
                    if (MainFrame.SourcePageType.Name == $"{rb.Content}Page")
                    {
                        rb.IsChecked = true;
                        return;
                    }
                }
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += MainFrame_BackRequested;
            
            switch ((int)_LocalSettings.Values[THEME_SETTING])
            {
                case 0:
                default:
                    RequestedTheme = ElementTheme.Default;
                    break;
                case 1:
                    RequestedTheme = ElementTheme.Light;
                    break;
                case 2:
                    RequestedTheme = ElementTheme.Dark;
                    break;
            }
        }

        public void MainFrame_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (PreviousPageType != null)
            {
                if (PreviousPageType == typeof(AssignmentDetailPage))
                {
                    MainFrame.Navigate(typeof(AssignmentsPage));
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainFrame_BackRequested;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as RadioButton;

            switch (s.Content.ToString())
            {
                case "Home":
                    MainFrame.Navigate(typeof(HomePage));
                    break;
                case "Timetable":
                    MainFrame.Navigate(typeof(TimetablePage));
                    break;
                case "Assignments":
                    MainFrame.Navigate(typeof(AssignmentsPage));
                    break;
                case "Events":
                    MainFrame.Navigate(typeof(EventsPage));
                    break;
                case "Settings":
                    MainFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }

        private List<RadioButton> allRadioButtons(DependencyObject parent)
        {
            var list = new List<RadioButton>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is RadioButton)
                {
                    list.Add(child as RadioButton);
                    continue;
                }

                list.AddRange(allRadioButtons(child));
            }
            return list;

        }
    }
}
