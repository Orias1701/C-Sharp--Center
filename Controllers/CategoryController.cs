using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n danh má»¥c sáº£n pháº©m
    /// </summary>
    public class CategoryController
    {
        private readonly CategoryService _categoryService;

        public CategoryController()
        {
            _categoryService = new CategoryService();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ danh má»¥c
        /// </summary>
        public List<Category> GetAllCategories()
        {
            return _categoryService.GetAllCategories();
        }

        /// <summary>
        /// Láº¥y danh má»¥c theo ID
        /// </summary>
        public Category GetCategoryById(int categoryId)
        {
            return _categoryService.GetCategoryById(categoryId);
        }

        /// <summary>
        /// TÃ¬m kiáº¿m danh má»¥c theo tÃªn
        /// </summary>
        public List<Category> SearchCategory(string keyword)
        {
            return _categoryService.SearchCategoryByName(keyword);
        }

        /// <summary>
        /// ThÃªm danh má»¥c má»›i
        /// </summary>
        public int CreateCategory(string categoryName)
        {
            return _categoryService.AddCategory(categoryName);
        }

        /// <summary>
        /// Cáº­p nháº­t danh má»¥c
        /// </summary>
        public bool UpdateCategory(int categoryId, string categoryName)
        {
            return _categoryService.UpdateCategory(categoryId, categoryName);
        }

        /// <summary>
        /// XÃ³a danh má»¥c
        /// </summary>
        public bool DeleteCategory(int categoryId)
        {
            return _categoryService.DeleteCategory(categoryId);
        }

        /// <summary>
        /// Kiá»ƒm tra danh má»¥c cÃ³ sáº£n pháº©m hay khÃ´ng
        /// </summary>
        public bool CategoryHasProducts(int categoryId)
        {
            return _categoryService.CategoryHasProducts(categoryId);
        }
    }
}




