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
    public class SONTrainer : IOnGoingTrainer
    {
        //public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();
        public SONAdapter Adapter { get; set; }
        public IDataProvider DataProvider { get; set; }
        public Datum[] DataSet { get; }

        private int dataIndexInEpoch = 0;
        private int epochNumber = 0;
        private int dataSetLength;  // helper variable to shorten code
        public SONTrainer(
            IDataProvider dataProvider,
            NeuralNetwork network,
            SONLearningAlgorithm learningAlgorithm,
            SONLearningRateHandler learningRateHandler,
            ILengthCalculator lengthCalculator,
            ConscienceWithPotential conscience = null
            )
        {
            DataProvider = dataProvider;
            Adapter = new SONAdapter(learningAlgorithm, learningRateHandler, lengthCalculator, conscience);

            DataSet = dataProvider.Points;
            int shuffleAmount = DataProvider.Points.Length; // shuffle data
            DataProvider.ShuffleDataSet(DataSet, shuffleAmount);

            dataSetLength = DataSet.Length;
            NetworkStatesHistory.Add(network.DeepCopy());
        }

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount, bool shouldStoreNetworks = true)
        {
            Vector<double> currentPoint;
            for (int trainCounter = 0; trainCounter < dataCount; trainCounter++)
            {
                CheckForNewEpoch();
                currentPoint = DataSet[dataIndexInEpoch].X; // get next point
                // learn with current point
                Adapter.AdaptWeights(networkToTrain, currentPoint, epochNumber * dataSetLength + dataIndexInEpoch);
                //store network state
                if(shouldStoreNetworks)
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
                    epochNumber++;
                    int shuffleAmount = DataProvider.Points.Length; // reshuffle data
                    DataProvider.ShuffleDataSet(DataSet, shuffleAmount);
                }
            }
        }
    }
}
