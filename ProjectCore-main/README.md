
# 🍰 THE BAKERY – ASP.NET Core Project

**The Bakery** is an educational project developed during my studies, focusing mainly on the server side using ASP.NET Core.  
The system simulates a bakery management system with admin and customer roles.

## 📌 Overview

- The system includes a backend (main focus) and a lightweight frontend interface.  
- Data is stored in JSON files (no external database required).  
- Data persistence is maintained even after the application is closed.

## 👥 Roles and Features

### 🛠️ Manager
- View, add, edit, and delete users  
- View, add, and delete cakes  
- Supports multiple managers  
- Only managers can delete users and manage cakes

### 🎂 Customer
- View the list of available cakes  
- Add and remove cakes from their purchase list  

## ⚙️ Technologies  
- ASP.NET Core  
- Authentication and authorization using JWT  
- Data stored in JSON files  

## 🚀 Getting Started

### ✅ Prerequisites  
Make sure to install:  
- [.NET SDK 7.0 or higher](https://dotnet.microsoft.com/en-us/download)  
- (Optional) Visual Studio or VS Code  

### 📂 Clone the repository  
```bash
git clone https://github.com/your-username/bakery-project.git
cd bakery-project
```

### ▶️ Run the application  
```bash
dotnet run
```

The API server will run by default at:  
`http://localhost:5000` or `https://localhost:5001`

### 📬 Example API Endpoints  
- `POST /login` – Login as admin or customer  
- `GET /manager/cakes` – Get list of cakes (admin only)  
- `POST /customer/buy` – Add cake to customer's purchase list  
*(Adjust the routes according to your project)*

## 📚 Notes  
This is an educational project designed to practice building secure server-side applications with role management and persistent data storage.
