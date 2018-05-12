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
        Kohonen = 8,
        Gas = 16,
        WTA = 32,
        PictureCompression = 64,
        KMeans = 128,
        AnySON = Kohonen | Gas | WTA | KMeans,
        AnyMLP = Approximation | Classification | Transformation
    }
}
