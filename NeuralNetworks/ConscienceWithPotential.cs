using MathNet.Numerics.LinearAlgebra;
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
        private Dictionary<int, double> potentials;
        private int maxIterations;
        public ConscienceWithPotential(double minimalPotential, int neuronAmount, int maxIterations)
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
            if (maxIterations <= 0)
            {
                throw new ArgumentException("Invalid iterations value: " + maxIterations.ToString());
            }
            this.maxIterations = maxIterations;
            potentials = new Dictionary<int, double>();
            for (int i = 0; i < neuronAmount; i++) // init dictionary
            {
                potentials.Add(i, minimalPotential);
            }
        }

        public void FilterPossibleWinners(ref List<int> neuronsIndexes, int iterationNumber)
        {
            if (iterationNumber > maxIterations)
            {
                return;
            }
            else
            {
                var possWinners = new List<int>();
                foreach (var neuronIndex in neuronsIndexes)
                {
                    if (CanBeWinner(neuronIndex))
                        possWinners.Add(neuronIndex);
                }
                if(possWinners.Count != 0)
                {
                    neuronsIndexes = possWinners;
                }
            }
        }

        private bool CanBeWinner(int neuronIndex)
        {
            if(neuronIndex >= neuronAmount)
            {
                throw new ArgumentException("Invalid neuron index >= than amount: " + neuronIndex.ToString() + " >= " + neuronAmount.ToString());
            }
            return potentials[neuronIndex] >= minimalPotential;
        }

        public void UpdatePotential(int winnerindex)
        {
            for (int i = 0; i < neuronAmount; i++)
            {
                if(i == winnerindex)
                {
                    potentials[i] -= minimalPotential;
                    if (potentials[i] < 0)
                        potentials[i] = 0;
                }
                else
                {
                    potentials[i] += 1 / (double)neuronAmount;
                    if (potentials[i] > 1)
                        potentials[i] = 1;
                }
            }
        }
    }
}
