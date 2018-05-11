using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public class SONAdapter
    {
        public SONAdapter(SONLearningAlgorithm learningAlgorithm, SONLearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator, ConscienceWithPotential conscience)
        {
            Conscience = conscience;
            LengthCalculator = lengthCalculator;
            LearningAlgorithm = learningAlgorithm ?? throw new ArgumentNullException(nameof(learningAlgorithm));
            LearningRateHandler = learningRateHandler ?? throw new ArgumentNullException(nameof(learningRateHandler));
        }
        public ILengthCalculator LengthCalculator { get; set; }
        public ConscienceWithPotential Conscience { get; set; }
        public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public SONLearningRateHandler LearningRateHandler { get; set; }

        public void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;
            var neuronsToAdapt = new List<int>();
            for (int col = 0; col < weights.ColumnCount; col++)
            {
                neuronsToAdapt.Add(col);
            }
            Conscience?.FilterPossibleWinners(neuronsToAdapt);
            int winnerIndex = FindWinnerIndex(weights, neuronsToAdapt, learningPoint);
            var neuronsAdaptCoefficients = LearningAlgorithm.GetCoefficients(network, neuronsToAdapt, winnerIndex, learningPoint, iterationNumber);
            UpdateNeurons(learningPoint, weights, iterationNumber, neuronsAdaptCoefficients);
            Conscience?.UpdatePotential(winnerIndex);
        }

        private void UpdateNeurons(Vector<double> learningPoint, Matrix<double> weights, int iterationNumber, Dictionary<int, double> neuronsAdaptCoefficients)
        {
            foreach (var neuronIndex in neuronsAdaptCoefficients.Keys)
            {
                Vector<double> correctionVector = LearningRateHandler.GetLearningRate(iterationNumber) * (learningPoint - weights.Column(neuronIndex));
                var adaptCoef = neuronsAdaptCoefficients[neuronIndex];
                for (int i = 0; i < correctionVector.Count; i++)
                {
                    weights[i, neuronIndex] += adaptCoef * correctionVector[i];
                }
            }
        }
        private int FindWinnerIndex(Matrix<double> weights, List<int> possibleNeurons, Vector<double> learningPoint)
        {
            int winnerIndex = possibleNeurons[(new Random().Next(0, possibleNeurons.Count()))]; // assume some neuron is the winner
            double winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            foreach (var neuronIndex in possibleNeurons)
            {
                if (IsBetter(neuronIndex))
                {
                    UpdateWinnerIndex(neuronIndex);
                }
            }
            return winnerIndex;

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
