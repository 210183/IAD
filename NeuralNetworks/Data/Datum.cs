using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    /// <summary>
    /// Single entry of data storing input vector X and matching correct output vector - D
    /// </summary>
    public class Datum
    {
        public Datum(Vector<double> x, Vector<double> d)
        {
            X = x;
            D = d;
        }

        public Vector<double> X { get; set; }
        public Vector<double> D { get; set; }
    }
}
