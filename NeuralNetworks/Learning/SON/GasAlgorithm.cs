using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks
{
    public class GasAlgorithm : SONLearningAlgorithm
    {
        public GasAlgorithm(Lambda lambda) : base(lambda)
        {
        }

        public override Dictionary<RadialNeuron, double> GetCoefficients(NeuralNetworkRadial network, List<int> possibleNeurons, int winnerIndex, Vector<double> learningPoint, int iterationNumber)
        {
            var neurons = network.RadialLayer.Neurons;

            var unsortedNeuron = new Dictionary<RadialNeuron, double>();
            foreach (var neuron in neurons)
            {
                unsortedNeuron.Add(neuron, LengthCalculator.Distance(learningPoint, neuron.Center));
            }
            var sortedNeuron = unsortedNeuron.OrderBy(pair => pair.Value) // neuronindex in matrix and its index as ordered by distance
               .Select((pair, index) => new { pair.Key, index })
               .ToDictionary(x => x.Key, x => (double)x.index);

            var neuronCoefficients = new Dictionary<RadialNeuron, double>(); // neuron index in matrix and its coefficient for learning
            var lambda = Lambda.GetValue(iterationNumber);
            if (lambda == 0)
            {
                neuronCoefficients.Add(neurons[winnerIndex], 1);
            }
            else
            {
                foreach (var neuron in sortedNeuron.Keys)
                {
                    var nPosition = sortedNeuron[neuron];
                    neuronCoefficients.Add(neuron, Math.Exp(-(nPosition / lambda)));
                }
            }
            return neuronCoefficients.Where(x => x.Value >= 0.001).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
