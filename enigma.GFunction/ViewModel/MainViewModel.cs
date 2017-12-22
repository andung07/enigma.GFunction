using GalaSoft.MvvmLight;
using enigma.GFunction.Model;
using System.IO;
using System;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using CsvHelper;
using System.Linq;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Defaults;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.ComponentModel;

namespace enigma.GFunction.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public PumpingSummary PumpingSummary
        {
            get
            {
                if (DataSourcePopulated)
                {
                    return new PumpingSummary(_dataSource.ToList(), _beginPumping, _endPumping, _endDecline);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool DataSourcePopulated
        {
            get
            {
                if (DataSource != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private int _maxHeight;
        public int MaxHeight 
        { 
            get
            {
                if(this.IsInDesignMode)
                {
                    return Convert.ToInt16(0.75 * 768);
                }
                else
                {
                    return _maxHeight;
                }                
            }
            set
            {
                _maxHeight = value;
                RaisePropertyChanged(() => MaxHeight);
            } 
        }

        private ObservableCollection<LogDataFinal> _dataSource;
        public ObservableCollection<LogDataFinal> DataSource 
        { 
            get 
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;

                ConstructPlot1();
                ConstructPlot2();
                RaisePropertyChanged(() => DataSource);
                RaisePropertyChanged(() => DataSourcePopulated);
                RaisePropertyChanged(() => MaximumTime);
                RaisePropertyChanged(() => PumpingSummary);
            }    
        }

        private SeriesCollection _plotOneSeries;
        public SeriesCollection PlotOneSeries 
        { 
            get
            {
                return _plotOneSeries;
            }
            set
            {
                _plotOneSeries = value;
                RaisePropertyChanged(() => PlotOneSeries);
            }
        }

        private SeriesCollection _plotTwoSeries;
        public SeriesCollection PlotTwoSeries
        {
            get
            {
                return _plotTwoSeries;
            }
            set
            {
                _plotTwoSeries = value;
                RaisePropertyChanged(() => PlotTwoSeries);
            }
        }

        private double _phydrostatic;
        public double Phydrostatic
        {
            get
            {
                return _phydrostatic;         
            }
            set
            {
                _phydrostatic = value;
                RaisePropertyChanged(() => Phydrostatic);
            }
        }

        private double _beginPumping, _endPumping, _endDecline;

        public double BeginPumping 
        { 
            get
            {
                return _beginPumping;
            }
            set 
            {
                _beginPumping = value;                
                RaisePropertyChanged(() => BeginPumping);
                RaisePropertyChanged(() => PumpingSummary);
            }
        }
        public double EndPumping 
        { 
            get
            {
                return _endPumping;
            }
            set
            {
                _endPumping = value;
                ConstructPlot2();
                RaisePropertyChanged(() => EndPumping);
                RaisePropertyChanged(() => PumpingSummary);
            }
        }
        public double EndDecline 
        { 
            get
            {
                return _endDecline;
            }
            set
            {
                _endDecline = value;
                ConstructPlot2();
                RaisePropertyChanged(() => EndDecline);
                RaisePropertyChanged(() => PumpingSummary);
            }
        }

        public double MaximumTime 
        {
            get
            {
                if (DataSource != null)
                {
                    return DataSource.Select(log => log.Time).Max();
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public MainViewModel()
        {            
            #region ########### MESSAGING ############
            //Wait for FilePath
            Messenger.Default.Register<MessageObject>(this, (message) =>
            {
                if(message.Id == "FilePath")
                {
                    var reader = new CsvReader(new StreamReader(message.Content.ToString()));   
                    
                    try
                    {
                        var logDataTemp = reader.GetRecords<LogData>().ToList();

                        ObservableCollection<LogDataFinal> DataSourceTemp = new ObservableCollection<LogDataFinal>();
                        foreach(var data in logDataTemp)
                        {
                            DataSourceTemp.Add(new LogDataFinal
                            {
                                Time = data.Time,
                                PumpingRate = data.PumpingRate,
                                SurfacePressure = data.SurfacePressure,
                                BHProppantConcentration = null,
                                ProppantConcentration = null,
                                Bla1 = null,
                                Bla2 = null
                            });
                        }
                        DataSource = DataSourceTemp;

                        foreach (var item in DataSource)
                        {
                            item.PropertyChanged += (o, e) =>
                            {
                                ConstructPlot1();
                                ConstructPlot2();
                                RaisePropertyChanged(() => DataSource);
                            };
                        }
                    }
                    catch(Exception e)
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(e.Message));
                    }                    
                }
                //else if (message.Id == "DataSourceEdited")
                //{

                //}
            });

            //Wait for new Window Size
            Messenger.Default.Register<WindowSize>(this, (message) =>
            {
                MaxHeight = Convert.ToInt16(0.7 * message.Height);
            });
            #endregion

            #region ########### Initialization ###########

            _beginPumping = 2;
            _endPumping = 8;
            _endDecline = 45;

            _phydrostatic = 6477;

            #endregion      
        }

        private void ConstructPlot1()
        {
            var SurfacePressure = new ChartValues<ObservablePoint>();
            var PumpingRate = new ChartValues<ObservablePoint>();

            foreach(var log in DataSource)
            {
                var tempPressure = new ObservablePoint(log.Time, log.SurfacePressure);
                SurfacePressure.Add(tempPressure);

                var tempRate = new ObservablePoint(log.Time, log.PumpingRate);
                PumpingRate.Add(tempRate);
            }

            PlotOneSeries = new SeriesCollection
            {                
                new LineSeries
                {
                    Title = "Pressure",
                    Values = SurfacePressure,
                    ScalesYAt = 0,
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Stroke = Brushes.Red,
                    Fill = Brushes.Transparent                    
                },
                new LineSeries
                {
                    Title = "Rate",
                    Values = PumpingRate,
                    ScalesYAt = 1,
                    PointGeometry = null,
                    LineSmoothness = 1,
                    Stroke = Brushes.Blue,
                    Fill = Brushes.Transparent                    
                }                
            };
        }

        private void ConstructPlot2()
        {
            if (DataSource != null)
            {
                var BHPChartValues = new ChartValues<ScatterPoint>();
                var dPdGChartValues = new ChartValues<ScatterPoint>();
                var GdPdGChartValues = new ChartValues<ScatterPoint>();

                var SelectedData = DataSource.Where((x) => x.Time >= EndPumping && x.Time <= EndDecline);

                foreach (var log in SelectedData)
                {
                    var InitialTime = SelectedData.ElementAt(0).Time;
                    var DeltaT = (log.Time - InitialTime) / InitialTime;
                    var gDtD = (1 + DeltaT) * (Math.Asin(Math.Pow((1 + DeltaT), -0.5)) + Math.Pow(DeltaT, 0.5));
                    var xValue = 4 / Math.PI * (gDtD - Math.PI / 2);

                    var BHPPoint = new ScatterPoint();
                    BHPPoint.X = xValue;
                    BHPPoint.Y = log.SurfacePressure + Phydrostatic;
                    BHPPoint.Weight = 1;

                    BHPChartValues.Add(BHPPoint);
                }

                for (int i = 0; i < BHPChartValues.Count - 2; i++)
                {
                    var tempDPDG = (BHPChartValues.ElementAt(i).Y - BHPChartValues.ElementAt(i + 2).Y) /
                        (BHPChartValues.ElementAt(i + 2).X - BHPChartValues.ElementAt(i).X);

                    var dPdGPoint = new ScatterPoint();
                    dPdGPoint.X = BHPChartValues.ElementAt(i).X;
                    dPdGPoint.Y = tempDPDG;
                    dPdGPoint.Weight = 1;

                    dPdGChartValues.Add(dPdGPoint);
                }

                for (int i = 0; i < dPdGChartValues.Count; i++)
                {
                    var tempGDPDG = dPdGChartValues.ElementAt(i).Y * dPdGChartValues.ElementAt(i).X;

                    var GdPdGPoint = new ScatterPoint();
                    GdPdGPoint.X = dPdGChartValues.ElementAt(i).X;
                    GdPdGPoint.Y = tempGDPDG;
                    GdPdGPoint.Weight = 1;

                    GdPdGChartValues.Add(GdPdGPoint);
                }

                PlotTwoSeries = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = BHPChartValues,
                        MinPointShapeDiameter = 5,
                        MaxPointShapeDiameter = 5,
                        Title = "BHP",
                        ScalesYAt = 0,
                        Fill = Brushes.Red
                    },
                    new ScatterSeries
                    {
                        Values = dPdGChartValues,
                        MinPointShapeDiameter = 5,
                        MaxPointShapeDiameter = 5,
                        Title = "dP/dG",
                        ScalesYAt = 1,
                        Fill = Brushes.Blue                 
                    },
                    new ScatterSeries
                    {
                        Values = GdPdGChartValues,
                        MinPointShapeDiameter = 5,
                        MaxPointShapeDiameter = 5,
                        Title = "GdP/dG",
                        ScalesYAt = 1,
                        Fill = Brushes.Green
                    }
                };
            }
        }
    }
}