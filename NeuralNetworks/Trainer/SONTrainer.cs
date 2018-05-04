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
    class SONTrainer : IOnGoingTrainer
    {
        public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();

        private Datum[] dataSet;
        private int dataIndexInEpoch = 0;
        private int dataSetLength;  // helper variable to shorten code

        public SONTrainer(SONLearningAlgorithm learningAlgorithm, IDataProvider dataProvider)
        {
            LearningAlgorithm = learningAlgorithm;
            dataSet = dataProvider.Points;
            dataSetLength = dataSet.Length;
        }

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount)
        {
            if(NetworkStatesHistory.Count() == 0) // assume network not yet trained, so store its first state
            {
                SaveNetworkState(networkToTrain);
            }
            Vector<double> currentPoint;
            for (int trainCounter = 0; trainCounter < dataCount; trainCounter++)
            {
                CheckForNewEpoch();
                currentPoint = dataSet[dataIndexInEpoch].X; // get next point
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
