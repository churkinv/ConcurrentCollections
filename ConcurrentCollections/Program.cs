using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            var orders = new Queue<string>();
            //PlaceOrders(orders, "Mark1");
            //PlaceOrders(orders, "Sergii1");
            Task task1 = Task.Run(() => PlaceOrders(orders, "Mark2"));
            Task task2 = Task.Run(() => PlaceOrders(orders, "Sergii2"));
            Task.WaitAll(task1, task2);

            foreach (string order in orders)
            {
                Console.WriteLine("Order " + order);
            }

            Console.ReadKey();
        }

        private static void PlaceOrders(Queue<string> orders, string customerName)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1);
                string orderName = string.Format($"{customerName}, {i + 1}");
                orders.Enqueue(orderName);
            }
        }
    }
}
