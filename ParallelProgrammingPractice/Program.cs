using System;
using System.Threading.Tasks;

namespace ParallelProgrammingPractice
{
    internal static class Program
    {
        public static void Main()
        {

            //TasksBasics.CallingMethodsWithoutReturnTypeViaTasks();
            TasksBasics.CallingMethodsWithReturnTypeViaTasks();
            //CallingMethodsWithReturnType();
            Console.WriteLine("Main Program Done"); //this line gets printed randomly in between.
                                                    //so, the tasks are running parallely
            Console.ReadKey();
        }

        
    }
}
