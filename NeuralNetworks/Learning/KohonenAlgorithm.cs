using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning.MLP;
using NeuralNetworks.Learning.NeighborhoodFunctions;

namespace NeuralNetworks.Learning
{
    public class KohonenAlgorithm : SONLearningAlgorithm
    {
        public KohonenAlgorithm(
            ILengthCalculator lengthCalculator,
            Lambda lambda,
            INeighborhoodFunction neighborhoodFunction
            ) : base(lengthCalculator, lambda)
        {
            NeighborhoodFunction = neighborhoodFunction;
        }

        public INeighborhoodFunction NeighborhoodFunction { get; }
        public ConscienceWithPotential Conscience { get;}

        public override Dictionary<int, double> GetCoefficients(NeuralNetwork network, List<int> possibleNeurons, int winnerIndex, Vector<double> learningPoint, int iterationNumber)
        {
            var weights = network.Layers[0].Weights;

            // find neighbors
            var winner = weights.Column(winnerIndex);
            var neighborsGrade = new Dictionary<int, double>();
            var lambda = Lambda.GetValue(iterationNumber);
            if (lambda == 0)
            {
                neighborsGrade.Add(winnerIndex, 1);
            }
            else
            {
                for (int colIndex = 0; colIndex < weights.ColumnCount; colIndex++) // winner also will be included
                {
                    var colDistance = LengthCalculator.Distance(weights.Column(colIndex), winner);
                    neighborsGrade.Add(colIndex, NeighborhoodFunction.Calculate(colDistance, lambda));
                }
            }
            return neighborsGrade.Where(x => x.Value > 0.0001).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
