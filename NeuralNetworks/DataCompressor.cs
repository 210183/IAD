using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC = NeuralNetworks.CompressionConstants;

namespace NeuralNetworks
{
    public class DataCompressor 
    {
        private static readonly int neuronsInFrame = CC.neuronsInFrame;
        private static readonly int stepSize = CC.stepSize;

        public DataCompressor(ILengthCalculator lengthCalculator)
        {
            LengthCalculator = lengthCalculator ?? throw new ArgumentNullException(nameof(lengthCalculator));
        }

        public ILengthCalculator LengthCalculator { get; set; }

        public void CompressData(NeuralNetwork network, string dataToCompressPath, string compressedDataPath, string bookCodePath)
        {
            var weights = network.Layers[0].Weights;
            var dataProvider = new PointsDataProvider(dataToCompressPath, neuronsInFrame);
            using (var writer = new StreamWriter(compressedDataPath)) //save compressed data
            {
                foreach (var data in dataProvider.Points) 
                {
                    int winnerIndex = FindWinnerIndex(weights, data.X);
                    writer.WriteLine(winnerIndex.ToString());
                }
            }
            using (var writer = new StreamWriter(bookCodePath)) //save codebook
            {
                for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
                {
                    for (int weightIndex = 0; weightIndex < weights.RowCount - 1; weightIndex++)
                    {
                        int scaledWeight = ScaleWeight(weights[weightIndex, neuronIndex]);
                        writer.Write(scaledWeight.ToString() + ",");
                    }
                    writer.WriteLine( ScaleWeight(weights[weights.RowCount - 1, neuronIndex]).ToString() ); // last without , but with end line
                }
            }
        }

        private int ScaleWeight(double weight)
        {
            return (int)Math.Round(weight);
        }
        private int FindWinnerIndex(Matrix<double> weights, Vector<double> learningPoint)
        {
            int winnerIndex = new Random().Next(0, weights.ColumnCount); // assume some neuron is the winner
            double winnerDistance = LengthCalculator.Distance(learningPoint, weights.Column(winnerIndex));
            for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
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
