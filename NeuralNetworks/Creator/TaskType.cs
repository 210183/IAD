using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    /// <summary>
    /// Signalizes which type of task should network be able to solve
    /// </summary>
    [Flags]
    public enum TaskType
    {
        None = 0,
        Approximation = 1,
        Classification = 2,
        Transformation = 4,
        SONKohonen = 8,
        SONGas = 16,
        SONWTA = 32,
        AnySON = SONKohonen | SONGas | SONWTA
    }
}
