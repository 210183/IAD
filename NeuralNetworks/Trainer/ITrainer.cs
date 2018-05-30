﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    interface ITrainer
    {
        void TrainNetwork(ref NeuralNetworkRadial networkToTrain, int maxEpochs, double desiredErrorRate);
    }
}
