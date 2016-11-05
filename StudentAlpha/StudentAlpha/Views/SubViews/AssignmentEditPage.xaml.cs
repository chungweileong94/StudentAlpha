using static StudentAlpha.App;
using StudentAlpha.ViewModels;
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
using StudentAlpha.Models;
using Windows.UI.Core;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentEditPage : Page
    {
        public AssignmentsViewModel _AssignmentsViewModel { get; set; }

        public AssignmentEditPage()
        {
            this.InitializeComponent();

            _AssignmentsViewModel = _AssignmentsViewModel_Share;
            _AssignmentsViewModel.Title_Input = string.Empty;
            _AssignmentsViewModel.Subject_Input = string.Empty;
            _AssignmentsViewModel.Description_Input = string.Empty;
            _AssignmentsViewModel.DueDate_Input = DateTime.Now;
            DataContext = _AssignmentsViewModel;

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
            var assignment = e.Parameter as Assignment;

            _AssignmentsViewModel.Title_Input = assignment.Title;
            _AssignmentsViewModel.Subject_Input = assignment.Subject;
            _AssignmentsViewModel.Description_Input = assignment.Description;
            _AssignmentsViewModel.DueDate_Input = assignment.DueDate;

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
            var result = await _AssignmentsViewModel.EditAsync();

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
