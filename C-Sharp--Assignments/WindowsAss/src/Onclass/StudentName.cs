using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    internal class StudentName
    {
        static string GetName(string fullName)
        {
            var parts = fullName.Split(' ');
            return parts.Last();
        }

        static string GetRest(string fullName)
        {
            var parts = fullName.Split(' ');
            if (parts.Length <= 1) return "";
            return string.Join(" ", parts.Take(parts.Length - 1));
        }

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.Write("Nhập số sinh viên: ");
            if (!int.TryParse(Console.ReadLine(), out int n)) return;

            List<string> danhSach = new List<string>();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Sinh viên {i + 1}: ");
                danhSach.Add(Console.ReadLine().Trim());
            }

            var danhSachSapXep = danhSach.OrderBy(fullName => GetName(fullName))
                                         .ThenBy(fullName => GetRest(fullName))
                                         .ToList();

            Console.WriteLine("\nSắp xếp: ");
            for (int i = 0; i < danhSachSapXep.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {danhSachSapXep[i]}");
            }

            while (true)
            {
                Console.Write("\nTìm kiếm (Nhập \"Exit\" để thoát): ");
                string target = Console.ReadLine().Trim();
                if (target == "Exit")
                {
                    Console.WriteLine("Đã thoát");
                    break;
                }

                int index = danhSachSapXep.IndexOf(target);

                if (index != -1)
                    Console.WriteLine($"Vị trí: {index + 1}");
                else
                    Console.WriteLine("Not Found");
            }

        }
    }
}
