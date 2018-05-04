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
        public void Normalize(IDataProvider dataProvider)
        {
            var set = dataProvider.Points;
            foreach(var p in set)
            {
                p.X = p.X / (Math.Sqrt(p.X.PointwisePower(2).Sum())); // x= x/Sqrt(E(x^2))
            }
        }
    }
}
