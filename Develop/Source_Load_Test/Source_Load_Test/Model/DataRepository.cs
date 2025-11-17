using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class DataRepository
    {
        // 🔹 외부에서 new로 만들지 못하게
        private DataRepository() { }
        // 🔹 싱글톤 인스턴스
        private static readonly Lazy<DataRepository> _instance =
            new Lazy<DataRepository>(() => new DataRepository());
        public static DataRepository Instance => _instance.Value;

        // 🔹 공용 ObservableCollection (데이터 수집용)
        public ObservableCollection<Data> LoadDataList { get; } = new ObservableCollection<Data>();
        public ObservableCollection<Data> SourceDataList { get; } = new ObservableCollection<Data>();

        public SeriesCollection ChartSeriesLoad { get; } = 
            new SeriesCollection            
            {
                new LineSeries
                {
                    Title = "Voltage",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    LineSmoothness = 0
                },
                new LineSeries
                {
                    Title = "Current",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    LineSmoothness = 0
                }
            };
        public SeriesCollection ChartSeriesSource { get; } =
            new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Voltage",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    LineSmoothness = 0
                },
                new LineSeries
                {
                    Title = "Current",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    LineSmoothness = 0
                }
            };

        public string[] TimeArrayLoad { get; } = new string[0];
        public string[] TimeArraySource { get; } = new string[0];


    }
}
