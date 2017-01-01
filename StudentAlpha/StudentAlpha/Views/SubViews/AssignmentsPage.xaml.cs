using StudentAlpha.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using StudentAlpha.ViewModels;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using System.Threading.Tasks;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentsPage : Page
    {
        public AssignmentsViewModel ViewModel { get; set; }

        public AssignmentsPage()
        {
            this.InitializeComponent();
            ViewModel = new AssignmentsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = ViewModel;
            await ViewModel.LoadAsync();
            Bindings.Update();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (AssignmentsVisualStateGroup.CurrentState == SmallWidth_Detail)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= AssignmentsPage_BackRequested;
                SystemNavigationManager.GetForCurrentView().BackRequested += ((Window.Current.Content as Frame).Content as MainPage).MainFrame_BackRequested;
            }
        }

        #region Events
        private void AssignmentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Compositor _compositor = ElementCompositionPreview.GetElementVisual(DetailGrid).Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(DetailGrid);

            if (AssignmentsVisualStateGroup.CurrentState.Name == nameof(SmallWidth_ListView))
            {
                VisualStateManager.GoToState(this, nameof(SmallWidth_Detail), false);

                visual.Opacity = 0;
                visual.Offset = new Vector3((float)ListViewGrid.ActualWidth, 0, 0);

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.Target = "Opacity";
                fadeAnimation.InsertKeyFrame(1f, 1f);
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(400);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.Target = "Offset.X";
                offsetAnimation.InsertKeyFrame(1f, 0);
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(400);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var animationGroup = _compositor.CreateAnimationGroup();
                animationGroup.Add(fadeAnimation);
                animationGroup.Add(offsetAnimation);

                visual.StartAnimationGroup(animationGroup);
            }
            else
            {
                visual.Opacity = 0;
                visual.Offset = new Vector3((float)ListViewGrid.ActualWidth * 3, 0, 0);

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.Target = "Opacity";
                fadeAnimation.InsertKeyFrame(1f, 1f);
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(600);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.Target = "Offset.X";
                offsetAnimation.InsertKeyFrame(1f, (float)ListViewGrid.ActualWidth);
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(600);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var animationGroup = _compositor.CreateAnimationGroup();
                animationGroup.Add(fadeAnimation);
                animationGroup.Add(offsetAnimation);

                visual.StartAnimationGroup(animationGroup);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < 720)
            {
                if (ViewModel.SelectedAssignment == null)
                {
                    VisualStateManager.GoToState(this, nameof(SmallWidth_ListView), true);
                }
                else
                {
                    VisualStateManager.GoToState(this, nameof(SmallWidth_Detail), true);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, nameof(LargeWidth), true);
            }
        }
        
        private void CommandBarVisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState == SmallWidth_Detail)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested -= ((Window.Current.Content as Frame).Content as MainPage).MainFrame_BackRequested;
                SystemNavigationManager.GetForCurrentView().BackRequested += AssignmentsPage_BackRequested;

            }
            else
            {
                if (e.OldState == SmallWidth_Detail && e.NewState != null)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= AssignmentsPage_BackRequested;
                    SystemNavigationManager.GetForCurrentView().BackRequested += ((Window.Current.Content as Frame).Content as MainPage).MainFrame_BackRequested;
                }
            }
        }

        private async void AssignmentsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (AssignmentsVisualStateGroup.CurrentState == SmallWidth_Detail)
            {
                e.Handled = true;
                Compositor _compositor = ElementCompositionPreview.GetElementVisual(DetailGrid).Compositor;
                var visual = ElementCompositionPreview.GetElementVisual(DetailGrid);

                visual.Opacity = 1f;
                visual.Offset = new Vector3(0, 0, 0);

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.Target = "Opacity";
                fadeAnimation.InsertKeyFrame(1f, 0);
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(400);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.Target = "Offset.X";
                offsetAnimation.InsertKeyFrame(1f, (float)ListViewGrid.ActualWidth);
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(400);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

                var animationGroup = _compositor.CreateAnimationGroup();
                animationGroup.Add(fadeAnimation);
                animationGroup.Add(offsetAnimation);

                visual.StartAnimationGroup(animationGroup);

                await Task.Delay(400);
                VisualStateManager.GoToState(this, nameof(SmallWidth_ListView), false);
                ViewModel.SelectedAssignment = null;
            }
        }

        private void NewAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AssignmentAddPage), ViewModel);
        }

        private void EditAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AssignmentEditPage), ViewModel);
        }

        private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Are you sure to delete?", "Delete Assignment");
            msg.Commands.Add(new UICommand("Yes", delegate
            {
                ViewModel.Remove();
            }));
            msg.Commands.Add(new UICommand("No"));
            await msg.ShowAsync();
        }

        private async void SlidableListItem_SwipeStatusChanged(SlidableListItem sender, SwipeStatusChangedEventArgs args)
        {
            if (args.NewValue == SwipeStatus.Idle)
            {
                if (args.OldValue == SwipeStatus.SwipingPassedLeftThreshold)
                {
                    await ViewModel.ChangeStatusAsync(sender.RightCommandParameter as Assignment);
                }
            }
        }
        #endregion
    }

    #region Converters
    public class DetailVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (value == null) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AssignmentToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (value == null) ? null : value as Assignment;
    }

    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (bool)value ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (bool)value ? Symbol.Cancel : Symbol.Accept;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (bool)value ? "Incomplete" : "Complete";

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (bool)value ? new SolidColorBrush(Colors.Red) : App.Current.Resources["SystemControlHighlightListAccentLowBrush"];

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
