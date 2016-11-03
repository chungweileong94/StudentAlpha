using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentAlpha.Models
{
    public class TimetableData : INotifyPropertyChanged
    {
        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set { Set(ref _Subject, value); }
        }

        private string _Lecture;
        public string Lecture
        {
            get { return _Lecture; }
            set { Set(ref _Lecture, value); }
        }

        private string _Venue;

        public string Venue
        {
            get { return _Venue; }
            set { Set(ref _Venue, value); }
        }

        private DateTime _StartDateTime;
        public DateTime StartDateTime
        {
            get { return _StartDateTime; }
            set
            {
                Set(ref _StartDateTime, value);
                StartTimeStringFormat = _StartDateTime.ToString("h:mm tt");
            }
        }

        private DateTime _EndDateTime;
        public DateTime EndDateTime
        {
            get { return _EndDateTime; }
            set
            {
                Set(ref _EndDateTime, value);
                EndTimeStringFormat = _EndDateTime.ToString("h:mm tt");
            }
        }

        private string _StartTimeStringFormat;
        public string StartTimeStringFormat
        {
            get { return _StartTimeStringFormat; }
            set { Set(ref _StartTimeStringFormat, value); }
        }

        private string _EndTimeStringFormat;
        public string EndTimeStringFormat
        {
            get { return _EndTimeStringFormat; }
            set { Set(ref _EndTimeStringFormat, value); }
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
