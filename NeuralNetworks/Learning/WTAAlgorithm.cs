using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;

namespace NeuralNetworks.Learning
{
    public class WTAAlgorithm : SONLearningAlgorithm
    {
        public WTAAlgorithm(ILengthCalculator lengthCalculator) : base( lengthCalculator, null)
        {

        }

        public override Dictionary<int, double> GetCoefficients(NeuralNetwork network, List<int> possibleNeuronsIndexes, int winnerIndex, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;

            double winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            //result
            var neuronsToAdapt = new Dictionary<int, double>()
            {
                {winnerIndex, winnerDistance }
            };
            return neuronsToAdapt;
        }
    }
}
