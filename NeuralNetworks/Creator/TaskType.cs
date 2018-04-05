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
    public enum TaskType
    {
        None = 0,
        Approximation,
        Classification,
        Transformation
    }
}
