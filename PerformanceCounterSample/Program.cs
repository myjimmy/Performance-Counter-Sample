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
        public const int GET_DATA_INTERVAL = 1000; // in milliseconds
    }

    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounterCategory pcc_session;
            string[] instances_session;

            /* "User Input Delay per Session" Category */
            // Instances
            try
            {
                pcc_session = new PerformanceCounterCategory(Constants.COUNTER_PER_SESSION);
                instances_session = pcc_session.GetInstanceNames();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get instance information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_SESSION);
                Console.WriteLine(ex.Message);
                return;
            }

            // If an empty array is returned, the category has a single instance.
            if (instances_session.Length == 0)
            {
                Console.WriteLine("Category \"{0}\" on this computer" +
                    " is single-instance.", pcc_session.CategoryName);
            }
            else
            {
                // Otherwise, display the instances.
                Console.WriteLine("These instances exist in category \"{0}\" on this computer:",
                    pcc_session.CategoryName);

                Array.Sort(instances_session);

                int objX;
                for (objX = 0; objX < instances_session.Length; objX++)
                {
                    Console.WriteLine("{0,4} - {1}", objX + 1, instances_session[objX]);
                }
            }

            // Counters
            PrintCounterList(pcc_session, instances_session);

            /* "User Input Delay per Process" Category */
            PerformanceCounterCategory pcc_process;
            string[] instances_process;

            // Instances
            try
            {
                pcc_process = new PerformanceCounterCategory(Constants.COUNTER_PER_PROCESS);
                instances_process = pcc_process.GetInstanceNames();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get instance information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_PROCESS);
                Console.WriteLine(ex.Message);
                return;
            }

            // If an empty array is returned, the category has a single instance.
            if (instances_process.Length == 0)
            {
                Console.WriteLine("Category \"{0}\" on this computer" +
                    " is single-instance.", pcc_process.CategoryName);
            }
            else
            {
                // Otherwise, display the instances.
                Console.WriteLine("These instances exist in category \"{0}\" on this computer:",
                    pcc_process.CategoryName);

                Array.Sort(instances_process);

                int objX;
                for (objX = 0; objX < instances_process.Length; objX++)
                {
                    Console.WriteLine("{0,4} - {1}", objX + 1, instances_process[objX]);
                }
            }

            // Counters
            PrintCounterList(pcc_process, instances_process);

            /* Get & Show the values */
            PrintCounterValuePerSession(pcc_session, instances_session);
        }

        static void PrintCounterList(PerformanceCounterCategory pcc, string[] instances)
        {
            PerformanceCounter[] counters;
            string category = pcc.CategoryName;

            if (instances.Length == 0)
            {
                try
                {
                    counters = pcc.GetCounters();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get counter information for single-instance " +
                        "category \"{0}\" on this computer:", category);
                    Console.WriteLine(ex.Message);
                    return;
                }

                Console.WriteLine("**** Category \"{0}\": single-instance *****", category);

                if (counters != null)
                {
                    foreach (var counter in counters)
                    {
                        Console.WriteLine(counter.CounterName);
                    }
                }
                else
                {
                    Console.WriteLine("This category has no counter for single-instance.");
                }
            }
            else
            {
                foreach (var instance in instances)
                {
                    try
                    {
                        counters = pcc.GetCounters(instance);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to get counter information for instance \"{1}\" in " +
                            "category \"{0}\" on this computer:", category, instance);
                        Console.WriteLine(ex.Message);
                        return;
                    }

                    Console.WriteLine("**** Category \"{0}\": Instance \"{1}\" *****", category, instance);

                    if (counters != null)
                    {
                        foreach (var counter in counters)
                        {
                            Console.WriteLine(counter.CounterName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("This category has no counter for {0}.", instance);
                    }
                }
            }            
        }

        static void PrintCounterValuePerSession(PerformanceCounterCategory pcc, string[] instances)
        {
            PerformanceCounter[] counters;
            string category = pcc.CategoryName;

            if (instances.Length == 0)
            {
                counters = new PerformanceCounter[1];

                PerformanceCounter[] tempCounters;
                try
                {
                    tempCounters = pcc.GetCounters();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get counter information for single-instance " +
                        "category \"{0}\" on this computer:", category);
                    Console.WriteLine(ex.Message);
                    return;
                }

                if (tempCounters != null)
                {
                    counters[0] = tempCounters[0];
                }
                else
                {
                    Console.WriteLine("This category has no counter for single-instance.");
                    return;
                }
            }
            else
            {
                counters = new PerformanceCounter[instances.Length];

                int i = 0;
                foreach (var instance in instances)
                {
                    PerformanceCounter[] tempCounters;

                    try
                    {
                        tempCounters = pcc.GetCounters(instance);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to get counter information for instance \"{1}\" in " +
                            "category \"{0}\" on this computer:", category, instance);
                        Console.WriteLine(ex.Message);
                        return;
                    }

                    if (tempCounters != null)
                    {
                        counters[i] = tempCounters[0];
                    }
                    else
                    {
                        Console.WriteLine("This category has no counter for {0}.", instance);
                        return;
                    }

                    i++;
                }
            }

            Console.WriteLine("{0,-10}\t{1,-5}\t{2,-5}\t{3,-10}\t{4,-10}", "Time", "Category", "Instance", "Counter", "Value");

            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < counters.Length; i++)
                {
                    try
                    {
                        string instance_name = (instances.Length == 0) ? "single-instance" : instances[i];
                        float perfValue = counters[i].NextValue();
                        Console.WriteLine("{0:yyyy/MM/dd HH:mm:ss}\t{1,-5}\t{2,-5}\t{3,-10}\t{4,-10}",
                            DateTime.Now, category, instance_name, counters[i].CounterName, perfValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR: Could not read NextValue: {0}", ex.Message);
                        return;
                    }
                }

                System.Threading.Thread.Sleep(Constants.GET_DATA_INTERVAL);
            }

        }
    }
}
