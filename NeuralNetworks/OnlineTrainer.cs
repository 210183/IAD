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
                #region calculate epoch
                for(int dataIndex =0; dataIndex < learnSet.Length; dataIndex++)
                {
                    var output = networkToTrain.CalculateOutput(learnSet[dataIndex].X, CalculateMode.OutputsAndDerivatives);
                    var error = ErrorCalculator.CalculateErrorVector(learnSet[dataIndex].X, learnSet[dataIndex].D); // save error
                    #region adapt weighs
                    LearningAlgorithm.AdaptWeights(networkToTrain,error);
                    #endregion
                }
                #endregion
                #region epoch error
                currentEpochError = ErrorCalculator.CalculateEpochError();
                #endregion
                if(currentEpochError <= desiredErrorRate)
                {
                    return; //learning is done
                }

            }
        }
    }
}
