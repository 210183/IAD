using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworks.Learning
{
    public class SONAdapter
    {
        public SONAdapter(SONLearningAlgorithm learningAlgorithm, LearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator, ConscienceWithPotential conscience)
        {
            Conscience = conscience;
            LengthCalculator = lengthCalculator;
            LearningAlgorithm = learningAlgorithm ?? throw new ArgumentNullException(nameof(learningAlgorithm));
            LearningRateHandler = learningRateHandler ?? throw new ArgumentNullException(nameof(learningRateHandler));
        }
        public ILengthCalculator LengthCalculator { get; set; }
        public ConscienceWithPotential Conscience { get; set; }
        public SONLearningAlgorithm LearningAlgorithm { get; set; }
        public LearningRateHandler LearningRateHandler { get; set; }

        public void AdaptWeights(NeuralNetworkRadial network, Vector<double> learningPoint, int iterationNumber)
        {
            var neuronsToAdapt = new List<int>();
            for (int neuronIndex = 0; neuronIndex < network.RadialLayer.Neurons.Length; neuronIndex++)
            {
                neuronsToAdapt.Add(neuronIndex);
            }
            Conscience?.FilterPossibleWinners(ref neuronsToAdapt, iterationNumber);
            int winnerIndex = FindWinnerIndex(network.RadialLayer.Neurons, neuronsToAdapt, learningPoint);
            var neuronsAdaptCoefficients = LearningAlgorithm.GetCoefficients(network, neuronsToAdapt, winnerIndex, learningPoint, iterationNumber);
            UpdateNeurons(learningPoint, network.RadialLayer.Neurons, iterationNumber, neuronsAdaptCoefficients);
            Conscience?.UpdatePotential(winnerIndex);
        }

        private void UpdateNeurons(Vector<double> learningPoint, RadialNeuron[] neurons, int iterationNumber, Dictionary<RadialNeuron, double> neuronsAdaptCoefficients)
        {
            foreach (var neuron in neuronsAdaptCoefficients.Keys)
            {
                Vector<double> correctionVector = LearningRateHandler.GetLearningRate(iterationNumber) * (learningPoint - neuron.Center);
                var adaptCoef = neuronsAdaptCoefficients[neuron];
                neuron.Center += adaptCoef * correctionVector;
            }
        }
        private int FindWinnerIndex(RadialNeuron[] neurons, List<int> possibleNeurons, Vector<double> learningPoint)
        {
            int winnerIndex = possibleNeurons[(new Random().Next(0, possibleNeurons.Count()))]; // assume some neuron is the winner
            double winnerDistance = LengthCalculator.Distance(learningPoint, neurons[winnerIndex].Center);
            foreach (var neuronIndex in possibleNeurons)
            {
                if (IsBetter(neuronIndex))
                {
                    UpdateWinnerIndex(neuronIndex);
                }
            }
            return winnerIndex;

            #region local methods
            bool IsBetter(int neuronIndex)
            {
                return LengthCalculator.Distance(learningPoint, neurons[neuronIndex].Center) < winnerDistance;
            }
            void UpdateWinnerIndex(int newWinnerIndex)
            {
                winnerIndex = newWinnerIndex; 
                winnerDistance = LengthCalculator.Distance(learningPoint, neurons[winnerIndex].Center);
            }
            #endregion
        }
    }
}
