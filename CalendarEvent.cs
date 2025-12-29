using System;

namespace loan
{
    /// <summary>
    /// 代表行事曆事件的類別
    /// </summary>
    public class CalendarEvent
    {
        /// <summary>
        /// 事件唯一識別碼
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
        /// <param name="dateTime">事件日期時間</param>
        public CalendarEvent(string title, string description, DateTime dateTime)
        {
            Title = title;
            Description = description;
            DateTime = dateTime;
        }

        /// <summary>
        /// 覆寫ToString方法，用於顯示事件資訊
        /// </summary>
        /// <returns>事件的字符串表示</returns>
        public override string ToString()
        {
            return $"{DateTime:yyyy/MM/dd HH:mm} - {Title}";
        }

        /// <summary>
        /// 取得格式化的事件詳細資訊
        /// </summary>
        /// <returns>詳細的事件資訊</returns>
        public string GetDetailedInfo()
        {
            return $"時間: {DateTime:yyyy年MM月dd日 HH:mm}\n標題: {Title}\n內容: {Description}";
        }
    }
}