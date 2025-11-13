using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Source_Load_Test.ViewModel
{
    public class LoadListModeViewModel : ObservableObject
    {
        public event EventHandler<bool> EventListModeStarted;

        public LoadListModeViewModel()
        {
            RefreshListsFromDevice();
        }

        // ========================================
        // 저장된 리스트 목록
        // ========================================
        private ObservableCollection<LIST> _savedLists = new ObservableCollection<LIST>();
        public ObservableCollection<LIST> SavedLists
        {
            get => _savedLists;
            set => SetProperty(ref _savedLists, value);
        }

        private LIST _selectedList;
        public LIST SelectedList
        {
            get => _selectedList;
            set => SetProperty(ref _selectedList, value);
        }

        // ========================================
        // 현재 편집 중인 리스트
        // ========================================
        private ObservableCollection<ListStep> _listSteps = new ObservableCollection<ListStep>();
        public ObservableCollection<ListStep> ListSteps
        {
            get => _listSteps;
            set => SetProperty(ref _listSteps, value);
        }

        private ListStep _selectedStep;
        public ListStep SelectedStep
        {
            get => _selectedStep;
            set => SetProperty(ref _selectedStep, value);
        }

        private int _currentListNumber = 0;
        public int CurrentListNumber
        {
            get => _currentListNumber;
            set => SetProperty(ref _currentListNumber, value);
        }

        private string _currentListName = "새 리스트";
        public string CurrentListName
        {
            get => _currentListName;
            set => SetProperty(ref _currentListName, value);
        }

        // ========================================
        // 반복 모드
        // ========================================
        private bool _isOneShot = true;
        public bool IsOneShot
        {
            get => _isOneShot;
            set => SetProperty(ref _isOneShot, value);
        }

        private bool _isContinuous = false;
        public bool IsContinuous
        {
            get => _isContinuous;
            set => SetProperty(ref _isContinuous, value);
        }

        private bool _isRepeatCount = false;
        public bool IsRepeatCount
        {
            get => _isRepeatCount;
            set => SetProperty(ref _isRepeatCount, value);
        }

        private int _repeatCount = 5;
        public int RepeatCount
        {
            get => _repeatCount;
            set => SetProperty(ref _repeatCount, value);
        }

        // ========================================
        // 실행 상태
        // ========================================
        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                SetProperty(ref _isRunning, value);
                OnPropertyChanged(nameof(CanRun));
            }
        }

        public bool CanRun => ListSteps.Count > 0 && !IsRunning;

        private int _currentStep = 0;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                SetProperty(ref _currentStep, value);
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string Progress => $"{CurrentStep}/{ListSteps.Count}";

        private int _currentRepeat = 0;
        public int CurrentRepeat
        {
            get => _currentRepeat;
            set => SetProperty(ref _currentRepeat, value);
        }

        private double _progressPercentage = 0;
        public double ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }

        // ========================================
        // Window Loaded - 장비에서 리스트 목록 자동 조회
        // ========================================

        // ========================================
        // 장비에서 리스트 목록 새로고침
        // ========================================
        private RelayCommand _refreshListsCommand = null;
        private async Task RefreshListsFromDevice()
        {
            try
            {
                // 장비에서 리스트 가져오기
                List<LIST> lists = await DeviceManager.Load.GetListMemo();

                SavedLists.Clear();

                if (lists == null || lists.Count == 0)
                {
                    MessageBox.Show("저장된 리스트가 없습니다.", "정보",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // ObservableCollection에 추가
                foreach (var list in lists)
                {
                    SavedLists.Add(list);
                }

                MessageBox.Show($"{SavedLists.Count}개의 리스트를 찾았습니다.", "완료",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"리스트 목록 조회 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand RefreshListsCommand
        {
            get
            {
                if (_refreshListsCommand == null)
                {
                    _refreshListsCommand = new RelayCommand(param => RefreshListsFromDevice());
                }
                return _refreshListsCommand;
            }
        }

        // ========================================
        // 선택한 리스트 불러오기
        // ========================================
        private RelayCommand _loadListCommand = null;
        private void LoadList()
        {
            if (SelectedList == null)
            {
                MessageBox.Show("리스트를 선택하세요.", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 1. 현재 리스트 정보 업데이트
                CurrentListNumber = SelectedList.ListNumber;
                CurrentListName = SelectedList.ListName;

                // 2. 스텝 목록 복사
                ListSteps.Clear();
                foreach (var step in SelectedList.Steps)
                {
                    ListSteps.Add(new ListStep
                    {
                        StepNumber = step.StepNumber,
                        Mode = step.Mode,
                        Value = step.Value,
                        Time = step.Time
                    });
                }

                MessageBox.Show($"리스트 '{CurrentListName}' ({ListSteps.Count}개 스텝)을 불러왔습니다.",
                    "완료", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"리스트 불러오기 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand LoadListCommand
        {
            get
            {
                if (_loadListCommand == null)
                {
                    _loadListCommand = new RelayCommand(param => LoadList());
                }
                return _loadListCommand;
            }
        }

        // ========================================
        // 새 리스트 만들기
        // ========================================
        private RelayCommand _createNewListCommand = null;
        private void CreateNewList()
        {
            var result = MessageBox.Show(
                "현재 편집 중인 내용이 삭제됩니다. 계속하시겠습니까?",
                "확인",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ListSteps.Clear();
                CurrentListNumber = 0; // 0 = 새 리스트
                CurrentListName = "새 리스트";
                SelectedList = null;

                // 기본 스텝 추가
                AddStep();
            }
        }

        public ICommand CreateNewListCommand
        {
            get
            {
                if (_createNewListCommand == null)
                {
                    _createNewListCommand = new RelayCommand(param => CreateNewList());
                }
                return _createNewListCommand;
            }
        }

        // ========================================
        // 스텝 추가
        // ========================================
        private RelayCommand _addStepCommand = null;
        private void AddStep()
        {
            var newStep = new ListStep
            {
                StepNumber = ListSteps.Count + 1,
                Mode = "CC", // 기본 모드
                Value = 1.0f,
                Time = 5.0f
            };
            ListSteps.Add(newStep);
            OnPropertyChanged(nameof(CanRun));
        }

        public ICommand AddStepCommand
        {
            get
            {
                if (_addStepCommand == null)
                {
                    _addStepCommand = new RelayCommand(param => AddStep());
                }
                return _addStepCommand;
            }
        }

        // ========================================
        // 스텝 제거
        // ========================================
        private RelayCommand _removeStepCommand = null;
        private void RemoveStep(object parameter)
        {
            var step = parameter as ListStep;
            if (step != null)
            {
                ListSteps.Remove(step);

                // 스텝 번호 재정렬
                for (int i = 0; i < ListSteps.Count; i++)
                {
                    ListSteps[i].StepNumber = i + 1;
                }

                OnPropertyChanged(nameof(CanRun));
            }
        }

        public ICommand RemoveStepCommand
        {
            get
            {
                if (_removeStepCommand == null)
                {
                    _removeStepCommand = new RelayCommand(param => RemoveStep(param));
                }
                return _removeStepCommand;
            }
        }

        // ========================================
        // 전체 삭제
        // ========================================
        private RelayCommand _clearAllStepsCommand = null;
        private void ClearAllSteps()
        {
            var result = MessageBox.Show(
                "모든 스텝을 삭제하시겠습니까?",
                "확인",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ListSteps.Clear();
                OnPropertyChanged(nameof(CanRun));
            }
        }

        public ICommand ClearAllStepsCommand
        {
            get
            {
                if (_clearAllStepsCommand == null)
                {
                    _clearAllStepsCommand = new RelayCommand(param => ClearAllSteps());
                }
                return _clearAllStepsCommand;
            }
        }

        // ========================================
        // 장비에 저장
        // ========================================
        private RelayCommand _saveToDeviceCommand = null;
        private void SaveToDevice()
        {
            if (ListSteps.Count == 0)
            {
                MessageBox.Show("저장할 스텝이 없습니다.", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // TODO: 장비에 리스트 저장 로직 구현
                // 장비 SCPI 명령어에 따라 구현 필요

                MessageBox.Show($"리스트 '{CurrentListName}'이 장비에 저장되었습니다.",
                    "완료", MessageBoxButton.OK, MessageBoxImage.Information);

                // 목록 새로고침
                RefreshListsFromDevice();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveToDeviceCommand
        {
            get
            {
                if (_saveToDeviceCommand == null)
                {
                    _saveToDeviceCommand = new RelayCommand(param => SaveToDevice());
                }
                return _saveToDeviceCommand;
            }
        }

        // ========================================
        // 리스트 실행
        // ========================================
        private RelayCommand _runListCommand = null;
        private void RunList()
        {
            if (ListSteps.Count == 0)
            {
                MessageBox.Show("실행할 스텝이 없습니다.", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 1. 리스트 모드 활성화
                DeviceManager.Load.ListOnOff("ON");

                // 2. 리스트 실행 (장비 메소드 호출 - 구현 필요)
                // TODO: 리스트 실행 SCPI 명령 추가

                IsRunning = true;
                CurrentStep = 0;
                CurrentRepeat = 0;

                MessageBox.Show("리스트 실행이 시작되었습니다.", "완료",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // 3. 모니터링 시작 (부모 ViewModel 그래프 업데이트)
                EventListModeStarted?.Invoke(this, IsRunning); // 이벤트처리

                Close();
            }
            catch (Exception ex)
            {
                IsRunning = false;
                MessageBox.Show($"실행 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand RunListCommand
        {
            get
            {
                if (_runListCommand == null)
                {
                    _runListCommand = new RelayCommand(param => RunList());
                }
                return _runListCommand;
            }
        }

        // ========================================
        // 일시정지
        // ========================================
        private RelayCommand _pauseListCommand = null;
        private void PauseList()
        {
            try
            {
                // TODO: 일시정지 SCPI 명령 구현
                MessageBox.Show("리스트 일시정지", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"일시정지 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand PauseListCommand
        {
            get
            {
                if (_pauseListCommand == null)
                {
                    _pauseListCommand = new RelayCommand(param => PauseList());
                }
                return _pauseListCommand;
            }
        }

        // ========================================
        // 중단
        // ========================================
        private RelayCommand _abortListCommand = null;
        private void AbortList()
        {
            try
            {
                // 리스트 모드 비활성화
                DeviceManager.Load.ListOnOff("OFF");

                IsRunning = false;
                CurrentStep = 0;
                ProgressPercentage = 0;

                EventListModeStarted?.Invoke(this, IsRunning); // 이벤트처리

                MessageBox.Show("리스트 중단됨", "알림",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"중단 실패: {ex.Message}", "오류",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand AbortListCommand
        {
            get
            {
                if (_abortListCommand == null)
                {
                    _abortListCommand = new RelayCommand(param => AbortList());
                }
                return _abortListCommand;
            }
        }

        // ========================================
        // 파일로 저장
        // ========================================
        private RelayCommand _saveToFileCommand = null;
        private void SaveToFile()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = $"ListMode_{CurrentListName}.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                var lines = new List<string> { "Step,Mode,Value,Time(s)" };
                lines.AddRange(ListSteps.Select(s => $"{s.StepNumber},{s.Mode},{s.Value},{s.Time}"));
                System.IO.File.WriteAllLines(dialog.FileName, lines);
                MessageBox.Show("파일 저장 완료", "완료",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

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

        // ========================================
        // 닫기
        // ========================================
        private RelayCommand _closeCommand = null;
        private void Close()
        {
            // 실행 중이면 중단
            if (IsRunning)
            {
                var result = MessageBox.Show(
                    "리스트가 실행 중입니다. 중단하고 닫으시겠습니까?",
                    "확인",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    AbortList();
                }
                else
                {
                    return;
                }
            }

            // Window 닫기
            Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)?.Close();
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => Close());
                }
                return _closeCommand;
            }
        }

    }

    // ========================================
    // 리스트 스텝 모델
    // ========================================
    public class ListStep : ObservableObject
    {
        private int _stepNumber = 0;
        public int StepNumber
        {
            get => _stepNumber;
            set => SetProperty(ref _stepNumber, value);
        }

        private string _mode = "CC";
        public string Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        private float _value = 1.0f;
        public float Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private float _time = 5.0f;
        public float Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }
    }

    // ========================================
    // 리스트 모델
    // ========================================
    public class LIST : ObservableObject
    {
        private List<ListStep> _steps = new List<ListStep>();
        public List<ListStep> Steps
        {
            get => _steps;
            set => SetProperty(ref _steps, value);
        }

        private int _listNumber = 0;
        public int ListNumber
        {
            get => _listNumber;
            set => SetProperty(ref _listNumber, value);
        }

        private string _listName = "";
        public string ListName
        {
            get => _listName;
            set => SetProperty(ref _listName, value);
        }

        // ComboBox 표시용
        public string DisplayName => $"List {ListNumber} - {ListName} ({Steps?.Count ?? 0} steps)";
        public int StepCount => Steps?.Count ?? 0;
    }
}