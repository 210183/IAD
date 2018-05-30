using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class NetworkTester
    {
        public NetworkTester(IErrorCalculator errorCalculator)
        {
            ErrorCalculator = errorCalculator;
        }

        public IErrorCalculator ErrorCalculator { get; set; }

        /// <summary>
        /// Uses network on every data from test set and uses error calc to calc aggregated error
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public double TestNetwork(NeuralNetworkRadial network, IDataProvider dataProvider)
        {
            var testSet = dataProvider.DataSet; //var to clarify and shorten 
            var errors = Vector<double>.Build.Dense(testSet.Length,0);
            Vector<double> currentOutput;
            for (int dataIndex = 0; dataIndex < testSet.Length; dataIndex++)
            {
                currentOutput = network.CalculateOutput(testSet[dataIndex].X);
                errors[dataIndex] = ErrorCalculator.CalculateErrorSum(currentOutput, testSet[dataIndex].D);
            }
            var totalError = ErrorCalculator.CalculateEpochError(errors);
            return totalError;
        }
    }
}
