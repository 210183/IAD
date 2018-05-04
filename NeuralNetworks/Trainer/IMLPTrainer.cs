using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    interface IMLPTrainer
    {
        void TrainNetwork(ref NeuralNetwork networkToTrain, int maxEpochs, double desiredErrorRate);
    }
}
