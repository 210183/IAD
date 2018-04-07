using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworksTestsExtensions
{
    static class VectorExtension
    {
        public static Vector<double> RoundPointwise(this Vector<double> vector, int decimals = 4)
        {
            var outputVector = Vector<double>.Build.DenseOfVector(vector);
            for (int i = 0; i < vector.Count; i++)
            {
                outputVector[i] = Math.Round(vector[i], decimals);
            }
            return outputVector;
        }
    }
}
