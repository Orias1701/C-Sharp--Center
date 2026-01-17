using System;
using System.Windows.Forms;
using WarehouseManagement.Helpers;
using WarehouseManagement.Views;

namespace WarehouseManagement
{
    static class Program
    {
        /// <summary>
        /// Äiá»ƒm vÃ o chÃ­nh cá»§a á»©ng dá»¥ng
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Khá»Ÿi táº¡o visual styles cho á»©ng dá»¥ng Windows Forms
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Tá»± Ä‘á»™ng cháº¡y schema.sql Ä‘á»ƒ táº¡o database vÃ  báº£ng (náº¿u chÆ°a tá»“n táº¡i)
                try
                {
                    DatabaseHelper.ExecuteSchema();
                }
                catch
                {
                    // Schema cÃ³ thá»ƒ Ä‘Ã£ tá»“n táº¡i, khÃ´ng cáº§n bÃ¡o lá»—i
                }

                // Kiá»ƒm tra káº¿t ná»‘i database trÆ°á»›c khi cháº¡y á»©ng dá»¥ng
                if (!DatabaseHelper.TestDatabaseConnection())
                {
                    MessageBox.Show(
                        "KhÃ´ng thá»ƒ káº¿t ná»‘i tá»›i database!\n\nVui lÃ²ng kiá»ƒm tra:\n" +
                        "1. MySQL Server Ä‘ang cháº¡y\n" +
                        "2. Connection String trong App.config Ä‘Ãºng\n\n" +
                        DatabaseHelper.GetConnectionError(),
                        "Lá»—i Káº¿t Ná»‘i Database",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Hiá»ƒn thá»‹ form Ä‘Äƒng nháº­p
                Login loginForm = new Login();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Náº¿u Ä‘Äƒng nháº­p thÃ nh cÃ´ng, cháº¡y form chÃ­nh
                    Application.Run(new Main());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lá»—i khá»Ÿi Ä‘á»™ng á»©ng dá»¥ng: " + ex.Message + "\n\n" + ex.StackTrace, 
                    "Lá»—i", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}




