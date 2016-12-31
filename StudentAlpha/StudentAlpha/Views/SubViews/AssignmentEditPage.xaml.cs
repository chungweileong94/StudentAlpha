using static StudentAlpha.App;
using StudentAlpha.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentEditPage : Page
    {
        public AssignmentsViewModel ViewModel { get; set; }

        public AssignmentEditPage()
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
            ViewModel.Title_Input = ViewModel.SelectedAssignment.Title;
            ViewModel.Subject_Input = ViewModel.SelectedAssignment.Subject;
            ViewModel.Description_Input = ViewModel.SelectedAssignment.Description;
            ViewModel.DueDate_Input = ViewModel.SelectedAssignment.DueDate;
            DataContext = ViewModel;

            PreviousPageType = typeof(AssignmentsPage);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += AssignmentEditPage_BackRequested; ;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AssignmentEditPage_BackRequested;
        }

        private void AssignmentEditPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private async void DoneAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await ViewModel.EditAsync();

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
    }
}
