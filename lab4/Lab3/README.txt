# Task Manager — Laborator 3 & 4

Aplicație desktop realizată în .NET (WinForms) pentru gestionarea sarcinilor unei echipe. Proiectul implementează arhitectură stratificată, pattern-uri de design și principiile SOLID.

---

## 🧰 Tehnologii utilizate

- .NET 8 (WinForms)
- SQLite (Microsoft.Data.Sqlite)
- NUnit (pentru testare)
- C#

---

## 🏗 Arhitectură

Aplicația este împărțită în mai multe straturi:

### 🖥 UI Layer (TaskManager.UI)
- Interfață WinForms
- Nu conține logică de business
- Apelează doar serviciile

### ⚙️ Service Layer (TaskManager.Core)
- TaskService → logică de business
- ReportService → generare rapoarte
- TaskValidator → validare date

### 🗄 Repository Layer (TaskManager.Data)
- ITaskRepository → abstracție acces date
- SqliteTaskRepository → implementare SQLite
- InMemoryTaskRepository → pentru teste

---

## 🗃 Baza de date

- SQLite (fișier local)
- Se creează automat la prima rulare
- Stochează task-urile cu:
  - Title
  - Status
  - Priority
  - NotificationType
  - CreatedAt
  - DueDate (opțional)

---

## ✅ Funcționalități

- ➕ Add Task
- ❌ Delete Task
- ✔ Complete Task
- 🔄 Refresh
- 🔍 Filtrare după Status
- 📊 Raport (ReportService)
- 🔔 Notificări (Console, Email, FileLog, Slack)

---

## 🧪 Testare

- NUnit
- Teste pentru:
  - TaskService
  - TaskValidator
  - TaskHierarchy (LSP)
  - ReportService (ISP)

✔ Toate testele trec (11/11)

---

## 🧠 Principii SOLID

### 🔹 SRP (Single Responsibility)
- TaskService → logică business
- TaskValidator → validare
- Repository → acces date

---

### 🔹 OCP (Open/Closed)
- Sistemul de notificări este extensibil:
```csharp
ITaskNotifier