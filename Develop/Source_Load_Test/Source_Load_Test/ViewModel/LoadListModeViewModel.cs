using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Source_Load_Test.ViewModel
{
    public class LoadListModeViewModel : ObservableObject
    {
        // 리스트 스텝 컬렉션
        public ObservableCollection<ListStep> ListSteps { get; set; } = new ObservableCollection<ListStep>();

        // 선택된 스텝
        public ListStep SelectedStep { get; set; }

        // 반복 모드
        public bool IsOneShot { get; set; } = true;
        public bool IsContinuous { get; set; } = false;
        public bool IsRepeatCount { get; set; } = false;
        public int RepeatCount { get; set; } = 5;

        // 실행 상태
        public bool IsRunning { get; set; } = false;
        public bool CanRun => ListSteps.Count > 0 && !IsRunning;
        public int CurrentStep { get; set; } = 0;
        public string Progress => $"{CurrentStep}/{ListSteps.Count}";
        public int CurrentRepeat { get; set; } = 0;
        public double ProgressPercentage { get; set; } = 0;

        private RelayCommand _addStep = null;
        private void AddStep()
        {
            var newStep = new ListStep
            {
                StepNumber = ListSteps.Count + 1,
                Current = 1.0,
                Duration = 5.0
            };
            ListSteps.Add(newStep);
        }
        // 스텝 추가
        public ICommand AddStepCommand
        {
            get
            {
                if(_addStep == null)
                {
                    _addStep = new RelayCommand(param => AddStep());
                }
                return _addStep;
            }
        }

        private RelayCommand _removeStep = null;
        private void RemoveStep()
        {
            if(SelectedStep != null)
            {
                ListSteps.Remove(SelectedStep);
                // 스텝 번호 재정렬
                for (int i = 0; i < ListSteps.Count; i++)
                {
                    ListSteps[i].StepNumber = i + 1;
                }
            }
        }   
        // 스텝 제거
        public ICommand RemoveStepCommand
        {
            get
            {
                if(_removeStep == null)
                {
                    _removeStep = new RelayCommand(param => RemoveStep());
                }
                return _removeStep;
            }
        }

        private RelayCommand _clearAllSteps = null;
        private void ClearAllSteps()
        {
            var result = MessageBox.Show(
                "모든 스텝을 삭제하시겠습니까?",
                "확인",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ListSteps.Clear();
            }
        }

        //private RelayCommand _uploadToDeviceCommand = null;
        //private void UploadToDevice()
        //{
        //    if (ListSteps.Count == 0)
        //    {
        //        MessageBox.Show("업로드할 스텝이 없습니다.");
        //        return;
        //    }

        //    try
        //    {
        //        // 1. 리스트 모드 활성화
        //        SendCommand(":LIST:STATe ON");

        //        // 2. 전류값 시퀀스 생성
        //        var currentValues = string.Join(",", ListSteps.Select(s => s.Current));
        //        SendCommand($":LIST:CURRent {currentValues}");

        //        // 3. 지속시간 설정 (지원하는 경우)
        //        var durationValues = string.Join(",", ListSteps.Select(s => s.Duration));
        //        SendCommand($":LIST:DWELl {durationValues}");

        //        MessageBox.Show($"리스트 업로드 완료: {ListSteps.Count}개 스텝");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"업로드 실패: {ex.Message}");
        //    }
        //}
        //// 장비에 업로드
        //public ICommand UploadToDeviceCommand
        //{
        //    get
        //    {
        //        if (_uploadToDeviceCommand == null)
        //        {
        //            _uploadToDeviceCommand = new RelayCommand(param => UploadToDevice());
        //        }
        //        return _uploadToDeviceCommand;
        //    }
        //}

        //private RelayCommand _downloadFromDeviceCommand = null;
        //private void DownloadFromDevice()
        //{
        //    try
        //    {
        //        // 현재 리스트 조회
        //        string response = Query(":LIST:CURRent?");
        //        var values = response.Split(',');

        //        ListSteps.Clear();
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            ListSteps.Add(new ListStep
        //            {
        //                StepNumber = i + 1,
        //                Current = double.Parse(values[i]),
        //                Duration = 5.0 // 기본값
        //            });
        //        }

        //        MessageBox.Show($"다운로드 완료: {ListSteps.Count}개 스텝");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"다운로드 실패: {ex.Message}");
        //    }
        //}
        //// 장비에서 다운로드
        //public ICommand DownloadFromDeviceCommand
        //{
        //    get
        //    {
        //        if (_downloadFromDeviceCommand == null)
        //        {
        //            _downloadFromDeviceCommand = new RelayCommand(param => DownloadFromDevice());
        //        }
        //        return _downloadFromDeviceCommand;
        //    }
        //}

        //private AsyncRelayCommand _runListCommand = null;
        //private async Task RunList()
        //{
        //    if (ListSteps.Count == 0)
        //    {
        //        MessageBox.Show("실행할 스텝이 없습니다.");
        //        return;
        //    }

        //    IsRunning = true;
        //    CurrentRepeat = 0;

        //    // 리스트 실행 명령
        //    SendCommand(":LIST:RUN");

        //    // 진행 상태 모니터링 (별도 타이머)
        //    StartProgressMonitoring();
        //}
        //// 리스트 실행
        //// NOTE: 기존 코드가 async/await를 사용하므로 AsyncRelayCommand가 더 적합할 수 있습니다. 
        //public ICommand RunListCommand
        //{
        //    get
        //    {
        //        if (_runListCommand == null)
        //        {
        //            _runListCommand = new AsyncRelayCommand(param => RunList());
        //        }
        //        return _runListCommand;
        //    }
        //}

        //private RelayCommand _pauseListCommand = null;
        //private void PauseList()
        //{
        //    // 장비에 따라 일시정지 명령 다름
        //    SendCommand(":LIST:PAUSE");
        //    MessageBox.Show("리스트 일시정지");
        //}
        //// 일시정지
        //public ICommand PauseListCommand
        //{
        //    get
        //    {
        //        if (_pauseListCommand == null)
        //        {
        //            _pauseListCommand = new RelayCommand(param => PauseList());
        //        }
        //        return _pauseListCommand;
        //    }
        //}

        //private RelayCommand _abortListCommand = null;
        //private void AbortList()
        //{
        //    SendCommand(":LIST:ABORt");
        //    IsRunning = false;
        //    CurrentStep = 0;
        //    ProgressPercentage = 0;
        //    MessageBox.Show("리스트 중단됨");
        //}
        //// 중단
        //public ICommand AbortListCommand
        //{
        //    get
        //    {
        //        if (_abortListCommand == null)
        //        {
        //            _abortListCommand = new RelayCommand(param => AbortList());
        //        }
        //        return _abortListCommand;
        //    }
        //}

        private RelayCommand _saveToFileCommand = null;
        private void SaveToFile()
        {
            // NOTE: 'SaveFileDialog'와 'File'에 대한 using 지시문(System.Windows.Forms 또는 Microsoft.Win32 및 System.IO)이 필요합니다.
            // 현재 코드에 using System.Windows;가 있지만, WPF에서는 보통 Microsoft.Win32.SaveFileDialog를 사용합니다.
            var dialog = new Microsoft.Win32.SaveFileDialog // 혹은 using System.Windows.Forms; 를 추가하고 new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = "ListMode.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                var lines = new List<string> { "Step,Current(A),Duration(s)" };
                lines.AddRange(ListSteps.Select(s => $"{s.StepNumber},{s.Current},{s.Duration}"));
                System.IO.File.WriteAllLines(dialog.FileName, lines);
                MessageBox.Show("파일 저장 완료");
            }
        }
        // 파일로 저장
        public ICommand SaveToFileCommand
        {
            get
            {
                if (_saveToFileCommand == null)
                {
                    _saveToFileCommand = new RelayCommand(param => SaveToFile());
                }
                return _saveToFileCommand;
            }
        }

        // ... (생략된 기존 메서드들: StartProgressMonitoring, SendCommand, Query, ShowMessage 등) ...
    }

    // 리스트 스텝 모델
    public class ListStep : ObservableObject
    {
        public int StepNumber { get; set; }

        private double _current = 1.0;
        public double Current
        {
            get => _current;
            set => SetProperty(ref _current, value);
        }

        private double _duration = 5.0;
        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }
    }
}
