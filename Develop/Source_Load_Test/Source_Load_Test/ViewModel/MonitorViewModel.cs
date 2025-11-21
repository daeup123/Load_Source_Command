using LiveCharts;
using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.ViewModel
{
    public class MonitorViewModel : ObservableObject
    {
        public MonitorViewModel()
        {
            Debug.WriteLine("MonitorViewModel Init");

            Setting();
        }

        private void Setting()
        {
            DataListLoad = DataRepository.Instance.LoadDataList;
            DataListLoad.CollectionChanged += OnDataListLoadChanged;

            ChartSeriesLoad = DataRepository.Instance.ChartSeriesLoad;
            TimeArrayLoad = DataRepository.Instance.TimeArrayLoad;

            DataListSource = DataRepository.Instance.SourceDataList;
            DataListSource.CollectionChanged += OnDataListSourceChanged;    

            ChartSeriesSource = DataRepository.Instance.ChartSeriesSource;
            TimeArraySource = DataRepository.Instance.TimeArraySource;
        }

        private ObservableCollection<Data> _dataListLoad;
        private ObservableCollection<Data> _dataListSource;

        public ObservableCollection<Data> DataListLoad
        {
            get => _dataListLoad;
            set => SetProperty(ref _dataListLoad, value);
        }

        public ObservableCollection<Data> DataListSource
        {
            get => _dataListSource;
            set => SetProperty(ref _dataListSource, value);
        }

        private void OnDataListLoadChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataListLoad.Count == 0)
                return;

            CurrentDataLoad = DataListLoad.Last();
            Average(DataListLoad);
        }

        private void OnDataListSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DataListSource.Count == 0)
                return;

            CurrentDataSource = DataListSource.Last();
            Average(DataListSource);
        }

        private void Average(ObservableCollection<Data> dataList)
        {
            List<float> voltageList = dataList.Select(data => data.Voltage).ToList();
            float avgVoltage = voltageList.Count > 0 ? voltageList.Average() : 0.0f;

            List<float> currentList = dataList.Select(data => data.Current).ToList();
            float avgCurrent = currentList.Count > 0 ? currentList.Average() : 0.0f;

            List<float> powerList = dataList.Select(data => data.Power).ToList();

            Avg avg = new Avg()
            {
                Voltage = avgVoltage,
                Current = avgCurrent,
                Power = powerList.Count > 0 ? powerList.Average() : 0.0f
            };
            if (dataList == DataListLoad)
            {
                AvgLoad = avg;
            }
            else if (dataList == DataListSource)
            {
                AvgSource = avg;
            }
        }
        private Data _currentDataLoad = null;
        private Data _currentDataSource = null;

        public Data CurrentDataLoad
        {
            get => _currentDataLoad;
            set => SetProperty(ref _currentDataLoad, value);
        }
        public Data CurrentDataSource
        {
            get => _currentDataSource;
            set => SetProperty(ref _currentDataSource, value);
        }

        private SeriesCollection _chartSeriesLoad;
        public SeriesCollection ChartSeriesLoad
        {
            get => _chartSeriesLoad;
            set => SetProperty(ref _chartSeriesLoad, value);
        }
        private string[] _timeArrayLoad;
        public string[] TimeArrayLoad // 시간축
        {
            get => _timeArrayLoad;
            set => SetProperty(ref _timeArrayLoad, value);
        }

        private SeriesCollection _chartSeriesSource;
        public SeriesCollection ChartSeriesSource
        {
            get => _chartSeriesSource;
            set => SetProperty(ref _chartSeriesSource, value);
        }
        private string[] _timeArraySource;
        public string[] TimeArraySource // 시간축
        {
            get => _timeArraySource;
            set => SetProperty(ref _timeArraySource, value);
        }

        private Avg _avgLoad = new Avg();
        private Avg _avgSource = new Avg();

        public Avg AvgLoad
        {
            get => _avgLoad;
            set
            {
                SetProperty(ref _avgLoad, value);
                Debug.WriteLine($"AvgLoad Updated: V={value.Voltage}, I={value.Current}, P={value.Power}");
            }
        }
        public Avg AvgSource
        {
            get => _avgSource;
            set => SetProperty(ref _avgSource, value);
        }
    }
}
