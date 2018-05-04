using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Trainer
{
    public class SONTrainer : IOnGoingTrainer
    {
        public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();

        public Datum[] DataSet { get; }

        private int dataIndexInEpoch = 0;
        private int dataSetLength;  // helper variable to shorten code

        public SONTrainer(SONLearningAlgorithm learningAlgorithm, NeuralNetwork network, IDataProvider dataProvider)
        {
            LearningAlgorithm = learningAlgorithm;
            DataSet = dataProvider.Points;
            dataSetLength = DataSet.Length;
            NetworkStatesHistory.Add(network.DeepCopy());
        }

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount)
        {
            Vector<double> currentPoint;
            for (int trainCounter = 0; trainCounter < dataCount; trainCounter++)
            {
                CheckForNewEpoch();
                currentPoint = DataSet[dataIndexInEpoch].X; // get next point
                // learn with current point
                LearningAlgorithm.AdaptWeights(networkToTrain,currentPoint);
                //store network state
                SaveNetworkState(networkToTrain);
                dataIndexInEpoch++;
            }

            // local methods
            void SaveNetworkState(NeuralNetwork network)
            {
                NetworkStatesHistory.Add(network.DeepCopy());
            }
            void CheckForNewEpoch()
            {
                if (dataIndexInEpoch == dataSetLength) // start new epoch
                {
                    dataIndexInEpoch = 0;
                }
            }
        }
    }
}
