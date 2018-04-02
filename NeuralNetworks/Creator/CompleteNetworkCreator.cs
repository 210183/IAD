using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    /// <summary>
    /// Constructs neural networks to solve given problem. Creates many networks and choose best.
    /// Stores building parameters and history, so that user can view them.
    /// </summary>
    class CompleteNetworkCreator
    {
        #region properties to be set from outside
        public ILearningProvider DataProvider { get; set; }
        public LayerCharacteristic[] Layers { get; set; }
        public LearningAlgorithm LearningAlgorithm { get; set; }
        public int InputsNumber { get; set; }
        public bool IsBiasOn { get; set; }
        public IErrorCalculator ErrorCalculator { get; set; }
        public int MaxEpochs { get; set; }
        public double DesiredError { get; set; }
        #endregion

        public NeuralNetwork BestNetwork { get; set; }
        /// <summary>
        /// Best network learning history is saved here afer CreateNetwork.
        /// </summary>
        public Vector<double> BestNetworkEpochHistory { get; set; }
        /// <summary>
        /// Best network result on test set can be seen here after CreateNetwork.
        /// </summary>
        public double BestTestError { get; set; } = 0;


        /// <summary>
        /// Creates new networks with randomized initial weights, learns and tests them. Chooses best of all created. 
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="numberOfNetworksToTry"></param>
        /// <returns>Best network</returns>
        public NeuralNetwork CreateNetwork(TaskType taskType, int numberOfNetworksToTry)
        {
            var trainer = new OnlineTrainer(ErrorCalculator, DataProvider, LearningAlgorithm);
            for (int networkIndex = 0; networkIndex < numberOfNetworksToTry; networkIndex++)
            {  
                var currentNetwork = new NeuralNetwork(InputsNumber, Layers, IsBiasOn);
                trainer.TrainNetwork(currentNetwork, MaxEpochs, DesiredError);
                bool isUpdated = UpdateBestNetwork(currentNetwork);
                if (isUpdated)
                    BestNetworkEpochHistory = trainer.CurrentEpochErrorVector; // save learning history of best network
            }
            return BestNetwork;
        }

        /// <summary>
        /// Checks if new given network is better than current best. If it is, stores it as best.
        /// </summary>
        /// <param name="newNetwork"></param>
        /// <returns>True if best network was changed. Otherwise, false</returns>
        private bool UpdateBestNetwork(NeuralNetwork newNetwork)
        {
            if(BestNetwork is null) //save new network as best
            {
                BestNetwork = newNetwork;
                BestTestError = TestNetwork(BestNetwork);
                return true;
            }
            else // test new network and compare
            {
                var newNetworkError = TestNetwork(newNetwork);
                if (newNetworkError < BestTestError)
                {
                    BestNetwork = newNetwork;
                    BestTestError = newNetworkError;
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Uses network on every data from test set and uses error calc to calc aggregated error
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        private double TestNetwork(NeuralNetwork network)
        {
            var testSet = DataProvider.DataSet; //helper variable to shorten code and clarify;
            Vector<double> errors = Vector<double>.Build.Dense( testSet.Length );
            for (int dataIndex = 0; dataIndex < testSet.Length; dataIndex++)
            {
                var output = network.CalculateOutput(testSet[dataIndex].X, CalculateMode.NetworkOutput);
                var error = ErrorCalculator.CalculateErrorSum(output, testSet[dataIndex].D);
                errors[dataIndex] = error;
            }
            var errorSum = ErrorCalculator.CalculateEpochError(errors);
            return errorSum;
        }
    }
}
