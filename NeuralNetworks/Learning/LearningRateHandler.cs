using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    /// <summary>
    /// Stores learning rate. 
    /// Can adapt learning rate depending on error value change. If increasing more than acceptable rate, reduce by multiplying by reduction rate. Else increase similarly.
    /// </summary>
    public class LearningRateHandler
    {
        private double startingLearningRate;
        private double minLearningRate;
        private int maxIterations;

        public LearningRateHandler(double startingLearningRate, double minLearningRate, int maxIterations)
        {
            this.startingLearningRate = startingLearningRate;
            this.minLearningRate = minLearningRate;
            this.maxIterations = maxIterations;
        }

        public double GetLearningRate(int iterationNumber)
        {
            return startingLearningRate * Math.Pow((minLearningRate / startingLearningRate), (iterationNumber * 1.0 / maxIterations));
        }
    }
}
