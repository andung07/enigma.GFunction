using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enigma.GFunction.Model
{
    public class PumpingSummary
    {
        private List<LogDataFinal> _dataSource;
        private double _beginPumping, _endPumping, _endDecline;

        public PumpingSummary(List<LogDataFinal> dataSource, double beginPumping, double endPumping, double endDecline)
        {
            _dataSource = dataSource;
            _beginPumping = beginPumping;
            _endPumping = endPumping;
            _endDecline = endDecline;
        }

        public int PumpingTime
        {
            get
            {
                if (_endPumping > _beginPumping)
                {
                    return Convert.ToInt16(_endPumping - _beginPumping);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int DeclineTime
        {
            get
            {
                if (_endDecline > _endPumping)
                {
                    return Convert.ToInt16(_endDecline - _endPumping);
                }
                else
                {
                    return 0;
                }
            }
        }

        public int MaxPumpingRate
        {
            get
            {
                return Convert.ToInt16(_dataSource.Select(log => log.PumpingRate).Max());
            }
        }

        public int AvgPumpingRate
        {
            get
            {
                return Convert.ToInt16(_dataSource.Select(log => log.PumpingRate).Average());
            }
        }

        public int MaxPressure
        {
            get
            {
                return Convert.ToInt16(_dataSource.Select(log => log.SurfacePressure).Max());
            }
        }

        public int AvgPressure
        {
            get
            {
                return Convert.ToInt16(_dataSource.Select(log => log.SurfacePressure).Average());
            }
        }

        public int CalculatedBHPBeforePumping
        {
            get
            {
                return 0;
            }
        }

        public int MaxHP
        {
            get
            {
                return 0;
            }
        }

        public int AvgHP
        {
            get
            {
                return 0;
            }
        }

        public int DeclineRate1Hr
        {
            get
            {
                return 0;
            }
        }

        public int DeclineRateTillEndDecline
        {
            get
            {
                return 0;
            }
        }
    }
}
