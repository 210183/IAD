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
    public class CompleteNetworkCreator
    {
        #region properties to be set from outside
        public ILearningProvider DataProvider { get; set; }
        public LayerCharacteristic[] Layers { get; set; }
        public MLPLearningAlgorithm LearningAlgorithm { get; set; }
        public int InputsNumber { get; set; }
        public bool IsBiasOn { get; set; }
        public IErrorCalculator ErrorCalculator { get; set; }
        public int MaxEpochs { get; set; }
        public double DesiredError { get; set; }
        public int NumberOfPointForApproximationFunction { get; set; } = 1000;
        #endregion

        public NeuralNetwork BestNetwork { get; set; }
        /// <summary>
        /// Best network learning history is saved here afer CreateNetwork.
        /// </summary>
        public Vector<double> BestNetworkEpochHistory { get; set; }
        /// <summary>
        /// Best network results on test set during learning can be seen here after CreateNetwork.
        /// </summary>
        public Vector<double> BestNetworkTestHistory { get; set; }
        /// <summary>
        /// Best network result on test set can be seen here after CreateNetwork.
        /// </summary>
        public double BestTestError { get; set; } = 0;
        /// <summary>
        /// Matrix representing which class was recognized as which
        /// </summary>
        public Matrix<double> ClassificationFullResults { get; set; }
        /// <summary>
        /// Set of point generated to visualize approximation function built by best network
        /// </summary>
        public double[,] ApproximationFunctionPoints { get; set; }

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
                trainer.TrainNetwork(ref currentNetwork, MaxEpochs, DesiredError);
                bool isUpdated = UpdateBestNetwork(currentNetwork);
                if (isUpdated) // save learning history of best network
                {
                    BestNetworkEpochHistory = trainer.EpochErrorHistory; 
                    BestNetworkTestHistory = trainer.TestErrorHistory;
                }
            }
            if (taskType == TaskType.Approximation)
                CreateApproximationFunctionPoints(BestNetwork); //generate approximation function visualization
            else if (taskType == TaskType.Classification)
                CreateResultMatrixForClassification(BestNetwork); //generate full results (divided by classes)
            return BestNetwork;
        }

        /// <summary>
        /// Checks if new given network is better than current best. If it is, stores it as best.
        /// </summary>
        /// <param name="newNetwork"></param>
        /// <returns>True if best network was changed. Otherwise, false</returns>
        private bool UpdateBestNetwork(NeuralNetwork newNetwork)
        {
            var tester = new NetworkTester(ErrorCalculator);
            if(BestNetwork is null) //save new network as best
            {
                BestNetwork = newNetwork;
                BestTestError = tester.TestNetwork(BestNetwork, DataProvider);
                return true;
            }
            else // test new network and compare
            {
                var newNetworkError = tester.TestNetwork(newNetwork, DataProvider);
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

        private void CreateResultMatrixForClassification(NeuralNetwork network)
        {
            var testSet = DataProvider.DataSet; //helper variable to shorten code and clarify;
            int numberOfClasses = network.Layers[Layers.Length - 1].Weights.ColumnCount; //neurons in last layer
            ClassificationFullResults = Matrix<double>.Build.Dense(numberOfClasses, numberOfClasses);
            for (int dataIndex = 0; dataIndex < testSet.Length; dataIndex++)
            {
                var output = network.CalculateOutput(testSet[dataIndex].X, CalculateMode.NetworkOutput);
                int chosenClassNumber = output.MaximumIndex();
                int properClassNumber = testSet[dataIndex].D.MaximumIndex();
                ClassificationFullResults[properClassNumber,chosenClassNumber] += 1;
            }
        }

        /// <summary>
        /// Works correctly for approximation with single value output.
        /// Otherwise will just take first value from network output as function value.
        /// </summary>
        /// <param name="network"></param>
        private void CreateApproximationFunctionPoints(NeuralNetwork network)
        {
            ApproximationFunctionPoints = new Double[NumberOfPointForApproximationFunction, 2];
            double maximum = 0, minimum = 0;
            foreach (var data in DataProvider.DataSet) // choose max and min
            {
                if(data.X.Max() > maximum)
                {
                    maximum = data.X.Max();
                }
                if (data.X.Min() < minimum)
                {
                    minimum = data.X.Min();
                }
            }
            double step = (maximum - minimum) / NumberOfPointForApproximationFunction;
            double xValue = minimum;
            var output = Vector<double>.Build.Dense(1);
            for (int index = 0; index < NumberOfPointForApproximationFunction; index++)
            {
                if (network.IsBiasExisting)
                {
                    output = network.CalculateOutput( Vector<double>.Build.Dense(new double[2] {1, xValue }) );
                }
                else
                {
                    output = network.CalculateOutput( Vector<double>.Build.Dense(new double[1] {xValue}) );
                }
                ApproximationFunctionPoints[index, 0] = xValue;
                ApproximationFunctionPoints[index, 1] = output[0]; //Assumed approximation will have only single output
                xValue += step; 
            }
        }
    }
}
