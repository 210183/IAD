using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using System.Collections.Generic;

namespace NeuralNetworks
{
    public class DataProvider
    {
        /// <summary>
        /// OUTDATED: Array consisting of two vectors: input data vectors (on index 0) and desired outputs vectors ( on index 1).
        /// </summary>
        public Datum[] LearnSet { get; set; }
        public Datum[] TestSet { get; set; }
    }
}