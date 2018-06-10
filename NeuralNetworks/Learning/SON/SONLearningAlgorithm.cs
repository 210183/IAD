using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks
{
    public abstract class SONLearningAlgorithm
    {
        public ILengthCalculator LengthCalculator { get; set; } = new EuclideanLength();

        public Lambda Lambda { get; set; }

        protected SONLearningAlgorithm( Lambda lambda)
        {
            Lambda = lambda;
        }

        public abstract Dictionary<RadialNeuron, double> GetCoefficients(NeuralNetworkRadial network, List<int> possibleNeurons, int winnerIndex, Vector<double> learningPoint, int iterationNumber);
    }
}
