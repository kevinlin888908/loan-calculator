using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarMauiMobile.Models;

/// <summary>
/// 共享的行事曆事件資料模型 - 支援 MVVM 和跨平台 ??
/// </summary>
public class CalendarEvent : INotifyPropertyChanged
{
    private int _id;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTime _dateTime = DateTime.Now;
    private bool _isLucky;
    private int _luckyScore;

    /// <summary>
    /// 事件唯一識別號
    /// </summary>
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    /// <summary>
    /// 事件標題
    /// </summary>
    public string Title
    {
        get => _title;
        set 
        { 
            if (SetProperty(ref _title, value))
            {
                UpdateLuckyProperties();
            }
        }
    }

    /// <summary>
    /// 事件內容描述
    /// </summary>
    public string Description
    {
        get => _description;
        set 
        { 
            if (SetProperty(ref _description, value))
            {
                UpdateLuckyProperties();
            }
        }
    }

    /// <summary>
    /// 事件日期和時間
    /// </summary>
    public DateTime DateTime
    {
        get => _dateTime;
        set 
        { 
            if (SetProperty(ref _dateTime, value))
            {
                UpdateLuckyProperties();
                OnPropertyChanged(nameof(DateString));
                OnPropertyChanged(nameof(TimeString));
                OnPropertyChanged(nameof(FormattedDateTime));
            }
        }
    }

    /// <summary>
    /// 是否為幸運日期
    /// </summary>
    public bool IsLucky
    {
        get => _isLucky;
        private set => SetProperty(ref _isLucky, value);
    }

    /// <summary>
    /// 幸運評分
    /// </summary>
    public int LuckyScore
    {
        get => _luckyScore;
        private set => SetProperty(ref _luckyScore, value);
    }

    // 手機版顯示屬性
    public string DateString => DateTime.ToString("MM/dd");
    public string TimeString => DateTime.ToString("HH:mm");
    public string FormattedDateTime => DateTime.ToString("yyyy年MM月dd日 HH:mm");
    public string ShortDisplay => $"{DateString} {TimeString} - {Title}";

    // 幸運相關常數
    public const int LUCKY_NUMBER_8 = 8;
    public const int LUCKY_NUMBER_88 = 88;
    public const int LUCKY_NUMBER_888 = 888;
    public const int LUCKY_NUMBER_168 = 168;
    public const int LUCKY_NUMBER_1688 = 1688;

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

    private void UpdateLuckyProperties()
    {
        IsLucky = IsLuckyDate();
        LuckyScore = GetLuckyScore();
    }

    /// <summary>
    /// 檢查是否為幸運日期 ??
    /// </summary>
    public bool IsLuckyDate()
    {
        var dateString = DateTime.ToString("yyyyMMdd");
        return dateString.Contains("8") || 
               DateTime.Day == 8 || 
               DateTime.Day == 18 || 
               DateTime.Day == 28 ||
               DateTime.Month == 8;
    }

    /// <summary>
    /// 獲得幸運評分 ??
    /// </summary>
    public int GetLuckyScore()
    {
        var dateString = DateTime.ToString("yyyyMMddHHmm");
        var titleAndDesc = (Title + Description).ToUpper();
        
        int score = 0;
        
        // 日期中的8加分
        foreach (char c in dateString)
        {
            if (c == '8') score += LUCKY_NUMBER_88;
        }
        
        // 特殊日期加分
        if (DateTime.Day == 8) score += LUCKY_NUMBER_168;
        if (DateTime.Month == 8) score += LUCKY_NUMBER_168;
        if (DateTime.Day == 18) score += LUCKY_NUMBER_88;
        if (DateTime.Day == 28) score += LUCKY_NUMBER_88;
        
        // 內容包含幸運關鍵字加分
        if (titleAndDesc.Contains("發") || titleAndDesc.Contains("順利") || 
            titleAndDesc.Contains("成功") || titleAndDesc.Contains("賺錢") ||
            titleAndDesc.Contains("房東") || titleAndDesc.Contains("簽約"))
        {
            score += LUCKY_NUMBER_888;
        }
        
        return Math.Min(score, LUCKY_NUMBER_888 * 8);
    }

