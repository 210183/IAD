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
        public Vector<double> EpochErrorHistory { get; set; } = Vector<double>.Build.Dense(1);
        public Vector<double> CurrentEpochErrorVector { get; set; }

        public void TrainNetwork(NeuralNetwork networkToTrain, int maxEpochs, double desiredErrorRate = 0)
        {
            var learnSet = DataProvider.LearnSet;
            int currentEpochIndex = 1; // epochs are counted starting from 1

            Vector<double> TemporaryEpochErrorHistory = Vector<double>.Build.Dense(maxEpochs, 0); //place to temporary store epoch errors for all epochs
            TemporaryEpochErrorHistory[0] = Double.MaxValue; // assume error at beginning is maximal

            while(currentEpochIndex < maxEpochs)
            {
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
                
                currentEpochIndex++;
            }
            EpochErrorHistory = Vector<double>.Build.Dense(currentEpochIndex);
            TemporaryEpochErrorHistory.CopySubVectorTo(EpochErrorHistory, 0, 0, currentEpochIndex); // save errors for all epochs that really were calculated
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
