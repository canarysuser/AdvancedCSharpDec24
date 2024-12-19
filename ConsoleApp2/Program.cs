using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Console;
namespace ConsoleApp2
{
    internal class Program
    {
        static Mutex mutex;
        static List<string> thNames = new List<string> { "_First_", "_Second_", "_Third_", "_Fourth_", "_Fifth_" };
        static void Main(string[] args)
        {
            WriteLine("ConsoleApp2.... Press a key to start.....");
            ReadKey();

            mutex = new Mutex(
                initiallyOwned: false,
                name: "ProcessWide",
                createdNew: out bool createdNew);

            if (createdNew)
            {
                WriteLine($"ConsoleApp2.... Mutex {mutex.ToString()} is newly created.");
            }
            else
                WriteLine($"ConsoleApp2.... Obtained an existing system-wide Mutex");

            Program ts = new Program();
            Thread[] thArray = new Thread[5];
            for (int i = 0; i < thArray.Length; i++)
            {
                //thArray[i] = new Thread(ts.Run);
                //thArray[i] = new Thread(ts.RunInterlocked);
                //thArray[i] = new Thread(ts.RunMonitor);
                //thArray[i] = new Thread(ts.RunLockStatement);
                thArray[i] = new Thread(ts.RunMutex);
                thArray[i].Name = thNames[i];
                thArray[i].Start();
            }
            WriteLine("ConsoleApp2.... Press a key to terminate...");
            ReadKey();

        }
        private void RunMutex()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");

            mutex.WaitOne();
            WriteLine($"Thread {name} Begins the critical section in the current process");
            Process p = Process.GetCurrentProcess();
            WriteLine($"{name} reports Process Id:{p.Id}, Name: {p.ProcessName}, {p.PeakWorkingSet64}");
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} Exiting the critical section in the current process");
            mutex.ReleaseMutex();

            WriteLine($"Thread {name} completes/exits the Run");
        }
    }
}
