using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public abstract class SONLearningAlgorithm : ILearningAlgorithm
    {
        public double LearningRate { get; set; }
        public ILengthCalculator LengthCalculator { get; set; }

        protected SONLearningAlgorithm(double learningRate, ILengthCalculator lengthCalculator)
        {
            LearningRate = learningRate;
            LengthCalculator = lengthCalculator;
        }

        public abstract void AdaptWeights(NeuralNetwork network,Vector<double> learningPoint);
    }
}
