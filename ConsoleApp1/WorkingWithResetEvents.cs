using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
namespace ConsoleApp1
{
    internal class WorkingWithResetEvents
    {
        static AutoResetEvent _evenEvent = new AutoResetEvent(false);
        static AutoResetEvent _oddEvent = new AutoResetEvent(false);
        static void EvenNumberGenerator()
        {
            for (int i = 0; i < 100; i += 2)
            {
                Write($"{i:000} ");
                _evenEvent.Set();
                _oddEvent.WaitOne();
            }
        }
        static void OddNumberGenerator()
        {
            for(int i=1; i<101; i += 2)
            {
                _evenEvent.WaitOne();
                Write($"{i:000} ");
                _oddEvent.Set(); 
            }
        }
        internal static void Test()
        {
            WriteLine("Press a key to start.....");
            ReadKey();

            Thread th1 = new Thread(OddNumberGenerator);
            Thread th2 = new Thread(EvenNumberGenerator);
            th1.Start(); th2.Start();

            WriteLine("Press a key to terminate.....");
            ReadKey();
            _evenEvent.Close();
            _oddEvent.Close();

        }
    }
}
