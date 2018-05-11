using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Learning.MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public class SONAdapter
    {
        public ConscienceWithPotential Conscience { get; set; }
        public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public SONLearningRateHandler LearningRateHandler { get; set; }

        public void AdaptWeights(NeuralNetwork network, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;
            var neuronsToAdapt = Conscience.SelectPossibleWinners(weights);
            var neuronsAdaptCoefficients = LearningAlgorithm.GetCoefficients(network, learningPoint, iterationNumber);
            UpdateNeurons(learningPoint, weights, iterationNumber, neuronsAdaptCoefficients);
        }

        private void UpdateNeurons(Vector<double> learningPoint, Matrix<double> weights, int iterationNumber, Dictionary<int, double> neuronsAdaptCoefficients)
        {
            foreach (var neuronIndex in neuronsAdaptCoefficients.Keys)
            {
                Vector<double> correctionVector = LearningRateHandler.GetLearningRate(iterationNumber) * (learningPoint - weights.Column(neuronIndex));
                var adaptCoef = neuronsAdaptCoefficients[neuronIndex];
                for (int i = 0; i < correctionVector.Count; i++)
                {
                    weights[i, neuronIndex] += adaptCoef * correctionVector[i];
                }
            }
        }
    }
}
