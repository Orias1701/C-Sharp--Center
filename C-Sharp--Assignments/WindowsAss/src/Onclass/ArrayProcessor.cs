using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    public class ArrayProcessor
    {
        public static void Run()
        {
            Console.Write("\nNhap so luong phan tu cua mang: ");
            int n = int.Parse(Console.ReadLine());
            int[] arr = new int[n];

            for (int i = 0; i < n; i++)
            {
                Console.Write($"arr[{i}] = ");
                arr[i] = int.Parse(Console.ReadLine());
            }

            List<int> evens = arr.Where(x => x % 2 == 0)
                                 .OrderByDescending(x => x)
                                 .ToList();

            List<int> odds = arr.Where(x => x % 2 != 0)
                                .OrderBy(x => x)
                                .ToList();

            foreach (var item in evens)
            {
                Console.Write(item + " ");
            }

            foreach (var item in odds)
            {
                Console.Write(item + " ");
            }
        }
    }
}