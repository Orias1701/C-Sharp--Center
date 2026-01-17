using System;
using System.Collections.Generic;
using WarehouseManagement.Services;
using WarehouseManagement.Models;

namespace WarehouseManagement.Controllers
{
    /// <summary>
    /// Controller Ä‘iá»u hÆ°á»›ng cÃ¡c thao tÃ¡c liÃªn quan Ä‘áº¿n ngÆ°á»i dÃ¹ng
    /// </summary>
    public class UserController
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }

        /// <summary>
        /// ÄÄƒng nháº­p ngÆ°á»i dÃ¹ng
        /// </summary>
        public User Login(string username, string password)
        {
            return _userService.Login(username, password);
        }

        /// <summary>
        /// Láº¥y ngÆ°á»i dÃ¹ng theo ID
        /// </summary>
        public User GetUserById(int userId)
        {
            return _userService.GetUserById(userId);
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ ngÆ°á»i dÃ¹ng
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        /// <summary>
        /// Kiá»ƒm tra tÃªn Ä‘Äƒng nháº­p cÃ³ tá»“n táº¡i hay khÃ´ng
        /// </summary>
        public bool UsernameExists(string username)
        {
            return _userService.UsernameExists(username);
        }

        /// <summary>
        /// Kiá»ƒm tra ngÆ°á»i dÃ¹ng cÃ³ hoáº¡t Ä‘á»™ng hay khÃ´ng
        /// </summary>
        public bool IsUserActive(int userId)
        {
            return _userService.IsUserActive(userId);
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ ngÆ°á»i dÃ¹ng
        /// </summary>
        public int CountUsers()
        {
            return _userService.CountUsers();
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch ngÆ°á»i dÃ¹ng hoáº¡t Ä‘á»™ng
        /// </summary>
        public List<User> GetActiveUsers()
        {
            return _userService.GetActiveUsers();
        }
    }
}




