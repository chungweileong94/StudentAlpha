using static StudentAlpha.App;
using StudentAlpha.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAlpha.Services;
using Newtonsoft.Json;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace StudentAlpha.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TimetableData> _Timetable { get; set; }
        public ObservableCollection<Assignment> _Assignments { get; set; }


        public HomeViewModel()
        {
            _Timetable = new ObservableCollection<TimetableData>();
            _Assignments = new ObservableCollection<Assignment>();
        }

        public async Task LoadAsync()
        {
            FileService fs = new FileService();

            try
            {
                _Assignments = new ObservableCollection<Assignment>();
                var jsonString = await fs.ReadDataFromLocalStorageAsync(ASSIGNMENTS_JSONFILENAME);
                var temp = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Assignment>>(jsonString);
                foreach (var a in temp)
                {
                    if (a.DueDateShortStringFormat == DateTime.Now.ToString("d/M/yyyy"))
                    {
                        _Assignments.Add(a);
                    }
                }
            }
            catch { }

            try
            {
                _Timetable = new ObservableCollection<TimetableData>();
                var jsonString = await fs.ReadDataFromLocalStorageAsync(TIMETABLE_JSONFILENAME);
                var temp = await JsonConvert.DeserializeObjectAsync<ObservableCollection<TimetableData>>(jsonString);
                foreach (var c in temp)
                {
                    if (c.Day == DateTime.Now.DayOfWeek)
                    {
                        _Timetable.Add(c);
                    }
                }
                _Timetable = new ObservableCollection<TimetableData>(_Timetable.OrderBy(c => c.StartTime).ToList());
            }
            catch { }
        }

        #region INotifyPropertyChanged Helper
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
