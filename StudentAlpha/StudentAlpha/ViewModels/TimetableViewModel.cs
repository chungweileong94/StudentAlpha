using Newtonsoft.Json;
using static StudentAlpha.App;
using StudentAlpha.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StudentAlpha.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace StudentAlpha.ViewModels
{
    public class TimetableViewModel : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<TimetableData> Timetable { get; set; }

        public ObservableCollection<ObservableCollection<TimetableData>> Timetables { get; set; }

        #region AddPurpose
        public string Subject_Input { get; set; }
        public string Venue_Input { get; set; }
        public string Lecture_Input { get; set; }
        public DayOfWeek Day_Input { get; set; }
        public TimeSpan StartTime_Input { get; set; } = DateTime.Now.TimeOfDay;
        public TimeSpan EndTime_Input { get; set; } = DateTime.Now.TimeOfDay;
        #endregion
        #endregion

        public TimetableViewModel()
        {
            Timetable = new ObservableCollection<TimetableData>();
            Timetables = new ObservableCollection<ObservableCollection<TimetableData>>()
            {
                new ObservableCollection<TimetableData>(), //sun
                new ObservableCollection<TimetableData>(), //mon
                new ObservableCollection<TimetableData>(), //tue
                new ObservableCollection<TimetableData>(), //web
                new ObservableCollection<TimetableData>(), //thu
                new ObservableCollection<TimetableData>(), //fri
                new ObservableCollection<TimetableData>()  //sat
            };

            //sample data
            //for (int i = 0; i < 10; i++)
            //{
            //    Timetable.Add(new TimetableData()
            //    {
            //        Subject = $"Sample Subject {i}",
            //        Lecture = $"Lecture {i}",
            //        Venue = $"Sample R{i}",
            //        Day = DateTime.Now.AddDays(i).DayOfWeek,
            //        StartTime = DateTime.Now.AddDays(i).TimeOfDay,
            //        EndTime = DateTime.Now.AddHours(i + 1).TimeOfDay
            //    });
            //}
        }

        #region Methods
        public async Task<bool> AddAsync()
        {
            if (!string.IsNullOrWhiteSpace(Subject_Input) &&
                !string.IsNullOrWhiteSpace(Venue_Input) &&
                !string.IsNullOrWhiteSpace(Lecture_Input) &&
                StartTime_Input != null &&
                EndTime_Input != null)
            {
                Timetable.Add(new TimetableData()
                {
                    Subject = Subject_Input,
                    Venue = Venue_Input,
                    Lecture = Lecture_Input,
                    StartTime = StartTime_Input,
                    EndTime = EndTime_Input
                });

                await WriteToFileAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<TimetableViewModel> LoadAsync()
        {
            try
            {
                var jsonString = await new FileService().ReadDataFromLocalStorageAsync(TIMETABLE_JSONFILENAME);
                Timetable = JsonConvert.DeserializeObject<ObservableCollection<TimetableData>>(jsonString);

                //devide into day of week
                foreach (var c in Timetable)
                {
                    Timetables[(int)c.Day].Add(c);
                    Timetables[(int)c.Day].OrderBy(v => v.StartTime);
                }
            }
            catch { }

            return this;
        }

        private async Task WriteToFileAsync()
        {
            string jsonString = JsonConvert.SerializeObject(Timetable);
            await new FileService().WriteDataToLocalStorageAsync(TIMETABLE_JSONFILENAME, jsonString);
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
    }

    #region Convertors
    public class DateTimeToDateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => new DateTimeOffset((DateTime)value);
        public object ConvertBack(object value, Type targetType, object parameter, string language) => ((DateTimeOffset)value).DateTime;
    }
    #endregion
}
