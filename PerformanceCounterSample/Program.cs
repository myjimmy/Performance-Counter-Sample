using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PerformanceCounterSample
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounterCategory[] categories;

            /* Generate a list of categories registered on this computer. */
            try
            {
                categories = PerformanceCounterCategory.GetCategories();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get categories on this computer:");
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("These categories are registered on this computer:");

            // Create and sort an array of category names.
            string[] categoryNames = new string[categories.Length];
            int objX;
            for (objX = 0; objX < categories.Length; objX++)
            {
                categoryNames[objX] = categories[objX].CategoryName;
            }
            Array.Sort(categoryNames);

            for (objX = 0; objX < categories.Length; objX++)
            {
                Console.WriteLine("{0,4} - {1}", objX + 1, categoryNames[objX]);
            }
        }
    }
}
