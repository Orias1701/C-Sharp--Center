USE QL_KhoHang;

-- =================================================================================
-- PHẦN 1: LÀM SẠCH VÀ TẠO DỮ LIỆU NỀN (CATEGORIES & PRODUCTS)
-- =================================================================================

SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE TransactionDetails;
TRUNCATE TABLE StockTransactions;
TRUNCATE TABLE Products;
TRUNCATE TABLE Categories;
SET FOREIGN_KEY_CHECKS = 1;

-- 1. Thêm 10 Danh mục
INSERT INTO Categories (CategoryName) VALUES 
('Điện thoại & Phụ kiện'), ('Máy tính & Laptop'), ('Thiết bị gia dụng'), 
('Thời trang Nam'), ('Thời trang Nữ'), ('Mỹ phẩm & Làm đẹp'), 
('Sách & Văn phòng phẩm'), ('Thể thao & Du lịch'), ('Đồ chơi & Mẹ bé'), ('Ô tô & Xe máy');

-- 2. Thêm 50 Sản phẩm (Giá từ 100k đến 900tr)
INSERT INTO Products (ProductName, CategoryID, Price, Quantity, MinThreshold, InventoryValue) VALUES 
('iPhone 15 Pro Max 1TB', 1, 45000000, 50, 5, 2250000000), ('Samsung Galaxy S24 Ultra', 1, 30000000, 40, 5, 1200000000),
('Xiaomi 14 Ultra', 1, 25000000, 30, 5, 750000000), ('Ốp lưng MagSafe Cao cấp', 1, 500000, 200, 20, 100000000),
('Sạc dự phòng Anker 20000mAh', 1, 1200000, 100, 10, 120000000), ('MacBook Pro 16 inch M3 Max', 2, 90000000, 10, 2, 900000000),
('Dell XPS 15 9530', 2, 55000000, 15, 3, 825000000), ('Màn hình LG UltraGear 27"', 2, 8500000, 25, 5, 212500000),
('Bàn phím cơ Keychron Q1', 2, 4500000, 40, 5, 180000000), ('Chuột Logitech MX Master 3S', 2, 2500000, 60, 10, 150000000),
('Tủ lạnh Hitachi Inverter 569L', 3, 35000000, 20, 2, 700000000), ('Máy giặt Electrolux 10kg', 3, 12000000, 30, 5, 360000000),
('Robot hút bụi Roborock S8', 3, 18000000, 25, 5, 450000000), ('Nồi chiên không dầu Philips', 3, 3500000, 50, 10, 175000000),
('Máy lọc không khí Dyson', 3, 15000000, 20, 5, 300000000), ('Áo sơ mi Pierre Cardin', 4, 1500000, 100, 20, 150000000),
('Quần âu Việt Tiến', 4, 800000, 150, 20, 120000000), ('Giày da nam Biti\'s Hunter', 4, 1200000, 80, 15, 96000000),
('Thắt lưng da cá sấu thật', 4, 5000000, 30, 5, 150000000), ('Đồng hồ Rolex Submariner Date', 4, 350000000, 2, 1, 700000000),
('Đầm dạ hội thiết kế', 5, 5000000, 20, 5, 100000000), ('Túi xách Hermes Birkin 30', 5, 850000000, 1, 1, 850000000),
('Giày cao gót Louboutin', 5, 25000000, 10, 2, 250000000), ('Áo dài lụa tơ tằm', 5, 8000000, 30, 5, 240000000),
('Kính mát Gucci chính hãng', 5, 10000000, 25, 5, 250000000), ('Son môi Tom Ford', 6, 1500000, 100, 20, 150000000),
('Nước hoa Chanel No.5 100ml', 6, 4500000, 50, 10, 225000000), ('Kem dưỡng ẩm La Mer', 6, 12000000, 30, 5, 360000000),
('Máy rửa mặt Foreo Luna 4', 6, 5000000, 40, 10, 200000000), ('Bộ trang điểm chuyên nghiệp MAC', 6, 8000000, 20, 5, 160000000),
('Bộ sách Harry Potter (Full)', 7, 3000000, 30, 5, 90000000), ('Bút ký Parker Sonnet', 7, 5000000, 40, 5, 200000000),
('Máy tính Casio FX-580VN X', 7, 800000, 200, 50, 160000000), ('Sổ tay Moleskine Classic', 7, 1200000, 100, 20, 120000000),
('Bàn cắt giấy công nghiệp A3', 7, 2500000, 15, 3, 37500000), ('Máy chạy bộ Kingsport BK', 8, 15000000, 10, 2, 150000000),
('Xe đạp địa hình Giant ATX', 8, 12000000, 15, 3, 180000000), ('Lều cắm trại 4 người Naturehike', 8, 3500000, 30, 5, 105000000),
('Vợt Tennis Wilson Pro Staff', 8, 4500000, 25, 5, 112500000), ('Vali Samsonite size L', 8, 10000000, 20, 5, 200000000),
('Xe đẩy em bé Combi', 9, 12000000, 15, 3, 180000000), ('Máy hút sữa Medela Pump', 9, 8000000, 20, 5, 160000000),
('Bộ LEGO Technic Bugatti Chiron', 9, 10000000, 10, 2, 100000000), ('Ghế ngồi ô tô cho bé Joie', 9, 6000000, 25, 5, 150000000),
('Nhà banh cầu trượt liên hoàn', 9, 2500000, 30, 5, 75000000), ('Xe máy Honda SH 350i', 10, 150000000, 5, 1, 750000000),
('Xe mô tô Ducati Panigale V4', 10, 890000000, 1, 1, 890000000), ('Mũ bảo hiểm AGV Pista GP', 10, 35000000, 10, 2, 350000000),
('Lốp xe Michelin Pilot Road 6', 10, 4500000, 40, 10, 180000000), ('Camera hành trình Vietmap SpeedMap', 10, 5000000, 50, 10, 250000000);

