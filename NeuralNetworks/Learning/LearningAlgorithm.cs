using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public abstract class LearningAlgorithm
    {
        public double LearningRate { get; set; }
        public abstract void AdaptWeights(NeuralNetwork network, Vector<double> errors);
    }
}
