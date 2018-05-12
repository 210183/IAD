using NeuralNetworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Trainer
{
    public interface IOnGoingTrainer
    {
        Datum[] DataSet { get; }
        void TrainNetwork(ref NeuralNetwork networkToTrain, int dataCount);
    }
}
