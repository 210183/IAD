using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks
{
    public class RadialNeuron
    {
        public RadialNeuron(double widthModifier, Vector<double> center)
        {
            WidthModifier = widthModifier;
            Center = center ?? throw new ArgumentNullException(nameof(center));
        }

        public double WidthModifier { get; set; }
        public Vector<double> Center { get; set; }
        
    }
}
