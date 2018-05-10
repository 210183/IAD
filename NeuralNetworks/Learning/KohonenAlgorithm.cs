using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;
using NeuralNetworks.Learning.NeighborhoodFunctions;

namespace NeuralNetworks.Learning
{
    public class KohonenAlgorithm : SONLearningAlgorithm
    {
        public KohonenAlgorithm(
            SONLearningRateHandler learningRateHandler,
            ILengthCalculator lengthCalculator,
            Lambda borderParameter,
            INeighborhoodFunction neighborhoodFunction, 
            ConscienceWithPotential conscience) : base(learningRateHandler, lengthCalculator)
        {
            BorderParameter = borderParameter;
            NeighborhoodFunction = neighborhoodFunction;
            Conscience = conscience;
        }

        /// <summary>
        /// lambda
        /// </summary>
        public Lambda BorderParameter { get; }

        public INeighborhoodFunction NeighborhoodFunction { get; }

        public ConscienceWithPotential Conscience { get;}

        private int iterationNumber = 0;

        public override void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint)
        {
            var weights = network.Layers[0].Weights;

            // find winner neuron
            int winnerIndex = FindWinnerIndex(weights, learningPoint, LengthCalculator);
               
            // find neighbors
            var winner = weights.Column(winnerIndex);
            var neighborsGrade = new Dictionary<int, double>();
            for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++)
            {
                var colDistance = LengthCalculator.Distance(weights.Column(colIndex), winner);   
                neighborsGrade.Add(colIndex, NeighborhoodFunction.Calculate(colDistance, BorderParameter.GetValue(iterationNumber)));
            }
            // adapt winner and neighbors weights
            UpdateNeurons(learningPoint, weights, neighborsGrade);
            Conscience.UpdatePotential(winnerIndex);

            iterationNumber++;
        }

        private int FindWinnerIndex(Matrix<double> weights, Vector<double> learningPoint, ILengthCalculator calculator)
        {
            int winnerIndex = new Random().Next(0,weights.ColumnCount); // assume some neuron is the winner
            double winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++)
            {
                if (IsBetter(colIndex))
                {
                    if (Conscience.CanBeWinner(colIndex))
                    {
                        UpdateWinnerIndex(colIndex);
                    }
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
                winnerDistance = calculator.Distance(learningPoint, weights.Column(winnerIndex));
            }
            #endregion
        }

        private void UpdateNeurons(Vector<double> learningPoint, Matrix<double> weights, Dictionary<int, double> neighborsGrade)
        {
            foreach (var neuronIndex in neighborsGrade.Keys)
            {
                Vector<double> correctionVector = LearningRateHandler.GetLearningRate(iterationNumber) * (learningPoint - weights.Column(neuronIndex));
                for (int i = 0; i < correctionVector.Count; i++)
                {
                    weights[i, neuronIndex] += neighborsGrade[neuronIndex] * correctionVector[i];
                }
            }
        }
    }
}
