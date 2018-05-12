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
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();
        public SONAdapter Adapter { get; set; }
        public IDataProvider DataProvider { get; set; }
        public LearningHistoryObserver Observer { get; set; }
        public Datum[] DataSet { get; }

        public int DataIndexInEpoch { get => dataIndexInEpoch; }
        public int EpochNumber { get => epochNumber;}
        public int DataSetLength { get => dataSetLength;}

        private int dataIndexInEpoch = 0;
        private int epochNumber = 0;
        private int dataSetLength;  // helper variable to shorten code
        public SONTrainer(
            IDataProvider dataProvider,
            NeuralNetwork network,
            SONLearningAlgorithm learningAlgorithm,
            SONLearningRateHandler learningRateHandler,
            ILengthCalculator lengthCalculator,
            ConscienceWithPotential conscience = null,
            LearningHistoryObserver observer = null
            )
        {
            Adapter = new SONAdapter(learningAlgorithm, learningRateHandler, lengthCalculator, conscience);
            DataProvider = dataProvider;
            DataSet = dataProvider.Points;
            dataSetLength = DataSet.Length;
            int shuffleAmount = DataProvider.Points.Length; // shuffle data
            DataProvider.ShuffleDataSet(DataSet, shuffleAmount);
            Observer = observer;
            Observer?.SaveNetworkState(network);
        }

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount)
        {
            Vector<double> currentPoint;
            for (int trainCounter = 0; trainCounter < dataCount; trainCounter++)
            {
                CheckForNewEpoch();
                currentPoint = DataSet[dataIndexInEpoch].X; // get next point
                // learn with current point
                Adapter.AdaptWeights(networkToTrain, currentPoint, epochNumber * dataSetLength + dataIndexInEpoch);
                //store network state if needed
                Observer?.SaveNetworkState(networkToTrain);
                dataIndexInEpoch++;
            }
            //local methods
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
