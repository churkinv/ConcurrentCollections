﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentCollections
{
    class Program
    {
        static object _lockObj = new object();

        static void Main(string[] args)
        {
            var orders = new Queue<string>();
            //PlaceOrders(orders, "Mark1");
            //PlaceOrders(orders, "Sergii1");
            Task task1 = Task.Run(() => PlaceOrders(orders, "Mark2"));
            Task task2 = Task.Run(() => PlaceOrders(orders, "Sergii2"));
            Task.WaitAll(task1, task2);

            //foreach (string order in orders)
            //{
            //    ProcessOrder(order);
            //}  we can replace with ==>
            Parallel.ForEach(orders, ProcessOrder);
            // but in this case we actulay don`t care how 
            // the values are partioned between the threads (we delegate it to Parallel class)
            // to handle it by our own we use partitioners (abstract classes?) from concurrent collections:
            // Partitioner<T>, OrderablePartitioner<T>, Partitioner, EnumerablePartitionerOptions

            foreach (string order in orders)
            {
                Console.WriteLine("Order:  " + order);
            }

            Console.ReadKey();
        }

        static void ProcessOrder(string order)
        {
        }

        private static void PlaceOrders(Queue<string> orders, string customerName)
        {
            #region regular way, to use with ConcurrentQueue
            //for (int i = 0; i < 5; i++)
            //{
            //    Thread.Sleep(1);
            //    string orderName = string.Format($"{customerName}, {i + 1}");
            //    orders.Enqueue(orderName);
            //}
            // another option to keep multithreading code working properly is 
            // using lock to prevent data corruption
            #endregion
            
            #region lock
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1);
                string orderName = string.Format($"{customerName} wants T-shirt, {i + 1}");
                lock (_lockObj)
                {
                    orders.Enqueue(orderName);
                }                
            }
            #endregion 

        }
    }
}
