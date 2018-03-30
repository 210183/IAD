using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    /// <summary>
    /// This class assumes that input has bias in input given.
    /// </summary>
    class NeuralNetwork
    {

        public int NumberOfInputs { get; set; }
        public int NumberOfLayers { get; set; }
        internal Layer[] Layers { get; set; }

        public NeuralNetwork(int numberOfLayers, int numberOfInputs, LayerCharacteristic[] layersChars  )
        {
            this.NumberOfLayers = numberOfLayers;
            this.NumberOfInputs = numberOfInputs;
            int numberOfNeurons;
            Layers[0] = new Layer(Matrix<double>.Build.Dense(NumberOfInputs, layersChars[0].NumberOfNeurons));
            for (int i=1; i< numberOfLayers; i++)
            {
                numberOfNeurons = layersChars[i].NumberOfNeurons;
                Layers[i] = new Layer(Matrix<double>.Build.Dense(Layers[i-1].Weights.ColumnCount + 1, numberOfNeurons)); // +1 becuase of bias weights.
            }
        }
    }
}
