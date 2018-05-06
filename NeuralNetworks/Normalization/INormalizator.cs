using NeuralNetworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Normalization
{
    public interface INormalizator
    {
        void Normalize(Datum[] dataProvider);
    }
}
