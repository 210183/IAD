using NeuralNetworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Normalization
{
    interface INormalizator
    {
        void Normalize(IDataProvider dataProvider);
    }
}
