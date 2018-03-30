using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    class Layer
    {
        public Matrix<double> Weights  { get; set; }
    
        public Layer(Matrix<double> weights)
        {
            Weights = weights;
        }
    }
}
