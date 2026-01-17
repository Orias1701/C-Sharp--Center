using System;
using WarehouseManagement.Helpers;

namespace WarehouseManagement.Models
{
    /// <summary>
    /// Lá»›p thá»±c thá»ƒ NgÆ°á»i dÃ¹ng
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  // LÆ°u Ã½: NÃªn lÆ°u dáº¡ng hash SHA256 trong cÆ¡ sá»Ÿ dá»¯ liá»‡u
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Kiá»ƒm tra xem user cÃ³ pháº£i Admin hay khÃ´ng
        /// </summary>
        public bool IsAdmin => Role == "Admin";

        /// <summary>
        /// Hash máº­t kháº©u báº±ng SHA256 trÆ°á»›c khi lÆ°u vÃ o database
        /// </summary>
        public string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentNullException(nameof(plainPassword));
            
            return IdGenerator.GenerateSHA256Hash(plainPassword);
        }

        /// <summary>
        /// XÃ¡c minh máº­t kháº©u ngÆ°á»i dÃ¹ng nháº­p vá»›i hash Ä‘Ã£ lÆ°u
        /// </summary>
        public bool VerifyPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(this.Password))
                return false;

            return IdGenerator.VerifySHA256Hash(plainPassword, this.Password);
        }

        /// <summary>
        /// Thiáº¿t láº­p máº­t kháº©u má»›i (tá»± Ä‘á»™ng hash)
        /// </summary>
        public void SetPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentNullException(nameof(plainPassword));
            this.Password = HashPassword(plainPassword);
        }
    }
}





