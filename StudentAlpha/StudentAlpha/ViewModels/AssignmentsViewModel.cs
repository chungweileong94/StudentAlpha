﻿using static StudentAlpha.App;
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

namespace StudentAlpha.ViewModels
{
    public class AssignmentsViewModel : INotifyPropertyChanged
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

        public async void Remove()
        {
            if (SelectedAssignment != null)
            {
                MessageDialog msg = new MessageDialog("Are you sure to delete?", "Delete Assignment");
                msg.Commands.Add(new UICommand("Yes", async delegate
                {
                    Assignments.Remove(SelectedAssignment);
                    await WriteToFileAsync();
                }));
                msg.Commands.Add(new UICommand("No"));
                await msg.ShowAsync();
            }
        }

        public async void ChangeStatus()
        {
            var assignment = Assignments.First(a => (a == SelectedAssignment));
            assignment.Status = !assignment.Status;
            await WriteToFileAsync();
        }

        public async Task<AssignmentsViewModel> LoadAsync()
        {
            try
            {
                var jsonString = await new FileService().ReadDataFromLocalStorageAsync(ASSIGNMENTS_JSONFILENAME);
                Assignments = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Assignment>>(jsonString);
            }
            catch { }

            return this;
        }

        private async Task WriteToFileAsync()
        {
            string jsonString = JsonConvert.SerializeObject(Assignments);
            await new FileService().WriteDataToLocalStorageAsync(ASSIGNMENTS_JSONFILENAME, jsonString);
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
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
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