-- =================================================================================
-- PHẦN 2: 100 PHIẾU NHẬP (Mỗi phiếu 2-5 SP, SL 5-20, Giá 95%)
-- =================================================================================

-- Phiếu 1 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 100 DAY), 1, 'Phiếu nhập #1');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 2 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 99 DAY), 2, 'Phiếu nhập #2');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 3 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 98 DAY), 1, 'Phiếu nhập #3');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 4 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 97 DAY), 2, 'Phiếu nhập #4');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 5 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 96 DAY), 1, 'Phiếu nhập #5');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 6 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 95 DAY), 2, 'Phiếu nhập #6');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 7 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 94 DAY), 1, 'Phiếu nhập #7');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 8 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 93 DAY), 2, 'Phiếu nhập #8');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 9 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 92 DAY), 1, 'Phiếu nhập #9');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 10 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 91 DAY), 2, 'Phiếu nhập #10');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 11 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 90 DAY), 1, 'Phiếu nhập #11');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 12 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 89 DAY), 2, 'Phiếu nhập #12');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 13 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 88 DAY), 1, 'Phiếu nhập #13');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 14 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 87 DAY), 2, 'Phiếu nhập #14');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 15 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 86 DAY), 1, 'Phiếu nhập #15');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 16 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 85 DAY), 2, 'Phiếu nhập #16');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 17 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 84 DAY), 1, 'Phiếu nhập #17');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 18 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 83 DAY), 2, 'Phiếu nhập #18');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 19 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 82 DAY), 1, 'Phiếu nhập #19');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 20 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 81 DAY), 2, 'Phiếu nhập #20');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 21 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 80 DAY), 1, 'Phiếu nhập #21');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 22 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 79 DAY), 2, 'Phiếu nhập #22');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 23 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 78 DAY), 1, 'Phiếu nhập #23');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 24 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 77 DAY), 2, 'Phiếu nhập #24');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 25 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 76 DAY), 1, 'Phiếu nhập #25');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 26 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 75 DAY), 2, 'Phiếu nhập #26');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 27 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 74 DAY), 1, 'Phiếu nhập #27');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 28 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 73 DAY), 2, 'Phiếu nhập #28');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 29 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 72 DAY), 1, 'Phiếu nhập #29');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 30 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 71 DAY), 2, 'Phiếu nhập #30');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 31 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 70 DAY), 1, 'Phiếu nhập #31');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 32 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 69 DAY), 2, 'Phiếu nhập #32');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 33 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 68 DAY), 1, 'Phiếu nhập #33');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 34 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 67 DAY), 2, 'Phiếu nhập #34');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 35 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 66 DAY), 1, 'Phiếu nhập #35');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 36 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 65 DAY), 2, 'Phiếu nhập #36');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 37 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 64 DAY), 1, 'Phiếu nhập #37');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 38 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 63 DAY), 2, 'Phiếu nhập #38');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 39 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 62 DAY), 1, 'Phiếu nhập #39');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 40 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 61 DAY), 2, 'Phiếu nhập #40');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 41 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 60 DAY), 1, 'Phiếu nhập #41');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 42 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 59 DAY), 2, 'Phiếu nhập #42');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 43 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 58 DAY), 1, 'Phiếu nhập #43');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 44 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 57 DAY), 2, 'Phiếu nhập #44');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 45 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 56 DAY), 1, 'Phiếu nhập #45');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 46 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 55 DAY), 2, 'Phiếu nhập #46');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 47 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 54 DAY), 1, 'Phiếu nhập #47');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 48 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 53 DAY), 2, 'Phiếu nhập #48');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 49 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 52 DAY), 1, 'Phiếu nhập #49');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 50 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 51 DAY), 2, 'Phiếu nhập #50');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 51 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 50 DAY), 1, 'Phiếu nhập #51');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 52 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 49 DAY), 2, 'Phiếu nhập #52');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 53 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 48 DAY), 1, 'Phiếu nhập #53');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 54 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 47 DAY), 2, 'Phiếu nhập #54');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 55 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 46 DAY), 1, 'Phiếu nhập #55');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 56 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 45 DAY), 2, 'Phiếu nhập #56');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 57 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 44 DAY), 1, 'Phiếu nhập #57');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 58 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 43 DAY), 2, 'Phiếu nhập #58');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 59 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 42 DAY), 1, 'Phiếu nhập #59');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 60 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 41 DAY), 2, 'Phiếu nhập #60');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 61 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 40 DAY), 1, 'Phiếu nhập #61');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 62 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 39 DAY), 2, 'Phiếu nhập #62');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 63 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 38 DAY), 1, 'Phiếu nhập #63');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 64 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 37 DAY), 2, 'Phiếu nhập #64');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 65 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 36 DAY), 1, 'Phiếu nhập #65');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 66 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 35 DAY), 2, 'Phiếu nhập #66');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 67 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 34 DAY), 1, 'Phiếu nhập #67');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 68 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 33 DAY), 2, 'Phiếu nhập #68');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 69 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 32 DAY), 1, 'Phiếu nhập #69');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 70 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 31 DAY), 2, 'Phiếu nhập #70');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 71 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 30 DAY), 1, 'Phiếu nhập #71');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 72 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 29 DAY), 2, 'Phiếu nhập #72');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 73 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 28 DAY), 1, 'Phiếu nhập #73');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 74 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 27 DAY), 2, 'Phiếu nhập #74');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 75 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 26 DAY), 1, 'Phiếu nhập #75');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 76 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 25 DAY), 2, 'Phiếu nhập #76');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 77 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 24 DAY), 1, 'Phiếu nhập #77');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 78 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 23 DAY), 2, 'Phiếu nhập #78');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 79 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 22 DAY), 1, 'Phiếu nhập #79');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 80 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 21 DAY), 2, 'Phiếu nhập #80');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 81 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 20 DAY), 1, 'Phiếu nhập #81');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 82 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 19 DAY), 2, 'Phiếu nhập #82');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 83 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 18 DAY), 1, 'Phiếu nhập #83');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 84 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 17 DAY), 2, 'Phiếu nhập #84');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 85 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 16 DAY), 1, 'Phiếu nhập #85');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 86 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 15 DAY), 2, 'Phiếu nhập #86');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 87 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 14 DAY), 1, 'Phiếu nhập #87');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 88 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 13 DAY), 2, 'Phiếu nhập #88');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 89 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 12 DAY), 1, 'Phiếu nhập #89');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 90 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 11 DAY), 2, 'Phiếu nhập #90');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 91 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 10 DAY), 1, 'Phiếu nhập #91');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 92 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 9 DAY), 2, 'Phiếu nhập #92');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 93 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 8 DAY), 1, 'Phiếu nhập #93');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 94 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 7 DAY), 2, 'Phiếu nhập #94');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 95 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 6 DAY), 1, 'Phiếu nhập #95');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 96 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 5 DAY), 2, 'Phiếu nhập #96');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 97 (2 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 4 DAY), 1, 'Phiếu nhập #97');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 98 (4 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 3 DAY), 2, 'Phiếu nhập #98');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 99 (3 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 2 DAY), 1, 'Phiếu nhập #99');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu 100 (5 SP)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Import', DATE_SUB(NOW(), INTERVAL 1 DAY), 2, 'Phiếu nhập #100');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 0.95 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- =================================================================================
-- PHẦN 3: 100 PHIẾU XUẤT (Mỗi phiếu 2-5 SP, SL 5-20, Giá 120% giá gốc, 2-3 phiếu/ngày)
-- Kết thúc: 17/01/2026. Lùi 40 ngày -> Bắt đầu khoảng 08/12/2025
-- =================================================================================

