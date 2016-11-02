﻿using static StudentAlpha.App;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StudentAlpha.Models;
using StudentAlpha.ViewModels;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentDetailPage : Page
    {
        public AssignmentsViewModel _AssignmentsViewModel { get; set; }

        public AssignmentDetailPage()
        {
            this.InitializeComponent();
            _AssignmentsViewModel = _AssignmentsViewModel_Share;

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
            PreviousPageType = typeof(AssignmentsPage);

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += AssignmentDetailPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AssignmentDetailPage_BackRequested;
        }

        private void AssignmentDetailPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                _AssignmentsViewModel_Share.SelectedAssignment = null;
                Frame.GoBack();
            }
        }

        #region Events
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= 720)
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            _AssignmentsViewModel.Remove();

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        #endregion
    }
}
