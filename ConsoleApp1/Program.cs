using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void TestMEFClient()
        {
            MEFClient client = new MEFClient();
            string result = client.DataRetriever.GetData(MEFInterfaces.OperationTypes.Account);
            Console.WriteLine("Account Result: {0}", result);
            result = client.DataRetriever.GetData(MEFInterfaces.OperationTypes.Product);
            Console.WriteLine("Product Result: {0}", result);
        }
        static void Main(string[] args)
        {
            TestMEFClient();
            /*string email = "someone@example.com"; 
            if(email.IsValidEmail("@")) //StringUtilities.IsValidEmail(email, pattern)
            {
                Console.WriteLine($"{email} is valid");
            } else
            {
                Console.WriteLine($"{email} is not valid");
            }
            Console.WriteLine($"{email} is{(email.IsValidEmail("#") ? " " : " not ")}valid"); 
            foreach(var item in Power(2,10))
            {
                Console.WriteLine($"Main item: {item}");
            }*/
            //DelegatesIntroduction.Test();
            //BuiltInDelegates.Test();
            //LINQOperators.Test();
            // ReflectionExample.TestDynamicAssembly();
            //ThreadExample1.Test();
            // ThreadSync.Test();
            //WorkingWithResetEvents.Test();
           // ParallelProgramming.Test();
            //TaskBasedProgramming.Test();
            //TaskBasedProgramming.TestTaskChains();
            //TaskBasedProgramming.TestTaskCancellations();
        }
        static IEnumerable<int> Power(int num, int multiplier)
        {
            int result = 1; 
            for(int i = 0; i<multiplier; i++)
            {
                result = result * num;
                Console.WriteLine($"Power() result: {result}");
                //return Enumerable.Range(result,10).ToList<int>();

                //if (result < 500)
                    yield return result;
                //else
                    //yield break;
            }
            yield return 100;
            yield return 200; 
            
        }
    }
    
}
