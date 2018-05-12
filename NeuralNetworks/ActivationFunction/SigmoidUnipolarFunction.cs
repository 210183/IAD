using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class SigmoidUnipolarFunction : IOnGoingTrainer, IDifferentiable
    {
        public double Beta { get; set; }

        public SigmoidUnipolarFunction(double beta = 1)
        {
            Beta = beta;
        }
        public double Calculate(double argument)
        {
            return 1 / (1 + Math.Pow(Math.E, -Beta * argument));
        }

        public double CalculateDerivative(double argument)
        {
            var valueOnArgument = Calculate(argument);
            return Beta * valueOnArgument * (1 - valueOnArgument);
        }
    }
}
