using System.Globalization;

namespace CalendarMauiMobile.Converters;

/// <summary>
/// 布林值轉幸運顏色轉換器 ??
/// </summary>
public class LuckyToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLucky)
        {
            return isLucky ? Color.FromArgb("#E8F6E8") : Color.FromArgb("#F8F9FA");
        }
        return Color.FromArgb("#F8F9FA");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 布林值轉幸運邊框顏色轉換器
/// </summary>
public class LuckyToBorderConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLucky)
        {
            return isLucky ? Color.FromArgb("#4CAF50") : Color.FromArgb("#E0E0E0");
        }
        return Color.FromArgb("#E0E0E0");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 布林值轉幸運 Emoji 轉換器
/// </summary>
public class BoolToLuckyEmojiConverter : IValueConverter
{
    private readonly string[] _luckyEmojis = { "??", "?", "??", "??", "??", "?", "??" };
    private readonly Random _random = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLucky && isLucky)
        {
            return _luckyEmojis[_random.Next(_luckyEmojis.Length)];
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 幸運分數轉顏色轉換器
/// </summary>
public class LuckyScoreToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int score)
        {
            return score switch
            {
                >= 1688 => Color.FromArgb("#FF6B6B"), // 超級幸運 - 紅色
                >= 888 => Color.FromArgb("#4ECDC4"),  // 非常幸運 - 青色
                >= 168 => Color.FromArgb("#45B7D1"),  // 頗有運氣 - 藍色
                >= 88 => Color.FromArgb("#96CEB4"),   // 小有幸運 - 綠色
                _ => Color.FromArgb("#FECA57")        // 基礎 - 黃色
            };
        }
        return Color.FromArgb("#95A5A6");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 日期轉中文星期轉換器
/// </summary>
public class DateToChineseWeekConverter : IValueConverter
{
    private readonly string[] _chineseWeeks = 
    {
        "星期日", "星期一", "星期二", "星期三", 
        "星期四", "星期五", "星期六"
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return _chineseWeeks[(int)date.DayOfWeek];
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 數字轉可見性轉換器 (大於0顯示)
/// </summary>
public class NumberToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int number)
        {
            return number > 0;
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 字串為空轉相反布林值轉換器
/// </summary>
public class StringNotEmptyToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrWhiteSpace(value?.ToString());
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 時間轉顯示格式轉換器
/// </summary>
public class TimeToDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            var now = DateTime.Now;
            var diff = dateTime - now;
            
            if (dateTime.Date == now.Date)
            {
                return $"今天 {dateTime:HH:mm}";
            }
            else if (dateTime.Date == now.Date.AddDays(1))
            {
                return $"明天 {dateTime:HH:mm}";
            }
            else if (dateTime.Date == now.Date.AddDays(-1))
            {
                return $"昨天 {dateTime:HH:mm}";
            }
            else if (Math.Abs(diff.Days) <= 7)
            {
                var weekDay = _chineseWeeks[(int)dateTime.DayOfWeek];
                return $"{weekDay} {dateTime:HH:mm}";
            }
            else
            {
                return dateTime.ToString("MM/dd HH:mm");
            }
        }
        return string.Empty;
    }

    private readonly string[] _chineseWeeks = 
    {
        "星期日", "星期一", "星期二", "星期三", 
        "星期四", "星期五", "星期六"
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 集合數量轉字串轉換器
/// </summary>
public class CountToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
        {
            var format = parameter?.ToString() ?? "{0} 個";
            return string.Format(format, count);
        }
        return "0 個";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 幸運分數轉建議文字轉換器
/// </summary>
public class LuckyScoreToAdviceConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int score)
        {
            return score switch
            {
                >= 1688 => "?? 超級幸運！發大財！",
                >= 888 => "?? 非常幸運！事事順利！",
                >= 168 => "? 頗有運氣！一路發！",
                >= 88 => "?? 小有幸運！加油！",
                _ => "?? 創造幸運！888！"
            };
        }
        return "?? 創造幸運！888！";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 多值轉換器 - 用於複雜的條件判斷
/// </summary>
public class MultiValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // 可以根據 parameter 來決定轉換邏輯
        var param = parameter?.ToString();
        
        switch (param)
        {
            case "EventDisplay":
                // 組合事件顯示文字
                if (values.Length >= 3 && 
                    values[0] is string title && 
                    values[1] is DateTime dateTime && 
                    values[2] is bool isLucky)
                {
                    var luckyIcon = isLucky ? " ??" : "";
                    return $"{dateTime:MM/dd HH:mm} - {title}{luckyIcon}";
                }
                break;
                
            case "LuckyPreview":
                // 幸運預覽顯示
                if (values.Length >= 2 && 
                    values[0] is int score && 
                    values[1] is string advice)
                {
                    return $"幸運指數：{score} 分\n{advice}";
                }
                break;
        }
        
        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}