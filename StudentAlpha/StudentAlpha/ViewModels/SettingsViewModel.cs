﻿using static StudentAlpha.App;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace StudentAlpha.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties
        public string AppName => Package.Current.DisplayName;
        public string Version => $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";

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
    }
}
