using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStuff {
    class Program {
        static int abs(int a) => (a < 0) ? -a : a;

        static int GCD(int a, int b) {
            a = abs(a);
            b = abs(b);

            // a is bigger of 2 inputs
            if (a < b) (a, b) = (b, a);

            if (b == 0) return a;

            int gcd = 1;
            for (int i = 1; i < b; i++) {
                if (a % i == 0 && b % i == 0) {
                    gcd *= i;
                    a /= gcd;
                }
            }

            return gcd;
        }

        static void Main(string[] args) {
            Console.WriteLine(GCD(1002, 3));

            Console.ReadKey(true);
        }
    }
}
