using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.NeighborhoodFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNApp
{
    public class SonParameters
    {
        public int NeuronsCounter { get; set; } = 300;
        public double ConscienceMinPotential { get; set; } = 0.75;
        public double LambdaMin { get; set; } = 0.1;
        public double LambdaMax { get; set; } = 0.2;
        public int LambdaMaxIterations { get; set; } = 20;
        public int MaxIterations { get; set; } = 800;
        public double MinimumLearningRate { get; set; } = 0.1;
        public double StartingLearningRate { get; set; } = 0.2;
        public INeighborhoodFunction NeighbourhoodFunction { get; set; } = new GaussianNeighborhood();
        public ILengthCalculator LengthCalculator { get; set; } = new EuclideanLength();
    }
}
