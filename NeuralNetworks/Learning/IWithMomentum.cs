using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    interface IWithMomentum
    {
        double MomentumCoefficient { get; set; }
        double MaxErrorIncreaseCoefficient { get; set; }
        Matrix<double> LastWeightsChange { get; set; }
    }
}
