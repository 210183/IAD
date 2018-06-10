using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public class LearningParameters
    {
        public int IterationsNumber { get; set; } = 10000;

        public double MinLearningRate { get; set; } = 0.01;
        public double MaxLearningRate { get; set; } = 0.1;
        public LearningRateHandler LearningRateHandler { get; set; }
        // momentum
        public double Momentum { get; set; } = 0.7;
        public double ErrorIncreaseCoefficient { get; set; } = 1.04;
        //SON
        public int NeighboursCount { get; set; } = 3;
        public double MinimalPotential { get; set; } = 0.75;
        public int LambdaIterations { get; set; } = 200;
        public double MinLambda { get; set; } = 0;
        public double MaxLambda { get; set; } = 10;
        public Lambda Lambda { get; set; } = new Lambda(10, 0.01, 10000);
        public ConscienceWithPotential Conscience { get; set; }
        public SONLearningAlgorithm SONLearningAlgorithm { get; set; }
        public SONAdapter CenterAdapter { get; set; }
        //trainer
        public int MaxEpochs { get; set; } = 150;
        public double DesiredMaxError { get; set; } = 0;
        public int NumberOfNetworksToTry { get; set; } = 1;
        public LearningAlgorithm LearningAlgorithm { get; set; }
        public IErrorCalculator ErrorCalculator { get; set; } = new MeanSquareErrorCalculator();
        public WidthModifierAdjuster WidthModifierAdjuster { get; set; } = new WidthModifierAdjuster(3);

    }
}
