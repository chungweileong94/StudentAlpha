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
using Windows.UI.Popups;

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
            DataContext = _TimetableViewModel;

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

        private async void TimetableDetailPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (isDiffer())
            {
                MessageDialog msg = new MessageDialog("All the changes will be discard. Are you sure to leave?", "Leave");
                msg.Commands.Add(new UICommand("Yes", delegate
                {
                    if (Frame.CanGoBack)
                    {
                        e.Handled = true;
                        Frame.GoBack();
                    }
                }));
                msg.Commands.Add(new UICommand("No"));
                await msg.ShowAsync();
            }
            else
            {
                if (Frame.CanGoBack)
                {
                    e.Handled = true;
                    Frame.GoBack();
                }
            }
        }

        private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Are you sure to delete?", "Delete Class");
            msg.Commands.Add(new UICommand("Yes", async delegate
            {
                await _TimetableViewModel.RemoveAsync(_Class);
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }));
            msg.Commands.Add(new UICommand("No"));
            await msg.ShowAsync();
        }

        private async void SaveAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await _TimetableViewModel.EditAsync(_Class);

            if (result)
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
            else
            {
                FlyoutBase.GetAttachedFlyout(sender as FrameworkElement).ShowAt(sender as FrameworkElement);
            }
        }

        private bool isDiffer()
        {
            if (_TimetableViewModel.Subject_Input != _Class.Subject ||
                _TimetableViewModel.Lecture_Input != _Class.Lecture ||
                _TimetableViewModel.Venue_Input != _Class.Venue ||
                _TimetableViewModel.Day_Input != _Class.Day ||
                _TimetableViewModel.StartTime_Input != _Class.StartTime ||
                _TimetableViewModel.EndTime_Input != _Class.EndTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
