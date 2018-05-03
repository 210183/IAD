using System;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworksTests
{
    [TestClass]
    public class NetworkTests
    {
        [TestMethod]
        public void OutputWithoutBiasTest()
        {
            var layers = new LayerCharacteristic[2]
            {
                new LayerCharacteristic(2, new SigmoidUnipolarFunction()),
                new LayerCharacteristic(1, new IdentityFunction())
            };
            var net = new NeuralNetwork(1, layers, false);
            var input = Vector<double>.Build.Dense(1);
            input[0] = 2;
            net.Layers[0].Weights[0, 0] = 1.5;
            net.Layers[0].Weights[0, 1] = 1 / 4.0;
            net.Layers[1].Weights[0, 0] = 1 / 4.0;
            net.Layers[1].Weights[1, 0] = 2;
            var output = net.CalculateOutput(input);
            Assert.AreEqual(1.483, Math.Round(output[0], 3));
        }
        [TestMethod]
        public void OutputWithBiasTest()
        {
            var layers = new LayerCharacteristic[2]
            {
                new LayerCharacteristic(2, new SigmoidUnipolarFunction()),
                new LayerCharacteristic(1, new IdentityFunction())
            };
            var net = new NeuralNetwork(1, layers, true);
            var input = Vector<double>.Build.Dense(2);
            input[0] = 1;
            input[1] = 2;
            net.Layers[0].Weights[0, 0] = -1 / 2.0;
            net.Layers[0].Weights[1, 0] = 1.5;
            net.Layers[0].Weights[0, 1] = 1 / 4.0;
            net.Layers[0].Weights[1, 1] = -1;
            net.Layers[1].Weights[0, 0] = 1.5;
            net.Layers[1].Weights[1, 0] = 1 / 4.0;
            net.Layers[1].Weights[2, 0] = 2;
            var output = net.CalculateOutput(input);
            Assert.AreEqual(0.527 + 1.5, Math.Round(output[0],3));
        }
    }
}
