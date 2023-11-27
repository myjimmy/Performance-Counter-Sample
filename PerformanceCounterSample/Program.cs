using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PerformanceCounterSample
{    static class Constants
    {
        public const string COUNTER_PER_SESSION = "User Input Delay per Session";
        public const string COUNTER_PER_PROCESS = "User Input Delay per Process";
    }

    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounterCategory pcc;
            PerformanceCounter[] counters;

            /* "User Input Delay per Session" Category */
            try
            {
                pcc = new PerformanceCounterCategory(Constants.COUNTER_PER_SESSION);
                counters = pcc.GetCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get counter information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_SESSION);
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("**** Category \"{0}\" *****", Constants.COUNTER_PER_SESSION);
            if (counters != null)
            {
                foreach (var counter in counters)
                {
                    Console.WriteLine(counter.CounterName);
                }
            }
            else
            {
                Console.WriteLine("This category has no counter.");
            }

            /* "User Input Delay per Session" Category */
            try
            {
                pcc = new PerformanceCounterCategory(Constants.COUNTER_PER_PROCESS);
                counters = pcc.GetCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get counter information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_PROCESS);
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("**** Category \"{0}\" *****", Constants.COUNTER_PER_PROCESS);
            if (counters != null)
            {
                foreach (var counter in counters)
                {
                    Console.WriteLine(counter.CounterName);
                }
            }
            else
            {
                Console.WriteLine("This category has no counter.");
            }
        }
    }
}
