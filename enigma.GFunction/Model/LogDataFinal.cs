using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma.GFunction.Model
{
    public class LogDataFinal : LogData
    {
        private double? _proppantConcentration;
        public double? ProppantConcentration
        {
            get
            {
                return _proppantConcentration;
            }
            set
            {
                _proppantConcentration = value;
                OnPropertyChanged("ProppantConcentration");
            }
        }

        private double? _BHProppantConcentration;
        public double? BHProppantConcentration
        {
            get
            {
                return _BHProppantConcentration;
            }
            set
            {
                _BHProppantConcentration = value;
                OnPropertyChanged("BHProppantConcentration");
            }
        }

        private double? _bla1;
        public double? Bla1
        {
            get
            {
                return _bla1;
            }
            set
            {
                _bla1 = value;
                OnPropertyChanged("Bla1");
            }
        }

        private double? _bla2;
        public double? Bla2
        {
            get
            {
                return _bla2;
            }
            set
            {
                _bla2 = value;
                OnPropertyChanged("Bla2");
            }
        }
    }
}
