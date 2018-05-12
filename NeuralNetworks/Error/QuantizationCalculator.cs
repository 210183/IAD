using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Error
{
    public class QuantizationCalculator
    {
        public QuantizationCalculator(ILengthCalculator lengthCalculator)
        {
            LengthCalculator = lengthCalculator ?? throw new ArgumentNullException(nameof(lengthCalculator));
        }

        public ILengthCalculator LengthCalculator { get; set; }

        public double CalculateError(NeuralNetwork network, IDataProvider dataProvider)
        {
            var weights = network.Layers[0].Weights;
            double errorSum = 0; // to be modifed at the end (to become mean squared error)
            var data = dataProvider.Points;
            double winnerDistance;
            foreach (var point in data)
            {
                FindWinnerIndex(weights, point.X, out winnerDistance);
                errorSum += winnerDistance;
            }
            errorSum = errorSum / (data.Length);
            return errorSum;
        }

        private int FindWinnerIndex(Matrix<double> weights, Vector<double> learningPoint, out double winnerDistance)
        {
            int winnerIndex = new Random().Next(0, weights.ColumnCount); // assume some neuron is the winner
            double winnerDist = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
            {
                if (IsBetter(neuronIndex))
                {
                    UpdateWinnerIndex(neuronIndex);
                }
            }
            winnerDistance = winnerDist;
            return winnerIndex;

            #region local methods
            bool IsBetter(int neuronIndex)
            {
                return LengthCalculator.Distance(learningPoint, weights.Column(neuronIndex)) < winnerDist;
            }
            void UpdateWinnerIndex(int newWinnerIndex)
            {
                winnerIndex = newWinnerIndex; // assume first neuron is the winner
                winnerDist = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            }
            #endregion
        }
    }
}
