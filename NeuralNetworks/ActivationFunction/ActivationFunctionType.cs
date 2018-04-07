using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public enum ActivationFunctionType
    {
        Identity = 1,
        SigmoidUnipolar = 2,
        SigmoidBipolar = 4,
        AnySigmoid = SigmoidUnipolar | SigmoidBipolar
    }
}