    /// <summary>
    /// 獲得幸運建議 ??
    /// </summary>
    public string GetLuckyAdvice()
    {
        var score = GetLuckyScore();
        
        if (score >= 1688) return "?? 超級幸運！今天是發大財的好日子！";
        if (score >= 888) return "?? 非常幸運！事事順利，財源廣進！";
        if (score >= 168) return "? 頗有運氣！一路發發發！";
        if (score >= 88) return "?? 小有幸運！加油努力！";
        
        return "?? 創造自己的幸運！努力就是最好的運氣！";
    }

    public override string ToString()
    {
        return ShortDisplay;
    }
}

/// <summary>
/// 行事曆事件服務 - MAUI 版本
/// </summary>
public class CalendarEventService : INotifyPropertyChanged
{
    private ObservableCollection<CalendarEvent> _events = new();
    private readonly string _dataFilePath;

    public CalendarEventService()
    {
        _dataFilePath = Path.Combine(FileSystem.AppDataDirectory, "calendar_events.json");
        LoadEventsAsync().ConfigureAwait(false);
    }

    public ObservableCollection<CalendarEvent> Events
    {
        get => _events;
        private set
        {
            _events = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<bool> AddEventAsync(CalendarEvent calendarEvent)
    {
        try
        {
            calendarEvent.Id = Events.Count > 0 ? Events.Max(e => e.Id) + 1 : 1;
            Events.Add(calendarEvent);
            await SaveEventsAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateEventAsync(CalendarEvent calendarEvent)
    {
        try
        {
            var existingEvent = Events.FirstOrDefault(e => e.Id == calendarEvent.Id);
            if (existingEvent != null)
            {
                var index = Events.IndexOf(existingEvent);
                Events[index] = calendarEvent;
                await SaveEventsAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteEventAsync(CalendarEvent calendarEvent)
    {
        try
        {
            Events.Remove(calendarEvent);
            await SaveEventsAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<CalendarEvent> GetEventsForDate(DateTime date)
    {
        return Events.Where(e => e.DateTime.Date == date.Date)
                    .OrderBy(e => e.DateTime.TimeOfDay);
    }

    public IEnumerable<CalendarEvent> GetLuckyEvents()
    {
        return Events.Where(e => e.IsLucky)
                    .OrderByDescending(e => e.LuckyScore);
    }

    private async Task SaveEventsAsync()
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Events.ToList(), new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            await File.WriteAllTextAsync(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"保存事件失敗: {ex.Message}");
        }
    }

    private async Task LoadEventsAsync()
    {
        try
        {
            if (File.Exists(_dataFilePath))
            {
                var json = await File.ReadAllTextAsync(_dataFilePath);
                var events = System.Text.Json.JsonSerializer.Deserialize<List<CalendarEvent>>(json);
                if (events != null)
                {
                    Events.Clear();
                    foreach (var evt in events.OrderBy(e => e.DateTime))
                    {
                        Events.Add(evt);
                    }
                }
            }
            else
            {
                await AddSampleEventsAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"載入事件失敗: {ex.Message}");
            await AddSampleEventsAsync();
        }
    }

    private async Task AddSampleEventsAsync()
    {
        var sampleEvents = new[]
        {
            new CalendarEvent 
            { 
                Title = "房東簽約 ??", 
                Description = "房東一月三號下午三點要簽約 - 祝簽約順利發財！", 
                DateTime = new DateTime(2024, 1, 3, 15, 0, 0) 
            },
            new CalendarEvent 
            { 
                Title = "健康檢查 ?", 
                Description = "例行健康檢查 - 身體健康最重要！", 
                DateTime = DateTime.Today.AddDays(1).AddHours(10) 
            },
            new CalendarEvent 
            { 
                Title = "發財會議 ??", 
                Description = "專案進度會議 - 討論如何賺大錢！", 
                DateTime = DateTime.Today.AddDays(3).AddHours(14) 
            },
            new CalendarEvent 
            { 
                Title = "幸運日 ??", 
                Description = "今天是發發發的好日子！", 
                DateTime = new DateTime(2024, 8, 8, 8, 8, 0) 
            }
        };

        foreach (var evt in sampleEvents)
        {
            await AddEventAsync(evt);
        }
    }
}