using System;
using System.Text;
using System.Security.Cryptography;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper tạo ID ngẫu nhiên và mã hóa mật khẩu
    /// </summary>
    public static class IdGenerator
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Tạo chuỗi ngẫu nhiên (alphanumeric)
        /// </summary>
        public static string GenerateRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[_random.Next(chars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Tạo mã hash SHA256 cho mật khẩu
        /// </summary>
        public static string GenerateSHA256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Kiểm tra mật khẩu có khớp với hash hay không
        /// </summary>
        public static bool VerifySHA256Hash(string input, string hash)
        {
            string hashOfInput = GenerateSHA256Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}