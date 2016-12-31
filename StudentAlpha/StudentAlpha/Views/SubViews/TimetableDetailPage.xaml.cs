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
        public TimetableViewModel ViewModel { get; set; }
        public TimetableData _Class { get; set; }

        public TimetableDetailPage()
        {
            this.InitializeComponent();

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
            ViewModel = e.Parameter as TimetableViewModel;
            _Class = ViewModel.ClickedItem;
            ViewModel.Subject_Input = _Class.Subject;
            ViewModel.Lecture_Input = _Class.Lecture;
            ViewModel.Venue_Input = _Class.Venue;
            ViewModel.Day_Input = _Class.Day;
            ViewModel.StartTime_Input = _Class.StartTime;
            ViewModel.EndTime_Input = _Class.EndTime;
            DataContext = ViewModel;

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
            e.Handled = true;
            if (isDiffer())
            {
                MessageDialog msg = new MessageDialog("All the changes will be discard. Are you sure to leave?", "Leave");
                msg.Commands.Add(new UICommand("Yes", delegate
                {
                    if (Frame.CanGoBack)
                    {
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
                    Frame.GoBack();
                }
            }
        }

        private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Are you sure to delete?", "Delete Class");
            msg.Commands.Add(new UICommand("Yes", async delegate
            {
                await ViewModel.RemoveAsync(_Class);
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
            var result = await ViewModel.EditAsync(_Class);

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
            if (ViewModel.Subject_Input != _Class.Subject ||
                ViewModel.Lecture_Input != _Class.Lecture ||
                ViewModel.Venue_Input != _Class.Venue ||
                ViewModel.Day_Input != _Class.Day ||
                ViewModel.StartTime_Input != _Class.StartTime ||
                ViewModel.EndTime_Input != _Class.EndTime)
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
