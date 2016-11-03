using static StudentAlpha.App;
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

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class AssignmentsPage : Page
    {
        public AssignmentsViewModel _AssignmentsViewModel { get; set; }

        public AssignmentsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_AssignmentsViewModel_Share == null)
            {
                _AssignmentsViewModel_Share = await new AssignmentsViewModel().LoadAsync();
            }
            _AssignmentsViewModel = _AssignmentsViewModel_Share;
            DataContext = _AssignmentsViewModel;
            Bindings.Update();
        }

        #region Events
        private void AssignmentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommandBarVisualStateGroup.CurrentState.Name == nameof(SmallWidth))
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(AssignmentDetailPage), _AssignmentsViewModel.SelectedAssignment = e.ClickedItem as Assignment);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (CommandBarVisualStateGroup.CurrentState.Name == nameof(SmallWidth))
            {
                if (_AssignmentsViewModel.SelectedAssignment != null)
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(AssignmentDetailPage), _AssignmentsViewModel.SelectedAssignment);
                }
            }
        }

        private void NewAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AssignmentAddPage));
        }

        private void EditAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AssignmentEditPage), _AssignmentsViewModel.SelectedAssignment);
        }

        private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("Are you sure to delete?", "Delete Assignment");
            msg.Commands.Add(new UICommand("Yes", delegate
            {
                _AssignmentsViewModel.Remove();
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
                    await _AssignmentsViewModel.ChangeStatusAsync(sender.RightCommandParameter as Assignment);
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
