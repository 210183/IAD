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
        public WTAAlgorithm(SONLearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator) : base(learningRateHandler, lengthCalculator, null)
        {

        }

        public override void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint, int iterationNumber)
        {
            var LC = LengthCalculator; // to shorten
            var weights = network.Layers[0].Weights;
            // find winner neuron
            int winnerIndex = 0; // assume first neuron is the winner
            double winnerDistance = LC.Distance(learningPoint, weights.Column(winnerIndex));
            for (int colIndex = 1; colIndex < weights.ColumnCount; colIndex++)
            {
                if(IsBetter(colIndex))
                {
                    UpdateWinnerIndex(colIndex);
                }
            }
            //local methods
            bool IsBetter(int neuronIndex)
            {
                return LC.Distance(learningPoint, weights.Column(neuronIndex)) < winnerDistance;
            }
            void UpdateWinnerIndex(int newWinnerIndex)
            {
                winnerIndex = newWinnerIndex; // assume first neuron is the winner
                winnerDistance = LC.Distance(learningPoint, weights.Column(winnerIndex));
            }
        }

        public override Dictionary<int, double> GetCoefficients(NeuralNetwork network, List<int> possibleNeuronsIndexes, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;
            // find winner neuron
            int winnerIndex = 0; // assume first neuron is the winner
            double winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            foreach (var neuronIndex in possibleNeuronsIndexes)
            {
                if (IsBetter(neuronIndex))
                {
                    UpdateWinnerIndex(neuronIndex);
                }
            }

            //result
            var neuronsToAdapt = new Dictionary<int, double>()
            {
                {winnerIndex, winnerDistance }
            };
            return neuronsToAdapt;

            #region local methods
            bool IsBetter(int neuronIndex)
            {
                return LengthCalculator.Distance(learningPoint, weights.Column(neuronIndex)) < winnerDistance;
            }
            void UpdateWinnerIndex(int newWinnerIndex)
            {
                winnerIndex = newWinnerIndex; // assume first neuron is the winner
                winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            }
            #endregion
        }
    }
}
