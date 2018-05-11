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

            potentials = new Dictionary<int, double>();
            for (int i = 0; i < neuronAmount; i++) // init dictionary
            {
                potentials.Add(i, minimalPotential);
            }
        }

        public List<int> SelectPossibleWinners(Matrix<double> weights)
        {
            var possWinners = new List<int>();
            for (int col = 0; col < weights.ColumnCount; col++)
            {
                if (potentials[col] >= minimalPotential)
                    possWinners.Add(col);
            }
            return possWinners;
        }

        public bool CanBeWinner(int neuronIndex)
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
