﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using OxyPlot;
using OxyPlot.Series;

namespace NeuralNetworkApp
{
    public class MainViewModel
    {
        public PlotModel MyModel { get; private set; }

        public MainViewModel()
        {
  
        }

    }
}
