using NeuralNetworks.ActivationFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks.NetworkFactory
{
    public class RadialNetworkParameters
    {
        public int NumberOfRadialNeurons { get; set; } = 500;
        public int NumberOfOutputNeurons { get; set; } = 1;
        public IActivationFunction ActivationFunction { get; set; } = new IdentityFunction();
        public int NumberOfInputs { get; set; } = 1;
        public double Min { get; set; } = -1;
        public double Max { get; set; } = 1;
        public bool IsBiased { get; set; } = true;
    }
}
