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
        public GasAlgorithm(SONLearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator, Lambda lambda) : base(learningRateHandler, lengthCalculator, lambda)
        {
        }

        public override void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;

            var unsortedNeuron = new Dictionary<int, double>();
            for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++) // add neuroins with distances
            {
                if (true) //place for conscience mechainsm
                {
                    unsortedNeuron.Add(colIndex, LengthCalculator.Distance(learningPoint, weights.Column(colIndex)));
                }
            }
            var sortedNeuron = unsortedNeuron.OrderBy(pair => pair.Value)
               .Select((pair, index) => new { pair.Key, index })
               .ToDictionary(x => x.Key, x => (double)x.index);

            UpdateNeurons(learningPoint, weights, iterationNumber, sortedNeuron);
        }

        private void UpdateNeurons(Vector<double> learningPoint, Matrix<double> weights, int iterationNumber, Dictionary<int, double> neighborsPositions)
        {
            foreach (var neuronIndex in neighborsPositions.Keys)
            {
                Vector<double> correctionVector = LearningRateHandler.GetLearningRate(iterationNumber) * (learningPoint - weights.Column(neuronIndex));
                var neighborCoef = Math.Exp(-neighborsPositions[neuronIndex] / Lambda.GetValue(iterationNumber));
                for (int i = 0; i < correctionVector.Count; i++)
                {
                    weights[i, neuronIndex] += neighborCoef * correctionVector[i];
                }
            }
        }
    }
}
