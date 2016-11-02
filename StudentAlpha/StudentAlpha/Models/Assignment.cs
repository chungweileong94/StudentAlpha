using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentAlpha.Models
{
    public class Assignment : INotifyPropertyChanged
    {
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { Set(ref _Title, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { Set(ref _Subject, value); }
        }

        private DateTime _CreatedDateTime;
        public DateTime CreatedDateTime
        {
            get { return _CreatedDateTime; }
            set { Set(ref _CreatedDateTime, value); }
        }

        private DateTime _DueDate;
        public DateTime DueDate
        {
            get { return _DueDate; }
            set { Set(ref _DueDate, value); }
        }

        public string DueDateShortStringFormat => DueDate.ToString("d/M/yyyy");
        public string DueDateLongStringFormat => DueDate.ToString("d MMM yyyy");

        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                Set(ref _Status, value);
                StatusStringFormat = Status ? "Complete" : "Incomplete";
            }
        }

        private string _StatusStringFormat;

        public string StatusStringFormat
        {
            get { return _StatusStringFormat; }
            set { Set(ref _StatusStringFormat, value); }
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
}
