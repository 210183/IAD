using System.Globalization;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Data;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace NeuralNetworks.Data
{
    public abstract class DataWithDProvider : BasicDataProvider, IDataWithDProvider
    {

        public DatumWithD[] DataSet { get; set; }

    }
}