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
        public ILengthCalculator LengthCalculator { get; set; }
        /// <summary>
        /// lambda
        /// </summary>
        public Lambda Lambda { get; set; }

        protected SONLearningAlgorithm(ILengthCalculator lengthCalculator, Lambda lambda)
        {
            Lambda = lambda;
            LengthCalculator = lengthCalculator;
        }

        public abstract Dictionary<RadialNeuron, double> GetCoefficients(NeuralNetworkRadial network, List<int> possibleNeurons, int winnerIndex, Vector<double> learningPoint, int iterationNumber);
    }
}
