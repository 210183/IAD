using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class SigmoidLayer
    {
        public Matrix<double> Weights  { get; set; }
        public IActivationFunction ActivationFunction { get; set; }
        public bool IsBiased { get; set; }

        public SigmoidLayer(Matrix<double> weights, IActivationFunction activationFunction, bool isBiased)
        {
            Weights = weights ?? throw new ArgumentNullException(nameof(weights));
            ActivationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            IsBiased = isBiased;
        }

        public Vector<double> CalculateOutput(Vector<double> input)
        {
            Vector<double> output = Vector<double>.Build.Dense(Weights.ColumnCount);
            for (int neuronIndex = 0; neuronIndex < Weights.ColumnCount; neuronIndex++)
            {
                double weightedSum = input * Weights.Column(neuronIndex);
                output[neuronIndex] = ActivationFunction.Calculate(weightedSum);
            }
            return output;
        }

        public SigmoidLayer DeepCopy()
        {
            var copyWeights = Matrix<double>.Build.DenseOfMatrix(Weights);
            var actFunc = ActivationFunction;
            var copyLayer = new SigmoidLayer(copyWeights, actFunc, IsBiased);
            return copyLayer;
        }
    }
}
