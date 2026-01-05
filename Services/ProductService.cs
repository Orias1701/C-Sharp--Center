using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xử lý logic sản phẩm (tìm kiếm, kiểm tra ngưỡng)
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepository _productRepo;

        public ProductService()
        {
            _productRepo = new ProductRepository();
        }

        /// <summary>
        /// Lấy tất cả sản phẩm
        /// </summary>
        public List<Product> GetAllProducts()
        {
            try
            {
                return _productRepo.GetAllProducts();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo tên (không phân biệt hoa thường)
        /// </summary>
        public List<Product> SearchProductByName(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetAllProducts();

                var products = GetAllProducts();
                return products.Where(p => p.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo danh mục
        /// </summary>
        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = GetAllProducts();
                return products.Where(p => p.CategoryID == categoryId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy sản phẩm theo danh mục: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra sản phẩm có cảnh báo tồn kho hay không
        /// </summary>
        public bool IsProductLowStock(int productId)
        {
            try
            {
                var product = _productRepo.GetProductById(productId);
                return product != null && product.IsLowStock;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tồn kho: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        public int AddProduct(string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Tên sản phẩm không được để trống");
                if (price < 0)
                    throw new ArgumentException("Giá sản phẩm phải >= 0");

                var product = new Product
                {
                    ProductName = name,
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                return _productRepo.AddProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        public bool UpdateProduct(int productId, string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Tên sản phẩm không được để trống");

                var product = new Product
                {
                    ProductID = productId,
                    ProductName = name,
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                return _productRepo.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            try
            {
                return _productRepo.DeleteProduct(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            try
            {
                return _productRepo.GetProductById(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }
    }
}
