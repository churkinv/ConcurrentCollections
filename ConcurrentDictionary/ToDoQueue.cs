using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentDictionary
{
    public class ToDoQueue
    {
        private readonly ConcurrentQueue<Trade> _queue = new ConcurrentQueue<Trade>();
        private bool _workingDayComplete = false;
        private readonly StaffLogsForBonuses _staffLogsForBonuses;

        public ToDoQueue(StaffLogsForBonuses staffResults)
        {
            _staffLogsForBonuses = staffResults;
        }

        public void AddTrade(Trade trade)
        {
            _queue.Enqueue(trade);
        }

        public void CompleteAdding()
        {
            _workingDayComplete = true;
        }

        public void MonitorAndLogTrates()
        {
            while (true)
            {
                Trade nextTrade;
                bool done = _queue.TryDequeue(out nextTrade);
                if (done)
                {
                    _staffLogsForBonuses.ProcessTrade(nextTrade);
                    Console.WriteLine("Processing transaction frim " + nextTrade.Person.Name);
                }
                else if (_workingDayComplete)
                {
                    Console.WriteLine("No more sales to log - exiting");
                    return;
                }
                else
                {
                    Console.WriteLine("No transaction availible");
                    Thread.Sleep(500);
                }
            }
        }
    }
}