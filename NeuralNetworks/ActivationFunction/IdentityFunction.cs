using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class IdentityFunction : IActivationFunction, IDifferentiable
    {
        public double Calculate(double argument)
        {
            return argument;
        }

        public double CalculateDerivative(double argument)
        {
            return 1;
        }
    }
}
