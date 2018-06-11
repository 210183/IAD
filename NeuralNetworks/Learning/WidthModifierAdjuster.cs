using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Learning
{
    public class WidthModifierAdjuster
    {
        public WidthModifierAdjuster(int countedNeighbours)
        {
            if(countedNeighbours < 0)
            {
                throw new ArgumentException($"Counted neighbours count hasto be greater than 0, not: {countedNeighbours}");
            }
            CountedNeighbours = countedNeighbours;
        }

        public int CountedNeighbours { get; private set; } = 2;
        public void AdapthWidthModifier(NeuralNetworkRadial network)
        {
            if(CountedNeighbours == 0)
            {
                return;
            }
            // adapt width modifier (lambda)
            var neurons = network.RadialLayer.Neurons;
            var lengthCalculator = new EuclideanLength();
            if (CountedNeighbours > neurons.Length)
            {
                throw new ArgumentOutOfRangeException("K cannot be greater than number of neurons.");
            }
            foreach (var neuron in neurons)
            {
                //find k neighbours
                RadialNeuron[] neighbours = new RadialNeuron[CountedNeighbours];
                Array.Copy(neurons, neighbours, CountedNeighbours); // at the beginning assume that first neurons are closest neighbors.
                for (int i = CountedNeighbours; i < neurons.Length; i++)
                {
                    if (neuron != neurons[i])
                    {
                        for (int j = 0; j < neighbours.Length; j++)
                        {
                            if (lengthCalculator.Distance(neurons[i].Center, neuron.Center) < lengthCalculator.Distance(neighbours[j].Center, neuron.Center))
                            {
                                neighbours[j] = neurons[i];
                            }
                        }
                    }
                }
                //adapt width using neighbours
                double distanceSum = 0;
                for (int i = 0; i < neighbours.Length; i++)
                {
                    distanceSum += lengthCalculator.Distance(neuron.Center, neighbours[i].Center);
                }
                neuron.WidthModifier = Math.Sqrt((distanceSum) * (1.0 / CountedNeighbours));
            }
        }
    }
}
