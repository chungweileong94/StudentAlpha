using StudentAlpha.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using StudentAlpha.Models;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new HomeViewModel();
            await ViewModel.LoadAsync();
            Bindings.Update();
        }

        private void TimetableGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var s = sender as Grid;
            Compositor _compositor = ElementCompositionPreview.GetElementVisual(s).Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(s);

            visual.Opacity = 0;

            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, 1f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
            fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(0);

            visual.StartAnimation("Opacity", fadeAnimation);
        }

        private void AssignmentsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var s = sender as Grid;
            Compositor _compositor = ElementCompositionPreview.GetElementVisual(s).Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(s);

            visual.Opacity = 0;

            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, 1f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
            fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(400);

            visual.StartAnimation("Opacity", fadeAnimation);
        }
    }

    #region Converters
    public class NoAssignmentTextVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => ((ObservableCollection<Assignment>)value).Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToSymbol_NagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => !(bool)value ? Symbol.Cancel : Symbol.Accept;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToColor_NagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => !(bool)value ? new SolidColorBrush(Colors.Red) : App.Current.Resources["SystemControlHighlightListAccentLowBrush"];

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
