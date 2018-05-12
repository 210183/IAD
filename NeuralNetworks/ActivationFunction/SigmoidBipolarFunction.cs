using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class SigmoidBipolarFunction : IActivationFunction, IDifferentiable
    {
        public double Beta { get; set; }

        public SigmoidBipolarFunction(double beta = 1)
        {
            Beta = beta;
        }

        public double Calculate(double argument)
        {
            return Math.Tanh(Beta * argument);
        }

        public double CalculateDerivative(double argument)
        {
            return Beta * (1 - Calculate(argument) * Calculate(argument));
        }
    }
}
