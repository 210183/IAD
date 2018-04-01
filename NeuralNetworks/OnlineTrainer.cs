using MathNet.Numerics.LinearAlgebra;
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
        public OnlineTrainer(IErrorCalculator errorCalculator, DataProvider dataProvider, LearningAlgorithm learningAlgorithm)
        {
            ErrorCalculator = errorCalculator;
            DataProvider = dataProvider;
            LearningAlgorithm = learningAlgorithm;
        }

        public IErrorCalculator ErrorCalculator { get; set; } = new MeanSquareErrorCalculator();
        public DataProvider DataProvider { get; set; }
        public LearningAlgorithm LearningAlgorithm { get; set; }
        public double LastEpochError { get; set; } = 0;
        public Vector<double> CurrentEpochErrors { get; set; }

        public void TrainNetwork(NeuralNetwork networkToTrain, int maxEpochs, double desiredErrorRate = 0)
        {
            /* dopoki epoki
             *  przejd przez epoke
             *  policz blad
             *  jezeli blad wystarczajacy
             *      zakoncz
             *  adaptuj wagi
             *  
             */
            var learnSet = DataProvider.LearnSet;
            int currentEpoch = 0;
            double currentEpochError = 0;
            while(currentEpoch < maxEpochs)
            {
                CurrentEpochErrors = Vector<double>.Build.Dense(learnSet.Length, 0); // init with 0s
                #region first data must be handled separately, because of indexing to previous element
                var output = networkToTrain.CalculateOutput(learnSet[0].X, CalculateMode.OutputsAndDerivatives);
                var error = ErrorCalculator.CalculateErrorVector(output, learnSet[0].D); // save error
                CurrentEpochErrors[0] = ErrorCalculator.CalculateSingleError(output, learnSet[0].D);
                LearningAlgorithm.AdaptWeights(networkToTrain, error, CurrentEpochErrors[0], CurrentEpochErrors[0]);
                #endregion 
                #region calculate rest of  epoch
                for (int dataIndex = 1; dataIndex < learnSet.Length; dataIndex++)
                {
                    output = networkToTrain.CalculateOutput(learnSet[dataIndex].X, CalculateMode.OutputsAndDerivatives);
                    error = ErrorCalculator.CalculateErrorVector(output, learnSet[dataIndex].D); // save error
                    CurrentEpochErrors[dataIndex] = ErrorCalculator.CalculateSingleError(output, learnSet[dataIndex].D);
                    #region adapt weights
                    LearningAlgorithm.AdaptWeights(networkToTrain,error, CurrentEpochErrors[dataIndex], CurrentEpochErrors[dataIndex-1] );
                    #endregion

                }
                #endregion

                #region epoch error
                currentEpochError = ErrorCalculator.CalculateEpochError(CurrentEpochErrors);
                #endregion

                if(currentEpochError <= desiredErrorRate) //learning is done
                {
                    return; 
                }
                #region Adapt Learning Rate
                LearningAlgorithm.AdaptLearningRate(currentEpochError, LastEpochError);
                #endregion

                LastEpochError = currentEpochError; // store epoch error
            }
        }
    }
}
