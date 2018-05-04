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
        public static int SafePrevious (this int index)
        {
            return index > 0 ? index - 1 : 0;
        }
    }
}
