using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ParallelProgramming
    {
        const int MaxSize = 1_000_000; 
        internal static void Test()
        {
            Console.WriteLine("Press a key to start.");
            Console.ReadKey();
            Console.WriteLine("Normal Sequential Execution....");
            /*Stopwatch watch = Stopwatch.StartNew();
            DrawCircle();
            DrawEllipse();
            DrawRectangle();
            DrawSquare();
            watch.Stop();
            Console.WriteLine($"Normal Execution duration: {watch.ElapsedMilliseconds}");
            
            Console.WriteLine("\nParallel Execution....");
            watch = Stopwatch.StartNew();
            Parallel.Invoke(
                () => DrawCircle(),
                () => DrawEllipse(),
                () => DrawRectangle(),
                () => DrawSquare()
                );
            watch.Stop();
            Console.WriteLine($"Parallel Execution duration: {watch.ElapsedMilliseconds}");*/

          /*  Console.WriteLine("Iterations ");
            //SequentialKeysGeneration();
            ParallelKeysGeneration();*/
          var cts = new CancellationTokenSource();
            var ct = cts.Token;
            var t1 = Task.Factory.StartNew(() => SequentialKeysGeneration(ct, 1));
            var t2 = Task.Factory.StartNew(() => SequentialKeysGeneration(ct, 2));
            Thread.Sleep(2000);
            cts.Cancel();
            try
            {
                if (Task.WaitAny(new Task[] { t1, t2 }, 3000, ct)==-1)
                {
                    cts.Cancel();
                    Console.WriteLine("Sequential generation terminated;");
                    Console.WriteLine(t1.Status);
                    Console.WriteLine(t2.Status);
                   
                }
                Console.WriteLine("Tasks started..");
                //Task.WaitAll(t1, t2, );
              
            }
            catch (OperationCanceledException oce) { Console.WriteLine("Canceled: {0}", oce.Message); }
            catch (AggregateException ex) { ex.InnerExceptions.ToList().ForEach(e => Console.WriteLine(e.Message)); }
            Console.WriteLine(t1.Status);
            Console.WriteLine(t2.Status);
            if (t1.IsCanceled || t1.IsFaulted)
                Console.WriteLine("Task t1 is cancelled.");
            if (t2.IsCanceled)
                Console.WriteLine("Task t2 is cancelled.");

            Console.WriteLine("Press a key to terminate....");
            Console.ReadKey();
        }
        static void SequentialKeysGeneration(CancellationToken ct, int timeout)
        {
            ct.ThrowIfCancellationRequested(); 
            Console.WriteLine("Sequential Key generation started....");
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < MaxSize; i++)
            {
                var aes = AesManaged.Create();
                aes.GenerateIV();
                var key = aes.Key;
                string s = ConvertToHexString(key);
                if(sw.Elapsed.Seconds
                    > timeout)
                {
                    throw new TimeoutException($"SequentialKeysGeneration timed out at {timeout} seconds ");
                }
                ct.ThrowIfCancellationRequested();
            }
            sw.Stop();
            Console.WriteLine($"{nameof(SequentialKeysGeneration)}() completed in {sw.ElapsedMilliseconds} ms");
        }
        static string ConvertToHexString(byte[] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < arr.Length; i++) { 
                stringBuilder.AppendFormat("{0:X}",arr[i]);
            }
            return stringBuilder.ToString();    
        }
        static void ParallelKeysGeneration()
        {
            Console.WriteLine("Parallel Key generation started....");
            Stopwatch sw = Stopwatch.StartNew();
            Parallel.For(
                fromInclusive: 1,
                toExclusive: MaxSize + 1,
                //parallelOptions: new ParallelOptions { MaxDegreeOfParallelism=Environment.ProcessorCount/2}
                body: (i) =>
                {
                    var aes = AesManaged.Create();
                    aes.GenerateIV();
                    var key = aes.Key;
                    string s = ConvertToHexString(key);
                });
            sw.Stop();
            Console.WriteLine($"{nameof(ParallelKeysGeneration)}() completed in {sw.ElapsedMilliseconds} ms");
        }

        static void SequentialKeysGeneration()
        {
            Console.WriteLine("Sequential Key generation started....");
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < MaxSize; i++) {
                var aes = AesManaged.Create();
                aes.GenerateIV();
                var key = aes.Key;
                string s = ConvertToHexString(key); 
            }
            sw.Stop();
            Console.WriteLine($"{nameof(SequentialKeysGeneration)}() completed in {sw.ElapsedMilliseconds} ms");
        }

        static void DrawCircle()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int t = i * i / (i * i + 1);
            }
            Thread.Sleep(500);
            Console.WriteLine($"{nameof(DrawCircle)}() called.");
        }
        static void DrawEllipse()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int t = i * i / (i * i + 1);
            }
            Thread.Sleep(200);
            Console.WriteLine($"{nameof(DrawEllipse)}() called.");
        }
        static void DrawSquare()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int t = i * i / (i * i + 1);
            }
            Thread.Sleep(100);
            Console.WriteLine($"{nameof(DrawSquare)}() called.");
        }
        static void DrawRectangle()
        {
            for (int i = 0; i < MaxSize; i++)
            {
                int t = i * i / (i * i + 1);
            }
            Thread.Sleep(900);
            Console.WriteLine($"{nameof(DrawRectangle)}() called.");
        }
    }
}
