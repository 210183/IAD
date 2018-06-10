using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public abstract class LearningAlgorithm
    {
        public abstract void AdaptWeights(NeuralNetworkRadial network, 
                                          Vector<double> errors,
                                          double learningRate,
                                          double currentDataError,
                                          double previousDataError);
    }
}
