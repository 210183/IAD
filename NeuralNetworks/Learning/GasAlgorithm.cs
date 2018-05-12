using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;

namespace NeuralNetworks.Learning
{
    public class GasAlgorithm : SONLearningAlgorithm
    {
        public GasAlgorithm(ILengthCalculator lengthCalculator, Lambda lambda) : base(lengthCalculator, lambda)
        {
        }

        public override Dictionary<int, double> GetCoefficients(NeuralNetwork network, List<int> possibleNeurons, int winnerIndex, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;

            var unsortedNeuron = new Dictionary<int, double>();
            for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++) // add neurons with distances
            {
                unsortedNeuron.Add(colIndex, LengthCalculator.Distance(learningPoint, weights.Column(colIndex)));
            }
            var sortedNeuron = unsortedNeuron.OrderBy(pair => pair.Value) // neuronindex in matrix and its index as ordered by distance
               .Select((pair, index) => new { pair.Key, index })
               .ToDictionary(x => x.Key, x => (double)x.index);

            var neuronCoefficients = new Dictionary<int, double>(); // neuron index in matrix and its coefficient for learning
            var lambda = Lambda.GetValue(iterationNumber);
            if (lambda == 0)
            {
                neuronCoefficients.Add(winnerIndex, 1);
            }
            else
            {
                foreach (var neuronIndex in sortedNeuron.Keys)
                {
                    var nPosition = sortedNeuron[neuronIndex];
                    neuronCoefficients.Add(neuronIndex, Math.Exp(-(nPosition / lambda)));
                }
            } 
            return neuronCoefficients.Where(x => x.Value > 0.0001).ToDictionary(x => x.Key, x =>x.Value);
        }
    }
}
