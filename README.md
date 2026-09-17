# 家計日和

## 📱 アプリ概要

「家計日和」は、日々の収入・支出を管理する家計簿アプリです。

単純な収支管理だけではなく、
「前月の未使用予算を繰り越したり、翌月の予算を前借りしたりできる」
機能を実装しています。

自分自身の家計管理で感じていた
「前月から前借りした予算を翌月に忘れてしまう」
という課題を解決するために、この機能を考案しました。

---

## 📱 Android版を試す

Android版のAPKをダウンロードして、実際にアプリを試すことができます。

[⬇️ 家計日和 v1.0.0 をダウンロード](https://github.com/maruyamamao/kakeibo-app/releases/tag/v1.0.0)

※Android端末で利用できます。

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
  <img width="200" alt="ホーム画面上側" src="https://github.com/user-attachments/assets/35e41cf9-2085-437d-8ed7-1b96fb9fd3b9" />
  <img width="200" alt="ホーム画面下側" src="https://github.com/user-attachments/assets/4b0c45c0-3d34-44db-9d2b-ba250c67c6b0" />
</div>

### 収入・支出登録

金額、日付、カテゴリ、メモを登録できます。

<div align="left">
  <img width="200" alt="収入登録" src="https://github.com/user-attachments/assets/2157188c-834b-43cb-83ab-9485c240509b" />
  <img width="200" alt="支出登録" src="https://github.com/user-attachments/assets/8098d66d-7243-4829-b2c1-ea8539b7a45d" />
</div>

### 収支履歴

登録した収支を一覧で確認し、編集・削除できます。

<div align="left">
  <img width="200" alt="収支履歴" src="https://github.com/user-attachments/assets/248677ec-7f33-4b46-8e72-e7e43ad8dfc3" />
  <img width="200" alt="収支編集" src="https://github.com/user-attachments/assets/54e709e2-4f0b-43fe-bda7-8c7c2439b33b" />
</div>

### カテゴリ別集計

カテゴリごとの支出額をグラフや一覧で確認できます。

<div align="left">
  <img width="200" alt="カテゴリ別集計" src="https://github.com/user-attachments/assets/4b3b94d8-3d7b-4262-8a8a-97d6b50c8f24" />
</div>

### 予算設定

月ごとの予算を設定できます。

<div align="left">
  <img width="200" alt="予算設定" src="https://github.com/user-attachments/assets/6eb73b95-5824-4b86-a2d1-9ea3d40b96eb" />
</div>

### ⭐ 予算の超過・繰り越し・前借り

当月の予算を超過した場合や、前月の予算を繰り越した場合、翌月の予算を前借りした場合は、
ホーム画面で金額と通知を確認できます。

<div align="left">
  <img width="200" alt="超過通知" src="https://github.com/user-attachments/assets/82785d82-4255-4570-a8c1-2cc985ec84f9" />
  <img width="200" alt="前借" src="https://github.com/user-attachments/assets/37876251-69f0-4d9b-9fbb-c70193be363e" />
  <img width="200" alt="繰り越し通知" src="https://github.com/user-attachments/assets/32f45e73-e26a-42a0-bed3-30254aa15efa" />
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

- より使いやすいUIへの継続的な改善
- 新しい家計管理機能の追加
- iOS向けの対応
- アプリの正式リリース

---

## 👤 制作目的

C#・.NET MAUIを使用したアプリ開発を通して、

- プログラミング
- UI設計
- データベース設計・管理
- システム開発
- GitHubを使用した開発管理

など、システム開発に必要な知識を実践的に学ぶことを目的として制作しました。
