using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
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

        ILengthCalculator lengthCalculator;
        double startingLearningRate;
        double minimumLearningRate;
        int maxIterations;
        int lambdaMaxIterations;
        double lambdaMax;
        double lambdaMin;
        double conscienceMinPotential;
        int neuronsCounter;

        public SonParametersWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ILengthCalculator lengthCalculator = new EuclideanLength();
            INeighborhoodFunction neighbourhoodFunction = new GaussianNeighborhood();
            startingLearningRate = Convert.ToDouble(LearningStartingRateBox.Text);
            minimumLearningRate = Convert.ToDouble(LearningMinRateBox.Text);
            maxIterations = Convert.ToInt32(MaxIterationsBox.Text);
            lambdaMaxIterations = Convert.ToInt32(LambdaMaxIterationsBox.Text);
            lambdaMax = Convert.ToDouble(LambdaMaxBox.Text);
            lambdaMin = Convert.ToDouble(LambdaMinBox.Text);
            conscienceMinPotential = Convert.ToDouble(MinimalPotentialBox.Text);
            neuronsCounter = Convert.ToInt32(NeuronsCounterBox.Text);

            MainWindow.CurrentNetwork = new NeuralNetwork
            (
               2,
               new LayerCharacteristic[1] { new LayerCharacteristic(neuronsCounter, new IdentityFunction()) },
               false
            );

            if (NeighborhoodfunctionComboBox.SelectedItem == BinaryNeighbourhood)
            {
                neighbourhoodFunction = new BinaryNeighborhood();
            }
            if (NeighborhoodfunctionComboBox.SelectedItem == GaussianNeighbourhood)
            {
                neighbourhoodFunction = new GaussianNeighborhood();
            }

            if (LengthCalculatorComboBox.SelectedItem == Euclidean)
            {
                lengthCalculator = new EuclideanLength();
            }

            if (TaskType.SONKohonen.HasFlag(MainWindow.ChosenTaskType))
            {
                
                MainWindow.LearningAlgorithm = new KohonenAlgorithm
                (
                    lengthCalculator,
                    new Lambda(lambdaMax, lambdaMin, lambdaMaxIterations),
                    neighbourhoodFunction
                );
            }

            // build trainer
            var trainer = new SONTrainer();
            //
            this.Close();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var par = MainWindow.SonParameters;

            ILengthCalculator lengthCalculator = par.LengthCalculator;
            INeighborhoodFunction neighbourhoodFunction = par.NeighbourhoodFunction;
            LearningStartingRateBox.Text = par.StartingLearningRate.ToString();
            LearningMinRateBox.Text = par.MinimumLearningRate.ToString();
            MaxIterationsBox.Text = par.MaxIterations.ToString();
            LambdaMaxIterationsBox.Text = par.LambdaMaxIterations.ToString();
            LambdaMaxBox.Text = par.LambdaMax.ToString();
            LambdaMinBox.Text = par.LambdaMin.ToString();
            MinimalPotentialBox.Text = par.ConscienceMinPotential.ToString();
            NeuronsCounterBox.Text = par.NeuronsCounter.ToString();

            if (lengthCalculator as EuclideanLength != null)
            {
                LengthCalculatorComboBox.SelectedItem = Euclidean;
            }
            if (neighbourhoodFunction as BinaryNeighborhood != null)
            {
                NeighborhoodfunctionComboBox.SelectedItem = BinaryNeighbourhood;
            }
            if (neighbourhoodFunction as GaussianNeighborhood != null)
            {
                NeighborhoodfunctionComboBox.SelectedItem = GaussianNeighbourhood;
            }

        }

    }
}
