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
            string[] instances;

            /* "User Input Delay per Session" Category */
            // Instances
            try
            {
                pcc = new PerformanceCounterCategory(Constants.COUNTER_PER_SESSION);
                instances = pcc.GetInstanceNames();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get instance information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_SESSION);
                Console.WriteLine(ex.Message);
                return;
            }

            // If an empty array is returned, the category has a single instance.
            if (instances.Length == 0)
            {
                Console.WriteLine("Category \"{0}\" on this computer" +
                    " is single-instance.", pcc.CategoryName);
            }
            else
            {
                // Otherwise, display the instances.
                Console.WriteLine("These instances exist in category \"{0}\" on this computer:",
                    pcc.CategoryName);

                Array.Sort(instances);

                int objX;
                for (objX = 0; objX < instances.Length; objX++)
                {
                    Console.WriteLine("{0,4} - {1}", objX + 1, instances[objX]);
                }
            }

            // Counters
            PrintCounterList(pcc, instances);

            /* "User Input Delay per Process" Category */
            // Instances
            try
            {
                pcc = new PerformanceCounterCategory(Constants.COUNTER_PER_PROCESS);
                instances = pcc.GetInstanceNames();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get instance information for " +
                    "category \"{0}\" on this computer:", Constants.COUNTER_PER_PROCESS);
                Console.WriteLine(ex.Message);
                return;
            }

            // If an empty array is returned, the category has a single instance.
            if (instances.Length == 0)
            {
                Console.WriteLine("Category \"{0}\" on this computer" +
                    " is single-instance.", pcc.CategoryName);
            }
            else
            {
                // Otherwise, display the instances.
                Console.WriteLine("These instances exist in category \"{0}\" on this computer:",
                    pcc.CategoryName);

                Array.Sort(instances);

                int objX;
                for (objX = 0; objX < instances.Length; objX++)
                {
                    Console.WriteLine("{0,4} - {1}", objX + 1, instances[objX]);
                }
            }

            // Counters
            PrintCounterList(pcc, instances);
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
    }
}
