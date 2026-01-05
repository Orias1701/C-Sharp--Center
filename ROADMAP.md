# 🎯 ROADMAP THỰC HIỆN

## 📍 Giai đoạn 1: Thiết lập & Kết nối ✅ HOÀN THIỆN

* [X] **Cấu trúc MRSC:** Đã tạo đủ thư mục và file nền tảng.
* [X] **Database:** Chạy file `Assets/SQL/schema.sql` để tạo bảng với hỗ trợ tiếng Việt.
* [X] **Config:** Cập nhật mật khẩu MySQL vào chuỗi kết nối trong `App.config`.
* [X] **Test:** Chạy ứng dụng để `DatabaseHelper` xác nhận kết nối thành công.

## 🏗️ Giai đoạn 2: Hoàn thiện Logic (Services) ✅ HOÀN THIỆN

* [X] **InventoryService:** Viết logic trừ tồn kho tự động khi xuất và cộng khi nhập.
* [X] **Undo Logic:** Xử lý đọc dữ liệu JSON từ `ActionLogs` để hoàn tác giao dịch.
* [X] **Alert Logic:** Kiểm tra ngưỡng `MinThreshold` để kích hoạt cảnh báo.

## 🎨 Giai đoạn 3: Giao diện & Sự kiện (UI) ✅ HOÀN THIỆN

* [X] **MainForm:** Đổ dữ liệu lên `DataGridView` và xử lý tìm kiếm thời gian thực.
* [X] **Cảnh báo:** Tô màu đỏ các dòng sản phẩm có tồn kho thấp (< ngưỡng).
* [X] **ProductForm:** Form thêm/sửa sản phẩm với kiểm tra dữ liệu.
* [X] **TransactionForm:** Form nhập/xuất hàng với chi tiết sản phẩm.
* [X] **ReportForm:** Biểu đồ cột tồn kho, giá trị, thống kê tổng quát.

## 🧪 Giai đoạn 4: Kiểm thử & Đóng gói

* [ ] **Test:** Kiểm tra luồng: Nhập hàng -> Tồn kho tăng -> Ghi Log -> Hoàn tác.
* [ ] **Tiếng Việt:** Kiểm tra hiển thị tên sản phẩm có dấu trên toàn bộ UI.
* [ ] **Release:** Build file `.exe` bản chính thức.
