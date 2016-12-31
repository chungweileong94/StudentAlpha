using static StudentAlpha.App;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using StudentAlpha.ViewModels;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentAddPage : Page
    {
        public AssignmentsViewModel ViewModel { get; set; }

        public AssignmentAddPage()
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

            ViewModel = e.Parameter as AssignmentsViewModel;
            ViewModel.Title_Input = string.Empty;
            ViewModel.Subject_Input = string.Empty;
            ViewModel.Description_Input = string.Empty;
            ViewModel.DueDate_Input = DateTime.Now;
            DataContext = ViewModel;

            PreviousPageType = typeof(AssignmentsPage);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += AssignmentAddPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AssignmentAddPage_BackRequested;
        }

        private void AssignmentAddPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        #region Events
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

        private void CancelAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        #endregion
    }
}
