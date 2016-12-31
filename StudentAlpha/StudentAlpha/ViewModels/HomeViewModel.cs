using static StudentAlpha.App;
using StudentAlpha.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using StudentAlpha.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using StudentAlpha.Helpers;

namespace StudentAlpha.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableRangeCollection<TimetableData> _Timetable { get; set; }
        public ObservableRangeCollection<Assignment> _Assignments { get; set; }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { Set(ref _IsLoading, value); }
        }

        public HomeViewModel()
        {
            _Timetable = new ObservableRangeCollection<TimetableData>();
            _Assignments = new ObservableRangeCollection<Assignment>();
            IsLoading = false;
        }

        public async Task LoadAsync()
        {
            IsLoading = true;

            try
            {
                _Timetable = new ObservableRangeCollection<TimetableData>();
                var jsonString = await FileService.ReadDataFromLocalStorageAsync(TIMETABLE_JSONFILENAME);
                var temp = JsonConvert.DeserializeObject<List<TimetableData>>(jsonString)
                    .Where(c => c.Day == DateTime.Now.DayOfWeek)
                    .OrderBy(c => c.StartTime);

                _Timetable.AddRange(temp);
            }
            catch { }

            try
            {
                var jsonString = await FileService.ReadDataFromLocalStorageAsync(ASSIGNMENTS_JSONFILENAME);
                var temp = JsonConvert.DeserializeObject<List<Assignment>>(jsonString)
                    .Where(a => a.DueDateShortStringFormat == DateTime.Now.ToString("d/M/yyyy"));

                _Assignments.AddRange(temp);
            }
            catch { }

            IsLoading = false;
        }
    }
}
