using static StudentAlpha.App;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using StudentAlpha.Models;
using StudentAlpha.Services;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Data;

namespace StudentAlpha.ViewModels
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<Event> _Events { get; set; }
        public ObservableCollection<ObservableCollection<Event>> Events { get; set; }

        #region AddPurpose
        public string Title_Input { get; set; }
        public string Description_Input { get; set; }
        public DateTime StartDateTime_Input { get; set; }
        public DateTime EndDateTime_Input { get; set; }
        public string LocationName_Input { get; set; }
        #endregion
        #endregion

        public EventsViewModel()
        {
            _Events = new ObservableCollection<Event>();
            Events = new ObservableCollection<ObservableCollection<Event>>();

        }

        //Sample data (to be completed)

        #region Methods
        public async Task<bool> AddAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title_Input) &&
                !string.IsNullOrWhiteSpace(Description_Input) &&
                !string.IsNullOrWhiteSpace(LocationName_Input) &&
                StartDateTime_Input != null && EndDateTime_Input != null)
            {
                _Events.Add(new Event()
                {
                    Title = Title_Input,
                    Description = Description_Input,
                    LocationName = LocationName_Input,
                    StartDateTime = StartDateTime_Input,
                    EndDateTime = EndDateTime_Input
                });

                await WriteToFileAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<EventsViewModel> LoadAsync()
        {
            try
            {
                //var jsonString = await new FileService().ReadDataFromLocalStorageAsync(EVENT_JSONFILENAME);
                //_Events = JsonConvert.DeserializeObject<ObservableCollection<TimetableData>>(jsonString);

                //Another sample data
            }
            catch { }
            return this;
        }

        private async Task WriteToFileAsync()
        {
            string jsonString = JsonConvert.SerializeObject(_Events);
            await new FileService().WriteDataToLocalStorageAsync(EVENTS_JSONFILENAME, jsonString);
        }
        #endregion

        #region INotifyPropertyChanged Helper
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Convertors
        public class DateTimeToDateTimeToDateTimeOffsetConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language) => new DateTimeOffset((DateTime)value);
            public object ConvertBack(object value, Type targetType, object parameter, string language) => ((DateTimeOffset)value).DateTime;
        }
        #endregion
    }
}
