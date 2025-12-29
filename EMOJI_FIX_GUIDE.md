## Emoji 符號問題解決方案

### 問題描述
在某些 Windows 系統上，emoji 符號可能無法正確顯示，表現為：
- 方框 (□) 或問號 (?)
- 空白字符
- 亂碼符號

### 解決方案 1：Lucida Sans Unicode 字型 ?
**已採用此方案！** 使用系統內建的 `Lucida Sans Unicode` 字型：
- 主標題使用 14pt Lucida Sans Unicode Bold
- 群組標題使用 9pt Lucida Sans Unicode Regular
- 此字型對 Unicode 字符有優秀的支援，包括 emoji 符號

### 解決方案 2：Segoe UI Emoji 字型（備選）
如果需要更好的 emoji 支援，可以改為 `Segoe UI Emoji`，這是 Windows 系統內建的專用 emoji 字型。

### 解決方案 3：替代符號
如果 emoji 仍有問題，可以使用以下替代符號：

#### 原版（使用 emoji）?
- 主標題：??? 行事曆管理系統
- 新增事件：? 新增事件  
- 事件列表：?? 事件列表

#### 備選版本（純文字）
- 主標題：[行事曆] 管理系統
- 新增事件：+ 新增事件
- 事件列表：■ 事件列表

### 解決方案 4：系統設定
1. 確保 Windows 系統語言包完整安裝
2. 檢查字型設定是否支援 Unicode
3. 更新 .NET Runtime 到最新版本

### Lucida Sans Unicode 優勢 ??
- ? Windows 系統內建，相容性佳
- ? 優秀的 Unicode 支援
- ? 清晰的字符渲染
- ? 中英文混合顯示效果好
- ? 專業的視覺呈現

### 測試方法
運行應用程式並檢查：
1. 主標題是否正確顯示 emoji ???
2. 群組標題是否正確顯示符號 ? ??
3. 視窗標題列是否正確顯示
4. 字型是否清晰易讀

### 測試結果預期 ?
使用 Lucida Sans Unicode 後，您應該能看到：
- 清晰的日曆 emoji (???) 在主標題
- 正確的加號符號 (?) 在新增事件群組
- 整體字型美觀且易讀
- 中文字符顯示完美

如需切換到其他字型或純文字版本，請告知。