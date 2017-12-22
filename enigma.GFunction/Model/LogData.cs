using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace enigma.GFunction.Model
{
    public class LogData : INotifyPropertyChanged
    {
        private double _time;
        public double Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        private double _surfacePressure;
        public double SurfacePressure
        {
            get
            {
                return _surfacePressure;
            }
            set
            {
                _surfacePressure = value;
                OnPropertyChanged("SurfacePressure");
            }
        }

        private double _pumpingRate;
        public double PumpingRate
        {
            get
            {
                return _pumpingRate;            
            }
            set
            {
                _pumpingRate = value;
                OnPropertyChanged("PumpingRate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
