using System;

namespace loan
{
    /// <summary>
    /// 行事曆事件資料模型 ?? 幸運版本 v8.8.8
    /// </summary>
    public class CalendarEvent
    {
        /// <summary>
        /// 事件唯一識別號 (幸運數字：8, 88, 888, 168, 1688)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 事件標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 事件內容描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 事件日期和時間
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 幸運數字常數 ??
        /// </summary>
        public const int LUCKY_NUMBER_8 = 8;
        public const int LUCKY_NUMBER_88 = 88;
        public const int LUCKY_NUMBER_888 = 888;
        public const int LUCKY_NUMBER_168 = 168;  // 一路發
        public const int LUCKY_NUMBER_1688 = 1688; // 一路發發

        /// <summary>
        /// 建構子
        /// </summary>
        public CalendarEvent()
        {
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// 帶參數的建構子
        /// </summary>
        /// <param name="title">事件標題</param>
        /// <param name="description">事件描述</param>
        /// <param name="dateTime">事件日時間</param>
        public CalendarEvent(string title, string description, DateTime dateTime)
        {
            Title = title;
            Description = description;
            DateTime = dateTime;
        }

        /// <summary>
        /// 覆寫ToString方法，用於顯示事件資訊
        /// </summary>
        /// <returns>事件的字串表示</returns>
        public override string ToString()
        {
            return $"{DateTime:yyyy/MM/dd HH:mm} - {Title}";
        }

        /// <summary>
        /// 獲得格式化的事件詳細資訊 ??
        /// </summary>
        /// <returns>詳細的事件資訊</returns>
        public string GetDetailedInfo()
        {
            return $"時間: {DateTime:yyyy年MM月dd日 HH:mm}\n標題: {Title}\n內容: {Description}\n\n?? 祝您事事順利，發發發！";
        }

        /// <summary>
        /// 檢查是否為幸運日期 (包含8的日期) ??
        /// </summary>
        /// <returns>是否為幸運日期</returns>
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
        /// 獲得幸運評分 (根據包含的8的數量) ??
        /// </summary>
        /// <returns>幸運評分 (0-888分)</returns>
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
                titleAndDesc.Contains("成功") || titleAndDesc.Contains("賺錢"))
            {
                score += LUCKY_NUMBER_888;
            }
            
            return Math.Min(score, LUCKY_NUMBER_888 * 8); // 最高 888*8 = 7104 分
        }

        /// <summary>
        /// 獲得幸運建議 ??
        /// </summary>
        /// <returns>幸運建議文字</returns>
        public string GetLuckyAdvice()
        {
            var score = GetLuckyScore();
            
            if (score >= 1688) return "?? 超級幸運！今天是發大財的好日子！";
            if (score >= 888) return "?? 非常幸運！事事順利，財源廣進！";
            if (score >= 168) return "? 頗有運氣！一路發發發！";
            if (score >= 88) return "?? 小有幸運！加油努力！";
            
            return "?? 創造自己的幸運！努力就是最好的運氣！";
        }
    }
}