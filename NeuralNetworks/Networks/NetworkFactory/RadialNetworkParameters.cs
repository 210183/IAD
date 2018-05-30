using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks.NetworkFactory
{
    public class RadialNetworkParameters
    {
        public int NumberOfRadialNeurons { get; set; }
        public int NumberOfOutputNeurons { get; set; }
        public IActivationFunction ActivationFunction { get; set; }
        public int NumberOfInputs { get; set; }
        public double Min { get; set; } = -1;
        public double Max { get; set; } = 1;
        public bool IsBiased { get; set; } = true;
    }
}
