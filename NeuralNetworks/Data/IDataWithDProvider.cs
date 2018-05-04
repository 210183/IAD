using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public interface IDataWithDProvider
    {
       DatumWithD[] DataSet { get; set; }
    }
}
