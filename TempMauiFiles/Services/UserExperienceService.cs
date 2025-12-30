using CalendarMauiMobile.Models;

namespace CalendarMauiMobile.Services;

/// <summary>
/// 用戶體驗優化服務 - 提升應用程式的易用性和趣味性 ?
/// </summary>
public interface IUserExperienceService
{
    Task ShowSuccessToastAsync(string message);
    Task ShowErrorToastAsync(string message);
    Task ShowLuckyToastAsync(CalendarEvent calendarEvent);
    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "確定", string cancel = "取消");
    Task PlayHapticFeedbackAsync(HapticFeedbackType type = HapticFeedbackType.Click);
    Task ShowLoadingAsync(string message = "處理中...");
    Task HideLoadingAsync();
    Task AnimateViewAsync(View view, string animationType = "FadeIn");
    string GetMotivationalMessage();
    string GetLuckyTip();
    void TrackUserAction(string action, Dictionary<string, object>? properties = null);
}

public enum HapticFeedbackType
{
    Click,
    LongPress,
    Success,
    Warning,
    Error
}

/// <summary>
/// 用戶體驗優化服務實現
/// </summary>
public class UserExperienceService : IUserExperienceService
{
    private bool _isLoadingVisible = false;

    #region Toast 通知

    public async Task ShowSuccessToastAsync(string message)
    {
        try
        {
            await Application.Current!.MainPage!.DisplayAlert("成功 ?", message, "確定");
            await PlayHapticFeedbackAsync(HapticFeedbackType.Success);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowSuccessToastAsync 錯誤: {ex.Message}");
        }
    }

    public async Task ShowErrorToastAsync(string message)
    {
        try
        {
            await Application.Current!.MainPage!.DisplayAlert("錯誤 ?", message, "確定");
            await PlayHapticFeedbackAsync(HapticFeedbackType.Error);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowErrorToastAsync 錯誤: {ex.Message}");
        }
    }

