﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.DataGenerators
{
    interface IDataGenerator
    {
        void GenerateData(int numberOfPoints, string fileToSaveData);
    }
}
