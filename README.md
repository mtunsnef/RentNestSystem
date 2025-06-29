# 🏡 HỆ THỐNG CHO THUÊ PHÒNG TRỌ - RENTNEST

✔️ **RentNestSystem** là nền tảng hỗ trợ đăng tin cho thuê – tìm kiếm phòng trọ hiện đại, tích hợp nhiều công nghệ mới như AI, OAuth2, Real-time chat, và thanh toán trực tuyến, giúp kết nối người thuê, chủ trọ (Landlord) và quản trị viên (Admin/Staff) một cách hiệu quả và an toàn.

🌐 [Truy cập hệ thống tại đây](https://bluedreamhouse.runasp.net)

---

## 🚀 Tính năng nổi bật

- 🔐 **Google/Facebook OAuth 2.0 Login**  
- 👤 **Phân quyền đăng nhập**: `Landlord` / `Tenant` / `Staff` / `Admin`  
- 📄 **Đăng, chỉnh sửa và quản lý bài đăng cho thuê**  
- 📦 **Chọn gói đăng bài**: Thường, VIP, VIP nổi bật  
- ✅ **Staff/Admin duyệt bài viết** trước khi hiển thị  
- 🔔 **Thông báo realtime** bằng SignalR khi có cập nhật  
- 💬 **Chat trực tiếp** giữa người dùng và AI hỗ trợ (SignalR + Azure OpenAI)  
- 🧠 **AI sinh tiêu đề & nội dung bài viết** từ dữ liệu nhập bằng Azure OpenAI  
- 🔍 **Tìm kiếm nâng cao**, hỗ trợ **phân trang thông minh**  
- 📍 **Tích hợp Google Maps API** hiển thị vị trí phòng trọ  
- 📸 **Upload hình ảnh bất động sản** kèm bài viết  
- 💬 **Đánh giá và phản hồi** bài viết  
- 📊 **Dashboard thống kê cho Admin/Staff**  
- 🧾 **Gửi email xác nhận khi đăng ký**  
- 🎁 **Mã khuyến mãi khi đăng bài**  
- 💵 **Thanh toán trực tiếp bằng PayOS**  
- 👥 **Quản lý thông tin người dùng** đầy đủ  
- 🕵️‍♂️ **Bảo mật với Session & Role-based Authorization**  
- ☁️ **Triển khai trên Azure Cloud + Azure SQL Database**

---

## 🧱 Cấu trúc Project

````
RentNestSystem.sln
├── RentNest.Core           # Entity, Enum, Constant, Helpers, Configs, Business logic base
├── RentNest.Infrastructure # DbContext, EF Configs, Repositories
├── RentNest.Service        # DTOs, Services, Unit of Work, Business logic
├── RentNest.Web            # ASP.NET Core MVC (UI, Views, ViewModels, Controllers, MiddleWare)
````

---

## 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ |
|-----------|-----------|
| Backend   | ASP.NET Core MVC, EF Core, SignalR |
| Frontend  | Razor View, Bootstrap 5, jQuery, SweetAlert2 |
| AI Content | Azure OpenAI GPT-4, Embedding Vector Search |
| Auth      | OAuth 2.0 (Google, Facebook), Session, Role-based |
| Database  | SQL Server (Azure SQL Database) |
| Thanh toán | PayOS Integration |
| Bản đồ    | Google Maps API |
| Realtime  | SignalR |
| Triển khai | Azure App Services |

---

## ⚙️ Hướng dẫn cài đặt & chạy

### 1. Clone source code

````
git clone https://github.com/mtunsnef/RentNestSystem.git
cd Rent_Nest_System
````

### 2. Cấu hình `appsettings.json`

Cấu hình chuỗi kết nối và các thông tin dịch vụ:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your_azure_sql;Database=RentNestDb;User Id=...;Password=...;"
},
"Authentication": {
  "Google": {
    "ClientId": "...",
    "ClientSecret": "..."
  },
  "Facebook": {
    "AppId": "...",
    "AppSecret": "..."
  }
},
"AzureOpenAISettings": {
  "Endpoint": "...",
  "DeploymentName": "...",
  "ApiKey": "..."
}
```

Chạy lệnh tạo database từ migration:

```bash
dotnet ef database update
```

### 3. Tạo database và seed dữ liệu

Chạy `Scripts/database.sql` trong SQL Server Management Studio.

### 4. Chạy project

```bash
dotnet run --project RentNest.Web
```

---

## 🔐 Ghi chú bảo mật

* Tất cả các endpoint quan trọng đều được bảo vệ bằng `Authorization`.
* Quản lý session chặt chẽ, timeout hợp lý.
* Các thao tác nhạy cảm như đăng bài, duyệt bài, thanh toán đều kiểm tra role rõ ràng.
* Tích hợp login Google/Facebook đảm bảo an toàn xác thực người dùng.
* Tương tác AI có cơ chế ghi log & kiểm soát theo vai trò người dùng.
---

## 📬 Liên hệ

* **Tác giả**: Minh Tuns (Nguyễn Minh Tuấn)
* **Email**: [tuannm23.dev@gmail.com](mailto:tuannm23.dev@gmail.com)
* **LinkedIn**: [linkedin.com/in/tuannm23dev](https://www.linkedin.com/in/tuannm23dev)

---
