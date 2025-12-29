using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace loan
{
    public partial class Form1 : Form
    {
        private List<CalendarEvent> events;
        private int nextEventId;
        private CalendarEvent? selectedEvent;
        private bool isEditMode;

        public Form1()
        {
            InitializeComponent();
            events = new List<CalendarEvent>();
            nextEventId = 1;
            selectedEvent = null;
            isEditMode = false;
            
            // 初始化功能表
            InitializeMenuStrip();
        }
        
        /// <summary>
        /// 初始化功能表列
        /// </summary>
        private void InitializeMenuStrip()
        {
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Top;  // 確保功能表停靠在頂部
            menuStrip.BackColor = SystemColors.MenuBar;  // 設定標準背景色
            
            // 檔案功能表
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("檔案(&F)");
            
            ToolStripMenuItem exportMenuItem = new ToolStripMenuItem("匯出事件到文字檔(&E)");
            exportMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            exportMenuItem.Click += (s, e) => ExportEventsToFile();
            
            ToolStripMenuItem importMenuItem = new ToolStripMenuItem("從文字檔匯入事件(&I)");
            importMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            importMenuItem.Click += (s, e) => ImportEventsFromFile();
            
            ToolStripSeparator separator = new ToolStripSeparator();
            
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("結束(&X)");
            exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitMenuItem.Click += (s, e) => this.Close();
            
            fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] 
            {
                exportMenuItem,
                importMenuItem,
                separator,
                exitMenuItem
            });
            
            // 說明功能表
            ToolStripMenuItem helpMenuItem = new ToolStripMenuItem("說明(&H)");
            
            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("關於(&A)");
            aboutMenuItem.Click += (s, e) => ShowAboutDialog();
            
            helpMenuItem.DropDownItems.Add(aboutMenuItem);
            
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem, helpMenuItem });
            
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
            
            // 確保功能表在最上層
            menuStrip.BringToFront();
        }
        
        /// <summary>
        /// 顯示關於對話框
        /// </summary>
        private void ShowAboutDialog()
        {
            string aboutText = "行事曆管理系統 v1.0\n\n" +
                             "功能特色：\n" +
                             "? 新增、編輯、刪除事件\n" +
                             "? 匯出事件到文字檔 (Ctrl+E)\n" +
                             "? 從文字檔匯入事件 (Ctrl+I)\n" +
                             "? 行事曆日期選擇\n" +
                             "? 事件詳細資訊顯示\n\n" +
                             "快捷鍵：\n" +
                             "? Ctrl+N: 新增事件\n" +
                             "? F2: 編輯事件\n" +
                             "? Delete: 刪除事件\n" +
                             "? ESC: 取消編輯\n\n" +
                             "基於 .NET 8.0 與 Windows Forms 開發";
                             
            MessageBox.Show(aboutText, "關於行事曆管理系統", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化日期時間選擇器為當前日期時間
            dateTimePickerEvent.Value = DateTime.Now;
            
            // 藉由當前日期時間載入事件列表
            LoadEventsByDate(DateTime.Now);
            
            // 設定初始狀態
            UpdateButtonStates();
            
            // 新增一些範例事件來展示功能
            AddSampleEvents();
        }
        
        /// <summary>
        /// 新增範例事件用於展示 ?? 幸運版本
        /// </summary>
        private void AddSampleEvents()
        {
            var sampleEvents = new List<CalendarEvent>
            {
                new CalendarEvent("房東簽約 ??", "房東一月三號下午三點要簽約 - 祝簽約順利發財！", new DateTime(2024, 1, 3, 15, 0, 0)),
                new CalendarEvent("醫生檢查 ?", "例行健康檢查 - 身體健康最重要！", DateTime.Today.AddDays(1).AddHours(10)),
                new CalendarEvent("發財會議 ??", "專案進度會議 - 討論如何賺大錢！", DateTime.Today.AddDays(3).AddHours(14)),
                new CalendarEvent("幸運日 ??", "今天是發發發的好日子！", new DateTime(2024, 8, 8, 8, 8, 0)),
                new CalendarEvent("一路發 ??", "重要商務會議 - 一路發發發！", new DateTime(2024, 1, 18, 16, 8, 0))
            };

            foreach (var evt in sampleEvents)
            {
                evt.Id = nextEventId++;
                events.Add(evt);
                
                // 顯示幸運資訊 ??
                if (evt.IsLuckyDate())
                {
                    System.Diagnostics.Debug.WriteLine($"?? 幸運事件：{evt.Title} - {evt.GetLuckyAdvice()}");
                }
            }
            
            RefreshEventList();
        }

        private void BtnAddEvent_Click(object sender, EventArgs e)
        {
            if (ValidateEventInput())
            {
                if (isEditMode && selectedEvent != null)
                {
                    // 編輯模式：更新現有事件
                    selectedEvent.Title = txtEventTitle.Text.Trim();
                    selectedEvent.Description = txtEventDescription.Text.Trim();
                    selectedEvent.DateTime = dateTimePickerEvent.Value;
                    
                    MessageBox.Show("事件更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ExitEditMode();
                }
                else
                {
                    // 新增模式：建立新事件
                    var newEvent = new CalendarEvent
                    {
                        Id = nextEventId++,
                        Title = txtEventTitle.Text.Trim(),
                        Description = txtEventDescription.Text.Trim(),
                        DateTime = dateTimePickerEvent.Value
                    };
                    
                    events.Add(newEvent);
                    MessageBox.Show("事件新增成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                ClearInputFields();
                RefreshEventList();
                UpdateCalendarDisplay();
            }
        }

        private void BtnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (selectedEvent != null)
            {
                var result = MessageBox.Show($"確定要刪除事件 \"{selectedEvent.Title}\" 嗎？", 
                    "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    events.Remove(selectedEvent);
                    selectedEvent = null;
                    
                    ClearInputFields();
                    RefreshEventList();
                    UpdateCalendarDisplay();
                    UpdateButtonStates();
                    
                    MessageBox.Show("事件已刪除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("請先選擇要刪除的事件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEditEvent_Click(object sender, EventArgs e)
        {
            if (selectedEvent != null)
            {
                EnterEditMode();
            }
            else
            {
                MessageBox.Show("請先選擇要編輯的事件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ListBoxEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEvents.SelectedItem is CalendarEvent selectedEventItem)
            {
                selectedEvent = selectedEventItem;
                DisplayEventDetails();
                UpdateButtonStates();
            }
        }

        private void CalendarControl_DateChanged(object sender, DateRangeEventArgs e)
        {
            // 當行事曆日期改變時，更新日期時間選擇器的日期部分
            var selectedDate = calendarControl.SelectionStart.Date;
            var currentTime = dateTimePickerEvent.Value.TimeOfDay;
            dateTimePickerEvent.Value = selectedDate.Add(currentTime);
            
            // 藉由選定日期過濾事件
            LoadEventsByDate(selectedDate);
        }

        /// <summary>
        /// 藉由指定日期載入事件
        /// </summary>
        /// <param name="date">選定日期</param>
        private void LoadEventsByDate(DateTime date)
        {
            // 先將事件列表清空
            listBoxEvents.Items.Clear();
            
            // 篩選指定日期的事件
            var dayEvents = events.Where(e => e.DateTime.Date == date.Date)
                                 .OrderBy(e => e.DateTime.TimeOfDay)
                                 .ToList();
            
            foreach (var evt in dayEvents)
            {
                listBoxEvents.Items.Add(evt);
            }
            
            if (dayEvents.Count == 0)
            {
                lblEventDetails.Text = $"{date:yyyy年MM月dd日} 沒有安排的事件";
            }
            else
            {
                lblEventDetails.Text = $"{date:yyyy年MM月dd日} 共有 {dayEvents.Count} 個事件";
            }
        }

        private bool ValidateEventInput()
        {
            if (string.IsNullOrWhiteSpace(txtEventTitle.Text))
            {
                MessageBox.Show("請輸入事件標題。", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEventTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEventDescription.Text))
            {
                MessageBox.Show("請輸入事件內容。", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEventDescription.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            txtEventTitle.Clear();
            txtEventDescription.Clear();
            dateTimePickerEvent.Value = DateTime.Now;
        }

        private void RefreshEventList()
        {
            listBoxEvents.Items.Clear();
            
            // 按日期時間排序事件
            var sortedEvents = events.OrderBy(e => e.DateTime).ToList();
            
            foreach (var evt in sortedEvents)
            {
                listBoxEvents.Items.Add(evt);
            }
            
            // 更新狀態顯示
            UpdateEventCountDisplay();
        }
        
        /// <summary>
        /// 更新事件計數顯示
        /// </summary>
        private void UpdateEventCountDisplay()
        {
            var totalEvents = events.Count;
            var todayEvents = events.Count(e => e.DateTime.Date == DateTime.Today);
            var upcomingEvents = events.Count(e => e.DateTime.Date > DateTime.Today);
            
            groupBoxEventList.Text = $"事件列表 (總共 {totalEvents} 個事件，今天 {todayEvents} 個，未來 {upcomingEvents} 個)";
        }

        private void DisplayEventDetails()
        {
            if (selectedEvent != null)
            {
                var detailText = selectedEvent.GetDetailedInfo();
                
                // 加入幸運資訊 ??
                if (selectedEvent.IsLuckyDate())
                {
                    detailText += $"\n\n?? 幸運指數：{selectedEvent.GetLuckyScore()} 分";
                    detailText += $"\n?? 幸運建議：{selectedEvent.GetLuckyAdvice()}";
                }
                
                lblEventDetails.Text = detailText;
            }
            else
            {
                lblEventDetails.Text = "請選擇一個事件查看詳細資訊\n\n?? 創造自己的幸運！888！";
            }
        }

        private void UpdateButtonStates()
        {
            btnDeleteEvent.Enabled = selectedEvent != null;
            btnEditEvent.Enabled = selectedEvent != null && !isEditMode;
        }

        private void EnterEditMode()
        {
            if (selectedEvent != null)
            {
                isEditMode = true;
                
                // 將選定事件的資料載入到輸入欄位
                txtEventTitle.Text = selectedEvent.Title;
                txtEventDescription.Text = selectedEvent.Description;
                dateTimePickerEvent.Value = selectedEvent.DateTime;
                
                // 更新按鈕文字和狀態
                btnAddEvent.Text = "更新事件";
                groupBoxEventInput.Text = "編輯事件";
                UpdateButtonStates();
                
                txtEventTitle.Focus();
            }
        }

        private void ExitEditMode()
        {
            isEditMode = false;
            selectedEvent = null;
            
            // 恢復按鈕文字和狀態
            btnAddEvent.Text = "新增事件";
            groupBoxEventInput.Text = "新增事件";
            UpdateButtonStates();
        }

        private void UpdateCalendarDisplay()
        {
            // 可以在此處實作在行事曆上標記有事件的日期
            // 但MonthCalendar控制項的自定義顯示功能有限
            // 這裡保留此方法以備將來擴展使用
        }

        /// <summary>
        /// 匯出事件到文字檔
        /// </summary>
        private void ExportEventsToFile()
        {
            if (events.Count == 0)
            {
                MessageBox.Show("沒有事件可以匯出。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
                saveDialog.Title = "匯出事件到文字檔";
                saveDialog.FileName = $"行事曆事件_{DateTime.Now:yyyyMMdd}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder content = new StringBuilder();
                        content.AppendLine("=== 行事曆事件列表 ===");
                        content.AppendLine($"匯出時間：{DateTime.Now:yyyy年MM月dd日 HH:mm:ss}");
                        content.AppendLine($"總事件數：{events.Count}");
                        content.AppendLine();

                        var sortedEvents = events.OrderBy(e => e.DateTime).ToList();
                        
                        foreach (var evt in sortedEvents)
                        {
                            content.AppendLine($"事件編號：{evt.Id}");
                            content.AppendLine($"標題：{evt.Title}");
                            content.AppendLine($"時間：{evt.DateTime:yyyy年MM月dd日 dddd HH:mm}");
                            content.AppendLine($"內容：{evt.Description}");
                            content.AppendLine(new string('-', 40));
                        }

                        File.WriteAllText(saveDialog.FileName, content.ToString(), Encoding.UTF8);
                        MessageBox.Show($"事件已成功匯出到：\n{saveDialog.FileName}", "匯出成功", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"匯出檔案時發生錯誤：\n{ex.Message}", "錯誤", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 從文字檔匯入事件
        /// </summary>
        private void ImportEventsFromFile()
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
                openDialog.Title = "從文字檔匯入事件";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(openDialog.FileName, Encoding.UTF8);
                        List<CalendarEvent> importedEvents = new List<CalendarEvent>();
                        
                        CalendarEvent currentEvent = null;
                        
                        foreach (string line in lines)
                        {
                            string trimmedLine = line.Trim();
                            
                            if (trimmedLine.StartsWith("事件編號："))
                            {
                                currentEvent = new CalendarEvent();
                                if (int.TryParse(trimmedLine.Substring(5), out int id))
                                {
                                    currentEvent.Id = id;
                                }
                            }
                            else if (trimmedLine.StartsWith("標題：") && currentEvent != null)
                            {
                                currentEvent.Title = trimmedLine.Substring(3);
                            }
                            else if (trimmedLine.StartsWith("時間：") && currentEvent != null)
                            {
                                string timeString = trimmedLine.Substring(3);
                                // 嘗試解析時間格式
                                if (DateTime.TryParse(timeString, out DateTime parsedTime))
                                {
                                    currentEvent.DateTime = parsedTime;
                                }
                            }
                            else if (trimmedLine.StartsWith("內容：") && currentEvent != null)
                            {
                                currentEvent.Description = trimmedLine.Substring(3);
                                importedEvents.Add(currentEvent);
                                currentEvent = null;
                            }
                        }

                        if (importedEvents.Count > 0)
                        {
                            var result = MessageBox.Show($"找到 {importedEvents.Count} 個事件。\n是否要匯入這些事件？", 
                                "確認匯入", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            
                            if (result == DialogResult.Yes)
                            {
                                foreach (var evt in importedEvents)
                                {
                                    evt.Id = nextEventId++;
                                    events.Add(evt);
                                }
                                
                                RefreshEventList();
                                MessageBox.Show($"成功匯入 {importedEvents.Count} 個事件！", "匯入成功", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("在選定的檔案中沒有找到有效的事件資料。", "匯入失敗", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"匯入檔案時發生錯誤：\n{ex.Message}", "錯誤", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 處理鍵盤快捷鍵
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N:
                    // Ctrl+N 新增事件
                    txtEventTitle.Focus();
                    return true;
                    
                case Keys.Delete:
                    // Delete 鍵刪除選中的事件
                    if (selectedEvent != null && !isEditMode)
                    {
                        BtnDeleteEvent_Click(this, EventArgs.Empty);
                    }
                    return true;
                    
                case Keys.F2:
                    // F2 編輯選中的事件
                    if (selectedEvent != null && !isEditMode)
                    {
                        BtnEditEvent_Click(this, EventArgs.Empty);
                    }
                    return true;
                    
                case Keys.Escape:
                    // ESC 取消編輯模式
                    if (isEditMode)
                    {
                        ExitEditMode();
                        ClearInputFields();
                    }
                    return true;
                    
                case Keys.Control | Keys.E:
                    // Ctrl+E 匯出事件
                    ExportEventsToFile();
                    return true;
                    
                case Keys.Control | Keys.I:
                    // Ctrl+I 匯入事件
                    ImportEventsFromFile();
                    return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 匯出按鈕點擊事件處理程序
        /// </summary>
        private void BtnExportToFile_Click(object sender, EventArgs e)
        {
            ExportEventsToFile();
        }

        /// <summary>
        /// 匯入按鈕點擊事件處理程序
        /// </summary>
        private void BtnImportFromFile_Click(object sender, EventArgs e)
        {
            ImportEventsFromFile();
        }
    }
}