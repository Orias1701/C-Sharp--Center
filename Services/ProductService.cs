using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xử lý logic sản phẩm (tìm kiếm, kiểm tra ngưỡng)
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepository _productRepo;
        private readonly LogRepository _logRepo;

        public ProductService()
        {
            _productRepo = new ProductRepository();
            _logRepo = new LogRepository();
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
                // Validation
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Tên sản phẩm không được để trống");
                if (name.Length > 200)
                    throw new ArgumentException("Tên sản phẩm không được vượt quá 200 ký tự");
                if (price < 0)
                    throw new ArgumentException("Giá sản phẩm phải >= 0");
                if (price > 999999999)
                    throw new ArgumentException("Giá sản phẩm quá lớn");
                if (quantity < 0)
                    throw new ArgumentException("Số lượng không được âm");
                if (quantity > 999999)
                    throw new ArgumentException("Số lượng quá lớn");
                if (minThreshold < 0)
                    throw new ArgumentException("Ngưỡng tối thiểu không được âm");
                if (minThreshold > quantity)
                    throw new ArgumentException("Ngưỡng tối thiểu không được vượt quá số lượng hiện tại");
                if (categoryId <= 0)
                    throw new ArgumentException("Danh mục không hợp lệ");

                var product = new Product
                {
                    ProductName = name.Trim(),
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                int productId = _productRepo.AddProduct(product);
                
                // Log action - for ADD, we log empty as DataBefore since it didn't exist
                _logRepo.LogAction("ADD_PRODUCT", 
                    $"Thêm sản phẩm: {name}", 
                    "");
                
                // Mark as changed for save manager
                SaveManager.Instance.MarkAsChanged();
                
                return productId;
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
                // Validation
                if (productId <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Tên sản phẩm không được để trống");
                if (name.Length > 200)
                    throw new ArgumentException("Tên sản phẩm không được vượt quá 200 ký tự");
                if (price < 0)
                    throw new ArgumentException("Giá sản phẩm phải >= 0");
                if (price > 999999999)
                    throw new ArgumentException("Giá sản phẩm quá lớn");
                if (quantity < 0)
                    throw new ArgumentException("Số lượng không được âm");
                if (quantity > 999999)
                    throw new ArgumentException("Số lượng quá lớn");
                if (minThreshold < 0)
                    throw new ArgumentException("Ngưỡng tối thiểu không được âm");
                if (minThreshold > quantity)
                    throw new ArgumentException("Ngưỡng tối thiểu không được vượt quá số lượng hiện tại");
                if (categoryId <= 0)
                    throw new ArgumentException("Danh mục không hợp lệ");

                // Get the old product data before updating
                var oldProduct = _productRepo.GetProductById(productId);
                if (oldProduct == null)
                    throw new ArgumentException("Sản phẩm không tồn tại");

                // Log the before-state
                var beforeData = new
                {
                    ProductID = oldProduct.ProductID,
                    ProductName = oldProduct.ProductName,
                    CategoryID = oldProduct.CategoryID,
                    Price = oldProduct.Price,
                    Quantity = oldProduct.Quantity,
                    MinThreshold = oldProduct.MinThreshold
                };

                var product = new Product
                {
                    ProductID = productId,
                    ProductName = name.Trim(),
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                bool result = _productRepo.UpdateProduct(product);
                
                if (result)
                {
                    _logRepo.LogAction("UPDATE_PRODUCT",
                        $"Cập nhật sản phẩm: {name}",
                        JsonConvert.SerializeObject(beforeData));
                    
                    // Mark as changed for save manager
                    SaveManager.Instance.MarkAsChanged();
                }

                return result;
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
                if (productId <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");
                
                // Get product before deletion to log
                var product = _productRepo.GetProductById(productId);
                if (product != null)
                {
                    var beforeData = new
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        CategoryID = product.CategoryID,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        MinThreshold = product.MinThreshold
                    };
                    
                    bool result = _productRepo.DeleteProduct(productId);
                    
                    if (result)
                    {
                        _logRepo.LogAction("DELETE_PRODUCT",
                            $"Xóa sản phẩm: {product.ProductName}",
                            JsonConvert.SerializeObject(beforeData));
                        
                        // Mark as changed for save manager
                        SaveManager.Instance.MarkAsChanged();
                    }
                    
                    return result;
                }
                
                return false;
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
                if (productId <= 0)
                    throw new ArgumentException("ID sản phẩm không hợp lệ");
                return _productRepo.GetProductById(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin sản phẩm: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả danh mục
        /// </summary>
        public List<Category> GetAllCategories()
        {
            try
            {
                return _productRepo.GetAllCategories();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh mục: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm danh mục mới
        /// </summary>
        public int AddCategory(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Tên danh mục không được trống");
                if (categoryName.Length > 100)
                    throw new ArgumentException("Tên danh mục không được vượt 100 ký tự");
                
                int categoryId = _productRepo.AddCategory(categoryName);
                
                _logRepo.LogAction("ADD_CATEGORY",
                    $"Thêm danh mục: {categoryName}",
                    "");
                
                // Mark as changed for save manager
                SaveManager.Instance.MarkAsChanged();
                return categoryId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm danh mục: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        public bool UpdateCategory(int categoryId, string categoryName)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh mục không hợp lệ");
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Tên danh mục không được trống");
                if (categoryName.Length > 100)
                    throw new ArgumentException("Tên danh mục không được vượt 100 ký tự");
                
                // Get old data before updating
                var categories = _productRepo.GetAllCategories();
                var oldCategory = categories.FirstOrDefault(c => c.CategoryID == categoryId);
                
                var beforeData = new
                {
                    CategoryID = oldCategory?.CategoryID,
                    CategoryName = oldCategory?.CategoryName
                };
                
                bool result = _productRepo.UpdateCategory(categoryId, categoryName);
                
                if (result)
                {
                    _logRepo.LogAction("UPDATE_CATEGORY",
                        $"Cập nhật danh mục: {categoryName}",
                        JsonConvert.SerializeObject(beforeData));
                    SaveManager.Instance.MarkAsChanged();
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật danh mục: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh mục không hợp lệ");
                
                // Get category before deletion
                var categories = _productRepo.GetAllCategories();
                var category = categories.FirstOrDefault(c => c.CategoryID == categoryId);
                
                if (category != null)
                {
                    var beforeData = new
                    {
                        CategoryID = category.CategoryID,
                        CategoryName = category.CategoryName
                    };
                    
                    bool result = _productRepo.DeleteCategory(categoryId);
                    
                    if (result)
                    {
                        _logRepo.LogAction("DELETE_CATEGORY",
                            $"Xóa danh mục: {category.CategoryName}",
                            JsonConvert.SerializeObject(beforeData));
                        SaveManager.Instance.MarkAsChanged();
                    }
                    
                    return result;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa danh mục: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra sản phẩm có phụ thuộc khóa ngoài hay không
        /// </summary>
        public bool ProductHasDependencies(int productId)
        {
            try
            {
                return _productRepo.HasForeignKeyReferences(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra phụ thuộc: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra danh mục có sản phẩm hay không
        /// </summary>
        public bool CategoryHasProducts(int categoryId)
        {
            try
            {
                return _productRepo.CategoryHasProducts(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kiểm tra danh mục: " + ex.Message);
            }
        }
    }
}
