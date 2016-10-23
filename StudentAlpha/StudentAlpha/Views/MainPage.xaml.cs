using StudentAlpha.Views.SubViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    }
}
