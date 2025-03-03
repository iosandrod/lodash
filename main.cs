#nullable enable // 启用可空引用类型支持
using System;
using System.Threading;
using System.Threading.Tasks;
//
// Console.WriteLine($"初始账户余额：{}元");
// Namespace.Program.Main(new string[] { "" });
private static int counter = 0;  // 共享计数器
//
// 使用 Interlocked 来确保增加操作是原子性的
public static void IncrementCounter()
{
    // Interlocked.Increment 确保了对 counter 的操作是原子操作
    Interlocked.Increment(ref counter);
    Console.WriteLine($"Counter incremented to: {counter}");
}

public static void Main()
{
    // 创建多个线程来并发地增加计数器
    Thread thread1 = new Thread(IncrementCounter);
    Thread thread2 = new Thread(IncrementCounter);
    Thread thread3 = new Thread(IncrementCounter);//
    // 启动线程
    thread1.Start();
    thread2.Start();
    thread3.Start();
    // 等待线程完成
    thread1.Join();
    thread2.Join();
    thread3.Join();
    // 输出最终结果
    Console.WriteLine($"Final counter value: {counter}");
}
Main() 