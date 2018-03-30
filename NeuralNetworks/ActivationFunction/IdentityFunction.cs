using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class IdentityFunction : IActivationFunction
    {
        public double Calculate(double argument)
        {
            return argument;
        }
    }
}
