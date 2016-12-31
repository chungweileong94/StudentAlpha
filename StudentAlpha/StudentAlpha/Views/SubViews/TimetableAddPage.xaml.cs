using static StudentAlpha.App;
using StudentAlpha.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class TimetableAddPage : Page
    {
        public TimetableViewModel ViewModel { get; set; }

        public TimetableAddPage()
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
            ViewModel.Subject_Input = null;
            ViewModel.Lecture_Input = null;
            ViewModel.Venue_Input = null;
            ViewModel.Day_Input = 0;
            ViewModel.StartTime_Input = DateTime.Now.TimeOfDay;
            ViewModel.EndTime_Input = DateTime.Now.TimeOfDay;
            DataContext = ViewModel;

            PreviousPageType = typeof(TimetablePage);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += TimetableAddPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= TimetableAddPage_BackRequested;
        }

        private void TimetableAddPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void CancelAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void CreateAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await ViewModel.AddAsync();

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
    }
}
