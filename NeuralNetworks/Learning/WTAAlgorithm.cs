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
        public WTAAlgorithm(SONLearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator) : base(learningRateHandler, lengthCalculator)
        {

        }

        public override void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint)
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
                    UpdateWinner(colIndex);
                }
            }
            // adapt winner weights
            Vector<double> correctionVector = LearningRateHandler.GetLearningRate(1) *(learningPoint - weights.Column(winnerIndex));
            for (int i = 0; i < correctionVector.Count; i++)
            {
                weights[i, winnerIndex] += correctionVector[i]; 
            }

            //local methods
            bool IsBetter(int neuronIndex)
            {
                return LC.Distance(learningPoint, weights.Column(neuronIndex)) < winnerDistance;
            }
            void UpdateWinner(int newWinnerIndex)
            {
                winnerIndex = newWinnerIndex; // assume first neuron is the winner
                winnerDistance = LC.Distance(learningPoint, weights.Column(winnerIndex));
            }
        }
    }
}
