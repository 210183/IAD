using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning.NeighborhoodFunctions
{
    public class BinaryNeighborhood : INeighborhoodFunction
    {
        public double Calculate(double distance, double lambdaParameter)
        {
            return distance < lambdaParameter ? 1 : 0;
        }
    }
}
