using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class ConscienceWithPotential
    {
        private double minimalPotential;
        private int neuronAmount;
        private Dictionary<int, double> neuronPotential;
        public ConscienceWithPotential(double minimalPotential, int neuronAmount)
        {
            if(minimalPotential > 1 || minimalPotential < 0)
            {
                throw new ArgumentException("Invalid potential value: " + minimalPotential.ToString());
            }
            this.minimalPotential = minimalPotential;
            if (neuronAmount <= 0 )
            {
                throw new ArgumentException("Invalid amount value: " + neuronAmount.ToString());
            }
            this.neuronAmount = neuronAmount;

            neuronPotential = new Dictionary<int, double>();
            for (int i = 0; i < neuronAmount; i++) // init dictionary
            {
                neuronPotential.Add(i, minimalPotential);
            }
        }

        public bool CanBeWinner(int neuronIndex)
        {
            if(neuronIndex >= neuronAmount)
            {
                throw new ArgumentException("Invalid neuron index >= than amount: " + neuronIndex.ToString() + " >= " + neuronAmount.ToString());
            }
            return neuronPotential[neuronIndex] >= minimalPotential;
        }

        public void UpdatePotential(int winnerindex)
        {
            for (int i = 0; i < neuronAmount; i++)
            {
                if(i == winnerindex)
                {
                    neuronPotential[i] -= minimalPotential;
                    if (neuronPotential[i] < 0)
                        neuronPotential[i] = 0;
                }
                else
                {
                    neuronPotential[i] += 1 / (double)neuronAmount;
                    if (neuronPotential[i] > 1)
                        neuronPotential[i] = 1;
                }
            }
        }
    }
}
