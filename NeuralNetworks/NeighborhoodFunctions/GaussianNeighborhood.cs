using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning.NeighborhoodFunctions
{
    public class GaussianNeighborhood : INeighborhoodFunction
    {
        public double Calculate(double distance, double lambdaParameter)
        {
            double divisor = (2 * Math.Pow(lambdaParameter, 2)) > 0 ? (2 * Math.Pow(lambdaParameter, 2)) : 0.0000001;
            return Math.Exp( -((Math.Pow(distance, 2)) / divisor) );
        }
    }
}
