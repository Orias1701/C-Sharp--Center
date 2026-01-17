USE QL_KhoHang;

-- Thêm Danh mục bổ sung
INSERT INTO Categories (CategoryName) VALUES 
('Điện tử'), 
('Gia dụng'), 
('Công cụ');

-- Thêm Sản phẩm
INSERT INTO Products (ProductName, CategoryID, Price, Quantity, MinThreshold, InventoryValue) VALUES 
('iPhone 15 Pro Max', 1, 35000000, 60, 10, 2100000000),
('Samsung Galaxy S23', 1, 22000000, 30, 5, 660000000),
('Tủ lạnh LG Inverter', 2, 12000000, 8, 10, 96000000),
('Máy khoan Bosch', 3, 2500000, 20, 5, 50000000),
('Lò vi sóng Sharp', 2, 3500000, 12, 10, 42000000);

-- Phiếu Nhập ngày 2026-01-03
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, TotalValue, Note) VALUES 
('Import', '2026-01-03 09:00:00', 1, 340000000, 'Nhập hàng đầu năm');
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) VALUES 
(1, 1, 'iPhone 15 Pro Max', 10, 34000000);

-- Phiếu Xuất ngày 2026-01-04
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, TotalValue, Note) VALUES 
('Export', '2026-01-04 14:30:00', 2, 71000000, 'Xuất kho bán lẻ');
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) VALUES 
(2, 1, 'iPhone 15 Pro Max', 2, 35500000);

-- Phiếu Nhập ngày 2026-01-05 (BATCH: 2 sản phẩm trong 1 phiếu)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, TotalValue, Note) VALUES 
('Import', '2026-01-05 08:15:00', 1, 73500000, 'Bổ sung hàng gia dụng và điện tử');
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) VALUES 
(3, 3, 'Tủ lạnh LG Inverter', 5, 11500000),
(3, 5, 'Lò vi sóng Sharp', 5, 3200000);

-- Phiếu Nhập ngày 2026-01-06 (BATCH: 3 sản phẩm)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, TotalValue, Note) VALUES 
('Import', '2026-01-06 10:00:00', 2, 806000000, 'Nhập hàng điện tử');
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) VALUES 
(4, 1, 'iPhone 15 Pro Max', 15, 33500000),
(4, 2, 'Samsung Galaxy S23', 20, 21000000),
(4, 4, 'Máy khoan Bosch', 10, 2400000);

-- Nhật ký hoạt động
INSERT INTO Actions (ActionType, Descriptions, DataBefore, CreatedAt) VALUES 
('UPDATE_STOCK', 'Cập nhật tồn kho iPhone 15 Pro Max sau phiếu nhập', '{"ProductID": 1, "OldQty": 50, "NewQty": 60}', CURRENT_TIMESTAMP),
('CREATE_TRANSACTION', 'Tạo phiếu nhập batch số 3 (2 sản phẩm)', '{"TransactionID": 3, "TotalItems": 2}', CURRENT_TIMESTAMP),
('CREATE_TRANSACTION', 'Tạo phiếu nhập batch số 4 (3 sản phẩm)', '{"TransactionID": 4, "TotalItems": 3}', CURRENT_TIMESTAMP);
