﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public class LearningApproximationDataProvider : ApproximationDataProvider, ILearningProvider
    {

        public Datum[] LearnSet { get; set; }

        public LearningApproximationDataProvider(string learnFileName, string testFileName, int inputsNumber, int outputsNumber, bool isBiasOn) : base(testFileName, inputsNumber, outputsNumber, isBiasOn)
        {
            LearnSet = SetData(learnFileName, inputsNumber, outputsNumber, isBiasOn);
            
        }
    
    }
}
