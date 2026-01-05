DROP DATABASE IF EXISTS QL_KhoHang;
CREATE DATABASE IF NOT EXISTS QL_KhoHang CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE QL_KhoHang;

-- 1. Danh mục
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Mã danh mục',
    CategoryName VARCHAR(100) NOT NULL COMMENT 'Tên danh mục'
) COMMENT = 'Danh mục SP';

-- 2. Sản phẩm
CREATE TABLE Products (
    ProductID INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Mã SP',
    ProductName VARCHAR(255) NOT NULL COMMENT 'Tên SP',
    CategoryID INT COMMENT 'Mã danh mục',
    Price DECIMAL(18, 2) DEFAULT 0 COMMENT 'Giá bán',
    Quantity INT DEFAULT 0 COMMENT 'Tồn kho',
    MinThreshold INT DEFAULT 10 COMMENT 'Ngưỡng báo',
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
) COMMENT = 'Hàng hóa';

-- 3. Phiếu Nhập/Xuất
CREATE TABLE StockTransactions (
    TransactionID INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Mã phiếu',
    Type ENUM('Import', 'Export') NOT NULL COMMENT 'Loại: Nhập/Xuất',
    DateCreated DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Ngày lập',
    Note TEXT COMMENT 'Ghi chú'
) COMMENT = 'Phiếu kho';

-- 4. Chi tiết phiếu
CREATE TABLE TransactionDetails (
    DetailID INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Mã CT',
    TransactionID INT COMMENT 'Mã phiếu',
    ProductID INT COMMENT 'Mã SP',
    Quantity INT NOT NULL COMMENT 'Số lượng',
    UnitPrice DECIMAL(18, 2) COMMENT 'Đơn giá',
    FOREIGN KEY (TransactionID) REFERENCES StockTransactions(TransactionID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
) COMMENT = 'Chi tiết phiếu';

-- 5. Nhật ký (Hỗ trợ Undo)
CREATE TABLE ActionLogs (
    LogID INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Mã log',
    ActionType VARCHAR(50) COMMENT 'Hành động',
    Descriptions TEXT COMMENT 'Mô tả',
    DataBefore JSON COMMENT 'Dữ liệu cũ',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Thời gian'
) COMMENT = 'Nhật ký';
