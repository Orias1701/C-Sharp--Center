# Quick Test Checklist

## Build & Run
```powershell
cd "e:\_DevResources\1. C-Family\C-Sharp\C-Sharp--Projects\WindowsProj\WarehouseManagement"
dotnet build
dotnet run
```

## Test Steps (thứ tự quan trọng)

### ✅ Step 1: Chạy Ứng Dụng
- [ ] Ứng dụng khởi động được
- [ ] Đăng nhập thành công
- [ ] MainForm hiển thị với 4 tabs

### ✅ Step 2: Kiểm Tra Tab Giao Dịch
- [ ] Click vào tab "Giao Dịch"
- [ ] **Nút Edit phải thay đổi từ "✏️ Sửa" thành "👁️ Xem Chi Tiết"**
- [ ] Nút phải là "👁️ Xem Chi Tiết" khi ở tab Giao Dịch

### ✅ Step 3: Xem Chi Tiết (Cách 1 - Double-Click)
- [ ] Có ít nhất 1 giao dịch trong danh sách
- [ ] Double-click vào một hàng giao dịch
- [ ] **Form "Chi Tiết Giao Dịch #ID" phải hiện lên**
- [ ] Form hiển thị đúng:
  - [ ] Loại Giao Dịch (Import/Export)
  - [ ] Ngày Tạo
  - [ ] Ghi Chú
  - [ ] Danh sách chi tiết sản phẩm
- [ ] Nhấn "✖️ Đóng" để đóng form

### ✅ Step 4: Xem Chi Tiết (Cách 2 - Nút Edit)
- [ ] Chọn một giao dịch (click vào hàng)
- [ ] Nhấn nút "👁️ Xem Chi Tiết"
- [ ] **Form "Chi Tiết Giao Dịch #ID" phải hiện lên**
- [ ] Form hiển thị đúng thông tin
- [ ] Nhấn "✖️ Đóng" để đóng form

### ✅ Step 5: Kiểm Tra Tab Khác
- [ ] Click vào tab "Sản Phẩm"
- [ ] **Nút Edit phải thay đổi lại thành "✏️ Sửa"**
- [ ] Click vào tab "Danh Mục"
- [ ] **Nút Edit vẫn là "✏️ Sửa"**
- [ ] Click vào tab "Báo Cáo"
- [ ] **Nút Edit vẫn là "✏️ Sửa"** (hoặc disabled)

### ✅ Step 6: Kiểm Tra Debug Log
- [ ] Mở file: `Build\bin\Debug\net472\debug.log`
- [ ] Tìm log entries: `[MainForm]`, `[TransactionRepository]`, etc.
- [ ] Không nên có `ERROR` hoặc `Exception`
- [ ] Kiểm tra luồng thực thi:
  ```
  [MainForm] Tab changed to index: 2
  [MainForm] DgvTransactions_CellDoubleClick
  [TransactionRepository] Bắt đầu GetTransactionById với ID: X
  [TransactionDetailForm] TransactionDetailForm_Load bắt đầu
  ```

## 🎯 Success Criteria

Tất cả các dấu ✅ trên phải được check → **THÀNH CÔNG** ✓

## ⚠️ Nếu Có Lỗi

| Lỗi | Giải Pháp |
|-----|----------|
| "DataReader associated with this Connection" | Đã fix, build lại |
| Nút không thay đổi text | Kiểm tra TabControl_SelectedIndexChanged event |
| Form không hiện lên | Kiểm tra debug.log để xem lỗi chi tiết |
| Database connection error | Kiểm tra MySQL server đang chạy |
| Build failed | Xóa folder Build, chạy `dotnet clean && dotnet build` |

## 📝 Debug Log Interpretation

Tìm các pattern này trong debug.log:

**Success Path**:
```
[Program] Ứng dụng khởi động
[MainForm] Tab changed to index: 2
[MainForm] DgvTransactions_CellDoubleClick - RowIndex: 0
[MainForm] Gọi GetTransactionById...
[InventoryController] Gọi GetTransactionById với ID: 1
[InventoryService] GetTransactionById được gọi với ID: 1
[TransactionRepository] Bắt đầu GetTransactionById với ID: 1
[TransactionRepository] Lấy N chi tiết giao dịch
[TransactionRepository] GetTransactionById hoàn thành thành công
[TransactionDetailForm] TransactionDetailForm_Load bắt đầu
[TransactionDetailForm] Binding N chi tiết vào DataGridView
```

**Error Path** (để tránh):
```
[TransactionRepository] Lỗi GetTransactionById: There is already an open DataReader...
[TransactionDetailForm] Lỗi: ...
```

## Sau Test Thành Công

1. Xóa tất cả Debug.WriteLine
2. Xóa test files
3. Build lại
4. Commit code

Xem `CLEANUP.md` để chi tiết.
