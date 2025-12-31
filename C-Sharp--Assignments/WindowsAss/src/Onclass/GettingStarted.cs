using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAss.src.Onclass
{
    public class GettingStarted
    {
        public int GCD(int a, int b)
        {
            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }
        public void PrintPS(int a, int b, int c, int d)
        {
            int tu = a * d + c * b;
            int mau = b * d;

            int ucln = GCD(tu, mau);

            tu /= ucln;
            mau /= ucln;

            Console.WriteLine("result = {0}/{1}\n", tu, mau);
        }
        public static void Run()
        {
            int a, b, c, d;
            Console.Write("\na = ");
            a = int.Parse(Console.ReadLine());
            Console.Write("\nb = ");
            b = int.Parse(Console.ReadLine());
            Console.Write("\nc = ");
            c = int.Parse(Console.ReadLine());
            Console.Write("\nd = ");
            d = int.Parse(Console.ReadLine());
            if (b == 0 || d == 0)
            {
                Console.WriteLine("b & d != 0!");
                return;
            }
            GettingStarted pr = new GettingStarted();
            pr.PrintPS(a, b, c, d);
        }
    }
}
