using StudentAlpha.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentAlpha.ViewModels
{
    public class AssignmentsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Assignment> Assignments { get; set; }

        private Assignment _SelectedAssignment;
        public Assignment SelectedAssignment
        {
            get { return _SelectedAssignment; }
            set { Set(ref _SelectedAssignment, value); }
        }

        public AssignmentsViewModel()
        {
            Assignments = new ObservableCollection<Assignment>();
            //SelectedAssignment = new Assignment()
            //{
            //    Title = "Assignment {i}",
            //    Subject = "Subject {i}",
            //    DueDate = DateTime.Now
            //};
            //sample data
            for (int i = 0; i < 5; i++)
            {
                Assignments.Add(new Assignment()
                {
                    Title = $"Assignment {i}",
                    Subject = $"Subject {i}",
                    DueDate = DateTime.Now
                });
            }
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
