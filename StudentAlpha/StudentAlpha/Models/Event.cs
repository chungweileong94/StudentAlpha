using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentAlpha.Models
{
    public class Event : INotifyPropertyChanged
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

        private DateTime _StartDateTime;
        public DateTime StartDateTime
        {
            get { return _StartDateTime; }
            set { Set(ref _StartDateTime, value); }
        }

        private DateTime _EndDateTime;
        public DateTime EndDateTime
        {
            get { return _EndDateTime; }
            set { Set(ref _EndDateTime, value); }
        }

        private string _LocationName;
        public string LocationName
        {
            get { return _LocationName; }
            set { Set(ref _LocationName, value); }
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
