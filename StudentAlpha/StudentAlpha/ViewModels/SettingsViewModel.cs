using static StudentAlpha.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StudentAlpha.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Properties
        private int _ThemeSetting;
        public int ThemeSetting
        {
            get { return _ThemeSetting; }
            set
            {
                Set(ref _ThemeSetting, value);
                var frame = Window.Current.Content as Frame;
                var page = frame.Content as Page;
                switch (value)
                {
                    default:
                    case 0:
                        page.RequestedTheme = ElementTheme.Default;
                        break;
                    case 1:
                        page.RequestedTheme = ElementTheme.Light;
                        break;
                    case 2:
                        page.RequestedTheme = ElementTheme.Dark;
                        break;
                }
                _LocalSettings.Values[THEME_SETTING] = value;
            }
        }
        #endregion

        public SettingsViewModel()
        {
            ThemeSetting = (int)_LocalSettings.Values[THEME_SETTING];
        }

        #region INotifyProperty Helper
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
