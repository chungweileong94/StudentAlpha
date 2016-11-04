using static StudentAlpha.App;
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
using StudentAlpha.Models;
using StudentAlpha.ViewModels;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class TimetableDetailPage : Page
    {
        public TimetableViewModel _TimetableViewModel { get; set; }
        public TimetableData _Class { get; set; }

        public TimetableDetailPage()
        {
            this.InitializeComponent();

            _TimetableViewModel = _TimetableViewModel_Share;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _Class = e.Parameter as TimetableData;
            _TimetableViewModel.Subject_Input = _Class.Subject;
            _TimetableViewModel.Lecture_Input = _Class.Lecture;
            _TimetableViewModel.Venue_Input = _Class.Venue;
            _TimetableViewModel.Day_Input = _Class.Day;
            _TimetableViewModel.StartTime_Input = _Class.StartTime;
            _TimetableViewModel.EndTime_Input = _Class.EndTime;

            PreviousPageType = typeof(TimetablePage);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += TimetableDetailPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= TimetableDetailPage_BackRequested;
        }

        private void TimetableDetailPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
    }
}
