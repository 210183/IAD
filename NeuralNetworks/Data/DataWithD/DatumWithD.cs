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
    public class DatumWithD : Datum
    {
        public DatumWithD(Vector<double> x, Vector<double> d) : base(x)
        {
            D = d;
        }
        public Vector<double> D { get; set; }
    }
}
