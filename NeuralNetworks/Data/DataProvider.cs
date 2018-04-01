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


        /// <summary>
        /// TODO!
        /// </summary>
        /// 


        public Datum[] DataSet { get; set; }

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