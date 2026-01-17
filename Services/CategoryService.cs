using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;
using Newtonsoft.Json;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic danh má»¥c sáº£n pháº©m
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ danh má»¥c (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - TÃ¬m kiáº¿m danh má»¥c: Theo tÃªn
    /// - Kiá»ƒm tra phá»¥ thuá»™c: Kiá»ƒm tra danh má»¥c cÃ³ sáº£n pháº©m hay khÃ´ng
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o (ID, tÃªn, v.v...)
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Logging: Ghi nháº­t kÃ½ Actions
    /// 4. Change tracking: Gá»i ActionsService.MarkAsChanged()
    /// 5. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly ActionsRepository _logRepo;

        public CategoryService()
        {
            _categoryRepo = new CategoryRepository();
            _logRepo = new ActionsRepository();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ danh má»¥c
        /// </summary>
        public List<Category> GetAllCategories()
        {
            try
            {
                return _categoryRepo.GetAllCategories();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh má»¥c theo ID
        /// </summary>
        public Category GetCategoryById(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh má»¥c khÃ´ng há»£p lá»‡");
                return _categoryRepo.GetCategoryById(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// TÃ¬m kiáº¿m danh má»¥c theo tÃªn (khÃ´ng phÃ¢n biá»‡t hoa thÆ°á»ng)
        /// </summary>
        public List<Category> SearchCategoryByName(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetAllCategories();

                var categories = GetAllCategories();
                return categories.Where(c => c.CategoryName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi tÃ¬m kiáº¿m danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// ThÃªm danh má»¥c má»›i
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra tÃªn danh má»¥c khÃ´ng trá»‘ng, <= 100 kÃ½ tá»±
        /// 2. Repository.AddCategory(): ThÃªm vÃ o database
        /// 3. LogAction(): Ghi nháº­t kÃ½ (DataBefore trá»‘ng)
        /// 4. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 5. Return: Tráº£ vá» ID danh má»¥c vá»«a thÃªm
        /// </summary>
        public int AddCategory(string categoryName)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("TÃªn danh má»¥c khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (categoryName.Length > 100)
                    throw new ArgumentException("TÃªn danh má»¥c khÃ´ng Ä‘Æ°á»£c vÆ°á»£t 100 kÃ½ tá»±");

                var category = new Category
                {
                    CategoryName = categoryName.Trim()
                };

                // ThÃªm danh má»¥c vÃ o database
                int categoryId = _categoryRepo.AddCategory(category);

                // Ghi nháº­t kÃ½
                var log = new Actions
                {
                    ActionType = "ADD_CATEGORY",
                    Descriptions = $"ThÃªm danh má»¥c: {categoryName}",
                    DataBefore = "",
                    CreatedAt = DateTime.Now
                };
                _logRepo.LogAction(log);

                // ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i chÆ°a lÆ°u
                ActionsService.Instance.MarkAsChanged();
                return categoryId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi thÃªm danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t danh má»¥c
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra ID vÃ  tÃªn danh má»¥c
        /// 2. GetCategoryById(): Láº¥y dá»¯ liá»‡u cÅ© trÆ°á»›c khi cáº­p nháº­t
        /// 3. Repository.UpdateCategory(): Cáº­p nháº­t vÃ o database
        /// 4. LogAction(): Ghi nháº­t kÃ½ vá»›i dá»¯ liá»‡u cÅ©
        /// 5. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 6. Return: Tráº£ vá» káº¿t quáº£ thÃ nh cÃ´ng/tháº¥t báº¡i
        /// </summary>
        public bool UpdateCategory(int categoryId, string categoryName)
        {
            try
            {
                // Validation
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh má»¥c khÃ´ng há»£p lá»‡");
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("TÃªn danh má»¥c khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (categoryName.Length > 100)
                    throw new ArgumentException("TÃªn danh má»¥c khÃ´ng Ä‘Æ°á»£c vÆ°á»£t 100 kÃ½ tá»±");

                // Láº¥y dá»¯ liá»‡u cÅ© trÆ°á»›c khi cáº­p nháº­t
                var oldCategory = _categoryRepo.GetCategoryById(categoryId);
                if (oldCategory == null)
                    throw new ArgumentException("Danh má»¥c khÃ´ng tá»“n táº¡i");

                var beforeData = new
                {
                    CategoryID = oldCategory.CategoryID,
                    CategoryName = oldCategory.CategoryName
                };

                // Cáº­p nháº­t danh má»¥c vÃ o database
                var category = new Category
                {
                    CategoryID = categoryId,
                    CategoryName = categoryName.Trim()
                };

                bool result = _categoryRepo.UpdateCategory(category);

                if (result)
                {
                    // Ghi nháº­t kÃ½ vá»›i dá»¯ liá»‡u cÅ©
                    var log = new Actions
                    {
                        ActionType = "UPDATE_CATEGORY",
                        Descriptions = $"Cáº­p nháº­t danh má»¥c: {categoryName}",
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
                throw new Exception("Lá»—i khi cáº­p nháº­t danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// XÃ³a danh má»¥c (soft delete)
        /// 
        /// LUá»’NG:
        /// 1. Validation: Kiá»ƒm tra ID danh má»¥c
        /// 2. GetCategoryById(): Láº¥y dá»¯ liá»‡u cÅ© trÆ°á»›c khi xÃ³a
        /// 3. Repository.DeleteCategory(): XÃ³a má»m (soft delete)
        /// 4. LogAction(): Ghi nháº­t kÃ½ xÃ³a vá»›i dá»¯ liá»‡u cÅ©
        /// 5. MarkAsChanged(): ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i
        /// 6. Return: Tráº£ vá» káº¿t quáº£ thÃ nh cÃ´ng/tháº¥t báº¡i
        /// </summary>
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh má»¥c khÃ´ng há»£p lá»‡");

                // Kiá»ƒm tra danh má»¥c cÃ³ sáº£n pháº©m hay khÃ´ng
                if (_categoryRepo.CategoryHasProducts(categoryId))
                    throw new ArgumentException("KhÃ´ng thá»ƒ xÃ³a danh má»¥c vÃ¬ cÃ²n sáº£n pháº©m");

                // Láº¥y dá»¯ liá»‡u danh má»¥c trÆ°á»›c khi xÃ³a
                var category = _categoryRepo.GetCategoryById(categoryId);

                if (category != null)
                {
                    var beforeData = new
                    {
                        CategoryID = category.CategoryID,
                        CategoryName = category.CategoryName
                    };

                    // XÃ³a má»m: set Visible=FALSE trong database
                    bool result = _categoryRepo.DeleteCategory(categoryId);

                    if (result)
                    {
                        // Ghi nháº­t kÃ½ xÃ³a vá»›i dá»¯ liá»‡u cÅ©
                        var log = new Actions
                        {
                            ActionType = "DELETE_CATEGORY",
                            Descriptions = $"XÃ³a danh má»¥c: {category.CategoryName}",
                            DataBefore = JsonConvert.SerializeObject(beforeData),
                            CreatedAt = DateTime.Now
                        };
                        _logRepo.LogAction(log);

                        // ÄÃ¡nh dáº¥u cÃ³ thay Ä‘á»•i chÆ°a lÆ°u
                        ActionsService.Instance.MarkAsChanged();
                    }

                    return result;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi xÃ³a danh má»¥c: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra danh má»¥c cÃ³ sáº£n pháº©m hay khÃ´ng
        /// </summary>
        public bool CategoryHasProducts(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("ID danh má»¥c khÃ´ng há»£p lá»‡");
                return _categoryRepo.CategoryHasProducts(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i kiá»ƒm tra danh má»¥c: " + ex.Message);
            }
        }
    }
}




