﻿using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class BackPropagationAlgorithm : LearningAlgorithm, IWithMomentum
    {
        public BackPropagationAlgorithm( LearningRateHandler learningRateHandler, double momentum, double errorIncreaseCoefficient) : base(learningRateHandler)
        {
            MomentumCoefficient = momentum;
            MaxErrorIncreaseCoefficient = errorIncreaseCoefficient;
        }

        public double MomentumCoefficient { get; set; }
        public Matrix<double>[] LastWeightsChange { get; set; }
        public double MaxErrorIncreaseCoefficient { get ; set ; }

        public override void AdaptWeights(NeuralNetwork network, Vector<double> errors, double currentDataError, double previousDataError)
        {
            #region helper variables
            int numberOfLayers = network.Layers.Count();
            var layers = network.Layers;
            int biasModifier = network.IsBiasExisting ? 1 : 0; // TODO: check THat
            #endregion
            if(LastWeightsChange is null) // then initialize it with proper size
            {
                LastWeightsChange = new Matrix<double>[numberOfLayers];
                for (int layerIndex = 0; layerIndex < numberOfLayers; layerIndex++) // create weights changes vectors
                {
                    LastWeightsChange[layerIndex] = Matrix<double>.Build.Dense(layers[layerIndex].Weights.RowCount, layers[layerIndex].Weights.ColumnCount);
                }
            }

            #region calculate propagated errors
            Vector<double>[] propagatedErrors = new Vector<double>[numberOfLayers];
            for (int layerIndex = numberOfLayers-1; layerIndex >= 0; layerIndex--) // begin with the ulitmate layer
            {
                propagatedErrors[layerIndex] = Vector<double>.Build.Dense(layers[layerIndex].Weights.ColumnCount);
                for (int neuronIndex = 0; neuronIndex < layers[layerIndex].Weights.ColumnCount; neuronIndex++)
                {
                    if (layerIndex == numberOfLayers - 1) // for last layer is simple errors
                    {
                        propagatedErrors[layerIndex] = errors;
                    }
                    else
                    {
                        double propagatedError = 0;
                        for (int weightIndex = 0; weightIndex < layers[layerIndex +1].Weights.ColumnCount; weightIndex++)
                        {
                            propagatedError += layers[layerIndex +1].Weights[neuronIndex+biasModifier,weightIndex] * propagatedErrors[layerIndex +1][weightIndex]; //weight * propagated error
                        }
                        propagatedErrors[layerIndex][neuronIndex] = propagatedError;
                    }
                }
            }
            #endregion
            #region adapt weights using propagated error, outputs and derivatives
            for (int layerIndex = numberOfLayers-1; layerIndex >=0; layerIndex--) // begin with the lastlayer
            {
                for (int neuronIndex = 0; neuronIndex < layers[layerIndex].Weights.ColumnCount; neuronIndex++)
                {
                    for(int weightIndex = 0; weightIndex < layers[layerIndex].Weights.RowCount; weightIndex++)
                    {
                        var signal = network.LastOutputs[layerIndex][weightIndex];
                        var currentNeuronError = propagatedErrors[layerIndex][neuronIndex];
                        var activationFunc = network.Layers[layerIndex].ActivationFunction as IDifferentiable;
                        var derivative = network.LastDerivatives[layerIndex][neuronIndex];  
                        var backPropagationImpact = derivative * signal * currentNeuronError * LearningRateHandler.LearningRate;
                        if (currentDataError < previousDataError * MaxErrorIncreaseCoefficient) // accept that step and add momentum modifier
                        {
                            var momentumImpact = MomentumCoefficient * LastWeightsChange[layerIndex][weightIndex, neuronIndex];
                            layers[layerIndex].Weights[weightIndex, neuronIndex] += backPropagationImpact + momentumImpact;
                            LastWeightsChange[layerIndex][weightIndex, neuronIndex] = backPropagationImpact + momentumImpact; //update weights last change stored value
                        }
                        else // ignore momentum
                        {
                        layers[layerIndex].Weights[weightIndex, neuronIndex] += backPropagationImpact;
                        LastWeightsChange[layerIndex][weightIndex, neuronIndex] = backPropagationImpact;
                        }
                    }
                }
            }
            #endregion
        }
    }
}