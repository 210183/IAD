using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    /// <summary>
    /// Constructs neural networks to solve given problem. Creates many networks and choose best.
    /// Stores building parameters and history, so that user can view them.
    /// </summary>
    class CompleteNetworkCreator
    {

       
        public LearningAlgorithm LearningAlgorithm { get; set; }
       
        double learningRate,
        double reductionRate,
        double increaseRate,
        double maxErrorIncreaseRate
        double momentum,
        double errorIncreaseCoefficient
        public IErrorCalculator ErrorCalculator { get; set; }
        public int MaxEpochs { get; set; }
        public double DesiredMaxError { get; set; }
    }
}
