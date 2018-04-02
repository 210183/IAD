using System.Globalization;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace NeuralNetworks.Data
{
    public abstract class DataProvider : IDataProvider
    {

        public Datum[] DataSet { get; set; }

        protected string[] LoadFileToStringTable(int inputsNumber, int outputsNumber, string fileName)
        {
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            nonInvariantCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

            string[] tempSet = System.IO.File.ReadAllLines(fileName);

            return tempSet;
        }
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
                set[randomizer.Next(setLength)] = set[randomizer.Next(setLength)];
            }
        }
    }

}