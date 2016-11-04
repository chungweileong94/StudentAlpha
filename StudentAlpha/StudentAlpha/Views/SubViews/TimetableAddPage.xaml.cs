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
        public TimetableViewModel _TimetableViewModel { get; set; }

        public TimetableAddPage()
        {
            this.InitializeComponent();

            _TimetableViewModel = _TimetableViewModel_Share;
            _TimetableViewModel.Subject_Input = null;
            _TimetableViewModel.Lecture_Input = null;
            _TimetableViewModel.Venue_Input = null;
            _TimetableViewModel.Day_Input = 0;
            _TimetableViewModel.StartTime_Input = DateTime.Now.TimeOfDay;
            _TimetableViewModel.EndTime_Input = DateTime.Now.TimeOfDay;
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
            var result = await _TimetableViewModel.AddAsync();

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
