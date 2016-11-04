using static StudentAlpha.App;
using StudentAlpha.Views.SubViews;
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

namespace StudentAlpha.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MainFrame.Navigated += (s, e) =>
            {
                var sender = s as Frame;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = sender.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

                foreach (var rb in allRadioButtons(this))
                {
                    if (MainFrame.SourcePageType.Name == $"{rb.Content}Page")
                    {
                        rb.IsChecked = true;
                        return;
                    }
                }
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += MainFrame_BackRequested;

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

        public void MainFrame_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (PreviousPageType != null)
            {
                MainFrame.Navigate(PreviousPageType);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainFrame_BackRequested;
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

            MainSplitView.IsPaneOpen = false;
        }

        private List<RadioButton> allRadioButtons(DependencyObject parent)
        {
            var list = new List<RadioButton>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is RadioButton)
                {
                    list.Add(child as RadioButton);
                    continue;
                }

                list.AddRange(allRadioButtons(child));
            }
            return list;

        }

        #region Gesture
        Grid paneRoot;
        VisualTransition fromClosedToOpenOverlayLeft_transition;

        private void gestureBorder_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
            if (MainSplitView.DisplayMode != SplitViewDisplayMode.Overlay) { return; }

            setupGestureComponents();

            paneRoot.Visibility = Visibility.Visible;

            if (e.Cumulative.Translation.X >= 0 && e.Cumulative.Translation.X < MainSplitView.OpenPaneLength)
            {
                CompositeTransform ct = paneRoot.RenderTransform as CompositeTransform;
                ct.TranslateX = e.Cumulative.Translation.X - MainSplitView.OpenPaneLength;
            }
        }

        private void gestureBorder_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
            if (MainSplitView.DisplayMode != SplitViewDisplayMode.Overlay) { return; }

            setupGestureComponents();

            paneRoot.Visibility = Visibility.Collapsed;
            CompositeTransform ct = paneRoot.RenderTransform as CompositeTransform;

            if ((ct.TranslateX + MainSplitView.OpenPaneLength) > MainSplitView.OpenPaneLength / 3)
            {
                MainSplitView.IsPaneOpen = true;
                fromClosedToOpenOverlayLeft_transition.Storyboard.SkipToFill();
            }

            ct.TranslateX = 0;
        }

        //Helper Method
        private void setupGestureComponents()
        {
            if (paneRoot == null)
            {
                Grid grid = FindVisualChild<Grid>(MainSplitView);
                paneRoot = grid.FindName("PaneRoot") as Grid;

                if (fromClosedToOpenOverlayLeft_transition == null)
                {
                    var stateGroups = VisualStateManager.GetVisualStateGroups(grid);
                    var transitions = stateGroups[0].Transitions;
                    fromClosedToOpenOverlayLeft_transition = transitions.Where(t => t.From == "Closed" && t.To == "OpenOverlayLeft").First();
                }
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
        #endregion
    }
}
