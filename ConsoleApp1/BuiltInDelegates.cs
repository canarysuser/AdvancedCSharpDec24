using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class BuiltInDelegates
    {
        internal static void Test()
        {
            Action a1 = () => Console.WriteLine("Action a1 called");
            a1();
            Action<int, int> a2 = delegate (int x, int y)
            {
                var temp = x + y;
                Console.WriteLine("Action a2 with input args called: " + temp);
            };
            a2(100, 200);

            Func<string> f1 = () => "Hello World";
            Console.WriteLine(f1());
            Func<int, int, string> f2 = (a, b) => (a + b).ToString();
            Console.WriteLine(f2(999, 999));

            Predicate<int> p1 = a => a > 10;
            var result = p1(5);
            Console.WriteLine($"p1(5) yields {result}");

            var numbers = new List<int> { 1, 4, 5, 2, 67, 27, 4, 7, 9 };
            List1(numbers, p1);
            List1(numbers, a => a > 10 && a < 50);

        }
        static void List1(List<int> arr, Predicate<int> criteria)
        {
            foreach (int i in arr)
            {
                Console.WriteLine($"{i} > 10 = {criteria(i)}");

            }
        }
    }
}
