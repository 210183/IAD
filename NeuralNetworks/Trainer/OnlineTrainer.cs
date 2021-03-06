﻿using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
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
        /// <summary>
        /// Test error is not used in any way here for network training purposes. It is only to  compare how test error was changing vs learning error.
        /// </summary>
        public Vector<double> TestErrorHistory { get; set; }
        public Vector<double> EpochErrorHistory { get; set; } = Vector<double>.Build.Dense(1);
        public Vector<double> CurrentEpochErrorVector { get; set; }
        public double BestError { get; set; } = Double.MaxValue;
        public NeuralNetwork BestNetworkState { get; set; }

        private NetworkTester tester;

        public OnlineTrainer(IErrorCalculator errorCalculator, ILearningProvider dataProvider, LearningAlgorithm learningAlgorithm)
        {
            ErrorCalculator = errorCalculator;
            DataProvider = dataProvider;
            LearningAlgorithm = learningAlgorithm;
            tester = new NetworkTester(ErrorCalculator);
        }
        public void TrainNetwork(ref NeuralNetwork networkToTrain, int maxEpochs, double desiredErrorRate = 0)
        {
            var learnSet = DataProvider.LearnSet; //shorter 

            var TempTestErrorHistory = Vector<double>.Build.Dense(maxEpochs + 1);
            Vector<double> TemporaryEpochErrorHistory = Vector<double>.Build.Dense(maxEpochs, 0); //place to temporary store epoch errors for all epochs
            TempTestErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal
            TemporaryEpochErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal

            int shuffleAmount = (int)Math.Round(Math.Log(learnSet.Length, 2)); // calculate how much should shuffle learn set depending on its size
            Vector<double> output;
            Vector<double> errorVector;
            int EpochIndex = 1; // epochs are counted starting from 1

            while (EpochIndex < maxEpochs)
            {
                DataProvider.ShuffleDataSet(learnSet, shuffleAmount); // shuffle some data in learn set
                CurrentEpochErrorVector = Vector<double>.Build.Dense(learnSet.Length, 0); // init with 0s
                
                #region calculate epoch
                for (int dataIndex = 0; dataIndex < learnSet.Length; dataIndex++)
                {
                    output = networkToTrain.CalculateOutput(learnSet[dataIndex].X, CalculateMode.OutputsAndDerivatives);
                    errorVector = ErrorCalculator.CalculateErrorVector(output, learnSet[dataIndex].D); 
                    CurrentEpochErrorVector[dataIndex] = ErrorCalculator.CalculateErrorSum(output, learnSet[dataIndex].D);
                    #region adapt weights
                    LearningAlgorithm.AdaptWeights(networkToTrain,errorVector, CurrentEpochErrorVector[dataIndex], CurrentEpochErrorVector[dataIndex.Previous()] );
                    #endregion

                }
                #endregion
                #region epoch error
                TemporaryEpochErrorHistory[EpochIndex] = ErrorCalculator.CalculateEpochError(CurrentEpochErrorVector);
                if(TemporaryEpochErrorHistory[EpochIndex] <= desiredErrorRate) //learning is done
                {
                    return; 
                }
                #endregion
                #region Adapt Learning Rate
                LearningAlgorithm.AdaptLearningRate(TemporaryEpochErrorHistory[EpochIndex], TemporaryEpochErrorHistory[EpochIndex.Previous()]);
                #endregion
                #region create and store test results
                var testError = tester.TestNetwork(networkToTrain, DataProvider);
                TempTestErrorHistory[EpochIndex] = testError;
                #endregion
                #region update best network state
                if (TempTestErrorHistory[EpochIndex] < BestError)
                {
                    BestNetworkState = networkToTrain.DeepCopy();
                    BestError = TempTestErrorHistory[EpochIndex];
                }
                #endregion

                EpochIndex++;
            }
            #region save errors for all epochs that actually were calculated
            TestErrorHistory = Vector<double>.Build.Dense(EpochIndex);
            EpochErrorHistory = Vector<double>.Build.Dense(EpochIndex);
            TemporaryEpochErrorHistory.CopySubVectorTo(EpochErrorHistory, 0, 0, EpochIndex);
            TempTestErrorHistory.CopySubVectorTo(TestErrorHistory, 0, 0, EpochIndex);
            #endregion
            //restore best network state ( on set for verifying)
            networkToTrain = BestNetworkState;
        }
    }
}
