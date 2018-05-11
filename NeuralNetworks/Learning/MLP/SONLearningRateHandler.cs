using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning.MLP
{
    public class SONLearningRateHandler
    {
        private double startingLearningRate;
        private double minLearningRate;
        private int maxIterations;

        public SONLearningRateHandler(double startingLearningRate, double minLearningRate, int maxIterations)
        {
            this.startingLearningRate = startingLearningRate;
            this.minLearningRate = minLearningRate;
            this.maxIterations = maxIterations;
        }

        public double GetLearningRate(int iterationNumber)
        {
            return startingLearningRate * Math.Pow((minLearningRate / startingLearningRate), (iterationNumber / maxIterations));
        }
    }
}
