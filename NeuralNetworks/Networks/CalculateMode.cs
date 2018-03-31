using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks
{
    [Flags]
    public enum CalculateMode
    {
        /// <summary>
        /// Just network output
        /// </summary>
        NetworkOutput = 1,
        /// <summary>
        /// Store all neuron outputs
        /// </summary>
        AllOutputs = 2,
        /// <summary>
        /// Calc and store derivatives
        /// </summary>
        Derivatives = 4,
        OutputsAndDerivatives = AllOutputs | Derivatives,
    }
}
