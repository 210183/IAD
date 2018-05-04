using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public interface IBasicDataProvider
    {
        void ShuffleDataSet(DatumWithD[] set, int shuffleNumber = 100);
    }
}
