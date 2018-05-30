using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    /// <summary>
    /// This class assumes that input has bias on 0 index, if network is construced with bias.
    /// </summary>
    public class NeuralNetworkRadial
    {
        public static readonly int NumberOfLayers = 2;
        public int NumberOfInputs { get; set; }
        public bool IsBiasExisting { get; }
        public SigmoidLayer OutputLayer { get; set; }
        public RadialLayer RadialLayer { get; set; }
        /// <summary>
        /// Stores outputs for every neuron calculated during last execution of CalculateOutput method.
        /// Input is also stored as first vector (0 index) like output of virtual 'input layer'
        /// </summary>
        public Vector<double>[] LastOutputs { get; set; } = new Vector<double>[1];
        public Vector<double> LastDerivatives { get; set; } = Vector<double>.Build.Dense(1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfInputs">Size of input vector that network will take.</param>
        /// <param name="layersChars">Descripts each layer, one by one, to build network from. Number of layers is determined from this array size. Output layer should be also given here.</param>
        /// <param name="isBiasOn">Unmutable flag.</param>
        public NeuralNetworkRadial(RadialLayer radialLayer, SigmoidLayer outputLayer, bool isBiasOn = true)
        {
            this.NumberOfInputs = outputLayer.Weights.RowCount;
            this.IsBiasExisting = isBiasOn;
            // create layers
            RadialLayer = radialLayer;
            OutputLayer = outputLayer;
            //create place to store outputs
            LastOutputs = new Vector<double>[NumberOfLayers + 1]; // +1 to store input as output for first layer
            //create place to store derivatives
            LastDerivatives = Vector<double>.Build.Dense(OutputLayer.Weights.ColumnCount);
        }
        /// <summary>
        /// Calculates output. Can also calculate and store all neurons outputs and act. function derivatives.
        /// </summary>
        /// <exception cref="ArgumentException">If CalculateDerivatives is on and any Activation Function is not differentiable</exception>
        /// <remarks>
        /// You should usually provide proper mode enum.
        /// </remarks>
        /// <seealso cref="CalculateMode"/>
        /// <param name="input"></param>
        /// <returns>Output Vector</returns>
        public Vector<double> CalculateOutput(Vector<double> input, CalculateMode mode = CalculateMode.NetworkOutput)
        {
            if (mode.HasFlag(CalculateMode.AllOutputs))
            {
                LastOutputs[0] = Vector<double>.Build.DenseOfVector(input); //copy of input
            }
            Vector<double> currentOutput = RadialLayer.CalculateOutput(input);
            if (mode.HasFlag(CalculateMode.AllOutputs))
            {
                LastOutputs[1] = Vector<double>.Build.DenseOfVector(currentOutput); //copy of current
            }
            //calculate output layer
            currentOutput = OutputLayer.CalculateOutput(currentOutput);
            if (mode.HasFlag(CalculateMode.AllOutputs))
            {
                LastOutputs[2] = Vector<double>.Build.DenseOfVector(currentOutput); //copy of current
            }
            return currentOutput;
        }

        //public NeuralNetworkRadial DeepCopy()
        //{
        //    var copyOutputLayer = new SigmoidLayer(OutputLayer.Weights.Clone(), OutputLayer.ActivationFunction, OutputLayer.IsBiased);
        //    var copyNetwork = new NeuralNetworkRadial(NumberOfInputs, copyOutputLayer, IsBiasExisting);
        //    copyNetwork.LastOutputs = new Vector<double>[LastOutputs.Length];
        //    for (int i = 0; i < LastOutputs.Length; i++)
        //    {
        //        copyNetwork.LastOutputs[i] = Vector<double>.Build.DenseOfVector(LastOutputs[i]);
        //    }
        //    copyNetwork.LastDerivatives = Vector<double>.Build.DenseOfVector(LastDerivatives);
        //    return copyNetwork;
        //}
    }
}
