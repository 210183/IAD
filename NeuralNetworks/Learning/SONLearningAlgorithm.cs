using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;
using NeuralNetworks.Learning.NeighborhoodFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public abstract class SONLearningAlgorithm : ILearningAlgorithm
    {
        public ILengthCalculator LengthCalculator { get; set; }
        /// <summary>
        /// lambda
        /// </summary>
        public Lambda Lambda { get; set; }

        protected SONLearningAlgorithm( ILengthCalculator lengthCalculator, Lambda lambda)
        {
            Lambda = lambda;
            LengthCalculator = lengthCalculator;
        }

        public abstract void AdaptWeights(NeuralNetwork network,Vector<double> learningPoint, int iterationNumber);

        public abstract Dictionary<int, double> GetCoefficients(NeuralNetwork network, List<int> possibleNeurons, Vector<double> learningPoint, int iterationNumber);
    }
}
