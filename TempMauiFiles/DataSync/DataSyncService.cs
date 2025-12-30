using System.Text.Json;
using CalendarMauiMobile.Models;

namespace CalendarMauiMobile.Services;

/// <summary>
/// 資料同步服務 - 實現桌面版和手機版之間的資料同步 ??
/// </summary>
public interface IDataSyncService
{
    Task<bool> ExportToFileAsync(string filePath);
    Task<bool> ImportFromFileAsync(string filePath);
    Task<bool> SyncWithDesktopVersionAsync(string desktopDataPath);
    Task<(bool success, string message)> BackupDataAsync();
    Task<(bool success, string message)> RestoreDataAsync(string backupPath);
    event EventHandler<DataSyncEventArgs>? DataSyncCompleted;
}

public class DataSyncEventArgs : EventArgs
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int SyncedEventsCount { get; set; }
    public DateTime SyncTime { get; set; } = DateTime.Now;
}

/// <summary>
/// 資料同步服務實現
/// </summary>
public class DataSyncService : IDataSyncService
{
    private readonly CalendarEventService _eventService;
    private const string BACKUP_FOLDER = "Backups";
    private const string SYNC_FOLDER = "Sync";

    public DataSyncService(CalendarEventService eventService)
    {
        _eventService = eventService;
        InitializeDirectories();
    }

    public event EventHandler<DataSyncEventArgs>? DataSyncCompleted;

    #region 公共方法

    /// <summary>
    /// 匯出資料到檔案 (與桌面版相容的格式)
    /// </summary>
    public async Task<bool> ExportToFileAsync(string filePath)
    {
        try
        {
            var events = _eventService.Events.ToList();
            
            // 使用與桌面版相同的格式
            var content = new System.Text.StringBuilder();
            content.AppendLine("=== 行事曆事件列表 ===");
            content.AppendLine($"匯出時間：{DateTime.Now:yyyy年MM月dd日 HH:mm:ss}");
            content.AppendLine($"匯出來源：MAUI 手機版");
            content.AppendLine($"總事件數：{events.Count}");
            content.AppendLine($"幸運事件數：{events.Count(e => e.IsLucky)}");
            content.AppendLine();

            var sortedEvents = events.OrderBy(e => e.DateTime).ToList();
            
            foreach (var evt in sortedEvents)
            {
                content.AppendLine($"事件編號：{evt.Id}");
                content.AppendLine($"標題：{evt.Title}");
                content.AppendLine($"時間：{evt.DateTime:yyyy年MM月dd日 dddd HH:mm}");
                content.AppendLine($"內容：{evt.Description}");
                
                // 添加幸運資訊
                if (evt.IsLucky)
                {
                    content.AppendLine($"幸運指數：{evt.LuckyScore} 分");
                    content.AppendLine($"幸運建議：{evt.GetLuckyAdvice()}");
                }
                
                content.AppendLine(new string('-', 50));
            }

            await File.WriteAllTextAsync(filePath, content.ToString(), System.Text.Encoding.UTF8);
            
            OnDataSyncCompleted(new DataSyncEventArgs
            {
                Success = true,
                Message = "資料匯出成功！",
                SyncedEventsCount = events.Count
            });
            
            return true;
        }
        catch (Exception ex)
        {
            OnDataSyncCompleted(new DataSyncEventArgs
            {
                Success = false,
                Message = $"匯出失敗：{ex.Message}",
                SyncedEventsCount = 0
            });
            return false;
        }
    }

