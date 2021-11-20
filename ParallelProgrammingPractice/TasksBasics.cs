using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgrammingPractice
{
    public static class TasksBasics
    {
        public static void CallingMethodsWithoutReturnTypeViaTasks()
        {
            //Both the below tasks will run parallely and the output will be mix of ? - . randomly
            //The below task is created and started soon after its creation
            Task.Factory.StartNew(() => Write('?'));

            //The below task is only created. To start it, we need to call Start() function
            var t = new Task(() => Write('-'));
            t.Start();

            Write('.');

            //There is another way to create tasks and
            //pass arguments to the function you're going to call in it
            //Both the below tasks will run parallely and the output will be mix of hello, 123 randomly
            Task t1 = new Task(WriteObject, "hello");
            t1.Start();

            Task.Factory.StartNew(WriteObject, 123);
        }
        public static void Write(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        public static void WriteObject(Object o)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
            }
        }
    
        public static void CallingMethodsWithReturnTypeViaTasks()
        {
            string text1 = "string", text2 = "text";
            //Creating tasks with generic type as the method to be called has a return type
            var task1 = new Task<int>(GetLength, text1);
            task1.Start();

            Task<int> task2 = Task.Factory.StartNew(GetLength, text2);
            Console.WriteLine($"Length of '{text1}' is {task1.Result}");
            Console.WriteLine($"Length of '{text2}' is {task2.Result}");
        }
        public static int GetLength(Object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} processing object {o}...");
            return o.ToString().Length;
        }
    }
}
