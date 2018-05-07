using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.NeighborhoodFunctions;

namespace NeuralNetworks.Learning
{
    public class KohonenAlgorithm : SONLearningAlgorithm
    {
        public KohonenAlgorithm(double learningRate, ILengthCalculator lengthCalculator, Lambda borderParameter, INeighborhoodFunction neighborhoodFunction) : base(learningRate, lengthCalculator)
        {
            BorderParameter = borderParameter;
            NeighborhoodFunction = neighborhoodFunction;
        }

        /// <summary>
        /// lambda
        /// </summary>
        public Lambda BorderParameter { get; set; }

        public INeighborhoodFunction NeighborhoodFunction { get; set; }

        private int iterationNumber = 0;

        public override void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint)
        {
            var LC = LengthCalculator; // to shorten
            var weights = network.Layers[0].Weights;
            // find winner neuron
            int winnerIndex = 0; // assume first neuron is the winner
            double winnerDistance = LC.Distance(learningPoint, weights.Column(winnerIndex));
            for (int colIndex = 1; colIndex < weights.ColumnCount; colIndex++)
            {
                if (IsBetter(colIndex))
                {
                    UpdateWinnerIndex(colIndex);
                }
            }
            // adapt winners weights
            var winner = weights.Column(winnerIndex);
            var neighborsIndexes = new List<int>();
            for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++)
            {
                var colDistance = LC.Distance(weights.Column(colIndex), winner);
                if (colDistance < BorderParameter.GetValue(iterationNumber))
                {
                    neighborsIndexes.Add(colIndex);
                }
            }
            foreach (var neuronIndex in neighborsIndexes)
            {
                UpdateNeuron(learningPoint, weights, neuronIndex);
            }
            iterationNumber++;

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

        private void UpdateNeuron(Vector<double> learningPoint, Matrix<double> weights, int neuronIndex)
        {
            Vector<double> correctionVector = LearningRate * (learningPoint - weights.Column(neuronIndex));
            for (int i = 0; i < correctionVector.Count; i++)
            {
                weights[i, neuronIndex] += correctionVector[i];
            }
        }
    }
}
