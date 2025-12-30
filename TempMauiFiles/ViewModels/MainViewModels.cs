using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CalendarMauiMobile.Models;

namespace CalendarMauiMobile.ViewModels;

/// <summary>
/// 主頁面 ViewModel - 移植自 Windows Forms 版本 ??
/// </summary>
public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly CalendarEventService _eventService;
    private DateTime _selectedDate = DateTime.Today;
    private ObservableCollection<CalendarEvent> _filteredEvents = new();
    private CalendarEvent? _selectedEvent;
    private string _luckyAdvice = "?? 創造自己的幸運！888！";
    private bool _showLuckyEvents = false;

    public MainPageViewModel()
    {
        _eventService = new CalendarEventService();
        
        // 監聽事件服務變化
        _eventService.PropertyChanged += OnEventServiceChanged;
        
        // 初始化命令
        InitializeCommands();
        
        // 載入事件
        LoadEventsForSelectedDate();
        UpdateLuckyAdvice();
        UpdateStatistics();
    }

    #region Properties

    public CalendarEventService EventService => _eventService;

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (SetProperty(ref _selectedDate, value))
            {
                LoadEventsForSelectedDate();
                UpdateLuckyAdvice();
            }
        }
    }

    public ObservableCollection<CalendarEvent> FilteredEvents
    {
        get => _filteredEvents;
        private set => SetProperty(ref _filteredEvents, value);
    }

    public CalendarEvent? SelectedEvent
    {
        get => _selectedEvent;
        set => SetProperty(ref _selectedEvent, value);
    }

    public string LuckyAdvice
    {
        get => _luckyAdvice;
        private set => SetProperty(ref _luckyAdvice, value);
    }

    public bool ShowLuckyEvents
    {
        get => _showLuckyEvents;
        set
        {
            if (SetProperty(ref _showLuckyEvents, value))
            {
                RefreshFilteredEvents();
            }
        }
    }

    // 統計屬性
    public int TotalEvents => _eventService.Events.Count;
    public int LuckyEventsCount => _eventService.Events.Count(e => e.IsLucky);
    public int HighestLuckyScore => _eventService.Events.Any() ? _eventService.Events.Max(e => e.LuckyScore) : 0;
    public int TodayEventsCount => _eventService.GetEventsForDate(DateTime.Today).Count();

    #endregion

    #region Commands

    public ICommand TodayCommand { get; private set; } = null!;
    public ICommand AddEventCommand { get; private set; } = null!;
    public ICommand LuckyEventsCommand { get; private set; } = null!;
    public ICommand EditEventCommand { get; private set; } = null!;
    public ICommand DeleteEventCommand { get; private set; } = null!;
    public ICommand RefreshCommand { get; private set; } = null!;

    private void InitializeCommands()
    {
        TodayCommand = new Command(OnTodayCommand);
        AddEventCommand = new Command(OnAddEventCommand);
        LuckyEventsCommand = new Command(OnLuckyEventsCommand);
        EditEventCommand = new Command<CalendarEvent>(OnEditEventCommand);
        DeleteEventCommand = new Command<CalendarEvent>(OnDeleteEventCommand);
        RefreshCommand = new Command(OnRefreshCommand);
    }

    #endregion

    #region Command Handlers

    private void OnTodayCommand()
    {
        SelectedDate = DateTime.Today;
        ShowLuckyEvents = false;
    }

    private async void OnAddEventCommand()
    {
        try
        {
            // 導航到新增事件頁面
            await Shell.Current.GoToAsync("///AddEventPage");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("錯誤", $"無法開啟新增事件頁面：{ex.Message}", "確定");
        }
    }

    private void OnLuckyEventsCommand()
    {
        ShowLuckyEvents = !ShowLuckyEvents;
    }

    private async void OnEditEventCommand(CalendarEvent? calendarEvent)
    {
        if (calendarEvent == null) return;

        try
        {
            // 導航到編輯事件頁面
            await Shell.Current.GoToAsync($"///AddEventPage?EventId={calendarEvent.Id}");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("錯誤", $"無法開啟編輯頁面：{ex.Message}", "確定");
        }
    }

    private async void OnDeleteEventCommand(CalendarEvent? calendarEvent)
    {
        if (calendarEvent == null) return;

        try
        {
            var result = await Application.Current!.MainPage!.DisplayAlert(
                "確認刪除", 
                $"確定要刪除事件「{calendarEvent.Title}」嗎？", 
                "刪除", 
                "取消");

            if (result)
            {
                var success = await _eventService.DeleteEventAsync(calendarEvent);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("成功", "事件已刪除 ???", "確定");
                    RefreshFilteredEvents();
                    UpdateStatistics();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("錯誤", "刪除事件失敗", "確定");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("錯誤", $"刪除事件時發生錯誤：{ex.Message}", "確定");
        }
    }

    private void OnRefreshCommand()
    {
        RefreshFilteredEvents();
        UpdateStatistics();
        UpdateLuckyAdvice();
    }

    #endregion

    #region Private Methods

    private void OnEventServiceChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CalendarEventService.Events))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RefreshFilteredEvents();
                UpdateStatistics();
                UpdateLuckyAdvice();
            });
        }
    }

    private void LoadEventsForSelectedDate()
    {
        RefreshFilteredEvents();
    }

    private void RefreshFilteredEvents()
    {
        try
        {
            IEnumerable<CalendarEvent> events;

            if (ShowLuckyEvents)
            {
                // 顯示所有幸運事件
                events = _eventService.GetLuckyEvents();
            }
            else
            {
                // 顯示選定日期的事件
                events = _eventService.GetEventsForDate(SelectedDate);
            }

            FilteredEvents.Clear();
            foreach (var evt in events)
            {
                FilteredEvents.Add(evt);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RefreshFilteredEvents 錯誤: {ex.Message}");
        }
    }

    private void UpdateLuckyAdvice()
    {
        try
        {
            var todayEvents = _eventService.GetEventsForDate(DateTime.Today).ToList();
            
            if (!todayEvents.Any())
            {
                LuckyAdvice = "?? 今天還沒有事件，來創造一些精彩的計畫吧！";
                return;
            }

            var luckyEvents = todayEvents.Where(e => e.IsLucky).ToList();
            
            if (luckyEvents.Any())
            {
                var avgScore = (int)luckyEvents.Average(e => e.LuckyScore);
                LuckyAdvice = $"?? 今天有 {luckyEvents.Count} 個幸運事件！平均幸運指數 {avgScore} 分！";
            }
            else
            {
                LuckyAdvice = $"?? 今天有 {todayEvents.Count} 個事件，加油創造更多幸運！";
            }

            // 特殊日期的幸運提示
            if (DateTime.Today.Day == 8 || DateTime.Today.Day == 18 || DateTime.Today.Day == 28)
            {
                LuckyAdvice += " 今天是超級幸運日！888！";
            }
            else if (DateTime.Today.Month == 8)
            {
                LuckyAdvice += " 八月發發發！";
            }
        }
        catch (Exception ex)
        {
            LuckyAdvice = "?? 創造自己的幸運！888！";
            System.Diagnostics.Debug.WriteLine($"UpdateLuckyAdvice 錯誤: {ex.Message}");
        }
    }

    private void UpdateStatistics()
    {
        try
        {
            OnPropertyChanged(nameof(TotalEvents));
            OnPropertyChanged(nameof(LuckyEventsCount));
            OnPropertyChanged(nameof(HighestLuckyScore));
            OnPropertyChanged(nameof(TodayEventsCount));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateStatistics 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}

/// <summary>
/// 新增/編輯事件頁面 ViewModel
/// </summary>
public class AddEventViewModel : INotifyPropertyChanged
{
    private readonly CalendarEventService _eventService;
    private CalendarEvent? _editingEvent;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTime _eventDate = DateTime.Today;
    private TimeSpan _eventTime = DateTime.Now.TimeOfDay;
    private int _previewLuckyScore = 0;
    private string _previewLuckyAdvice = string.Empty;

    public AddEventViewModel()
    {
        _eventService = new CalendarEventService();
        
        // 初始化命令
        InitializeCommands();
        
        // 更新預覽
        UpdateLuckyPreview();
    }

    #region Properties

    public string Title
    {
        get => _title;
        set
        {
            if (SetProperty(ref _title, value))
            {
                UpdateLuckyPreview();
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (SetProperty(ref _description, value))
            {
                UpdateLuckyPreview();
            }
        }
    }

    public DateTime EventDate
    {
        get => _eventDate;
        set
        {
            if (SetProperty(ref _eventDate, value))
            {
                UpdateLuckyPreview();
            }
        }
    }

    public TimeSpan EventTime
    {
        get => _eventTime;
        set
        {
            if (SetProperty(ref _eventTime, value))
            {
                UpdateLuckyPreview();
            }
        }
    }

    public int PreviewLuckyScore
    {
        get => _previewLuckyScore;
        private set => SetProperty(ref _previewLuckyScore, value);
    }

    public string PreviewLuckyAdvice
    {
        get => _previewLuckyAdvice;
        private set => SetProperty(ref _previewLuckyAdvice, value);
    }

    public bool ShowLuckyPreview => !string.IsNullOrWhiteSpace(Title) || !string.IsNullOrWhiteSpace(Description);

    public bool IsEditing => _editingEvent != null;
    public string PageTitle => IsEditing ? "?? 編輯事件" : "? 新增事件";
    public string SaveButtonText => IsEditing ? "?? 更新事件" : "?? 保存事件";

    #endregion

    #region Commands

    public ICommand SaveCommand { get; private set; } = null!;
    public ICommand ClearCommand { get; private set; } = null!;

    private void InitializeCommands()
    {
        SaveCommand = new Command(OnSaveCommand, CanSave);
        ClearCommand = new Command(OnClearCommand);
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Description);
    }

    #endregion

    #region Command Handlers

    private async void OnSaveCommand()
    {
        try
        {
            var eventDateTime = EventDate.Add(EventTime);
            
            if (IsEditing && _editingEvent != null)
            {
                // 編輯模式
                _editingEvent.Title = Title.Trim();
                _editingEvent.Description = Description.Trim();
                _editingEvent.DateTime = eventDateTime;
                
                var success = await _eventService.UpdateEventAsync(_editingEvent);
                if (success)
                {
                    await Application.Current!.MainPage!.DisplayAlert("成功", "事件更新成功！??", "確定");
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("錯誤", "更新事件失敗", "確定");
                }
            }
            else
            {
                // 新增模式
                var newEvent = new CalendarEvent
                {
                    Title = Title.Trim(),
                    Description = Description.Trim(),
                    DateTime = eventDateTime
                };
                
                var success = await _eventService.AddEventAsync(newEvent);
                if (success)
                {
                    var message = newEvent.IsLucky ? 
                        $"事件新增成功！?? 幸運指數：{newEvent.LuckyScore} 分！" : 
                        "事件新增成功！?";
                    
                    await Application.Current!.MainPage!.DisplayAlert("成功", message, "確定");
                    ClearForm();
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("錯誤", "新增事件失敗", "確定");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("錯誤", $"保存事件時發生錯誤：{ex.Message}", "確定");
        }
    }

    private void OnClearCommand()
    {
        ClearForm();
    }

    #endregion

    #region Public Methods

    public void LoadEventForEdit(CalendarEvent calendarEvent)
    {
        _editingEvent = calendarEvent;
        Title = calendarEvent.Title;
        Description = calendarEvent.Description;
        EventDate = calendarEvent.DateTime.Date;
        EventTime = calendarEvent.DateTime.TimeOfDay;
        
        OnPropertyChanged(nameof(IsEditing));
        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(SaveButtonText));
    }

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        Title = string.Empty;
        Description = string.Empty;
        EventDate = DateTime.Today;
        EventTime = DateTime.Now.TimeOfDay;
        _editingEvent = null;
        
        OnPropertyChanged(nameof(IsEditing));
        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(SaveButtonText));
    }

    private void UpdateLuckyPreview()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Description))
            {
                PreviewLuckyScore = 0;
                PreviewLuckyAdvice = "輸入事件資訊查看幸運預測";
                OnPropertyChanged(nameof(ShowLuckyPreview));
                return;
            }

            // 創建臨時事件來計算幸運指數
            var tempEvent = new CalendarEvent
            {
                Title = Title ?? string.Empty,
                Description = Description ?? string.Empty,
                DateTime = EventDate.Add(EventTime)
            };

            PreviewLuckyScore = tempEvent.GetLuckyScore();
            PreviewLuckyAdvice = tempEvent.GetLuckyAdvice();
            
            OnPropertyChanged(nameof(ShowLuckyPreview));
            
            // 更新保存命令的可用狀態
            ((Command)SaveCommand).ChangeCanExecute();
        }
        catch (Exception ex)
        {
            PreviewLuckyScore = 0;
            PreviewLuckyAdvice = "計算幸運指數時發生錯誤";
            System.Diagnostics.Debug.WriteLine($"UpdateLuckyPreview 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}