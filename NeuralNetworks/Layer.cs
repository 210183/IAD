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
        public IActivationFunction ActivationFunction { get; set; }
        public Layer(Matrix<double> weights, IActivationFunction activationFunction)
        {
            Weights = weights;
            ActivationFunction = activationFunction;
        }
    }
}
