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

        private TimeSpan _StartTime;
        public TimeSpan StartTime
        {
            get { return _StartTime; }
            set
            {
                Set(ref _StartTime, value);
                StartTimeStringFormat = new DateTime().Add(_StartTime).ToString("h:mm tt");
            }
        }

        private TimeSpan _EndTime;
        public TimeSpan EndTime
        {
            get { return _EndTime; }
            set
            {
                Set(ref _EndTime, value);
                EndTimeStringFormat = new DateTime().Add(_EndTime).ToString("h:mm tt");
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

        private DayOfWeek _Day;

        public DayOfWeek Day
        {
            get { return _Day; }
            set { Set(ref _Day, value); }
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
