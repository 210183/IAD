using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Helper method that returns index -1 or 0,if current index is 0 or less. 
        /// Used e.g. to simplify iterating over data sets.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>index - 1 or 0</returns>
        public static int Previous (this int index)
        {
            return index > 0 ? index - 1 : 0;
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