    public async Task ShowLuckyToastAsync(CalendarEvent calendarEvent)
    {
        try
        {
            if (calendarEvent.IsLucky)
            {
                var luckyEmoji = GetLuckyEmoji(calendarEvent.LuckyScore);
                var message = $"{luckyEmoji} 幸運事件創建！\n" +
                             $"幸運指數：{calendarEvent.LuckyScore} 分\n" +
                             $"{calendarEvent.GetLuckyAdvice()}";
                
                await Application.Current!.MainPage!.DisplayAlert("幸運加持 ??", message, "發發發！");
                await PlayHapticFeedbackAsync(HapticFeedbackType.Success);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowLuckyToastAsync 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region 對話框

    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "確定", string cancel = "取消")
    {
        try
        {
            return await Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowConfirmationAsync 錯誤: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region 觸覺反饋

    public async Task PlayHapticFeedbackAsync(HapticFeedbackType type = HapticFeedbackType.Click)
    {
        try
        {
            // 在真實的 MAUI 應用中，這裡會使用 Microsoft.Maui.Authentication 或第三方庫
            // 目前使用簡單的延遲來模擬觸覺反饋
            await Task.Delay(10);
            
            // 可以根據不同的 type 播放不同的觸覺反饋
            switch (type)
            {
                case HapticFeedbackType.Click:
                    // 輕點反饋
                    break;
                case HapticFeedbackType.LongPress:
                    // 長按反饋
                    break;
                case HapticFeedbackType.Success:
                    // 成功反饋
                    break;
                case HapticFeedbackType.Warning:
                    // 警告反饋
                    break;
                case HapticFeedbackType.Error:
                    // 錯誤反饋
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PlayHapticFeedbackAsync 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region 載入動畫

    public async Task ShowLoadingAsync(string message = "處理中...")
    {
        try
        {
            if (_isLoadingVisible) return;
            
            _isLoadingVisible = true;
            
            // 在真實的應用中，這裡會顯示一個全屏的載入動畫
            // 目前使用簡單的 Debug 輸出
            System.Diagnostics.Debug.WriteLine($"顯示載入動畫: {message}");
            
            await Task.Delay(100); // 模擬動畫顯示時間
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowLoadingAsync 錯誤: {ex.Message}");
        }
    }

    public async Task HideLoadingAsync()
    {
        try
        {
            if (!_isLoadingVisible) return;
            
            _isLoadingVisible = false;
            
            System.Diagnostics.Debug.WriteLine("隱藏載入動畫");
            
            await Task.Delay(100); // 模擬動畫隱藏時間
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"HideLoadingAsync 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region 動畫效果

    public async Task AnimateViewAsync(View view, string animationType = "FadeIn")
    {
        try
        {
            switch (animationType.ToLower())
            {
                case "fadein":
                    view.Opacity = 0;
                    await view.FadeTo(1, 300);
                    break;
                    
                case "fadeout":
                    await view.FadeTo(0, 300);
                    break;
                    
                case "scalein":
                    view.Scale = 0.8;
                    await view.ScaleTo(1, 300, Easing.BounceOut);
                    break;
                    
                case "slideinfromleft":
                    view.TranslationX = -200;
                    await view.TranslateTo(0, 0, 300, Easing.CubicOut);
                    break;
                    
                case "slideinfromright":
                    view.TranslationX = 200;
                    await view.TranslateTo(0, 0, 300, Easing.CubicOut);
                    break;
                    
                case "bounceIn":
                    view.Scale = 0.3;
                    await view.ScaleTo(1.1, 200);
                    await view.ScaleTo(1, 100);
                    break;
                    
                default:
                    await view.FadeTo(1, 300);
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AnimateViewAsync 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region 激勵訊息

    public string GetMotivationalMessage()
    {
        var messages = new[]
        {
            "?? 努力是最好的運氣！",
            "?? 每一天都是新的開始！",
            "?? 相信自己，好運自然來！",
            "?? 堅持目標，必有收穫！",
            "? 今天比昨天更進步！",
            "?? 勇敢追夢，無所畏懼！",
            "?? 你比想像中更強大！",
            "?? 困難過後是彩虹！",
            "?? 保持熱情，持續前進！",
            "?? 每個小成功都值得慶祝！",
            "888 發發發！好運連連！",
            "168 一路發，努力必有回報！",
            "?? 今天的努力是明天的驕傲！"
        };
        
        var random = new Random();
        return messages[random.Next(messages.Length)];
    }

    public string GetLuckyTip()
    {
        var tips = new[]
        {
            "?? 選擇包含數字8的時間，提升幸運指數！",
            "?? 在事件標題加入「發財」、「順利」等關鍵字！",
            "?? 8日、18日、28日是超級幸運日！",
            "? 八月份的任何日期都有額外加成！",
            "?? 正面積極的描述會帶來更多好運！",
            "?? 相信幸運的力量，它會實現！",
            "?? 房東簽約、商務會議等關鍵字有特殊加成！",
            "?? 幸運不是偶然，而是積極心態的體現！",
            "? 每天至少安排一個讓自己開心的事件！",
            "?? 分享好運給他人，幸運會加倍回來！"
        };
        
        var random = new Random();
        return tips[random.Next(tips.Length)];
    }

    #endregion

    #region 使用者行為追蹤

    public void TrackUserAction(string action, Dictionary<string, object>? properties = null)
    {
        try
        {
            // 在真實的應用中，這裡會發送到分析服務（如 Application Insights, Firebase Analytics 等）
            var logMessage = $"用戶行為: {action}";
            
            if (properties != null && properties.Count > 0)
            {
                var propsString = string.Join(", ", properties.Select(p => $"{p.Key}:{p.Value}"));
                logMessage += $" | 屬性: {propsString}";
            }
            
            System.Diagnostics.Debug.WriteLine(logMessage);
            
            // 記錄一些基本統計資訊
            RecordActionStatistics(action, properties);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TrackUserAction 錯誤: {ex.Message}");
        }
    }

    #endregion

    #region 私有方法

    private string GetLuckyEmoji(int score)
    {
        return score switch
        {
            >= 1688 => "??",
            >= 888 => "??",
            >= 168 => "?",
            >= 88 => "??",
            _ => "??"
        };
    }

    private void RecordActionStatistics(string action, Dictionary<string, object>? properties)
    {
        try
        {
            // 這裡可以實現本地統計功能
            // 例如記錄最常用的功能、使用時間等
            var timestamp = DateTime.Now;
            var statEntry = $"{timestamp:yyyy-MM-dd HH:mm:ss} - {action}";
            
            // 可以保存到本地文件或資料庫
            System.Diagnostics.Debug.WriteLine($"統計記錄: {statEntry}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RecordActionStatistics 錯誤: {ex.Message}");
        }
    }

    #endregion
}

/// <summary>
/// 快捷方法擴展類別
/// </summary>
public static class UXExtensions
{
    /// <summary>
    /// 為 View 添加點擊動畫
    /// </summary>
    public static async Task AnimateClickAsync(this View view)
    {
        try
        {
            await view.ScaleTo(0.95, 50);
            await view.ScaleTo(1, 50);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AnimateClickAsync 錯誤: {ex.Message}");
        }
    }
    
    /// <summary>
    /// 為 View 添加彈跳進入動畫
    /// </summary>
    public static async Task BounceInAsync(this View view)
    {
        try
        {
            view.Scale = 0;
            view.IsVisible = true;
            await view.ScaleTo(1.2, 150);
            await view.ScaleTo(1, 100);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BounceInAsync 錯誤: {ex.Message}");
        }
    }
    
    /// <summary>
    /// 為 View 添加滑入動畫
    /// </summary>
    public static async Task SlideInFromBottomAsync(this View view)
    {
        try
        {
            view.TranslationY = 200;
            view.IsVisible = true;
            await view.TranslateTo(0, 0, 300, Easing.CubicOut);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"SlideInFromBottomAsync 錯誤: {ex.Message}");
        }
    }
}