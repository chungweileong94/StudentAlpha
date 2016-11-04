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
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using StudentAlpha.Models;
using Windows.UI;

namespace StudentAlpha.Views.SubViews
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel _HomeViewModel { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            progressRing.IsActive = true;
            base.OnNavigatedTo(e);
            if (_HomeViewModel_Share == null)
            {
                _HomeViewModel_Share = new HomeViewModel();
            }
            await Task.Run(async () => await _HomeViewModel_Share.LoadAsync());
            _HomeViewModel = _HomeViewModel_Share;
            Bindings.Update();
            progressRing.IsActive = false;
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
