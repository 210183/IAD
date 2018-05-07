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
        public LearningRateHandler(double learningRate, double reductionRate, double increaseRate, double maxErrorIncreaseRate)
        {
            LearningRate = learningRate;
            ReductionRate = reductionRate;
            IncreaseRate = increaseRate;
            MaxErrorIncreaseRate = maxErrorIncreaseRate;
        }

        public double LearningRate { get; set; }
        public double ReductionRate { get; set; }
        public double IncreaseRate { get; set; }
        public double MaxErrorIncreaseRate {get; set;}

        public void UpdateRate(double currentError, double previousError)
        {
            if (currentError >= MaxErrorIncreaseRate*previousError)
            {
                 LearningRate *= ReductionRate;
            }
            else
            {
                 LearningRate *= IncreaseRate;
            }
        }
    }
}
