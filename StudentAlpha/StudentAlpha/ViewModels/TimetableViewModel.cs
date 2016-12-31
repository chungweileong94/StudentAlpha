using Newtonsoft.Json;
using static StudentAlpha.App;
using StudentAlpha.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using StudentAlpha.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace StudentAlpha.ViewModels
{
    public class TimetableViewModel : BaseViewModel
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
        }

        #region Methods
        public async Task<bool> AddAsync()
        {
            if (!string.IsNullOrWhiteSpace(Subject_Input) &&
                !string.IsNullOrWhiteSpace(Venue_Input) &&
                StartTime_Input != null &&
                EndTime_Input != null)
            {
                Timetable.Add(new TimetableData()
                {
                    Subject = Subject_Input,
                    Venue = Venue_Input,
                    Lecture = Lecture_Input,
                    Day = Day_Input,
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

        public async Task<bool> EditAsync(TimetableData timetableData)
        {
            if (!string.IsNullOrWhiteSpace(Subject_Input) &&
                !string.IsNullOrWhiteSpace(Venue_Input) &&
                StartTime_Input != null &&
                EndTime_Input != null)
            {
                var _class = Timetable.First(a => (a == timetableData));
                _class.Subject = Subject_Input;
                _class.Venue = Venue_Input;
                _class.Lecture = Lecture_Input;
                _class.Day = Day_Input;
                _class.StartTime = StartTime_Input;
                _class.EndTime = EndTime_Input;

                await WriteToFileAsync();

                return true;
            }

            return false;
        }

        public async Task RemoveAsync(TimetableData timetableData)
        {
            Timetable.Remove(timetableData);
            await WriteToFileAsync();
        }

        public async Task LoadAsync()
        {
            try
            {
                var jsonString = await FileService.ReadDataFromLocalStorageAsync(TIMETABLE_JSONFILENAME);
                Timetable = JsonConvert.DeserializeObject<ObservableCollection<TimetableData>>(jsonString);

                Reorganize();
            }
            catch { }
        }

        private async Task WriteToFileAsync()
        {
            string jsonString = JsonConvert.SerializeObject(Timetable);
            await FileService.WriteDataToLocalStorageAsync(TIMETABLE_JSONFILENAME, jsonString);
        }

        public void Reorganize()
        {
            foreach (var t in Timetables)
            {
                t.Clear();
            }

            //devide into day of week
            foreach (var c in Timetable)
            {
                Timetables[(int)c.Day].Add(c);
                Timetables[(int)c.Day] = new ObservableCollection<TimetableData>(Timetables[(int)c.Day].OrderBy(v => v.StartTime).ToList());
            }
        }
        #endregion
    }

    #region Converters
    public class DayOfWeekToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (int)value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => (DayOfWeek)value;
    }
    #endregion
}
