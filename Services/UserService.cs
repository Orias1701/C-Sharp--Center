using System;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement.Services
{
    /// <summary>
    /// Service xá»­ lÃ½ logic ngÆ°á»i dÃ¹ng
    /// 
    /// CHá»¨C NÄ‚NG:
    /// - Quáº£n lÃ½ ngÆ°á»i dÃ¹ng (CRUD): ThÃªm, sá»­a, xÃ³a
    /// - XÃ¡c thá»±c: ÄÄƒng nháº­p, kiá»ƒm tra máº­t kháº©u
    /// - TÃ¬m kiáº¿m ngÆ°á»i dÃ¹ng: Theo tÃªn, ID
    /// 
    /// LUá»’NG:
    /// 1. Validation: Kiá»ƒm tra Ä‘áº§u vÃ o (ID, tÃªn, máº­t kháº©u, v.v...)
    /// 2. Repository call: Gá»i DB Ä‘á»ƒ thá»±c hiá»‡n thao tÃ¡c
    /// 3. Return: Tráº£ vá» káº¿t quáº£
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService()
        {
            _userRepo = new UserRepository();
        }

        /// <summary>
        /// ÄÄƒng nháº­p ngÆ°á»i dÃ¹ng
        /// </summary>
        public User Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("TÃªn Ä‘Äƒng nháº­p khÃ´ng Ä‘Æ°á»£c trá»‘ng");
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Máº­t kháº©u khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                var user = _userRepo.Login(username, password);
                if (user == null)
                    throw new Exception("TÃªn Ä‘Äƒng nháº­p hoáº·c máº­t kháº©u khÃ´ng Ä‘Ãºng");

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi Ä‘Äƒng nháº­p: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y ngÆ°á»i dÃ¹ng theo ID
        /// </summary>
        public User GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("ID ngÆ°á»i dÃ¹ng khÃ´ng há»£p lá»‡");
                return _userRepo.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y ngÆ°á»i dÃ¹ng: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch táº¥t cáº£ ngÆ°á»i dÃ¹ng
        /// </summary>
        public List<User> GetAllUsers()
        {
            try
            {
                return _userRepo.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y danh sÃ¡ch ngÆ°á»i dÃ¹ng: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra tÃªn Ä‘Äƒng nháº­p cÃ³ tá»“n táº¡i hay khÃ´ng
        /// </summary>
        public bool UsernameExists(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("TÃªn Ä‘Äƒng nháº­p khÃ´ng Ä‘Æ°á»£c trá»‘ng");

                var users = GetAllUsers();
                foreach (var user in users)
                {
                    if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi kiá»ƒm tra tÃªn Ä‘Äƒng nháº­p: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiá»ƒm tra ngÆ°á»i dÃ¹ng cÃ³ hoáº¡t Ä‘á»™ng hay khÃ´ng
        /// </summary>
        public bool IsUserActive(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("ID ngÆ°á»i dÃ¹ng khÃ´ng há»£p lá»‡");

                var user = _userRepo.GetUserById(userId);
                return user != null && user.IsActive;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi kiá»ƒm tra ngÆ°á»i dÃ¹ng: " + ex.Message);
            }
        }

        /// <summary>
        /// Äáº¿m tá»•ng sá»‘ ngÆ°á»i dÃ¹ng
        /// </summary>
        public int CountUsers()
        {
            try
            {
                return _userRepo.GetAllUsers().Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi Ä‘áº¿m ngÆ°á»i dÃ¹ng: " + ex.Message);
            }
        }

        /// <summary>
        /// Láº¥y danh sÃ¡ch ngÆ°á»i dÃ¹ng hoáº¡t Ä‘á»™ng
        /// </summary>
        public List<User> GetActiveUsers()
        {
            try
            {
                var users = _userRepo.GetAllUsers();
                var activeUsers = new List<User>();
                foreach (var user in users)
                {
                    if (user.IsActive)
                        activeUsers.Add(user);
                }
                return activeUsers;
            }
            catch (Exception ex)
            {
                throw new Exception("Lá»—i khi láº¥y ngÆ°á»i dÃ¹ng hoáº¡t Ä‘á»™ng: " + ex.Message);
            }
        }
    }
}




