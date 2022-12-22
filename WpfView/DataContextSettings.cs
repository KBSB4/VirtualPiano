using System.ComponentModel;

namespace WpfView
{
    /// <summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-7.0"/>
    /// </summary>
    internal class DataContextSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _indexInputDevice;
        private int _indexLanguage;

        public int IndexinputDevice
        {
            get { return _indexInputDevice; }
            set { _indexInputDevice = value; OnPropertyChanged(); }
        }

        public int IndexLanguage
        {
            get { return _indexLanguage; }
            set { _indexLanguage = value; OnPropertyChanged(); }
        }

        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndexinputDevice)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndexLanguage)));
        }
    }
}