﻿using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning;
using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class OnlineTrainer : ITrainer
    {
        public IErrorCalculator ErrorCalculator { get; set; } = new MeanSquareErrorCalculator();
        public ILearningProvider DataProvider { get; set; }
        public LearningAlgorithm LearningAlgorithm { get; set; }
        public LearningRateHandler LearningRateHandler { get; set; }
        public WidthModifierAdjuster WidthModifierAdjuster { get; set; }
        public SONAdapter CenterAdapter { get; set; }
        /// <summary>
        /// Test error is not used in any way here for network training purposes. It is only to  compare how test error was changing vs learning error.
        /// </summary>
        public Vector<double> TestErrorHistory { get; set; }
        public Vector<double> EpochErrorHistory { get; set; } = Vector<double>.Build.Dense(1);
        public Vector<double> CurrentEpochErrorVector { get; set; }
        public double BestError { get; set; } = Double.MaxValue;
        public NeuralNetworkRadial BestNetworkState { get; set; }

        private NetworkTester tester;

        public OnlineTrainer(IErrorCalculator errorCalculator,
                            ILearningProvider dataProvider, 
                            LearningAlgorithm learningAlgorithm,
                            LearningRateHandler learningRateHandler,
                            WidthModifierAdjuster widthModifierAdjuster,
                            SONAdapter centerAdapter)
        {
            ErrorCalculator = errorCalculator ?? throw new ArgumentNullException(nameof(errorCalculator));
            DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            LearningAlgorithm = learningAlgorithm ?? throw new ArgumentNullException(nameof(learningAlgorithm));
            WidthModifierAdjuster = widthModifierAdjuster ?? throw new ArgumentNullException(nameof(widthModifierAdjuster));
            LearningRateHandler = learningRateHandler ?? throw new ArgumentNullException(nameof(learningRateHandler));
            CenterAdapter = centerAdapter ?? throw new ArgumentNullException(nameof(centerAdapter));
            tester = new NetworkTester(ErrorCalculator);
        }

        public void TrainNetwork(ref NeuralNetworkRadial network, int maxEpochs, double desiredErrorRate = 0)
        {
            var learnSet = DataProvider.LearnSet; //shorter 

            var TempTestErrorHistory = Vector<double>.Build.Dense(maxEpochs + 1);
            Vector<double> TemporaryEpochErrorHistory = Vector<double>.Build.Dense(maxEpochs, 0); //place to temporary store epoch errors for all epochs
            TempTestErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal
            TemporaryEpochErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal

            int shuffleAmount = (int)Math.Round(Math.Log(learnSet.Length, 2)); // calculate how much should shuffle learn set depending on its size
            Vector<double> output;
            Vector<double> errorVector;
            int epochIndex = 1; // epochs are counted starting from 1
            int iterationNumber = 0; // how many data points were alreadu used in training
            while (epochIndex < maxEpochs)
            {
                DataProvider.ShuffleDataSet(learnSet, shuffleAmount); // shuffle some data in learn set
                CurrentEpochErrorVector = Vector<double>.Build.Dense(learnSet.Length, 0); // init with 0s

                // calculate epoch
                for (int dataIndex = 0; dataIndex < learnSet.Length; dataIndex++)
                {
                    // output and error
                    output = network.CalculateOutput(learnSet[dataIndex].X, CalculateMode.OutputsAndDerivatives);
                    errorVector = ErrorCalculator.CalculateErrorVector(output, learnSet[dataIndex].D);
                    CurrentEpochErrorVector[dataIndex] = ErrorCalculator.CalculateErrorSum(output, learnSet[dataIndex].D);
                   
                    // adapt weights
                    var learningRate = LearningRateHandler.GetLearningRate(iterationNumber);
                    LearningAlgorithm.AdaptWeights(network, errorVector, learningRate, CurrentEpochErrorVector[dataIndex], CurrentEpochErrorVector[dataIndex.Previous()]);
                   
                    //adapt center
                    CenterAdapter.AdaptWeights(network, learnSet[dataIndex].X, iterationNumber);

                    iterationNumber++;
                }
                #region epoch error
                TemporaryEpochErrorHistory[epochIndex] = ErrorCalculator.CalculateEpochError(CurrentEpochErrorVector);
                if (TemporaryEpochErrorHistory[epochIndex] <= desiredErrorRate) //learning is done
                {
                    return;
                }
                #endregion

                WidthModifierAdjuster.AdapthWidthModifier(network);

                // create and store test results
                var testError = tester.TestNetwork(network, DataProvider);
                TempTestErrorHistory[epochIndex] = testError;
                #region update best network state
                if (TempTestErrorHistory[epochIndex] < BestError)
                {
                    BestNetworkState = network.DeepCopy();
                    BestError = TempTestErrorHistory[epochIndex];
                }
                #endregion
                epochIndex++;
            }
            #region save errors for all epochs that actually were calculated
            TestErrorHistory = Vector<double>.Build.Dense(epochIndex);
            EpochErrorHistory = Vector<double>.Build.Dense(epochIndex);
            TemporaryEpochErrorHistory.CopySubVectorTo(EpochErrorHistory, 0, 0, epochIndex);
            TempTestErrorHistory.CopySubVectorTo(TestErrorHistory, 0, 0, epochIndex);
            #endregion
            //restore best network state ( on set for verifying)
            if(! (BestNetworkState == null))
                network = BestNetworkState;
        }
    }
}
