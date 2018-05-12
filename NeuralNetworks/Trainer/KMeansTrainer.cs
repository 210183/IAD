using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning;
using NeuralNetworks.Learning.MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Trainer
{
    public class KMeansTrainer : IOnGoingTrainer
    {
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();
        public IDataProvider DataProvider { get; set; }
        public LearningHistoryObserver Observer { get; set; }
        public Datum[] DataSet { get; }
        public ILengthCalculator LengthCalculator { get; set; }
        public int EpochNumber { get => epochNumber; }

        private int epochNumber = 0;
        private int dataSetLength;  // helper variable to shorten code
        public KMeansTrainer(
            IDataProvider dataProvider,
            NeuralNetwork network,
            ILengthCalculator lengthCalculator,
            LearningHistoryObserver observer = null
            )
        {
            DataProvider = dataProvider;
            DataSet = dataProvider.Points;
            dataSetLength = DataSet.Length;
            int shuffleAmount = DataProvider.Points.Length; // shuffle data
            DataProvider.ShuffleDataSet(DataSet, shuffleAmount);

            LengthCalculator = lengthCalculator;

            Observer = observer;
            Observer?.SaveNetworkState(network);
        }

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int epochCount)
        {
            var weights = networkToTrain.Layers[0].Weights;
            Vector<double> currentPoint;
            int winnerIndex;
            for (int epochCounter = 0; epochCounter < epochCount; epochCounter++)
            {
                var neuronsAndItsData = new Dictionary<int, List<Vector<double>> >();
                for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++) // init dictionary with winners
                {
                    neuronsAndItsData.Add(neuronIndex, new List<Vector<double>>());
                }

                for (int dataIndex = 0; dataIndex < DataSet.Length; dataIndex++)
                {
                    currentPoint = DataSet[dataIndex].X; // get next point
                    winnerIndex = FindWinnerIndex(weights, currentPoint);
                    neuronsAndItsData[winnerIndex].Add(currentPoint);
                }
                //Adapt weights
                for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
                {
                    Vector<double> newWeights = Vector<double>.Build.Dense(weights.Column(neuronIndex).Count());
                    newWeights = MeanFromVectors(neuronsAndItsData[neuronIndex]);
                    weights.SetColumn(neuronIndex,(newWeights.ToArray()));
                }
                Observer?.SaveNetworkState(networkToTrain);
            }            
        }
        private Vector<double> MeanFromVectors(List<Vector<double>> vectors)
        {
            Vector<double> newWeights = Vector<double>.Build.Dense(vectors[0].Count());
            foreach (var vector in vectors)
            {
                newWeights.Add(vector);
            }
            newWeights.Divide(vectors.Count());
            return newWeights;
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
