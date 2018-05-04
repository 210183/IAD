using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public class PointsDataProvider : BasicDataProvider, IDataProvider
    {
        public Datum[] Points { get; set; }
    }
}
