using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class BackPropagationAlgorithm : LearningAlgorithm
    {
        public BackPropagationAlgorithm(double learningRate)
        {
            this.LearningRate = learningRate;
        }
        public override void AdaptWeights(NeuralNetwork network, Vector<double> errors)
        {
            /* dla kazdej warstwy od konca
             *      dla kazdego neuronu
             *          dla kazdej wagi
             *          policz jego delta wagi (skladowa gradientu razy wspl uczenia)
             * dodaj delty do wag
             */
            #region helper variables
            int numberOfLayers = network.Layers.Count();
            var layers = network.Layers;

            #endregion
            #region propagate errors
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
                            propagatedError += layers[layerIndex +1].Weights[neuronIndex, weightIndex] * propagatedErrors[layerIndex +1][weightIndex]; //weight * propagated error
                        }
                        propagatedErrors[layerIndex][neuronIndex] = propagatedError;
                    }
                }
            }
            #endregion
            for (int layerIndex= numberOfLayers-1; layerIndex >=0; layerIndex--) // begin with the lastlayer
            {
                for (int neuronIndex = 0; neuronIndex < layers[layerIndex].Weights.ColumnCount; neuronIndex++)
                {
                    for(int weightIndex=0; weightIndex < layers[layerIndex].Weights.RowCount; weightIndex++)
                    {
                        var derivative = network.LastDerivatives[layerIndex][neuronIndex];
                        var signal = network.LastOutputs[layerIndex][weightIndex];
                        var currentError = propagatedErrors[layerIndex][neuronIndex];
                        layers[layerIndex].Weights[weightIndex, neuronIndex] += derivative * signal * currentError * LearningRate;
                    }
                }
            }
        }
    }
}
