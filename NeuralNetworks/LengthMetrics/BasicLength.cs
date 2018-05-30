using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks.DistanceMetrics
{
    public abstract class BasicLength : ILengthCalculator
    {
        public Vector<double> DifferenceVector(Vector<double> x, Vector<double> w)
        {
            return x - w;
        }
        public abstract double Distance(Vector<double> x, Vector<double> w);
        public abstract double Length(Vector<double> x);
    }
}
