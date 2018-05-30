using System;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Networks;
using NeuralNetworks.Networks.RadialFunction;

namespace NeuralNetworks
{
    public class RadialLayer
    {
        public RadialLayer(RadialNeuron[] neurons, ILengthCalculator lengthCalculator, GaussianFunction radialFunction)
        {
            Neurons = neurons ?? throw new ArgumentNullException(nameof(neurons));
            LengthCalculator = lengthCalculator ?? throw new ArgumentNullException(nameof(lengthCalculator));
            RadialFunction = radialFunction ?? throw new ArgumentNullException(nameof(radialFunction));
            NumberOfNeurons = Neurons.Length;
        }
        /// <summary>
        /// helper property calculated from Neurons.Length
        /// </summary>
        public int NumberOfNeurons { get;}
        public RadialNeuron[] Neurons { get; set; }
        public ILengthCalculator LengthCalculator{ get; set; }
        public GaussianFunction RadialFunction { get; set; }

        public Vector<double> CalculateOutput(Vector<double> input)
        {
            var output = Vector<double>.Build.Dense(NumberOfNeurons);
            for (int neuronIndex = 0; neuronIndex < NumberOfNeurons; neuronIndex++)
            {
                output[neuronIndex] = RadialFunction.Calculate(input, Neurons[neuronIndex].Center, Neurons[neuronIndex].WidthModifier);
            }
            return output;
        }
    }
}