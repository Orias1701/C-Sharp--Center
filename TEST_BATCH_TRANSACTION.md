# Test Batch Transaction Fix

## Vấn Đề Ban Đầu
- Mỗi lần nhập/xuất một sản phẩm, một transaction riêng được tạo
- Nếu nhập 3 sản phẩm = 3 transaction khác nhau (SAI)
- Cần 1 transaction với tất cả chi tiết sản phẩm (ĐÚNG)

## Giải Pháp
- Thêm `ImportBatch()` và `ExportBatch()` vào InventoryController
- Thêm `ImportStockBatch()` và `ExportStockBatch()` vào InventoryService
- Cập nhật TransactionForm.cs sử dụng batch methods
- Mỗi phiếu nhập/xuất giờ = 1 transaction với N details

## Test Steps

### 1. Build & Run
```powershell
dotnet build
dotnet run
```

### 2. Test Import Batch
1. Nhấn "📥 Nhập"
2. Thêm **3 sản phẩm khác nhau** (ví dụ):
   - Sản phẩm 1: Số lượng 5, Đơn giá 100.000
   - Sản phẩm 2: Số lượng 10, Đơn giá 50.000
   - Sản phẩm 3: Số lượng 3, Đơn giá 200.000
3. Ghi chú: "Phiếu nhập test batch"
4. Nhấn "💾 Lưu Phiếu"

### 3. Kiểm Tra Kết Quả
- Vào tab "Giao Dịch"
- **Chỉ nên có 1 hàng giao dịch** (không phải 3)
- **Nhấn nút Edit hoặc Double-click** để xem chi tiết
- **Chi tiết phải hiển thị 3 sản phẩm** trong 1 transaction:
  - Sản phẩm 1: Qty 5, Price 100.000, Thành tiền 500.000
  - Sản phẩm 2: Qty 10, Price 50.000, Thành tiền 500.000
  - Sản phẩm 3: Qty 3, Price 200.000, Thành tiền 600.000

### 4. Kiểm Tra Log
Mở `Build\bin\Debug\net472\debug.log` để xem:

**Trước (SAI)**:
```
[InventoryController] Import(productId=1, qty=5)
[InventoryService] ImportStock(1)
[TransactionRepository] CreateTransaction() -> ID 1
[TransactionRepository] AddTransactionDetail() -> Detail for Transaction 1

[InventoryController] Import(productId=2, qty=10)
[InventoryService] ImportStock(2)
[TransactionRepository] CreateTransaction() -> ID 2  ← SAI! ID khác
[TransactionRepository] AddTransactionDetail() -> Detail for Transaction 2

[InventoryController] Import(productId=3, qty=3)
[InventoryService] ImportStock(3)
[TransactionRepository] CreateTransaction() -> ID 3  ← SAI! ID khác
[TransactionRepository] AddTransactionDetail() -> Detail for Transaction 3
```

**Sau (ĐÚNG)**:
```
[TransactionForm] BtnSaveTransaction_Click: Lưu 3 sản phẩm dưới Import
[TransactionForm] Chi tiết: P1:Q5, P2:Q10, P3:Q3
[TransactionForm] Gọi ImportBatch...
[InventoryController] ImportBatch được gọi với 3 sản phẩm
[InventoryService] ImportStockBatch bắt đầu với 3 sản phẩm
[InventoryService] Tạo transaction ID: 10  ← ĐÚNG! 1 ID duy nhất
[InventoryService] Import sản phẩm 1: 5 (kho từ X → X+5)
[InventoryService] Import sản phẩm 2: 10 (kho từ Y → Y+10)
[InventoryService] Import sản phẩm 3: 3 (kho từ Z → Z+3)
[InventoryService] ImportStockBatch hoàn thành
[TransactionForm] Lưu phiếu thành công!
```

### 5. Test Export Batch (tương tự)
1. Nhấn "📤 Xuất"
2. Thêm 2-3 sản phẩm
3. Nhấn "💾 Lưu Phiếu"
4. Kiểm tra kết quả = 1 transaction, N details

## Verification Checklist

- [ ] Build thành công
- [ ] Ứng dụng chạy bình thường
- [ ] Import batch tạo 1 transaction (không phải N)
- [ ] Export batch tạo 1 transaction (không phải N)
- [ ] Chi tiết sản phẩm hiển thị đúng
- [ ] Log shows single CreateTransaction call (không phải N)
- [ ] Tồn kho cập nhật đúng cho từng sản phẩm
- [ ] Khi click vào transaction → Xem chi tiết đúng

## Success Criteria
✅ Tất cả steps trên pass = FIX thành công

## Cleanup (Sau Test)
Xóa `System.Diagnostics.Debug.WriteLine()` khỏi:
1. TransactionForm.cs (lines 249-262)
2. InventoryController.cs (ImportBatch, ExportBatch)
3. InventoryService.cs (ImportStockBatch, ExportStockBatch) - tất cả Debug lines

Sau đó `dotnet build` & test lại.
