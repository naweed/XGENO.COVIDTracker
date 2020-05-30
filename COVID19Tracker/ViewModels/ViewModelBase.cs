using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace COVID19Tracker.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public DateTime TodaysDate
        {
            get => DateTime.Now;
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _loadingText = string.Empty;
        public string LoadingText
        {
            get => _loadingText;
            set => SetProperty(ref _loadingText, value);
        }

        private bool _dataLoaded = false;
        public bool DataLoaded
        {
            get => _dataLoaded;
            set => SetProperty(ref _dataLoaded, value);
        }

        private bool _isErrorState = false;
        public bool IsErrorState
        {
            get => _isErrorState;
            set => SetProperty(ref _isErrorState, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private string _errorImage = string.Empty;
        public string ErrorImage
        {
            get => _errorImage;
            set => SetProperty(ref _errorImage, value);
        }

        public ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
