﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public interface IDataProvider
    {
        Datum[] DataSet { get; set; }
        void ShuffleDataSet(Datum[] set, int shuffleNumber = 100);
    }
}
