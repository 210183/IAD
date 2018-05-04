using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace NeuralNetworks.Data
{
    public class BasicDataProvider : IBasicDataProvider
    {
        /// <summary>
        /// Shuffles given data set by swaping two random elements n times.
        /// </summary>
        /// <param name="set">data set to shuffle</param>
        /// <param name="shuffleNumber"> How many times should two random elements be swaped</param>
        public void ShuffleDataSet(Datum[] set, int shuffleNumber = 100)
        {
            var randomizer = new Random();
            int setLength = set.Length;
            for (int i = 0; i < shuffleNumber; i++)
            {
                int firstIndex = randomizer.Next(setLength);
                int secondIndex = randomizer.Next(setLength);
                var temp = set[firstIndex];
                set[firstIndex] = set[secondIndex];
                set[secondIndex] = temp;
            }
        }

        protected string[] LoadFileToStringTable(int inputsNumber, int outputsNumber, string fileName)
        {
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            nonInvariantCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

            string[] tempSet = System.IO.File.ReadAllLines(fileName);

            return tempSet;
        }

    }
}
