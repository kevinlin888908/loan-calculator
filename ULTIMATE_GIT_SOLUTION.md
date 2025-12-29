# Git 多帳號管理配置

## ?? .gitconfig 設定

在用戶目錄下創建或編輯 `.gitconfig` 檔案：

```ini
[user]
    name = 林建平
    email = kevinlin888908@users.noreply.github.com

[credential]
    helper = manager-core
    credentialStore = dpapi
    useHttpPath = true

[includeIf "gitdir:~/lyixiong928-projects/"]
    path = ~/.gitconfig-lyixiong

[includeIf "gitdir:~/kevinlin888908-projects/"]
    path = ~/.gitconfig-kevin

[core]
    autocrlf = true
    safecrlf = false
```

## ?? 帳號特定設定

### ~/.gitconfig-lyixiong
```ini
[user]
    name = lyixiong928-netizen
    email = lyixiong928@users.noreply.github.com
[credential]
    username = lyixiong928-netizen
```

### ~/.gitconfig-kevin  
```ini
[user]
    name = 林建平
    email = kevinlin888908@users.noreply.github.com
[credential]
    username = kevinlin888908
```

## ?? 使用方法

1. **創建專案資料夾結構**：
```
~/lyixiong928-projects/     # lyixiong928 的專案
~/kevinlin888908-projects/  # kevinlin888908 的專案
```

2. **Clone 專案到對應資料夾**：
```bash
# lyixiong928 的專案
git clone https://github.com/lyixiong928-netizen/loan-calculator.git ~/lyixiong928-projects/loan-calculator

# kevinlin888908 的專案  
git clone https://github.com/kevinlin888908/loan-calculator.git ~/kevinlin888908-projects/loan-calculator
```

3. **Git 會自動使用對應的帳號設定**

## ?? 解決認證問題的終極方案

### 方案 A：Windows Credential Manager
```bash
# 清除舊認證
git credential-manager-core erase
# 重新設定
git push origin main  # 會提示重新輸入認證
```

### 方案 B：Personal Access Token 管理
1. 為每個帳號創建獨立的 PAT
2. 設定不同的權限範圍
3. 定期更新 token

### 方案 C：SSH 金鑰（最穩定）
```bash
# 為每個帳號創建不同的 SSH 金鑰
ssh-keygen -t ed25519 -C "lyixiong928@github" -f ~/.ssh/id_ed25519_lyixiong
ssh-keygen -t ed25519 -C "kevinlin888908@github" -f ~/.ssh/id_ed25519_kevin
```

## ?? GitHub Desktop 替代方案

### 推薦工具：
1. **GitKraken** - 專業 Git GUI
2. **SourceTree** - Atlassian 的免費工具
3. **Fork** - 快速且穩定
4. **VS Code Git** - 內建強大功能

## ?? 成本優化建議

### GitHub 帳號管理：
- 考慮 GitHub Organization 統一管理
- 使用免費的 private repo 額度
- 定期清理不必要的 repo
