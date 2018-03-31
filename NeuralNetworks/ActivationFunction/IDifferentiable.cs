using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    interface IDifferentiable
    {
        double CalculateDerivative(double argument);
    }
}
