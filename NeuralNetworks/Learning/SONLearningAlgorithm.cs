using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public abstract class SONLearningAlgorithm : ILearningAlgorithm
    {
        public SONLearningRateHandler LearningRateHandler { get; }
        public ILengthCalculator LengthCalculator { get; set; }

        protected SONLearningAlgorithm(SONLearningRateHandler learningRateHandler, ILengthCalculator lengthCalculator)
        {
            LearningRateHandler = learningRateHandler;
            LengthCalculator = lengthCalculator;
        }

        public abstract void AdaptWeights(NeuralNetwork network,Vector<double> learningPoint);
    }
}