-- Phiếu Xuất 1 (Ngày: 2025-12-08)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-08 09:00:00', 1, 'Phiếu xuất #1');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 2 (Ngày: 2025-12-08)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-08 14:00:00', 2, 'Phiếu xuất #2');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 3 (Ngày: 2025-12-08)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-08 16:30:00', 1, 'Phiếu xuất #3');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 4 (Ngày: 2025-12-09)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-09 10:15:00', 2, 'Phiếu xuất #4');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 5 (Ngày: 2025-12-09)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-09 15:45:00', 1, 'Phiếu xuất #5');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 6 (Ngày: 2025-12-10)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-10 09:30:00', 2, 'Phiếu xuất #6');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 7 (Ngày: 2025-12-10)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-10 13:20:00', 1, 'Phiếu xuất #7');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 8 (Ngày: 2025-12-10)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-10 16:50:00', 2, 'Phiếu xuất #8');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 9 (Ngày: 2025-12-11)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-11 11:00:00', 1, 'Phiếu xuất #9');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 10 (Ngày: 2025-12-11)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-11 14:40:00', 2, 'Phiếu xuất #10');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 11 (Ngày: 2025-12-12)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-12 09:15:00', 1, 'Phiếu xuất #11');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 12 (Ngày: 2025-12-12)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-12 13:50:00', 2, 'Phiếu xuất #12');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 13 (Ngày: 2025-12-12)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-12 16:10:00', 1, 'Phiếu xuất #13');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 14 (Ngày: 2025-12-13)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-13 10:30:00', 2, 'Phiếu xuất #14');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 15 (Ngày: 2025-12-13)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-13 15:00:00', 1, 'Phiếu xuất #15');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 16 (Ngày: 2025-12-14)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-14 09:45:00', 2, 'Phiếu xuất #16');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 17 (Ngày: 2025-12-14)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-14 14:15:00', 1, 'Phiếu xuất #17');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 18 (Ngày: 2025-12-14)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-14 16:45:00', 2, 'Phiếu xuất #18');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 19 (Ngày: 2025-12-15)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-15 11:30:00', 1, 'Phiếu xuất #19');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 20 (Ngày: 2025-12-15)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-15 15:30:00', 2, 'Phiếu xuất #20');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 21 (Ngày: 2025-12-16)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-16 09:00:00', 1, 'Phiếu xuất #21');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 22 (Ngày: 2025-12-16)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-16 13:40:00', 2, 'Phiếu xuất #22');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 23 (Ngày: 2025-12-16)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-16 16:20:00', 1, 'Phiếu xuất #23');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 24 (Ngày: 2025-12-17)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-17 10:10:00', 2, 'Phiếu xuất #24');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 25 (Ngày: 2025-12-17)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-17 14:50:00', 1, 'Phiếu xuất #25');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 26 (Ngày: 2025-12-18)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-18 09:20:00', 2, 'Phiếu xuất #26');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 27 (Ngày: 2025-12-18)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-18 13:30:00', 1, 'Phiếu xuất #27');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 28 (Ngày: 2025-12-18)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-18 17:00:00', 2, 'Phiếu xuất #28');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 29 (Ngày: 2025-12-19)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-19 11:15:00', 1, 'Phiếu xuất #29');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 30 (Ngày: 2025-12-19)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-19 15:40:00', 2, 'Phiếu xuất #30');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 31 (Ngày: 2025-12-20)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-20 09:50:00', 1, 'Phiếu xuất #31');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 32 (Ngày: 2025-12-20)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-20 14:10:00', 2, 'Phiếu xuất #32');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 33 (Ngày: 2025-12-20)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-20 16:40:00', 1, 'Phiếu xuất #33');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 34 (Ngày: 2025-12-21)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-21 10:20:00', 2, 'Phiếu xuất #34');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 35 (Ngày: 2025-12-21)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-21 15:10:00', 1, 'Phiếu xuất #35');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 36 (Ngày: 2025-12-22)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-22 09:05:00', 2, 'Phiếu xuất #36');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 37 (Ngày: 2025-12-22)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-22 13:25:00', 1, 'Phiếu xuất #37');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 38 (Ngày: 2025-12-22)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-22 16:55:00', 2, 'Phiếu xuất #38');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 39 (Ngày: 2025-12-23)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-23 11:45:00', 1, 'Phiếu xuất #39');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 40 (Ngày: 2025-12-23)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-23 15:50:00', 2, 'Phiếu xuất #40');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 41 (Ngày: 2025-12-24)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-24 09:35:00', 1, 'Phiếu xuất #41');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 42 (Ngày: 2025-12-24)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-24 14:05:00', 2, 'Phiếu xuất #42');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 43 (Ngày: 2025-12-24)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-24 16:35:00', 1, 'Phiếu xuất #43');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 44 (Ngày: 2025-12-25)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-25 10:00:00', 2, 'Phiếu xuất #44');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 45 (Ngày: 2025-12-25)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-25 15:25:00', 1, 'Phiếu xuất #45');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 46 (Ngày: 2025-12-26)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-26 09:55:00', 2, 'Phiếu xuất #46');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 47 (Ngày: 2025-12-26)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-26 13:45:00', 1, 'Phiếu xuất #47');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 48 (Ngày: 2025-12-26)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-26 17:15:00', 2, 'Phiếu xuất #48');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 49 (Ngày: 2025-12-27)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-27 11:20:00', 1, 'Phiếu xuất #49');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 50 (Ngày: 2025-12-27)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-27 16:00:00', 2, 'Phiếu xuất #50');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 51 (Ngày: 2025-12-28)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-28 09:10:00', 1, 'Phiếu xuất #51');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 52 (Ngày: 2025-12-28)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-28 13:35:00', 2, 'Phiếu xuất #52');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 53 (Ngày: 2025-12-28)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-28 16:05:00', 1, 'Phiếu xuất #53');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 54 (Ngày: 2025-12-29)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-29 10:25:00', 2, 'Phiếu xuất #54');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 55 (Ngày: 2025-12-29)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-29 15:15:00', 1, 'Phiếu xuất #55');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 56 (Ngày: 2025-12-30)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-30 09:40:00', 2, 'Phiếu xuất #56');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 57 (Ngày: 2025-12-30)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-30 14:30:00', 1, 'Phiếu xuất #57');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 58 (Ngày: 2025-12-30)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-30 17:20:00', 2, 'Phiếu xuất #58');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 59 (Ngày: 2025-12-31)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-31 11:50:00', 1, 'Phiếu xuất #59');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 60 (Ngày: 2025-12-31)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2025-12-31 16:15:00', 2, 'Phiếu xuất #60');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 61 (Ngày: 2026-01-01)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-01 09:25:00', 1, 'Phiếu xuất #61');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 62 (Ngày: 2026-01-01)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-01 13:55:00', 2, 'Phiếu xuất #62');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 63 (Ngày: 2026-01-01)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-01 16:25:00', 1, 'Phiếu xuất #63');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 64 (Ngày: 2026-01-02)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-02 10:40:00', 2, 'Phiếu xuất #64');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 65 (Ngày: 2026-01-02)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-02 15:05:00', 1, 'Phiếu xuất #65');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 66 (Ngày: 2026-01-03)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-03 09:30:00', 2, 'Phiếu xuất #66');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 67 (Ngày: 2026-01-03)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-03 14:00:00', 1, 'Phiếu xuất #67');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 68 (Ngày: 2026-01-03)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-03 17:30:00', 2, 'Phiếu xuất #68');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 69 (Ngày: 2026-01-04)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-04 11:10:00', 1, 'Phiếu xuất #69');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 70 (Ngày: 2026-01-04)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-04 15:55:00', 2, 'Phiếu xuất #70');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 71 (Ngày: 2026-01-05)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-05 09:10:00', 1, 'Phiếu xuất #71');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 72 (Ngày: 2026-01-05)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-05 13:40:00', 2, 'Phiếu xuất #72');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 73 (Ngày: 2026-01-05)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-05 16:10:00', 1, 'Phiếu xuất #73');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 74 (Ngày: 2026-01-06)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-06 10:20:00', 2, 'Phiếu xuất #74');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 75 (Ngày: 2026-01-06)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-06 15:10:00', 1, 'Phiếu xuất #75');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 76 (Ngày: 2026-01-07)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-07 09:45:00', 2, 'Phiếu xuất #76');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 77 (Ngày: 2026-01-07)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-07 14:20:00', 1, 'Phiếu xuất #77');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 78 (Ngày: 2026-01-07)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-07 17:15:00', 2, 'Phiếu xuất #78');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 79 (Ngày: 2026-01-08)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-08 11:35:00', 1, 'Phiếu xuất #79');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 80 (Ngày: 2026-01-08)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-08 16:00:00', 2, 'Phiếu xuất #80');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 81 (Ngày: 2026-01-09)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-09 09:15:00', 1, 'Phiếu xuất #81');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 82 (Ngày: 2026-01-09)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-09 13:50:00', 2, 'Phiếu xuất #82');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 83 (Ngày: 2026-01-09)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-09 16:40:00', 1, 'Phiếu xuất #83');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 84 (Ngày: 2026-01-10)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-10 10:30:00', 2, 'Phiếu xuất #84');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 85 (Ngày: 2026-01-10)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-10 15:20:00', 1, 'Phiếu xuất #85');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 86 (Ngày: 2026-01-11)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-11 09:50:00', 2, 'Phiếu xuất #86');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 87 (Ngày: 2026-01-11)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-11 14:10:00', 1, 'Phiếu xuất #87');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 88 (Ngày: 2026-01-11)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-11 17:00:00', 2, 'Phiếu xuất #88');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 89 (Ngày: 2026-01-12)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-12 11:25:00', 1, 'Phiếu xuất #89');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 90 (Ngày: 2026-01-12)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-12 15:55:00', 2, 'Phiếu xuất #90');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 91 (Ngày: 2026-01-13)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-13 09:05:00', 1, 'Phiếu xuất #91');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 92 (Ngày: 2026-01-13)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-13 13:30:00', 2, 'Phiếu xuất #92');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 93 (Ngày: 2026-01-13)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-13 16:15:00', 1, 'Phiếu xuất #93');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 94 (Ngày: 2026-01-14)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-14 10:45:00', 2, 'Phiếu xuất #94');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 95 (Ngày: 2026-01-14)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-14 15:35:00', 1, 'Phiếu xuất #95');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 96 (Ngày: 2026-01-15)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-15 09:20:00', 2, 'Phiếu xuất #96');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 97 (Ngày: 2026-01-15)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-15 14:05:00', 1, 'Phiếu xuất #97');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 3;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 98 (Ngày: 2026-01-15)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-15 17:00:00', 2, 'Phiếu xuất #98');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 2;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 99 (Ngày: 2026-01-16)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-16 11:40:00', 1, 'Phiếu xuất #99');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 4;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;

-- Phiếu Xuất 100 (Ngày: 2026-01-17)
INSERT INTO StockTransactions (Type, DateCreated, CreatedByUserID, Note) VALUES ('Export', '2026-01-17 16:30:00', 2, 'Phiếu xuất #100');
SET @tid = LAST_INSERT_ID();
INSERT INTO TransactionDetails (TransactionID, ProductID, ProductName, Quantity, UnitPrice) SELECT @tid, ProductID, ProductName, FLOOR(5 + RAND()*16), Price * 1.2 FROM Products ORDER BY RAND() LIMIT 5;
UPDATE StockTransactions SET TotalValue = (SELECT SUM(Quantity*UnitPrice) FROM TransactionDetails WHERE TransactionID = @tid) WHERE TransactionID = @tid;