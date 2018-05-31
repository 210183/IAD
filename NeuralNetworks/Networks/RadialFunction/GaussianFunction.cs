using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks.RadialFunction
{
    public class GaussianFunction
    {
        public GaussianFunction(ILengthCalculator lengthCalculator)
        {
            LengthCalculator = lengthCalculator ?? throw new ArgumentNullException(nameof(lengthCalculator));
        }

        public ILengthCalculator LengthCalculator { get; set; }
        public double Calculate(Vector<double> input, Vector<double> center, double widthModifier)
        {
            double result = Math.Exp( - 
                (
                    LengthCalculator.Distance(input, center)
                    / (2 * Math.Pow(widthModifier, 2))
                ));
            return result;
        }
    }
}
