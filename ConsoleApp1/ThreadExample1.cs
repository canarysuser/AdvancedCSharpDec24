using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Console;

namespace ConsoleApp1
{
    public class ThreadExample1
    {
        private int counter = 1; 
        internal static void Test()
        {
            ThreadStart ts1 = new ThreadStart(Execute); 
            Thread th1 = new Thread(ts1);
            th1.Name = "First"; 
            th1.Start();
            ThreadStart ts2 = Execute;
            Thread th2 = new Thread(ts2);
            th2.Name = "Second"; 
            th2.Start();
            Thread th3 = new Thread(() =>
            {
                var name = Thread.CurrentThread.Name;
                WriteLine($"Thread {name} begins execution");
                Thread.Sleep(millisecondsTimeout: 5000);
                WriteLine($"Thread {name} exiting...");
            });
            th3.Name = "Action Delegate Third";
            th3.Start();
            ParameterizedThreadStart ps1 = new ParameterizedThreadStart(DoWork); 
            Thread th4 = new Thread(ps1);
            th4.Name = "Parameterized"; 
            th4.Start(100);
            WriteLine("All threads started. Press a key to terminate.");
            ReadKey();
        }
        static void DoWork(object state)
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            if (int.TryParse(state.ToString(), out int number))
            {
                WriteLine($"Received number {number} as input.");
            }
            else
                WriteLine($"Received unknown type \"{state}\" as input");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} exiting...");
        }
        static void Execute()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} exiting...");
        }


        private void Run() //Method name can be anything, the signature should be void target(void)
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                int temp = counter;
            }
        }
    }
}
