

---

# SocialProject - Hệ thống Mạng xã hội Trực tuyến

**SocialProject** là một ứng dụng web được xây dựng trên nền tảng .NET hiện đại, cho phép người dùng kết nối, chia sẻ nội dung và tương tác thời gian thực. Dự án được thiết kế với kiến trúc phân lớp rõ ràng, đảm bảo tính mở rộng và hiệu năng cao.

## 🚀 Công nghệ sử dụng

Dự án tận dụng sức mạnh của các công nghệ hàng đầu:

* **Backend:** .NET 8 / ASP.NET Core MVC
* **Database:** SQL Server (Quản lý qua Entity Framework Core)
* **Real-time:** SignalR (Cung cấp thông báo và tương tác tức thời)
* **Frontend:** Razor Pages, jQuery, SignalR Client, Syntax Highlighter
* **Cấu trúc:** Multi-project solution (SocialProject.Data, SocialProject.Services, v.v.)

## ✨ Tính năng chính

* **Quản lý người dùng:** Đăng ký, đăng nhập và phân quyền hệ thống (Roles: Admin, HotelManager, User...).
* **Tương tác thời gian thực:** Thông báo và cập nhật dữ liệu không cần tải lại trang nhờ SignalR.
* **Quản lý nội dung:** Đăng bài, quản lý dữ liệu hệ thống (như trong dự án "NOVA Resort Enterprise" mà bạn đang tích hợp).
* **Xác thực biểu mẫu:** Kiểm tra dữ liệu phía client với jQuery Validation Unobtrusive.
* **Giao diện tùy chỉnh:** Hỗ trợ hiển thị mã nguồn đẹp mắt với Syntax Highlighter.

## 🛠 Hướng dẫn cài đặt

Để chạy dự án này trên máy cục bộ, bạn hãy làm theo các bước sau:

### 1. Yêu cầu hệ thống
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB hoặc Express)
* Visual Studio 2022 (v17.8 trở lên) hoặc VS Code

### 2. Cấu hình Database
Mở tệp `appsettings.json` trong dự án `SocialProject` và cập nhật chuỗi kết nối:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SocialProjectDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Chạy Migration
Mở **Package Manager Console** và thực hiện lệnh:
```bash
Update-Database
```

### 4. Khởi chạy ứng dụng
Nhấn `F5` trong Visual Studio hoặc dùng lệnh:
```bash
dotnet run --project SocialProject
```

## 📂 Cấu trúc thư mục

* `SocialProject/`: Dự án Web chính (Controllers, Views, Static Assets).
* `SocialProject.Data/`: Chứa DbContext, Migrations và các Entities.
* `wwwroot/lib/`: Các thư viện frontend (SignalR, jQuery, Syntax Highlighter).

---

## 🤝 Đóng góp

Dự án hiện đang trong quá trình phát triển và hoàn thiện. Mọi ý kiến đóng góp về mã nguồn hoặc tính năng mới đều được chào đón!

---


