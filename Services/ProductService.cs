using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic sáº£n pháº©m vÃ  danh má»¥c
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ sáº£n pháº©m (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - Quáº£n lÃ½ danh má»¥c (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - TÃ¬m kiáº¿m sáº£n pháº©m: Theo tÃªn, danh má»¥c
    /// - Cáº£nh bÃ¡o tá»“n kho: Kiá»ƒm tra ngÆ°á»¡ng tá»‘i thiá»ƒu
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o (ID, tÃªn, giÃ¡, v.v...)
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Logging: Ghi nháº­t kÃ½ Actions
    /// 4. Change tracking: Gá»i ActionsService.MarkAsChanged()
    /// 5. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepository _productRepo;
        private readonly ActionsRepository _logRepo;

        public ProductService()
        {
            _productRepo = new ProductRepository();
            _logRepo = new ActionsRepository();
        }

        // ========== PRODUCT CRUD ==========

        /// <summary>
        /// Láº¥y táº¥t cáº£ sáº£n pháº©m
        /// </summary>
        public List<Product> GetAllProducts()
        {
            try
            {
                return _productRepo.GetAllProducts();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ¬m kiáº¿m sáº£n pháº©m theo tÃªn (khÃ´ng phÃ¢n biá»‡t hoa thÆ°á»ng)
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
                throw new Exception("Lá»—i khi tÃ¬m kiáº¿m sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ¬m kiáº¿m sáº£n pháº©m theo danh má»¥c
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
                throw new Exception("Lá»—i khi láº¥y sáº£n pháº©m theo danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra sáº£n pháº©m cÃ³ cáº£nh bÃ¡o tá»“n kho hay khÃ´ng
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
                throw new Exception("Lá»—i khi kiá»ƒm tra tá»“n kho: " + ex.Message);
            }
        }

        /// <summary>
        /// ThÃªm sáº£n pháº©m má»›i
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra tÃªn, giÃ¡, sá»‘ lÆ°á»£ng, ngÆ°á»¡ng tá»‘i thiá»ƒu
        /// 2. Repository.AddProduct(): ThÃªm vÃ o database
        /// 3. LogAction(): Ghi nháº­t kÃ½ (DataBefore trá»‘ng vÃ¬ chÆ°a tá»“n táº¡i)
        /// 4. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 5. Return: Tráº£ vá» ID sáº£n pháº©m vá»«a thÃªm
        /// </summary>
        public int AddProduct(string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            try
            {
                // Validation cÃ¡c trÆ°á»ng Ä‘áº§u vÃ o
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng");
                if (name.Length > 200)
                    throw new ArgumentException("TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ 200 kÃ½ tá»±");
                if (price < 0)
                    throw new ArgumentException("GiÃ¡ sáº£n pháº©m pháº£i >= 0");
                if (price > 999999999)
                    throw new ArgumentException("GiÃ¡ sáº£n pháº©m quÃ¡ lá»›n");
                if (quantity < 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (minThreshold < 0)
                    throw new ArgumentException("NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (minThreshold > quantity)
                    throw new ArgumentException("NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ sá»‘ lÆ°á»£ng hiá»‡n táº¡i");
                if (categoryId <= 0)
                    throw new ArgumentException("Danh má»¥c khÃ´ng há»£p lá»‡");

                // Táº¡o Ä‘á»‘i tÆ°á»£ng Product vÃ  gá»i repository
                var product = new Product
                {
                    ProductName = name.Trim(),
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                int productId = _productRepo.AddProduct(product);
                
                // Ghi nháº­t kÃ½ (DataBefore trá»‘ng vÃ¬ Ä‘Ã¢y lÃ  thÃªm má»›i)
                var log = new Actions
                {
                    ActionType = "ADD_PRODUCT",
                    Descriptions = $"ThÃªm sáº£n pháº©m: {name}",
                    DataBefore = "",
                    CreatedAt = DateTime.Now
                };
                _logRepo.LogAction(log);
                
                // Mark as changed for save manager
                ActionsService.Instance.MarkAsChanged();
                
                return productId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t thÃ´ng tin sáº£n pháº©m
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra táº¥t cáº£ cÃ¡c trÆ°á»ng
        /// 2. GetProductById(): Láº¥y dá»¯ liá»‡u cÅ© trÆ°á»›c khi thay Ä‘á»•i
        /// 3. Repository.UpdateProduct(): Cáº­p nháº­t vÃ o database
        /// 4. LogAction(): Ghi nháº­t kÃ½ vá»›i dá»¯ liá»‡u cÅ© (beforeData)
        /// 5. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 6. Return: Tráº£ vá» káº¿t quáº£ thÃ nh cÃ´ng/tháº¥t báº¡i
        /// </summary>
        public bool UpdateProduct(int productId, string name, int categoryId, decimal price, int quantity, int minThreshold)
        {
            try
            {
                // Validation cÃ¡c trÆ°á»ng Ä‘áº§u vÃ o
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng");
                if (name.Length > 200)
                    throw new ArgumentException("TÃªn sáº£n pháº©m khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ 200 kÃ½ tá»±");
                if (price < 0)
                    throw new ArgumentException("GiÃ¡ sáº£n pháº©m pháº£i >= 0");
                if (price > 999999999)
                    throw new ArgumentException("GiÃ¡ sáº£n pháº©m quÃ¡ lá»›n");
                if (quantity < 0)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (quantity > 999999)
                    throw new ArgumentException("Sá»‘ lÆ°á»£ng quÃ¡ lá»›n");
                if (minThreshold < 0)
                    throw new ArgumentException("NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c Ã¢m");
                if (minThreshold > quantity)
                    throw new ArgumentException("NgÆ°á»¡ng tá»‘i thiá»ƒu khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ sá»‘ lÆ°á»£ng hiá»‡n táº¡i");
                if (categoryId <= 0)
                    throw new ArgumentException("Danh má»¥c khÃ´ng há»£p lá»‡");

                // Láº¥y dá»¯ liá»‡u cÅ© trÆ°á»›c khi cáº­p nháº­t (Ä‘á»ƒ ghi nháº­t kÃ½)
                var oldProduct = _productRepo.GetProductById(productId);
                if (oldProduct == null)
                    throw new ArgumentException("Sáº£n pháº©m khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    ProductID = oldProduct.ProductID,
                    ProductName = oldProduct.ProductName,
                    CategoryID = oldProduct.CategoryID,
                    Price = oldProduct.Price,
                    Quantity = oldProduct.Quantity,
                    MinThreshold = oldProduct.MinThreshold
                };

                // Táº¡o Ä‘á»‘i tÆ°á»£ng Product vá»›i dá»¯ liá»‡u má»›i
                var product = new Product
                {
                    ProductID = productId,
                    ProductName = name.Trim(),
                    CategoryID = categoryId,
                    Price = price,
                    Quantity = quantity,
                    MinThreshold = minThreshold
                };

                // Cáº­p nháº­t vÃ o database
                bool result = _productRepo.UpdateProduct(product);
                
                if (result)
                {
                    // Ghi nháº­t kÃ½ vá»›i dá»¯ liá»‡u cÅ©
                    var log = new Actions
                    {
                        ActionType = "UPDATE_PRODUCT",
                        Descriptions = $"Cáº­p nháº­t sáº£n pháº©m: {name}",
                        DataBefore = JsonConvert.SerializeObject(beforeData),
                        CreatedAt = DateTime.Now
                    };
                    _logRepo.LogAction(log);
                    
                    // ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i chÆ°a lÆ°u
                    ActionsService.Instance.MarkAsChanged();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi cáº­p nháº­t sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a sáº£n pháº©m (soft delete)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra ID sáº£n pháº©m
        /// 2. GetProductById(): Láº¥y dá»¯ liá»‡u trÆ°á»›c khi xÃ³a
        /// 3. Repository.DeleteProduct(): XÃ³a má»m (soft delete)
        /// 4. LogAction(): Ghi nháº­t kÃ½ xÃ³a vá»›i dá»¯ liá»‡u cÅ©
        /// 5. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 6. Return: Tráº£ vá» káº¿t quáº£ thÃ nh cÃ´ng/tháº¥t báº¡i
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            try
            {
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                
                // Láº¥y dá»¯ liá»‡u sáº£n pháº©m trÆ°á»›c khi xÃ³a (Ä‘á»ƒ ghi nháº­t kÃ½)
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
                    
                    // XÃ³a má»m: set Visible=FALSE trong database
                    bool result = _productRepo.DeleteProduct(productId);
                    
                    if (result)
                    {
                        // Ghi nháº­t kÃ½ xÃ³a vá»›i dá»¯ liá»‡u cÅ©
                        var log = new Actions
                        {
                            ActionType = "DELETE_PRODUCT",
                            Descriptions = $"XÃ³a sáº£n pháº©m: {product.ProductName}",
                            DataBefore = JsonConvert.SerializeObject(beforeData),
                            CreatedAt = DateTime.Now
                        };
                        _logRepo.LogAction(log);
                        
                        // Mark as changed for save manager
                        ActionsService.Instance.MarkAsChanged();
                    }
                    
                    return result;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y thÃ´ng tin sáº£n pháº©m theo ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            try
            {
                if (productId <= 0)
                    throw new ArgumentException("ID sáº£n pháº©m khÃ´ng há»£p lá»‡");
                return _productRepo.GetProductById(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y thÃ´ng tin sáº£n pháº©m: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra sáº£n pháº©m cÃ³ phá»¥ thuá»™c khÃ³a ngoÃ i hay khÃ´ng
        /// </summary>
        public bool ProductHasDependencies(int productId)
        {
            try
            {
                return _productRepo.HasForeignKeyReferences(productId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i kiá»ƒm tra phá»¥ thuá»™c: " + ex.Message);
            }
        }
    }
}




