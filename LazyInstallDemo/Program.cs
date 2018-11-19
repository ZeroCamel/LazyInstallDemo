using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyInstallDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            //一、LAZY惰性加载
            //1、None threadsafe
            //Lazy<Foo> F = new Lazy<Foo>();
            //Console.WriteLine("Foo is defined.");
            //if (!F.IsValueCreated)
            //{
            //    Console.WriteLine("Foo is not installed yet");
            //}
            ////Here the object of Foo is created only when f.value is accessed.
            //Console.WriteLine("Foo::ID"+(F.Value as Foo).ID);

            //if (F.IsValueCreated)
            //{
            //    Console.WriteLine("Foo is installed now...");
            //}

            //2、单线程安全
            //2.1 PublicationOnly 多线程实例创建多次 变量发布共享
            //2.2 ExecutionAndPublication 单线程 实例只允许创建一次 变量发布共享
            //Lazy<Foo> f = new Lazy<Foo>(()=> {
            //    Foo fobj = new Foo() { ID = 99 };
            //    return fobj;
            //}, LazyThreadSafetyMode.PublicationOnly);
            //Console.WriteLine("Foo is defined");

            //ThreadPool.QueueUserWorkItem(new WaitCallback(Run), f);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Run), f);

            //二、TPL中的未处理异常
            TaskScheduler.UnobservedTaskException +=TaskSchduler_UnobservedTaskException;
            Test();
            Console.ReadLine();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.Read();
        }

        static void TaskSchduler_UnobservedTaskException(object sender,UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception);
        }

        public static Task Test()
        {
            return Task.Run(()=> { throw new Exception(); });
        }

        static void Run(object obj)
        {
            Lazy<Foo> flazy = obj as Lazy<Foo>;
            Foo f = flazy.Value as Foo;
            f.ID++;
            //线程休眠一秒
            Thread.Sleep(1000);
            Console.WriteLine("Foo::ID="+f.ID);
        }

        public class Foo
        {
            public int ID { get; set; }

            public Foo()
            {
                Console.WriteLine("Foo::Constructor is called");
                ID = 1;
            }
        }
    }
}
