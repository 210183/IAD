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
        public ILengthCalculator LengthCalculator { get; set; }
        public double LearningRate { get; set; }

        public abstract void AdaptWeights(NeuralNetwork network,Vector<double> learningPoint);
    }
}
