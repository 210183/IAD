using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class BackPropagationRadialAlgorithm : LearningAlgorithm, IWithMomentum
    {
        private static readonly int outputLayerPosition = 2;

        public BackPropagationRadialAlgorithm(double momentum, double errorIncreaseCoefficient) : base()
        {
            MomentumCoefficient = momentum;
            MaxErrorIncreaseCoefficient = errorIncreaseCoefficient;
        }

        public double MomentumCoefficient { get; set; }
        public Matrix<double> LastWeightsChange { get; set; }
        public double MaxErrorIncreaseCoefficient { get; set; }

        public override void AdaptWeights(NeuralNetworkRadial network, Vector<double> errors, double learningRate, double currentDataError, double previousDataError)
        {
            #region helper variables
            var weights = network.OutputLayer.Weights;
            var layer = network.OutputLayer;
            int biasModifier = network.IsBiasExisting ? 1 : 0; // TODO: test That
            #endregion
            if (LastWeightsChange is null) // then initialize it with proper size
            {
                LastWeightsChange = Matrix<double>.Build.Dense(weights.RowCount, weights.ColumnCount);
            }

            #region calculate propagated errors
            Vector<double> propagatedErrors = Vector<double>.Build.Dense(layer.Weights.ColumnCount);
            propagatedErrors = errors;
            #endregion
            #region adapt weights using propagated error, outputs and derivatives
                for (int neuronIndex = 0; neuronIndex < layer.Weights.ColumnCount; neuronIndex++)
                {
                    for (int weightIndex = 0; weightIndex < layer.Weights.RowCount; weightIndex++)
                    {
                        var signal = network.LastOutputs[outputLayerPosition-1][weightIndex];
                        var currentNeuronError = propagatedErrors[neuronIndex];
                        var activationFunc = network.OutputLayer.ActivationFunction as IDifferentiable;
                        var derivative = network.OutputLayer.LastDerivatives[neuronIndex];
                        var backPropagationImpact = derivative * signal * currentNeuronError * learningRate;
                        if (currentDataError < previousDataError * MaxErrorIncreaseCoefficient) // accept that step and add momentum modifier
                        {
                            var momentumImpact = MomentumCoefficient * LastWeightsChange[weightIndex, neuronIndex];
                            layer.Weights[weightIndex, neuronIndex] += backPropagationImpact + momentumImpact;
                            LastWeightsChange[weightIndex, neuronIndex] = backPropagationImpact + momentumImpact; //update weights last change stored value
                        }
                        else // ignore momentum
                        {
                            layer.Weights[weightIndex, neuronIndex] += backPropagationImpact;
                            LastWeightsChange[weightIndex, neuronIndex] = backPropagationImpact;
                        }
                    }
                }
            #endregion
        }
    }
}