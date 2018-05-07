using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public abstract class MLPLearningAlgorithm :  ILearningAlgorithm
    {
        public LearningRateHandler LearningRateHandler { get; set; }

        public MLPLearningAlgorithm(LearningRateHandler learningRateHandler)
        {
            this.LearningRateHandler = learningRateHandler;
        }

        public void AdaptLearningRate(double currentEpochError, double previousEpochError)
        {
            LearningRateHandler.UpdateRate(currentEpochError, previousEpochError);
        }

        public abstract void AdaptWeights(NeuralNetwork network, Vector<double> errors, double currentDataError, double previousDataError);

    }
}
