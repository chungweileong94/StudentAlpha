using static StudentAlpha.App;
using StudentAlpha.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using StudentAlpha.ViewModels;

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
    #endregion
}
