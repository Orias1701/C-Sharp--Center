using System;
using Newtonsoft.Json;

namespace WarehouseManagement.Helpers
{
    /// <summary>
    /// Helper chuyá»ƒn Ä‘á»•i dá»¯ liá»‡u vÃ  Ä‘á»‹nh dáº¡ng tiá»n tá»‡
    /// </summary>
    public class DataConverter
    {
        /// <summary>
        /// Chuyá»ƒn Ä‘á»•i sá»‘ thÃ nh Ä‘á»‹nh dáº¡ng tiá»n tá»‡ VNÄ
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0") + " â‚«";
        }

        /// <summary>
        /// Chuyá»ƒn Ä‘á»•i chuá»—i JSON thÃ nh object
        /// </summary>
        public static T DeserializeJson<T>(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Chuyá»ƒn Ä‘á»•i object thÃ nh JSON
        /// </summary>
        public static string SerializeJson<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Äá»‹nh dáº¡ng ngÃ y thÃ¡ng
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Äá»‹nh dáº¡ng ngÃ y thÃ¡ng ngáº¯n
        /// </summary>
        public static string FormatDateShort(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Äá»‹nh dáº¡ng sá»‘ lÆ°á»£ng (vá»›i dáº¥u phÃ¢n cÃ¡ch hÃ ng nghÃ¬n)
        /// </summary>
        public static string FormatQuantity(int quantity)
        {
            return quantity.ToString("N0");
        }

        /// <summary>
        /// Kiá»ƒm tra xem chuá»—i cÃ³ pháº£i JSON há»£p lá»‡ khÃ´ng
        /// </summary>
        public static bool IsValidJson(string input)
        {
            try
            {
                JsonConvert.DeserializeObject(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}




