using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning.NeighborhoodFunctions
{
    public interface INeighborhoodFunction
    {
        double Calculate(double distance, double lambdaParameter);
    }
}
