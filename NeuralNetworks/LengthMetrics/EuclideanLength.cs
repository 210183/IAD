using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks.DistanceMetrics
{
    public class EuclideanLength : BasicLength
    {
        public override double Distance(Vector<double> x, Vector<double> w)
        {
            return Math.Sqrt((x - w).PointwisePower(2).Sum());
        }
        public override double Length(Vector<double> x)
        {
            return Math.Sqrt((x).PointwisePower(2).Sum());
        }
    }
}
