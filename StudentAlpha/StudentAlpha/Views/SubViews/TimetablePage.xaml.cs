using StudentAlpha.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class TimetablePage : Page
    {
        public List<TimetableData> list { get; set; }

        public TimetablePage()
        {
            this.InitializeComponent();

            list = new List<TimetableData>();

            for (int i = 0; i < 10; i++)
            {
                list.Add(new TimetableData()
                {
                    Subject = $"Subject {i}",
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddHours(1),
                    Lecture = $"Lecture {i}"
                });
            }
        }

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
    }
}
