using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.Data;

namespace NeuralNetworks.Normalization
{
    public class EuclideanNormalizator : INormalizator
    {
        public void Normalize(Datum[] dataSet)
        {
            for (int i = 0; i < dataSet.Length; i++)
            {
                dataSet[i].X = dataSet[i].X / (Math.Sqrt(dataSet[i].X.PointwisePower(2).Sum())); // x= x/Sqrt(E(x^2))
            }
        }
    }
}
