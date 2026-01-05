
# WarehouseManagement - Hệ Thống Quản Lý Kho Hàng

## INTRODUCTION

Hệ thống Quản lý kho hàng là ứng dụng desktop chạy trên Windows, được xây dựng để tối ưu hóa việc theo dõi hàng hóa và các hoạt động nhập-xuất kho. Ứng dụng giúp doanh nghiệp kiểm soát chặt chẽ lượng tồn kho hiện có và giảm thiểu sai sót trong quy trình vận hành.

**Các tính năng chính:**

* **Quản lý sản phẩm:** Thực hiện các thao tác thêm, sửa, xóa (CRUD) và tìm kiếm sản phẩm nhanh chóng.
* **Quản lý giao dịch:** Lập phiếu Nhập/Xuất kho với cơ chế tự động cập nhật số lượng tồn kho ngay khi ghi phiếu.
* **Cảnh báo tồn kho:** Hệ thống tự động phát hiện và cảnh báo khi số lượng hàng hóa giảm xuống dưới ngưỡng tối thiểu đã thiết lập.
* **Hoàn tác (Undo):** Cho phép hủy bỏ thao tác gần nhất để khôi phục trạng thái dữ liệu trước đó thông qua nhật ký hệ thống.
* **Báo cáo trực quan:** Cung cấp biểu đồ cột thống kê xu hướng Nhập/Xuất theo thời gian.

## STRUCTURE

```

WarehouseManagement/
├── Assets/                 # Tài nguyên tĩnh (SQL scripts)
│   └── SQL/                # File khởi tạo database (schema, seed)
├── Build/                  # Kết quả biên dịch (bin, obj)
├── Controllers/            # Lớp điều hướng UI và Service
├── Helpers/                # Các tiện ích bổ trợ (DBHelper, Converter)
├── Models/                 # Định nghĩa thực thể dữ liệu
├── Repositories/           # Lớp truy vấn Database trực tiếp
├── Services/               # Xử lý logic nghiệp vụ và hoàn tác
├── Views/                  # Giao diện người dùng WinForms
├── App.config              # Cấu hình chuỗi kết nối Database
├── Program.cs              # Điểm khởi chạy chính của ứng dụng
├── README.md               # Tài liệu hướng dẫn sử dụng
├── ROADMAP.md              # Lộ trình triển khai dự án
└── WarehouseManagement.csproj # File cấu hình project

```

## INSTALLATION

### Danh sách thư viện cần cài

Dự án sử dụng các thư viện sau để đảm bảo hiệu năng và tính năng:

* `MySql.Data` (v8.0.33): Driver kết nối C# với MySQL.
* `Newtonsoft.Json` (v13.0.3): Xử lý dữ liệu nhật ký cho tính năng Undo.
* `Dapper`: Micro-ORM hỗ trợ truy vấn SQL nhanh gọn.
* `LiveCharts.WinForms`: Thư viện vẽ biểu đồ báo cáo.

### Lệnh cài thư viện (NuGet Console)

Mở Package Manager Console trong Visual Studio và chạy các lệnh sau:

**PowerShell**

```
Install-Package MySql.Data
Install-Package Newtonsoft.Json
Install-Package Dapper
Install-Package LiveCharts.WinForms
```

### Các bước chạy dự án

**1. Tạo database:**

* Mở MySQL Workbench hoặc trình quản lý SQL bất kỳ.
* Chạy script trong file `WarehouseManagement/Assets/SQL/schema.sql` để tạo cấu trúc bảng.
* (Tùy chọn) Chạy file `seed.sql` để có dữ liệu mẫu ban đầu.

**2. Tạo App.config:**

* Mở file `App.config` trong thư mục gốc dự án.
* Chỉnh sửa đoạn mã sau, thay `YOUR_PASSWORD` bằng mật khẩu MySQL của bạn:

**XML**

```
<connectionStrings>
  <add name="WarehouseDB" 
       connectionString="Server=localhost;Database=QL_KhoHang;Uid=root;Pwd=YOUR_PASSWORD;CharSet=utf8mb4;" />
</connectionStrings>
```

**3. Chạy các lệnh phát triển:**
Sử dụng Terminal hoặc Visual Studio để thực thi:

* **Khôi phục thư viện:** `dotnet restore`
* **Biên dịch dự án:** `dotnet build`
* **Chạy ứng dụng:** `dotnet run`

**Tổng hợp**

```
dotnet restore
dotnet build
dotnet run
```

## USAGE

### Các tính năng trên giao diện

* **Trang chủ (Dashboard):** Hiển thị danh sách sản phẩm và trạng thái kho.
* **Quản lý hàng hóa:** Khu vực thực hiện CRUD sản phẩm.
* **Nhập/Xuất kho:** Giao diện lập phiếu giao dịch.
* **Lịch sử & Undo:** Danh sách các hành động đã thực hiện.
* **Báo cáo:** Biểu đồ xu hướng kho hàng.

### Cách sử dụng

* **Tìm kiếm:** Nhập tên sản phẩm vào ô tìm kiếm trên `MainForm`, danh sách sẽ tự động lọc theo thời gian thực.
* **Nhập hàng:** Chọn sản phẩm, nhập số lượng và đơn giá trong `TransactionForm`. Sau khi lưu, số tồn kho sẽ tự động cộng thêm.
* **Cảnh báo tồn:** Các sản phẩm có số lượng thấp hơn ngưỡng sẽ tự động được tô màu đỏ trên bảng danh sách để người dùng dễ nhận biết.
* **Hoàn tác:** Nếu lỡ nhập sai, nhấn nút **Undo** trên thanh công cụ để hủy giao dịch vừa thực hiện, hệ thống sẽ tự trả số tồn kho về trạng thái cũ.

---
