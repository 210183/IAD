using MathNet.Numerics.LinearAlgebra;
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
        public OnlineTrainer(IErrorCalculator errorCalculator, ILearningProvider dataProvider, LearningAlgorithm learningAlgorithm)
        {
            ErrorCalculator = errorCalculator;
            DataProvider = dataProvider;
            LearningAlgorithm = learningAlgorithm;
        }

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

        public void TrainNetwork(ref NeuralNetwork networkToTrain, int maxEpochs, double desiredErrorRate = 0)
        {
            var learnSet = DataProvider.LearnSet; //shorter
            int currentEpochIndex = 1; // epochs are counted starting from 1
            var tester = new NetworkTester(ErrorCalculator);
            var TempTestErrorHistory = Vector<double>.Build.Dense(maxEpochs + 1);
            TempTestErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal

            Vector<double> TemporaryEpochErrorHistory = Vector<double>.Build.Dense(maxEpochs, 0); //place to temporary store epoch errors for all epochs
            TemporaryEpochErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal
            int shuffleAmount = (int)Math.Round(Math.Log(learnSet.Length, 2)); // calculate how much should shuffle learn set depending on its size
            while (currentEpochIndex < maxEpochs)
            {
                DataProvider.ShuffleDataSet(learnSet, shuffleAmount); // shuffle some data in learn set
                CurrentEpochErrorVector = Vector<double>.Build.Dense(learnSet.Length, 0); // init with 0s

                #region first data must be handled separately, because of indexing to previous element
                var output = networkToTrain.CalculateOutput(learnSet[0].X, CalculateMode.OutputsAndDerivatives);
                var errorVector = ErrorCalculator.CalculateErrorVector(output, learnSet[0].D);
                CurrentEpochErrorVector[0] = ErrorCalculator.CalculateErrorSum(output, learnSet[0].D);
                LearningAlgorithm.AdaptWeights(networkToTrain, errorVector, CurrentEpochErrorVector[0], CurrentEpochErrorVector[0]);
                #endregion 
                #region calculate rest of  epoch
                for (int dataIndex = 1; dataIndex < learnSet.Length; dataIndex++)
                {
                    output = networkToTrain.CalculateOutput(learnSet[dataIndex].X, CalculateMode.OutputsAndDerivatives);
                    errorVector = ErrorCalculator.CalculateErrorVector(output, learnSet[dataIndex].D); 
                    CurrentEpochErrorVector[dataIndex] = ErrorCalculator.CalculateErrorSum(output, learnSet[dataIndex].D);
                    #region adapt weights
                    LearningAlgorithm.AdaptWeights(networkToTrain,errorVector, CurrentEpochErrorVector[dataIndex], CurrentEpochErrorVector[dataIndex-1] );
                    #endregion

                }
                #endregion
                #region epoch error
                TemporaryEpochErrorHistory[currentEpochIndex] = ErrorCalculator.CalculateEpochError(CurrentEpochErrorVector);
                if(TemporaryEpochErrorHistory[currentEpochIndex] <= desiredErrorRate) //learning is done
                {
                    return; 
                }
                #endregion
                #region Adapt Learning Rate
                LearningAlgorithm.AdaptLearningRate(TemporaryEpochErrorHistory[currentEpochIndex], TemporaryEpochErrorHistory[currentEpochIndex - 1]);
                #endregion
                #region create and store test results
                var testError = tester.TestNetwork(networkToTrain, DataProvider);
                TempTestErrorHistory[currentEpochIndex] = testError;
                #endregion
                #region update best network state
                if (TempTestErrorHistory[currentEpochIndex] < BestError)
                {
                    BestNetworkState = networkToTrain.DeepCopy();
                    BestError = TempTestErrorHistory[currentEpochIndex];
                }
                #endregion

                currentEpochIndex++;
            }
            #region save errors for all epochs that really were calculated
            TestErrorHistory = Vector<double>.Build.Dense(currentEpochIndex);
            EpochErrorHistory = Vector<double>.Build.Dense(currentEpochIndex);
            TemporaryEpochErrorHistory.CopySubVectorTo(EpochErrorHistory, 0, 0, currentEpochIndex);
            TempTestErrorHistory.CopySubVectorTo(TestErrorHistory, 0, 0, currentEpochIndex);
            #endregion
            //restore best network state ( on set for verifying)
            networkToTrain = BestNetworkState;
        }

        public NeuralNetwork DeepCopy()
        {

            throw new NotImplementedException();
        }
        ///// <summary>
        ///// Simple helper method that returns 0 if current index is 0 or less
        ///// </summary>
        ///// <param name="currentIndex"></param>
        ///// <returns>currentIndex - 1 or 0</returns>
        //private int GetPreviousIndex(int currentIndex)
        //{
        //    return currentIndex > 0 ? currentIndex - 1 : 0;
        //}
    }
}
