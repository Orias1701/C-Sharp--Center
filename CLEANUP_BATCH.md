# Cleanup Batch Transaction Logging

Sau khi test thành công batch transaction, hãy xóa logging bằng cách sau:

## 1. TransactionForm.cs

Xóa dòng 251-254 trong `BtnSaveTransaction_Click`:
```csharp
System.Diagnostics.Debug.WriteLine($"[TransactionForm] BtnSaveTransaction_Click: Lưu {_details.Count} sản phẩm dưới {_transactionType}");
System.Diagnostics.Debug.WriteLine($"[TransactionForm] Chi tiết: {string.Join(", ", _details.Select(d => $"P{d.ProductID}:Q{d.Quantity}"))}");
System.Diagnostics.Debug.WriteLine($"[TransactionForm] Gọi ImportBatch...");
System.Diagnostics.Debug.WriteLine($"[TransactionForm] Gọi ExportBatch...");
System.Diagnostics.Debug.WriteLine($"[TransactionForm] Lưu phiếu thành công!");
System.Diagnostics.Debug.WriteLine($"[TransactionForm] Lỗi: {ex.Message}");
```

**Giữ lại**: Chỉ giữ MessageBox và logic gọi ImportBatch/ExportBatch

## 2. InventoryController.cs

Xóa dòng 73-74 (ImportBatch method):
```csharp
System.Diagnostics.Debug.WriteLine($"[InventoryController] ImportBatch được gọi với {details.Count} sản phẩm");
```

Xóa dòng 81-82 (ExportBatch method):
```csharp
System.Diagnostics.Debug.WriteLine($"[InventoryController] ExportBatch được gọi với {details.Count} sản phẩm");
```

## 3. InventoryService.cs

Xóa **tất cả** dòng `System.Diagnostics.Debug.WriteLine()` trong:
- `ImportStockBatch()` method (14 debug calls)
- `ExportStockBatch()` method (16 debug calls)

Các dòng cần xóa:
```
Line ~250: [InventoryService] ImportStockBatch bắt đầu
Line ~254: [InventoryService] Tạo transaction ID
Line ~279: [InventoryService] Import sản phẩm
Line ~288: [InventoryService] ImportStockBatch hoàn thành
Line ~294: [InventoryService] Lỗi ImportStockBatch

Line ~308: [InventoryService] ExportStockBatch bắt đầu
Line ~313: [InventoryService] Tạo transaction ID
Line ~337: [InventoryService] Export sản phẩm
Line ~346: [InventoryService] ExportStockBatch hoàn thành
Line ~353: [InventoryService] Lỗi ExportStockBatch
```

## Refactored Code Example

**ImportBatch before**:
```csharp
public bool ImportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
{
    try
    {
        System.Diagnostics.Debug.WriteLine($"[InventoryService] ImportStockBatch bắt đầu với {details.Count} sản phẩm");
        
        if (details == null || details.Count == 0)
            throw new ArgumentException("Danh sách sản phẩm không thể rỗng");

        var transaction = new StockTransaction { ... };
        int transId = _transactionRepo.CreateTransaction(transaction);
        System.Diagnostics.Debug.WriteLine($"[InventoryService] Tạo transaction ID: {transId}");
        
        // ... rest of code
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"[InventoryService] Lỗi ImportStockBatch: {ex.Message}");
        throw new Exception("Lỗi khi nhập kho batch: " + ex.Message);
    }
}
```

**ImportBatch after** (clean):
```csharp
public bool ImportStockBatch(List<(int ProductId, int Quantity, decimal UnitPrice)> details, string note = "")
{
    try
    {
        if (details == null || details.Count == 0)
            throw new ArgumentException("Danh sách sản phẩm không thể rỗng");

        var transaction = new StockTransaction { ... };
        int transId = _transactionRepo.CreateTransaction(transaction);
        
        // ... rest of code
    }
    catch (Exception ex)
    {
        throw new Exception("Lỗi khi nhập kho batch: " + ex.Message);
    }
}
```

## Build After Cleanup

```powershell
dotnet clean
dotnet build
```

Verify:
- [ ] Build thành công
- [ ] Không có warning
- [ ] App chạy bình thường
- [ ] Batch transaction vẫn hoạt động

## Test After Cleanup

1. Nhập 3 sản phẩm → Kiểm tra 1 transaction được tạo
2. Xuất 2 sản phẩm → Kiểm tra 1 transaction được tạo
3. Không có debug output

✅ Nếu tất cả pass → Fix hoàn thành!
