using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using VectorD = MathNet.Numerics.LinearAlgebra.Vector<double>;

namespace NeuralNetworks.DistanceMetrics
{
    public interface ILengthCalculator
    {
        double Distance(VectorD x, VectorD w);
        double Length(VectorD x);
        VectorD DifferenceVector(VectorD x, VectorD w);
    }
}
