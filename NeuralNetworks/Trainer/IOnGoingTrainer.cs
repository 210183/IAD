using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Trainer
{
    interface IOnGoingTrainer
    {
        void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount, bool shouldStoreNetworks = true);
    }
}
