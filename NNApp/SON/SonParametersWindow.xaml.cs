﻿using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning;
using NeuralNetworks.Learning.MLP;
using NeuralNetworks.Learning.NeighborhoodFunctions;
using NeuralNetworks.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for SonParametersWindow.xaml
    /// </summary>
    public partial class SonParametersWindow : Window
    {
        public MainWindow MainWindow { get; set; } = ((MainWindow)Application.Current.MainWindow);
        SonParameters sonParameters;

        public SonParametersWindow()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            sonParameters.LengthCalculator = new EuclideanLength();
            sonParameters.NeighbourhoodFunction = new GaussianNeighborhood();
            sonParameters.StartingLearningRate = Convert.ToDouble(LearningStartingRateBox.Text);
            sonParameters.MinimumLearningRate = Convert.ToDouble(LearningMinRateBox.Text);
            sonParameters.MaxIterations = Convert.ToInt32(MaxIterationsBox.Text);
            sonParameters.LambdaMaxIterations = Convert.ToInt32(LambdaMaxIterationsBox.Text);
            sonParameters.LambdaMax = Convert.ToDouble(LambdaMaxBox.Text);
            sonParameters.LambdaMin = Convert.ToDouble(LambdaMinBox.Text);
            sonParameters.ConscienceMinPotential = Convert.ToDouble(MinimalPotentialBox.Text);
            sonParameters.NeuronsCounter = Convert.ToInt32(NeuronsCounterBox.Text);

            int numberOfInputs = 2;
            if(MainWindow.ChosenTaskType == TaskType.PictureCompression)
            {
                numberOfInputs = CompressionConstants.neuronsInFrame;
                MainWindow.CurrentNetwork = new NeuralNetwork
                (
                   numberOfInputs,
                   new LayerCharacteristic[1] { new LayerCharacteristic(sonParameters.NeuronsCounter, new IdentityFunction()) },
                   false,
                   0, 
                   255
                );
            }
            else
            {
                MainWindow.CurrentNetwork = new NeuralNetwork
                (
                   numberOfInputs,
                   new LayerCharacteristic[1] { new LayerCharacteristic(sonParameters.NeuronsCounter, new IdentityFunction()) },
                   false
                );
            }
            

            if (NeighborhoodfunctionComboBox.SelectedItem == BinaryNeighbourhood)
            {
                sonParameters.NeighbourhoodFunction = new BinaryNeighborhood();
            }
            if (NeighborhoodfunctionComboBox.SelectedItem == GaussianNeighbourhood)
            {
                sonParameters.NeighbourhoodFunction = new GaussianNeighborhood();
            }
            if (LengthCalculatorComboBox.SelectedItem == Euclidean)
            {
                sonParameters.LengthCalculator = new EuclideanLength();
            }

            Lambda lambda = new Lambda(sonParameters.LambdaMax, sonParameters.LambdaMin, sonParameters.LambdaMaxIterations);

            if (MainWindow.ChosenTaskType == TaskType.PictureCompression)
            {
                if (LearningAlgorithmComboBox.SelectedItem == Kohonen)
                {
                    MainWindow.LearningAlgorithm = new KohonenAlgorithm
                    (
                        sonParameters.LengthCalculator,
                        lambda,
                        sonParameters.NeighbourhoodFunction
                    );
                }
                if (LearningAlgorithmComboBox.SelectedItem == NeuralGas)
                {
                    MainWindow.LearningAlgorithm = new GasAlgorithm(sonParameters.LengthCalculator, lambda);
                }
                if (LearningAlgorithmComboBox.SelectedItem == WTA)
                {
                    MainWindow.LearningAlgorithm = new WTAAlgorithm(sonParameters.LengthCalculator);
                }
                //MainWindow.LearningAlgorithm = new GasAlgorithm(sonParameters.LengthCalculator, lambda);
            }
            if (MainWindow.ChosenTaskType == TaskType.Kohonen)
            {
                MainWindow.LearningAlgorithm = new KohonenAlgorithm
                (
                    sonParameters.LengthCalculator,
                    lambda,
                    sonParameters.NeighbourhoodFunction
                );
            }
            if(MainWindow.ChosenTaskType == TaskType.Gas)
            {
                MainWindow.LearningAlgorithm = new GasAlgorithm(sonParameters.LengthCalculator, lambda);                   
            }
            if (MainWindow.ChosenTaskType == TaskType.WTA)
            {
                MainWindow.LearningAlgorithm = new WTAAlgorithm(sonParameters.LengthCalculator);
            }
            MainWindow.Observer = new LearningHistoryObserver();
            if (MainWindow.ChosenTaskType == TaskType.KMeans)
            {
                MainWindow.Trainer = new KMeansTrainer
                (
                (IDataProvider)MainWindow.DataProvider,
                MainWindow.CurrentNetwork,
                sonParameters.LengthCalculator,
                MainWindow.Observer
                );
            }
            else if (TaskType.AnySON.HasFlag(MainWindow.ChosenTaskType))
            {
                MainWindow.Trainer = new SONTrainer
               (
                (IDataProvider)MainWindow.DataProvider,
                MainWindow.CurrentNetwork,
                (SONLearningAlgorithm)MainWindow.LearningAlgorithm,
                new SONLearningRateHandler(sonParameters.StartingLearningRate, sonParameters.MinimumLearningRate, sonParameters.MaxIterations),
                sonParameters.LengthCalculator,
                new ConscienceWithPotential(sonParameters.ConscienceMinPotential, sonParameters.NeuronsCounter, ((IDataProvider)MainWindow.DataProvider).Points.Length * 4),
                MainWindow.Observer
               );
            }

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sonParameters = MainWindow.SonParameters;

            LearningStartingRateBox.Text = sonParameters.StartingLearningRate.ToString();
            LearningMinRateBox.Text = sonParameters.MinimumLearningRate.ToString();
            MaxIterationsBox.Text = sonParameters.MaxIterations.ToString();
            LambdaMaxIterationsBox.Text = sonParameters.LambdaMaxIterations.ToString();
            LambdaMaxBox.Text = sonParameters.LambdaMax.ToString();
            LambdaMinBox.Text = sonParameters.LambdaMin.ToString();
            MinimalPotentialBox.Text = sonParameters.ConscienceMinPotential.ToString();
            NeuronsCounterBox.Text = sonParameters.NeuronsCounter.ToString();

            if (sonParameters.LengthCalculator as EuclideanLength != null)
            {
                LengthCalculatorComboBox.SelectedItem = Euclidean;
            }
            if (sonParameters.NeighbourhoodFunction as BinaryNeighborhood != null)
            {
                NeighborhoodfunctionComboBox.SelectedItem = BinaryNeighbourhood;
            }
            if (sonParameters.NeighbourhoodFunction as GaussianNeighborhood != null)
            {
                NeighborhoodfunctionComboBox.SelectedItem = GaussianNeighbourhood;
            }
            if (MainWindow.ChosenTaskType == TaskType.PictureCompression)
            {
                LearningAlgorithmText.Visibility = Visibility.Visible;
                LearningAlgorithmComboBox.Visibility = Visibility.Visible;
            }

        }
        private void TopBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            MainWindow.WindowState = WindowState.Minimized;
        }

    }
}
