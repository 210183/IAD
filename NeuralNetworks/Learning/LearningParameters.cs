using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public class LearningParameters
    {
        public double LearningRate { get; set; } = 0.01;
        public double ReductionRate { get; set; } = 0.8;
        public double IncreaseRate { get; set; } = 1.1;
        public double MaxErrorIncreaseRate { get; set; } = 1.04;
        public double Momentum { get; set; } = 0.7;
        public double ErrorIncreaseCoefficient { get; set; } = 1.04;
        //public int NeigboursCount { get; set; } = 3;
        public int MaxEpochs { get; set; } = 150;
        public double DesiredMaxError { get; set; } = 0;
        public int NumberOfNetworksToTry { get; set; } = 1;
        public LearningAlgorithm LearningAlgorithm { get; set; }
        public IErrorCalculator ErrorCalculator { get; set; } = new MeanSquareErrorCalculator();
        public WidthModifierAdjuster WidthModifierAdjuster { get; set; } = new WidthModifierAdjuster(3);

    }
}
