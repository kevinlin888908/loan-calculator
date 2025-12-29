# ?? 行事曆 MAUI App 創建指南

## 第一步：安裝 MAUI 工作負載

首先需要安裝 .NET MAUI 工作負載：

```bash
dotnet workload install maui
```

## 第二步：創建 MAUI 專案

在您的解決方案目錄中創建新的 MAUI 專案：

```bash
# 在 loan 目錄的上一層創建
cd ..
dotnet new maui -n CalendarMauiApp
cd CalendarMauiApp
```

## 第三步：檢查專案結構

MAUI 專案將包含：
```
CalendarMauiApp/
├── MainPage.xaml          # 主頁面 UI
├── MainPage.xaml.cs       # 主頁面邏輯
├── App.xaml               # App 設定
├── App.xaml.cs            # App 啟動邏輯
├── AppShell.xaml          # 導航殼層
├── MauiProgram.cs         # 程式進入點
└── Platforms/             # 平台特定程式碼
    ├── Android/
    ├── iOS/
    ├── Windows/
    └── Tizen/
```

## 要繼續創建 MAUI 版本嗎？

選項 1：我可以直接在此處為您創建所有 MAUI 檔案
選項 2：您先安裝 MAUI 工作負載，然後我們一起創建專案
選項 3：我先展示完整的 MAUI 程式碼，您可以稍後複製使用