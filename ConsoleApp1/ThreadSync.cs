using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.XPath;
using static System.Console;

namespace ConsoleApp1
{

    public class ThreadSync
    {
        private int counter = 0;
        private object _syncRoot = new object();
        static List<string> thNames = new List<string> { "_First_", "_Second_", "_Third_", "_Fourth_", "_Fifth_" };
        static Mutex mutex;
        static Semaphore sem; 
        internal static void Test()
        {
            WriteLine("Press a key to start.....");
            ReadKey();

            /*mutex = new Mutex(
                initiallyOwned: false,
                name: "ProcessWide",
                createdNew: out bool createdNew);

            if (createdNew)
            {
                WriteLine($"Mutex {mutex.ToString()} is newly created.");
            }
            else
                WriteLine($"Obtained an existing system-wide Mutex"); */

            sem = new Semaphore(
                initialCount: 5,
                maximumCount: 5,
                name: "SystemWideSem",
                createdNew: out bool createdNew);
            if (createdNew)
            {
                WriteLine($"Semaphore {sem} is newly created.");
            }
            else
                WriteLine($"Obtained an existing system-wide Semaphore");

            ThreadSync ts = new ThreadSync();
            Thread[] thArray = new Thread[5];
            for (int i = 0; i < thArray.Length; i++)
            {
                //thArray[i] = new Thread(ts.Run);
                //thArray[i] = new Thread(ts.RunInterlocked);
                //thArray[i] = new Thread(ts.RunMonitor);
                //thArray[i] = new Thread(ts.RunLockStatement);
                //thArray[i] = new Thread(ts.RunMutex);
                // thArray[i] = new Thread(ts.RunSemaphore);
                thArray[i] = new Thread(ts.RunWithAttribute);
                thArray[i].Name = thNames[i];
                thArray[i].Start();
            }

            /* WriteLine("Type a char and press enter to continue, 'stop' to terminate.");
             string input = "";
             ThreadSync ts = new ThreadSync();
             while ((input = ReadLine()) != "stop")
             {
                 Thread th = new Thread(ts.RunSemaphore);
                 th.Name = $"_T_{ts.counter++}_";
                 th.Start();
                 Write("Next input: ");
             }*/


            //sem.Release(2); 
            //mutex.ReleaseMutex();
            WriteLine("Press a key to terminate...");
            ReadKey();
        }
        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.Synchronized 
        )]
        private void RunWithAttribute()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                int temp = counter;
                temp++;
                Thread.Sleep(1);
                WriteLine($"Thread {name} reports counter at {counter}");
                counter = temp;
            }
            WriteLine($"Thread {name} completes/exits the Run");
        }
        private void RunSemaphore()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");

            sem.WaitOne();
            WriteLine($"Thread {name} Begins the critical section in the current process");
            Process p = Process.GetCurrentProcess();
            WriteLine($"{name} reports Process Id:{p.Id}, Name: {p.ProcessName}, {p.PeakWorkingSet64}");
            counter = 0;
            RunMonitor();
            Thread.Sleep(millisecondsTimeout: 5000);
            WriteLine($"Thread {name} Exiting the critical section in the current process");
            sem.Release(1);

            WriteLine($"Thread {name} completes/exits the Run");
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

        private void RunLockStatement()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                lock (_syncRoot) //Monitor.Enter(...)
                {
                    int temp = counter;
                    temp++;
                    Thread.Sleep(1);
                    WriteLine($"Thread {name} reports counter at {counter}");
                    counter = temp;
                } //Monitor.Exit(...);
            }
            WriteLine($"Thread {name} completes/exits the Run");
        }
        private void RunMonitor()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                //Monitor.Enter(_syncRoot);'
                Monitor.TryEnter(_syncRoot, millisecondsTimeout: 500); //minimize deadlock scenarios
                //Monitor.Enter(this); //not recommended approach to use the current data object 
                //preferred practice is to use dummy objects for locking 
                //Every object in .NET begins with the SyncLock Bit
                //Monitor sets the SyncLock Bit, when it enters 
                int temp = counter;
                temp++;
                Thread.Sleep(1);
                WriteLine($"Thread {name} reports counter at {counter}");
                counter = temp;
                Monitor.Exit(_syncRoot);// SyncLock bit is released/reset 
            }
            WriteLine($"Thread {name} completes/exits the Run");
        }
        private void RunInterlocked()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                Interlocked.Increment(ref counter);

                //Thread.Sleep(1);
                WriteLine($"Thread {name} reports counter at {counter}");
            }
            WriteLine($"Thread {name} completes/exits the Run");
        }
        private void Run()
        {
            var name = Thread.CurrentThread.Name;
            WriteLine($"Thread {name} begins execution");
            while (counter < 100)
            {
                int temp = counter;
                temp++;
                Thread.Sleep(1);
                WriteLine($"Thread {name} reports counter at {counter}");
                counter = temp;
            }
            WriteLine($"Thread {name} completes/exits the Run");
        }
    }
}
