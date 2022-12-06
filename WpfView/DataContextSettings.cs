using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfView
{

    internal class DataContextSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
       
        
        private int _indexInputDevice;

        public int IndexinputDevice
        {
            get { return _indexInputDevice; }
            set { _indexInputDevice = value; OnPropertyChanged(); } 
        }

      

        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndexinputDevice)));
        }
    }
}
