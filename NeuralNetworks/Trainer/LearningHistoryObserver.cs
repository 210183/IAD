using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Trainer
{
    public class LearningHistoryObserver
    {
        public List<NeuralNetwork> NetworkStatesHistory { get; set; } = new List<NeuralNetwork>();

        public void SaveNetworkState(NeuralNetwork network)
        {
            NetworkStatesHistory.Add(network.DeepCopy());
        }
    }
}
