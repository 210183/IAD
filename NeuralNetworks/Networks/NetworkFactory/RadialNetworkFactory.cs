using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Networks.RadialFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks.NetworkFactory
{
    public class RadialNetworkFactory
    {
        public NeuralNetworkRadial CreateRadialNetwork(RadialNetworkParameters parameters, IDataProvider dataProvider)
        {
            int biasModifier;
            if (parameters.IsBiased)
            {
                biasModifier = 1;
            }
            else
            {
                biasModifier = 0;
            }
            var radialLayer = CreateRadialLayer(parameters, biasModifier, dataProvider);
            var outputLayer = CreateOutputLayer(parameters, biasModifier);
            return new NeuralNetworkRadial(radialLayer, outputLayer, parameters.IsBiased);
        }

        private RadialLayer CreateRadialLayer(RadialNetworkParameters parameters, int biasModifier, IDataProvider dataProvider)
        {
 
            var dataSet = (dataProvider as ILearningProvider)?.LearnSet ?? dataProvider.DataSet; // try getting learn set, if cannot take data set (test set)
            var randomizer = new Random();
            var lengthCalculator = new EuclideanLength();
            var radialNeurons = new RadialNeuron[parameters.NumberOfRadialNeurons];
            int dataIndex;
            for (int i = 0; i < radialNeurons.Length; i++)
            {
                radialNeurons[i] = new RadialNeuron(1, Vector<double>.Build.Dense(parameters.NumberOfInputs + biasModifier));
                dataIndex = randomizer.Next(dataSet.Length);
                dataSet[dataIndex].X.CopySubVectorTo(radialNeurons[i].Center, biasModifier, biasModifier, dataSet[dataIndex].X.Count - biasModifier);
            }
            return new RadialLayer(radialNeurons, lengthCalculator, new GaussianFunction(lengthCalculator));
        }
        private SigmoidLayer CreateOutputLayer(RadialNetworkParameters parameters, int biasModifier)
        {
            var randomizer = new Random();
            return new SigmoidLayer(
                Matrix<double>.Build.Dense(
                    parameters.NumberOfRadialNeurons + biasModifier,
                    parameters.NumberOfOutputNeurons,
                    (x, y) => randomizer.NextDouble() * (parameters.Max - parameters.Min) + parameters.Min),
                parameters.ActivationFunction,
                parameters.IsBiased
                );
        }
    }
}
