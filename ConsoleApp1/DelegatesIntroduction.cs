using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    //Step 1: Declaration 
    public delegate int ArithmeticDelegate(int a, int b);   


    public class DelegatesIntroduction
    {
        //Functions which match the delegate signature 
        public int Add(int a, int b) {  return a + b; } 
        static int Minus(int a, int b) { return a - b; }


        internal static void Test()
        {
            DelegatesIntroduction di = new DelegatesIntroduction();

            //Step 2: Instantiation of Delegates 
            ArithmeticDelegate ad = new ArithmeticDelegate(di.Add);
            //Step 3: Invocation Style 1 
            int x = 10, y = 20;
            var result = ad(x, y); 
            Console.WriteLine($"Invocation md({x},{y}) yields {result}");
            x += result;
            y += result; 
            result = ad.Invoke(x, y);
            Console.WriteLine($"INvocation of md.Invoke({x},{y}) yields {result}");

            //Multi-cast - add more functions to the list 
            ad += new ArithmeticDelegate(Minus);
            x += result;
            y += result;
            result = ad.Invoke(x, y);
            Console.WriteLine($"INvocation of md.Invoke({x},{y}) yields {result}");

            //To remove functions from the list 
            //ad-=new ArithmeticDelegate(Minus);

            //Anonymous Method - unnamed method at the class level
            //- kind of local function
            //- function within a function 
            //-- the inner function can access variables from the outer function 
            ad += delegate (int a, int b)
            {
                return a * b;
            };
            x += result;
            y += result;
            result = ad.Invoke(x, y);
            Console.WriteLine($"INvocation of md.Invoke({x},{y}) yields {result}");

            //Lambda Expressions 
            ad += (a, b) => (b > 0) ? a / b : 0;
            x = 500;
            y = 5;
            result = ad.Invoke(x, y);
            Console.WriteLine($"INvocation of md.Invoke({x},{y}) yields {result}");
            /*Lambdas
             * -> Expression Lambdas = single line of statement - usually the return type
             * -> Statement Lambdas = multiple lines of code enclosed in { ... } 
             * -> argument passing 
             * --> Zero argument         => () 
             * --> One Argument          => (a), a 
             * --> two or more arguments => (a,b,c) 
             */
            ManualInvocationOfDelegate(ad);

        }
        static void ManualInvocationOfDelegate(ArithmeticDelegate ad)
        {
            Console.WriteLine($"\n{nameof(ManualInvocationOfDelegate)} started....");
            int x = 10, y = 20;
            foreach(Delegate item in ad.GetInvocationList())
            {
                object objResult = item.DynamicInvoke(x, y); 
                int intResult = Convert.ToInt32(objResult);
               
                Console.WriteLine($"{item.Method.Name}({x}, {y}) yields {intResult}");
                x += intResult; y += intResult;
            }
            Console.WriteLine($"========= END ==============");
        }
    }
}
