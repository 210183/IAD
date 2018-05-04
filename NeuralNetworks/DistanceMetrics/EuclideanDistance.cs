using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks.DistanceMetrics
{
    class EuclideanDistance : BasicDistance
    {
        public override double Distance(Vector<double> x, Vector<double> w)
        {
            return Math.Sqrt((x - w).PointwisePower(2).Sum());
        }
    }
}
