# ?? 行事曆 App 轉換指南

## .NET MAUI 轉換方案 (推薦)

### 什麼是 .NET MAUI
- Multi-platform App UI 的縮寫
- 使用 C# 和 .NET 開發跨平台應用程式
- 一套程式碼可以部署到 iOS、Android、Windows、macOS

### 轉換步驟

#### 1. 安裝 Visual Studio 2022 with MAUI
```bash
# 需要安裝的工作負載：
- .NET Multi-platform App UI development
- Android SDK
- iOS development tools (需要 Mac 或 Mac 連接)
```

#### 2. 創建新的 MAUI 專案
```bash
dotnet new maui -n CalendarApp
```

#### 3. 移植現有程式碼

##### 資料模型 (可直接使用)
```csharp
// CalendarEvent.cs - 幾乎不需要修改
public class CalendarEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    // ... 其他屬性保持不變
}
```

##### UI 重新設計 (使用 XAML)
```xml
<!-- MainPage.xaml -->
<ContentPage x:Class="CalendarApp.MainPage"
             Title="?? 行事曆管理">
    <ScrollView>
        <StackLayout Padding="20">
            <!-- 日期選擇器 -->
            <DatePicker x:Name="eventDatePicker" />
            <TimePicker x:Name="eventTimePicker" />
            
            <!-- 事件輸入 -->
            <Entry x:Name="titleEntry" Placeholder="事件標題" />
            <Editor x:Name="descriptionEditor" Placeholder="事件內容" />
            
            <!-- 按鈕 -->
            <Button Text="新增事件" Clicked="OnAddEventClicked" />
            
            <!-- 事件列表 -->
            <CollectionView x:Name="eventsCollection">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <Grid Padding="10">
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <Label Grid.Row="0" Text="{Binding Title}" FontSize="16" FontAttributes="Bold"/>
                            <Label Grid.Row="1" Text="{Binding Description}" FontSize="14"/>
                        </Grid>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>
        </StackLayout>
    </ScrollView>
</ContentPage>
```

##### 程式邏輯調整
```csharp
// MainPage.xaml.cs
public partial class MainPage : ContentPage
{
    private List<CalendarEvent> events = new List<CalendarEvent>();
    
    public MainPage()
    {
        InitializeComponent();
    }
    
    private void OnAddEventClicked(object sender, EventArgs e)
    {
        var newEvent = new CalendarEvent
        {
            Title = titleEntry.Text,
            Description = descriptionEditor.Text,
            DateTime = eventDatePicker.Date.Add(eventTimePicker.Time)
        };
        
        events.Add(newEvent);
        eventsCollection.ItemsSource = null;
        eventsCollection.ItemsSource = events;
        
        // 清空輸入欄位
        titleEntry.Text = "";
        descriptionEditor.Text = "";
    }
}
```

### 檔案儲存功能調整
```csharp
// 使用 MAUI 的檔案系統
public async Task SaveEventsAsync()
{
    var json = JsonSerializer.Serialize(events);
    var fileName = Path.Combine(FileSystem.AppDataDirectory, "events.json");
    await File.WriteAllTextAsync(fileName, json);
}

public async Task LoadEventsAsync()
{
    var fileName = Path.Combine(FileSystem.AppDataDirectory, "events.json");
    if (File.Exists(fileName))
    {
        var json = await File.ReadAllTextAsync(fileName);
        events = JsonSerializer.Deserialize<List<CalendarEvent>>(json) ?? new List<CalendarEvent>();
    }
}
```

## 2. Xamarin.Forms (舊技術)
- 已被 MAUI 取代，不建議新專案使用
- 但仍可以作為選項

## 3. Flutter + .NET Backend
- UI 使用 Flutter (Dart 語言)
- 後端 API 使用您現有的 C# 邏輯
- 需要學習 Dart 語言

## 4. React Native + .NET Backend
- UI 使用 React Native (JavaScript)
- 後端 API 使用您現有的 C# 邏輯
- 需要學習 JavaScript/TypeScript

## Visual Studio 支援

### Visual Studio 2022 功能
? **完整支援 .NET MAUI 開發**
- 視覺化設計器
- 即時預覽
- 跨平台除錯
- 內建模擬器支援

? **一鍵部署**
- Android 模擬器測試
- iOS 模擬器測試 (需要 Mac)
- 實機測試和部署

### 開發需求
- **Windows**: Visual Studio 2022
- **Android**: 完全支援
- **iOS**: 需要 Mac 電腦或 Mac 連接
- **發布**: Google Play Store / Apple App Store

## 轉換工作量評估

### ?? 容易移植的部分 (80%)
- 資料模型 (`CalendarEvent.cs`)
- 商業邏輯 (事件管理、檔案操作)
- 驗證邏輯

### ?? 需要調整的部分 (15%)
- 檔案存取 API
- 日期/時間選擇器
- 通知系統

### ?? 需要重寫的部分 (5%)
- UI 界面 (Windows Forms → XAML)
- 平台特定功能

## 專案時程估算
- **學習 MAUI**: 1-2 週
- **UI 重新設計**: 1 週  
- **功能移植**: 3-5 天
- **測試和除錯**: 1 週
- **總計**: 約 3-4 週

## 建議步驟
1. 先學習 .NET MAUI 基礎
2. 創建簡單的 Hello World App
3. 逐步移植現有功能
4. 在模擬器上測試
5. 最後進行實機測試和發布

您想要我協助您開始 MAUI 轉換嗎？