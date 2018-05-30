using MathNet.Numerics.LinearAlgebra;
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
        public NeuralNetworkRadial CreateRadialNetwork(RadialNetworkParameters parameters)
        {
            var lengthCalculator = new EuclideanLength();
            var randomizer = new Random();
            var radialNeurons = new RadialNeuron[parameters.NumberOfRadialNeurons];
            for (int i = 0; i < radialNeurons.Length; i++)
            {
                radialNeurons[i] = new RadialNeuron(1,
                        Vector<double>.Build.Dense(
                            parameters.NumberOfInputs,
                            (x) => randomizer.NextDouble() * (parameters.Max - parameters.Min) + parameters.Min
                            )
                );
            }
            var radialLayer = new RadialLayer(radialNeurons, lengthCalculator, new GaussianFunction());
            var outputLayer = new SigmoidLayer(
                Matrix<double>.Build.Dense(
                    parameters.NumberOfInputs,
                    parameters.NumberOfOutputNeurons,
                    (x, y) => randomizer.NextDouble() * (parameters.Max - parameters.Min) + parameters.Min),
                parameters.ActivationFunction,
                parameters.IsBiased
                );
            return new NeuralNetworkRadial(radialLayer, outputLayer, parameters.IsBiased);
        }
    }
}
