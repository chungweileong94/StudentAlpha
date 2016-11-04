using static StudentAlpha.App;
using StudentAlpha.Models;
using StudentAlpha.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class TimetablePage : Page
    {
        public TimetableViewModel _TimetableViewModel { get; set; }

        public TimetablePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (_TimetableViewModel_Share == null)
            {
                _TimetableViewModel_Share = new TimetableViewModel();
                await Task.Run(async () => await _TimetableViewModel_Share.LoadAsync());
            }
            else
            {
                _TimetableViewModel_Share.Reorganize();
            }
            _TimetableViewModel = _TimetableViewModel_Share;
            Bindings.Update();
        }

        #region Events
        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            var s = sender as Pivot;
            s.SelectedIndex = (int)DateTime.Now.DayOfWeek;
        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var s = sender as GridView;

            if (e.NewSize.Width < 500)
            {
                ((ItemsWrapGrid)s.ItemsPanelRoot).ItemWidth = e.NewSize.Width;
            }
            else
            {
                ((ItemsWrapGrid)s.ItemsPanelRoot).ItemWidth = double.NaN;
            }
        }

        private GridView currentGridView;

        private void GridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue)
            {
                currentGridView = sender as GridView;
                args.ItemContainer.Loaded += ItemContainer_Loaded;
            }
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemContainer = sender as GridViewItem;
            var itempanel = currentGridView.ItemsPanelRoot as ItemsWrapGrid;
            var itemIndex = currentGridView.IndexFromContainer(itemContainer);

            var sp = itemContainer.ContentTemplateRoot as StackPanel;

            if (itemIndex >= itempanel.FirstVisibleIndex && itemIndex <= itempanel.LastVisibleIndex)
            {
                var width = (float)sp.RenderSize.Width;
                var height = (float)sp.RenderSize.Height;

                var _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
                var visual = ElementCompositionPreview.GetElementVisual(itemContainer);
                visual.Size = new Vector2(width, height);
                visual.Scale = new Vector3(1, 1, 1);
                visual.Offset = new Vector3(0, 0, 0);
                visual.CenterPoint = new Vector3(width / 2, height / 2, 0);
                visual.Opacity = 0f;

                var scaleAnimation = _compositor.CreateVector3KeyFrameAnimation();
                scaleAnimation.InsertKeyFrame(0f, new Vector3(0, 0, 0));
                scaleAnimation.InsertKeyFrame(0.1f, new Vector3(.5f, .5f, .5f));
                scaleAnimation.InsertKeyFrame(1f, new Vector3(1f, 1f, 1f));
                scaleAnimation.Duration = TimeSpan.FromMilliseconds(400);
                scaleAnimation.DelayTime = TimeSpan.FromMilliseconds(itemIndex * 150);

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertExpressionKeyFrame(1f, "1");
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(1500);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(itemIndex * 150);

                visual.StartAnimation("Scale", scaleAnimation);
                visual.StartAnimation("Opacity", fadeAnimation);
            }
            itemContainer.Loaded -= ItemContainer_Loaded;
        }

        private void TodayAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            TimetablePivot.SelectedIndex = (int)DateTime.Now.DayOfWeek;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TimetableDetailPage), e.ClickedItem);
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(TimetableAddPage));
        }
        #endregion
    }

    public class NoClassTextVisibilityCovnerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => ((ObservableCollection<TimetableData>)value).Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
