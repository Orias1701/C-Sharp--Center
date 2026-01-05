USE QL_KhoHang;

-- Thêm Danh mục
INSERT INTO Categories (CategoryName) VALUES 
('Điện tử'), 
('Gia dụng'), 
('Công cụ');

-- Thêm Sản phẩm
INSERT INTO Products (ProductName, CategoryID, Price, Quantity, MinThreshold) VALUES 
('iPhone 15 Pro Max', 1, 35000000, 50, 10),
('Samsung Galaxy S23', 1, 22000000, 15, 5),
('Tủ lạnh LG Inverter', 2, 12000000, 3, 10),
('Máy khoan Bosch', 3, 2500000, 20, 5),
('Lò vi sóng Sharp', 2, 3500000, 8, 10);

-- Phiếu Nhập ngày 2026-01-03
INSERT INTO StockTransactions (Type, DateCreated, Note) VALUES ('Import', '2026-01-03 09:00:00', 'Nhập hàng đầu năm');
INSERT INTO TransactionDetails (TransactionID, ProductID, Quantity, UnitPrice) VALUES (1, 1, 10, 34000000);

-- Phiếu Xuất ngày 2026-01-04
INSERT INTO StockTransactions (Type, DateCreated, Note) VALUES ('Export', '2026-01-04 14:30:00', 'Xuất kho bán lẻ');
INSERT INTO TransactionDetails (TransactionID, ProductID, Quantity, UnitPrice) VALUES (2, 1, 2, 35500000);

-- Phiếu Nhập ngày 2026-01-05
INSERT INTO StockTransactions (Type, DateCreated, Note) VALUES ('Import', '2026-01-05 08:15:00', 'Bổ sung hàng gia dụng');
INSERT INTO TransactionDetails (TransactionID, ProductID, Quantity, UnitPrice) VALUES (3, 3, 5, 11500000);
INSERT INTO TransactionDetails (TransactionID, ProductID, Quantity, UnitPrice) VALUES (3, 5, 5, 32000000);

-- Nhật ký
INSERT INTO ActionLogs (ActionType, Descriptions, DataBefore) VALUES 
('UPDATE_STOCK', 'Cập nhật tồn kho iPhone 15', '{"ProductID": 1, "OldQty": 40, "NewQty": 50}'),
('CREATE_TRANSACTION', 'Tạo phiếu nhập số 3', '{"TransactionID": 3, "TotalItems": 2}');
