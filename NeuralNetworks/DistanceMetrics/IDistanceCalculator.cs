using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using VectorD = MathNet.Numerics.LinearAlgebra.Vector<double>;

namespace NeuralNetworks.DistanceMetrics
{
    interface IDistanceCalculator
    {
        double Distance(VectorD x, VectorD w);
        VectorD DifferenceVector(VectorD x, VectorD w);
    }
}
