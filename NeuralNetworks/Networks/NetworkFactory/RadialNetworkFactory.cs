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
            int biasModifier;
            if (parameters.IsBiased)
            {
                biasModifier = 1;
            }
            else
            {
                biasModifier = 0;
            }
            var randomizer = new Random();
            var lengthCalculator = new EuclideanLength();
            var radialNeurons = new RadialNeuron[parameters.NumberOfRadialNeurons];
            for (int i = 0; i < radialNeurons.Length; i++)
            {
                radialNeurons[i] = new RadialNeuron(1,
                        Vector<double>.Build.Dense(
                            parameters.NumberOfInputs + biasModifier,
                            (x) => randomizer.NextDouble() * (parameters.Max - parameters.Min) + parameters.Min
                            )
                );
            }
            var radialLayer = new RadialLayer(radialNeurons, lengthCalculator, new GaussianFunction(lengthCalculator));
            var outputLayer = new SigmoidLayer(
                Matrix<double>.Build.Dense(
                    parameters.NumberOfRadialNeurons + biasModifier,
                    parameters.NumberOfOutputNeurons,
                    (x, y) => randomizer.NextDouble() * (parameters.Max - parameters.Min) + parameters.Min),
                parameters.ActivationFunction,
                parameters.IsBiased
                );
            return new NeuralNetworkRadial(radialLayer, outputLayer, parameters.IsBiased);
        }
    }
}
