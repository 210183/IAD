using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;

namespace NeuralNetworks.Normalization
{
    public class MinMaxNormalizator : INormalizator
    {
        public MinMaxNormalizator(int max, int min)
        {
            this.max = max;
            this.min = min;
        }
        public void Normalize(Datum[] dataProvider)
        {
            var xValues = dataProvider.Select(x => x.X[0]);
            var xMax = xValues.Max();
            var yValues = dataProvider.Select(x => x.X[1]);
            var yMax = xValues.Max();
            var xMin = 0;    //TODO: replace this assumption with calculated value
            var yMin = 0;    //TODO: replace this assumption with calculated value
            foreach (var vector in dataProvider.Select(x => x.X))
            {
                vector[0] = ((vector[0] - xMin) / (xMax - xMin)) * (max - min) + min;
                vector[1] = ((vector[1] - yMin) / (yMax - yMin)) * (max - min) + min;
            }
        }
        private int min;
        private int max;
    }
}
