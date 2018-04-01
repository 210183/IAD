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
    public class NeuralNetwork
    {

        public int NumberOfInputs { get; set; }
        public int NumberOfLayers { get; set; }
        public bool IsBiasExisting { get; }
        public Layer[] Layers { get; set; }
        /// <summary>
        /// Stores outputs for every neuron calculated during last execution of CalculateOutput method.
        /// Input is also stored as first vector (0 index) like output of virtual 'input layer'
        /// </summary>
        public Vector<double>[] LastOutputs { get; set; } = new Vector<double>[1];
        public Vector<double>[] LastDerivatives { get; set; } = new Vector<double>[1];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfInputs">Size of input vector that network will take.</param>
        /// <param name="layersChars">Descripts each layer, one by one, to build network from. Number of layers is determined from this array size. Output layer should be also given here.</param>
        /// <param name="isBiasOn">Unmutable flag.</param>
        public NeuralNetwork(int numberOfInputs, LayerCharacteristic[] layersChars, bool isBiasOn = true)
        {
            this.NumberOfLayers = layersChars.Length;
            this.Layers = new Layer[NumberOfLayers];
            this.NumberOfInputs = numberOfInputs;
            this.IsBiasExisting = isBiasOn;
            Random randomizer = new Random();
            int inputNumberModifier;
            if (isBiasOn)
                inputNumberModifier = 1;
            else
                inputNumberModifier = 0;
            // create layers
            Layers[0] = new Layer(Matrix<double>.Build.Dense(NumberOfInputs + inputNumberModifier, layersChars[0].NumberOfNeurons, (x,y) => randomizer.NextDouble()), layersChars[0].ActivationFunction); // input layer
            for (int i=1; i< NumberOfLayers; i++) 
            {
                int numberOfNeurons = layersChars[i].NumberOfNeurons;
                int inputsAmount = Layers[i - 1].Weights.ColumnCount + inputNumberModifier;
                Layers[i] = new Layer(Matrix<double>.Build.Dense(inputsAmount, numberOfNeurons, (x, y) => randomizer.NextDouble()), layersChars[i].ActivationFunction); // +1 becuase of bias weights.
            }
            //create place to store outputs
            LastOutputs = new Vector<double>[NumberOfLayers + 1]; // +1 to store input as output for first layer
            //create place to store derivatives
            LastDerivatives = new Vector<double>[NumberOfLayers];
            for (int layerIndex = 0; layerIndex < NumberOfLayers; layerIndex++)
            {
                LastDerivatives[layerIndex] = Vector<double>.Build.Dense(Layers[layerIndex].Weights.ColumnCount);
            }
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
                LastOutputs[0] = Vector<double>.Build.DenseOfVector(input); //copy of
            }
            Vector<double> neuronSums;
            Vector<double> output = Vector<double>.Build.Dense(1); //initial unused value
            for (int layerIndex=0; layerIndex < NumberOfLayers; layerIndex++)
            {
                neuronSums = (input.ToRowMatrix() * Layers[layerIndex].Weights).Row(0);
                int biasModifier = 0;
                if (layerIndex != NumberOfLayers - 1 && IsBiasExisting )
                {
                    biasModifier = 1;
                }
                output = Vector<double>.Build.Dense(neuronSums.Count() + biasModifier);
                output[0] = 1; // if biasModifier is 0, that would be overwritten
                //calculate activation functions
                for (int outputIndex = biasModifier; outputIndex < neuronSums.Count() + biasModifier; outputIndex++)
                {
                    output[outputIndex] = Layers[layerIndex].ActivationFunction.Calculate(neuronSums[outputIndex - biasModifier]);
                    if (mode.HasFlag(CalculateMode.Derivatives))
                    {
                        var differentiableFunction = Layers[layerIndex].ActivationFunction as IDifferentiable;
                        LastDerivatives[layerIndex][outputIndex - biasModifier] = differentiableFunction.CalculateDerivative(neuronSums[outputIndex - biasModifier]);
                    }
                }
                input = output;
                if (mode.HasFlag(CalculateMode.AllOutputs))
                    LastOutputs[layerIndex +1 ] = Vector<double>.Build.DenseOfVector(output); //copy of, +1 because first place is occupied by input
            }
            return output;
        }

        public void ConsoleDisplay()
        {
            Console.WriteLine($"Neural network with : {NumberOfLayers} layers, {NumberOfInputs} inputs");
            foreach(var layer in Layers)
            {
                Console.Write(layer.Weights);
                Console.WriteLine("---------------------------------");
            }
        }
    }
}
