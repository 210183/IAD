using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public class Datum
    {
        public Vector<double> X { get; set; }

        public Datum(Vector<double> x)
        {
            X = x;
        }
    }
}
