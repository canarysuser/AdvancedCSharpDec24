using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class TaskBasedProgramming
    {
        static void ShowThreadId(string name)
        {
            Console.WriteLine(
                "Current {0} Thread Id: {1}, Is background:{2}", 
                    name,
                    Thread.CurrentThread.ManagedThreadId, 
                    Thread.CurrentThread.IsBackground);
        }
        internal static void TestTaskCancellations()
        {
            Console.WriteLine("Press a key to start.");
            Console.ReadKey();
            var tokenSource = new CancellationTokenSource(); 
            var ct = tokenSource.Token;
            Task t1 = Task.Factory.StartNew(() =>
            {
                ct.ThrowIfCancellationRequested();
                ShowThreadId("TaskFactory 1");
                Task.Delay(10);
                if (ct.IsCancellationRequested) {
                    Console.WriteLine("Task 1 is being canceled");
                }
                ct.ThrowIfCancellationRequested();
                Console.WriteLine("First Task using Task.Factory");
            }, ct);
            Task t2 = Task.Factory.StartNew(() =>
            {
                ct.ThrowIfCancellationRequested();
                ShowThreadId("TaskFactory 2");
                Task.Delay(10);
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Task 2 is being canceled");
                }
                ct.ThrowIfCancellationRequested();
                Console.WriteLine("Second Task using Task.Factory");
            }, ct);

            Task.Delay(50);
            tokenSource.Cancel();
            try
            {
                Task.WaitAll(t1, t2);
                Console.WriteLine("Task 1 Status: " + t1.Status.ToString());
                Console.WriteLine("Task 2 Status: " + t2.Status.ToString());
            }
            catch (OperationCanceledException oce) { Console.WriteLine("Canceled: {0}", oce.Message); }
            catch (AggregateException ae)
            {
                Console.WriteLine("Aggregate: {0}", ae.Message);
                ae.InnerExceptions.ToList().ForEach(ex => Console.WriteLine(ex.ToString()));
            }
            if(t1.IsCanceled)
                Console.WriteLine("Task t1 is cancelled.");
            if (t2.IsCanceled)
                Console.WriteLine("Task t2 is cancelled.");



            Console.WriteLine("Press a key to terminate....");
            Console.ReadKey();
        }
        internal static void TestTaskChains()
        {
            Console.WriteLine("Press a key to start.");
            Console.ReadKey();

            var chainedTask = Task.Factory.StartNew(() => 10)
                .ContinueWith((res) => res.Result * 100)
                .ContinueWith((res2) => res2.Result * 500)
                .ContinueWith((res3) => Console.WriteLine("Final result: {0}", res3.Result))
                .ContinueWith((err) => Console.WriteLine("Error: {0}", err.Exception?.Message));

            Task.WaitAll(chainedTask);
            Console.WriteLine("Press a key to terminate....");
            Console.ReadKey();
        }
        internal static void Test()
        {
            Console.WriteLine("Press a key to start.");
            Console.ReadKey();

            ShowThreadId("Main");
            
            Task t1 = new Task(() => { ShowThreadId("Task 1"); Console.WriteLine("First Task"); }); 
            t1.Start();
            Task t2 = Task.Factory.StartNew(() =>
            {
                ShowThreadId("TaskFactory 1");
                Console.WriteLine("Second Task using Task.Factory");
            });
            Task t3 = Task.Run(() =>
            {
                ShowThreadId("TaskRun 1");
                Console.WriteLine("Third Task with Task.Run");
            });
            Task<int> t4 = Task.Factory.StartNew<int>( () =>
            {
                ShowThreadId("TaskWithReturn 1");
                Task.Delay(1000).GetAwaiter().GetResult();
                ShowThreadId("TaskWithReturn 2");
                Console.WriteLine($"Fourth Task..");
               //.GetAwaiter().GetResult();
                return DateTime.Now.Millisecond;
            });
            
            Task.WaitAll(t1,t2,t3,t4);
            Console.WriteLine($"Task 4 returned: {t4.Result}");

            Console.WriteLine("Press a key to terminate....");
            Console.ReadKey();
        }
    }
}
