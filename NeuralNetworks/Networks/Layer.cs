using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Layer
    {
        public Matrix<double> Weights  { get; set; }
        public IOnGoingTrainer ActivationFunction { get; set; }
        public Layer(Matrix<double> weights, IOnGoingTrainer activationFunction)
        {
            Weights = weights;
            ActivationFunction = activationFunction;
        }

        public Layer DeepCopy()
        {
            var copyWeights = Matrix<double>.Build.DenseOfMatrix(Weights);
            var actFunc = ActivationFunction;
            var copyLayer = new Layer(copyWeights, actFunc);
            return copyLayer;
        }
    }
}
