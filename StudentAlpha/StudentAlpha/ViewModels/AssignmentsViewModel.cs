using static StudentAlpha.App;
using StudentAlpha.Models;
using StudentAlpha.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using System.Windows.Input;

namespace StudentAlpha.ViewModels
{
    public class AssignmentsViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<Assignment> Assignments { get; set; }

        private Assignment _SelectedAssignment;
        public Assignment SelectedAssignment
        {
            get { return _SelectedAssignment; }
            set { Set(ref _SelectedAssignment, value); }
        }

        #region Add purpose
        public string Title_Input { get; set; }
        public string Description_Input { get; set; }
        public string Subject_Input { get; set; }
        public DateTime DueDate_Input { get; set; } = DateTime.Now;
        #endregion
        #endregion

        public AssignmentsViewModel()
        {
            Assignments = new ObservableCollection<Assignment>();
        }

        #region Methods
        public async Task<bool> AddAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title_Input) &&
                !string.IsNullOrWhiteSpace(Description_Input) &&
                !string.IsNullOrWhiteSpace(Subject_Input) &&
                DueDate_Input != null)
            {
                Assignments.Add(new Assignment()
                {
                    Title = Title_Input,
                    Description = Description_Input,
                    Subject = Subject_Input,
                    DueDate = DueDate_Input,
                    CreatedDateTime = DateTime.Now,
                    Status = false
                });

                await WriteToFileAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title_Input) &&
                !string.IsNullOrWhiteSpace(Description_Input) &&
                !string.IsNullOrWhiteSpace(Subject_Input) &&
                DueDate_Input != null)
            {
                var assignment = Assignments.First(a => (a == SelectedAssignment));
                assignment.Title = Title_Input;
                assignment.Description = Description_Input;
                assignment.Subject = Subject_Input;
                assignment.DueDate = DueDate_Input;

                await WriteToFileAsync();

                return true;
            }

            return false;
        }

        public async void Remove()
        {
            if (SelectedAssignment != null)
            {
                Assignments.Remove(SelectedAssignment);
                await WriteToFileAsync();
            }
        }

        public async void ChangeStatus()
        {
            await ChangeStatusAsync(SelectedAssignment);
        }

        public async Task ChangeStatusAsync(Assignment assignment)
        {
            var _assignment = Assignments.First(a => (a == assignment));
            _assignment.Status = !_assignment.Status;
            await WriteToFileAsync();
        }

        public async Task LoadAsync()
        {
            try
            {
                var jsonString = await FileService.ReadDataFromLocalStorageAsync(ASSIGNMENTS_JSONFILENAME);
                Assignments = JsonConvert.DeserializeObject<ObservableCollection<Assignment>>(jsonString);
            }
            catch { }
        }

        private async Task WriteToFileAsync()
        {
            string jsonString = JsonConvert.SerializeObject(Assignments);
            await FileService.WriteDataToLocalStorageAsync(ASSIGNMENTS_JSONFILENAME, jsonString);
        }
        #endregion
    }

    #region Converters
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => new DateTimeOffset((DateTime)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => ((DateTimeOffset)value).DateTime;
    }
    #endregion
}
