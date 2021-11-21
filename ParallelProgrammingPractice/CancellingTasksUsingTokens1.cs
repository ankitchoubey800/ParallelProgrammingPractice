using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgrammingPractice
{
    public static class CancellingTasksUsingTokens
    {
        public static void cancellationTokens1stWay()
        {
            //If a task is running, and we want to cancel it at any moment, then
            //we need to have two things: 1. CancellationTokenSource 2. Cancellation Token
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(() =>
              {
                  int i = 0;
                  while (true)
                  {
                      //Cancel by any one of the ways
                      //1st way
                      if (token.IsCancellationRequested)
                      {
                          break;
                          //or
                          throw new OperationCanceledException();
                          //here it checks if cancellation is requested, if yes then break
                          //instead of break, microsoft recommends using OperationCanceledException
                          //this will just end the task, by throwing an exception on low level,
                          //and won't create any exception in your code.
                      }
                      else
                          Console.WriteLine($"{i++}\t");
                  }
              }, token);
            t.Start();
            Console.ReadKey();
            cts.Cancel();   //this is how we cancel the running task
            Console.WriteLine("Main Program Done");
            Console.ReadLine();
        }
        public static void cancellationTokens2ndWay()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    //2nd Way
                    //more simpler way of cancelling a task calling 'ThrowIfCancellationRequested()'
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                }
            }, token);
            t.Start();

            Console.ReadKey();
            cts.Cancel();   //this is how we cancel the running task
            Console.WriteLine("Main Program Done");
            Console.ReadLine();
        }
        public static void cancellationTokens3rdWay()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    //3rd way
                    //In this way, we subscribe to the token and as soon as cancel()
                    //will be called on cancellation token, the task will be stopped.
                    token.Register(() => {
                        Console.WriteLine($"Cancellation has been requested");
                    });
                    Console.WriteLine($"{i++}\t");
                }
            }, token);
            t.Start();
            Console.ReadKey();
            cts.Cancel();   //this is how we cancel the running task
            Console.WriteLine("Main Program Done");
            Console.ReadLine();
        }
        public static void cancellationTokens4thWay()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            //4th Way
            Task.Factory.StartNew(() => {
                //this line awaits for cancellation
                //and cancels the task when Cancel()
                //is called on the cancellationtoken
                token.WaitHandle.WaitOne();
                Console.WriteLine("wait handle released, cancellation was requested");
            });

            Console.ReadKey();
            cts.Cancel();   //this is how we cancel the running task
            Console.WriteLine("Main Program Done");
            Console.ReadLine();
        }

        public static void LinkedMultipleCancellationTokens()
        {
            //multiple cancellation token can be created and can be linked to one token
            //and whenever any one token is cancelled, the linkedCancellationTokens will automatically
            //be cancelled
            var planned = new CancellationTokenSource();
            var preventive = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var linkedCancellationTokens = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token,preventive.Token,emergency.Token);

            //4th Way
            Task.Factory.StartNew(() => {
                int i = 0;
                while (true)
                {
                    linkedCancellationTokens.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}\t");
                    Thread.Sleep(100);
                }
            },linkedCancellationTokens.Token);

            emergency.Cancel();   //this is how we cancel the running task
            Console.WriteLine("Main Program Done");
            Console.ReadKey();
        }
    }
}
