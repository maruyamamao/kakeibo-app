# 家計日和

## 📱 アプリ概要

「家計日和」は、日々の収入・支出を管理する家計簿アプリです。

単純な収支管理だけではなく、
「今月使える予算を、前月から前借り・繰り越しできる」
機能を実装しています。

自分自身の家計管理で感じていた
「前月から前借りした予算を翌月に忘れてしまう」
という課題を解決するために、この機能を考案しました。

---

## ✨ 主な機能

- 💰 収入の登録
- 💸 支出の登録
- 📋 収支履歴の確認
- ✏️ 収支の編集・削除
- 📅 月別の収支管理
- 🎯 月間予算の設定
- ⭐ 予算の前借り
- 🔄 予算の繰り越し
- ⚠️ 予算超過・前借りの通知
- 📊 カテゴリ別支出の集計
- 📈 カテゴリ別支出グラフ
- 💾 SQLiteによるデータ保存

---

## ⭐ こだわった機能

### 予算の前借り・繰り越し

例えば、趣味の予算を毎月10,000円に設定している場合、

9月に12,000円使うと、

- 9月の予算：10,000円
- 使用額：12,000円
- 前借り：2,000円
- 10月の使用可能予算：8,000円

となるようにしています。

反対に、9月に8,000円しか使わなかった場合は、

- 9月の予算：10,000円
- 使用額：8,000円
- 繰り越し：2,000円
- 10月の使用可能予算：12,000円

となります。

前借り・繰り越しを自動的に翌月へ反映することで、
「前月の使いすぎ・使わなかった分を忘れてしまう」
という問題を防ぎます。

---

## 🛠 使用技術

- C#
- .NET MAUI
- SQLite
- Visual Studio
- Git / GitHub

---

## 📱 画面

### ホーム画面

収入・支出・残高・予算状況を確認できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/3df20952-bd60-4034-ad3e-8fadbeedfecd" width="200">
  <img src="https://github.com/user-attachments/assets/e18e98dc-2fe7-452d-9c43-597f69969bcd" width="200">
</div>

### 収入・支出登録

金額、日付、カテゴリ、メモを登録できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/41bc1930-f83d-4471-8945-14f335d917d7" width="200">
  <img src="https://github.com/user-attachments/assets/d0da891a-c777-49d2-a1d4-2e83fe94db71" width="200">
</div>

### 収支履歴

登録した収支を一覧で確認し、編集・削除できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/7611a14a-db48-46bd-ad5b-8f73d8b83ef5" width="200">
  <img src="https://github.com/user-attachments/assets/5c997f3a-e733-42fc-9a46-f5eba930f4fb" width="200">
</div>

### カテゴリ別集計

カテゴリごとの支出額をグラフや一覧で確認できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/52dab506-55ea-4c98-b0cb-4291dc49209f" width="200">
</div>

### 予算設定

月ごとの予算を設定できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/420fc011-d88a-4b7c-90d3-ac803466d5aa" width="200">
</div>

### ⭐ 予算の繰り越し・前借り

前月の予算を繰り越した場合や、翌月の予算を前借りした場合は、
ホーム画面で金額と通知を確認できます。

<div align="left">
  <img src="https://github.com/user-attachments/assets/f0e44788-bc30-4d7a-b96f-473cb024c686" width="200">
  <img src="https://github.com/user-attachments/assets/a04df56c-330d-4042-8b61-992e51064631" width="200">
</div>

---

## 💡 開発の工夫

機能を一度に実装するのではなく、
GitHubのIssueを使って機能ごとにタスクを分け、

**「実装 → 動作確認 → Commit → Push」**

という流れで開発しました。

また、実際にアプリを使用しながら、
使いにくい部分や分かりにくい部分を確認し、
UIや機能の改善を行いました。

---

## 🎯 今後の改善

- より使いやすいUIへの改善
- 新しい家計管理機能の追加
- iOS向けの対応
- リリースに向けた最終調整

---

## 👤 制作目的

C#・.NET MAUIを使用したアプリ開発を通して、

- プログラミング
- UI設計
- データベース設計・管理
- システム開発
- GitHubを使用した開発管理

など、システム開発に必要な知識を実践的に学ぶことを目的として制作しました。
