using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n sáº£n pháº©m
    /// </summary>
    public class ProductController
    {
        private readonly ProductService _productService;

        public ProductController()
        {
            _productService = new ProductService();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ sáº£n pháº©m
        /// </summary>
        public List<Product> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        /// <summary>
        /// TÃ¬m kiáº¿m sáº£n pháº©m theo tÃªn
        /// </summary>
        public List<Product> SearchProduct(string keyword)
        {
            return _productService.SearchProductByName(keyword);
        }

        /// <summary>
        /// Láº¥y sáº£n pháº©m theo danh má»¥c
        /// </summary>
        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _productService.GetProductsByCategory(categoryId);
        }

        /// <summary>
        /// Láº¥y sáº£n pháº©m theo ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            return _productService.GetProductById(productId);
        }

        /// <summary>
        /// ThÃªm sáº£n pháº©m má»›i (overload)
        /// </summary>
        public int AddProduct(Product product)
        {
            return _productService.AddProduct(product.ProductName, product.CategoryID, product.Price, product.Quantity, product.MinThreshold);
        }

        /// <summary>
        /// ThÃªm sáº£n pháº©m má»›i
        /// </summary>
        public int CreateProduct(string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            return _productService.AddProduct(name, categoryId, price, quantity, minThreshold);
        }

        /// <summary>
        /// Cáº­p nháº­t sáº£n pháº©m (overload)
        /// </summary>
        public bool UpdateProduct(Product product)
        {
            return _productService.UpdateProduct(product.ProductID, product.ProductName, product.CategoryID, product.Price, product.Quantity, product.MinThreshold);
        }

        /// <summary>
        /// Cáº­p nháº­t sáº£n pháº©m
        /// </summary>
        public bool UpdateProductFull(int productId, string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            return _productService.UpdateProduct(productId, name, categoryId, price, quantity, minThreshold);
        }

        /// <summary>
        /// XÃ³a sáº£n pháº©m
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            return _productService.DeleteProduct(productId);
        }

        /// <summary>
        /// Kiá»ƒm tra sáº£n pháº©m cÃ³ cáº£nh bÃ¡o tá»“n kho
        /// </summary>
        public bool IsLowStock(int productId)
        {
            return _productService.IsProductLowStock(productId);
        }

        /// <summary>
        /// Kiá»ƒm tra sáº£n pháº©m cÃ³ phá»¥ thuá»™c khÃ³a ngoÃ i
        /// </summary>
        public bool ProductHasDependencies(int productId)
        {
            return _productService.ProductHasDependencies(productId);
        }
    }
}




