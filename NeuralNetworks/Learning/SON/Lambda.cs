using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Networks
{
    public class Lambda
    {
        private double max;
        private double min;
        private int maxIterations;

        public Lambda(double max, double min, int maxIterations)
        {
            if (min > max)
                throw new ArgumentException("Min cannot be greater than max");
            if (maxIterations <= 0)
                throw new ArgumentException("Iteration max number cannot be negative or zero");
            this.max = max;
            this.min = min;
            this.maxIterations = maxIterations;
        }
        public double GetValue(int iterationNumber)
        {
            double iterationCoef = iterationNumber <= maxIterations ? (iterationNumber / maxIterations) : 1;
            return max * Math.Pow((min / max), iterationCoef);
        }
    }
}