    /// <summary>
    /// 從檔案匯入資料 (相容桌面版格式)
    /// </summary>
    public async Task<bool> ImportFromFileAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                OnDataSyncCompleted(new DataSyncEventArgs
                {
                    Success = false,
                    Message = "檔案不存在",
                    SyncedEventsCount = 0
                });
                return false;
            }

            var lines = await File.ReadAllLinesAsync(filePath, System.Text.Encoding.UTF8);
            var importedEvents = new List<CalendarEvent>();
            
            CalendarEvent? currentEvent = null;
            
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
                // 詢問使用者是否要覆蓋現有資料
                bool shouldMerge = await AskUserForMergeOption(importedEvents.Count);
                
                if (!shouldMerge)
                {
                    // 清除現有事件
                    var existingEvents = _eventService.Events.ToList();
                    foreach (var evt in existingEvents)
                    {
                        await _eventService.DeleteEventAsync(evt);
                    }
                }
                
                // 匯入新事件
                foreach (var evt in importedEvents)
                {
                    // 重新分配 ID 以避免衝突
                    evt.Id = 0;
                    await _eventService.AddEventAsync(evt);
                }
                
                OnDataSyncCompleted(new DataSyncEventArgs
                {
                    Success = true,
                    Message = $"成功匯入 {importedEvents.Count} 個事件！",
                    SyncedEventsCount = importedEvents.Count
                });
                
                return true;
            }
            else
            {
                OnDataSyncCompleted(new DataSyncEventArgs
                {
                    Success = false,
                    Message = "檔案中沒有找到有效的事件資料",
                    SyncedEventsCount = 0
                });
                return false;
            }
        }
        catch (Exception ex)
        {
            OnDataSyncCompleted(new DataSyncEventArgs
            {
                Success = false,
                Message = $"匯入失敗：{ex.Message}",
                SyncedEventsCount = 0
            });
            return false;
        }
    }

    /// <summary>
    /// 與桌面版本同步資料
    /// </summary>
    public async Task<bool> SyncWithDesktopVersionAsync(string desktopDataPath)
    {
        try
        {
            // 1. 備份目前的手機版資料
            var backupResult = await BackupDataAsync();
            if (!backupResult.success)
            {
                OnDataSyncCompleted(new DataSyncEventArgs
                {
                    Success = false,
                    Message = "備份手機版資料失敗",
                    SyncedEventsCount = 0
                });
                return false;
            }

            // 2. 檢查桌面版資料檔案
            if (!File.Exists(desktopDataPath))
            {
                // 如果桌面版沒有資料，匯出手機版資料給桌面版
                var exportResult = await ExportToFileAsync(desktopDataPath);
                return exportResult;
            }

            // 3. 比較兩邊的資料時間戳
            var desktopModifiedTime = File.GetLastWriteTime(desktopDataPath);
            var mobileDataPath = Path.Combine(FileSystem.AppDataDirectory, "calendar_events.json");
            var mobileModifiedTime = File.Exists(mobileDataPath) ? File.GetLastWriteTime(mobileDataPath) : DateTime.MinValue;

            // 4. 決定同步方向
            if (desktopModifiedTime > mobileModifiedTime)
            {
                // 桌面版較新，匯入桌面版資料
                return await ImportFromFileAsync(desktopDataPath);
            }
            else
            {
                // 手機版較新或時間相同，匯出手機版資料
                return await ExportToFileAsync(desktopDataPath);
            }
        }
        catch (Exception ex)
        {
            OnDataSyncCompleted(new DataSyncEventArgs
            {
                Success = false,
                Message = $"同步失敗：{ex.Message}",
                SyncedEventsCount = 0
            });
            return false;
        }
    }

    /// <summary>
    /// 備份資料
    /// </summary>
    public async Task<(bool success, string message)> BackupDataAsync()
    {
        try
        {
            var backupDir = Path.Combine(FileSystem.AppDataDirectory, BACKUP_FOLDER);
            Directory.CreateDirectory(backupDir);
            
            var backupFileName = $"Calendar_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var backupPath = Path.Combine(backupDir, backupFileName);
            
            var events = _eventService.Events.ToList();
            var backupData = new
            {
                BackupTime = DateTime.Now,
                Version = "1.0",
                Platform = "MAUI Mobile",
                EventCount = events.Count,
                LuckyEventCount = events.Count(e => e.IsLucky),
                Events = events
            };
            
            var json = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            
            await File.WriteAllTextAsync(backupPath, json, System.Text.Encoding.UTF8);
            
            return (true, $"備份成功！檔案：{backupFileName}");
        }
        catch (Exception ex)
        {
            return (false, $"備份失敗：{ex.Message}");
        }
    }

    /// <summary>
    /// 還原資料
    /// </summary>
    public async Task<(bool success, string message)> RestoreDataAsync(string backupPath)
    {
        try
        {
            if (!File.Exists(backupPath))
            {
                return (false, "備份檔案不存在");
            }
            
            var json = await File.ReadAllTextAsync(backupPath, System.Text.Encoding.UTF8);
            var backupData = JsonSerializer.Deserialize<BackupData>(json);
            
            if (backupData?.Events == null)
            {
                return (false, "備份檔案格式錯誤");
            }
            
            // 清除現有資料
            var existingEvents = _eventService.Events.ToList();
            foreach (var evt in existingEvents)
            {
                await _eventService.DeleteEventAsync(evt);
            }
            
            // 還原備份資料
            foreach (var evt in backupData.Events)
            {
                evt.Id = 0; // 重新分配 ID
                await _eventService.AddEventAsync(evt);
            }
            
            OnDataSyncCompleted(new DataSyncEventArgs
            {
                Success = true,
                Message = $"還原成功！恢復了 {backupData.Events.Count} 個事件",
                SyncedEventsCount = backupData.Events.Count
            });
            
            return (true, $"還原成功！恢復了 {backupData.Events.Count} 個事件");
        }
        catch (Exception ex)
        {
            return (false, $"還原失敗：{ex.Message}");
        }
    }

    /// <summary>
    /// 獲取可用的備份檔案列表
    /// </summary>
    public async Task<List<BackupFileInfo>> GetAvailableBackupsAsync()
    {
        try
        {
            var backupDir = Path.Combine(FileSystem.AppDataDirectory, BACKUP_FOLDER);
            if (!Directory.Exists(backupDir))
            {
                return new List<BackupFileInfo>();
            }
            
            var backupFiles = Directory.GetFiles(backupDir, "*.json")
                .Select(f => new BackupFileInfo
                {
                    FileName = Path.GetFileName(f),
                    FilePath = f,
                    CreatedTime = File.GetCreationTime(f),
                    Size = new FileInfo(f).Length
                })
                .OrderByDescending(b => b.CreatedTime)
                .ToList();
            
            // 讀取每個備份檔案的詳細資訊
            foreach (var backup in backupFiles)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(backup.FilePath);
                    var backupData = JsonSerializer.Deserialize<BackupData>(json);
                    if (backupData != null)
                    {
                        backup.EventCount = backupData.EventCount;
                        backup.LuckyEventCount = backupData.LuckyEventCount;
                        backup.Platform = backupData.Platform ?? "Unknown";
                    }
                }
                catch
                {
                    // 忽略讀取錯誤的備份檔案
                    backup.EventCount = 0;
                    backup.Platform = "Unknown";
                }
            }
            
            return backupFiles;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAvailableBackupsAsync 錯誤: {ex.Message}");
            return new List<BackupFileInfo>();
        }
    }

    #endregion

    #region 私有方法

    private void InitializeDirectories()
    {
        try
        {
            var backupDir = Path.Combine(FileSystem.AppDataDirectory, BACKUP_FOLDER);
            var syncDir = Path.Combine(FileSystem.AppDataDirectory, SYNC_FOLDER);
            
            Directory.CreateDirectory(backupDir);
            Directory.CreateDirectory(syncDir);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InitializeDirectories 錯誤: {ex.Message}");
        }
    }

    private async Task<bool> AskUserForMergeOption(int importEventCount)
    {
        try
        {
            var existingCount = _eventService.Events.Count;
            
            if (existingCount == 0)
            {
                // 沒有現有資料，直接匯入
                return false;
            }
            
            var result = await Application.Current!.MainPage!.DisplayAlert(
                "資料匯入選項",
                $"即將匯入 {importEventCount} 個事件\n" +
                $"目前已有 {existingCount} 個事件\n\n" +
                $"選擇「合併」保留現有事件並新增匯入的事件\n" +
                $"選擇「替換」將刪除現有事件並匯入新事件",
                "合併",
                "替換");
                
            return result; // true = 合併, false = 替換
        }
        catch
        {
            // 如果無法詢問使用者，預設為合併模式
            return true;
        }
    }

    private void OnDataSyncCompleted(DataSyncEventArgs args)
    {
        DataSyncCompleted?.Invoke(this, args);
    }

    #endregion
}

/// <summary>
/// 備份資料結構
/// </summary>
public class BackupData
{
    public DateTime BackupTime { get; set; }
    public string Version { get; set; } = "1.0";
    public string Platform { get; set; } = "MAUI Mobile";
    public int EventCount { get; set; }
    public int LuckyEventCount { get; set; }
    public List<CalendarEvent> Events { get; set; } = new();
}

/// <summary>
/// 備份檔案資訊
/// </summary>
public class BackupFileInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; }
    public long Size { get; set; }
    public int EventCount { get; set; }
    public int LuckyEventCount { get; set; }
    public string Platform { get; set; } = string.Empty;
    
    public string DisplayName => $"{CreatedTime:MM/dd HH:mm} - {EventCount}個事件 ({Platform})";
    public string SizeDisplay => Size < 1024 ? $"{Size} B" : $"{Size / 1024} KB";
}