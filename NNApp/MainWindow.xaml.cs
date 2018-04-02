using Microsoft.Win32;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using OxyPlot;
using OxyPlot.Series;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties
        private string learnFileName;
        private string testFileName;

        private int inputsNumber;
        private int outputsNumber;
        private bool isBiasOn;
        private LayerCharacteristic[] layers;

        
        private double learningRate;
        private double reductionRate;
        private double increaseRate;
        private double maxErrorIncreaseRate;
        private double momentum;
        private double errorIncreaseCoefficient;
        private IErrorCalculator errorCalculator;
        private int maxEpochs;
        private double desiredMaxError;
        private LearningAlgorithm learningAlgorithm;

        public int InputsNumber { get => inputsNumber; set => inputsNumber = value; }
        public int OutputsNumber { get => outputsNumber; set => outputsNumber = value; }
        public bool IsBiasOn { get => isBiasOn; set => isBiasOn = value; }

        public LayerCharacteristic[] Layers { get => layers; set => layers = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }
        public double ReductionRate { get => reductionRate; set => reductionRate = value; }
        public double IncreaseRate { get => increaseRate; set => increaseRate = value; }
        public double MaxErrorIncreaseRate { get => maxErrorIncreaseRate; set => maxErrorIncreaseRate = value; }
        public double Momentum { get => momentum; set => momentum = value; }
        public double ErrorIncreaseCoefficient { get => errorIncreaseCoefficient; set => errorIncreaseCoefficient = value; }

        public LearningAlgorithm LearningAlgorithm { get => learningAlgorithm; set => learningAlgorithm = value; }
        public IErrorCalculator ErrorCalculator { get => errorCalculator; set => errorCalculator = value; }
        public int MaxEpochs { get => maxEpochs; set => maxEpochs = value; }
        public double DesiredMaxError { get => desiredMaxError; set => desiredMaxError = value; }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

        }

        private void DataFilesButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Learn file";
            if (openFileDialog.ShowDialog() == true)
                learnFileName = openFileDialog.FileName;
            openFileDialog.Title = "Test file";
            if (openFileDialog.ShowDialog() == true)
                testFileName = openFileDialog.FileName;

        }

        private void SetParameters_Click(object sender, RoutedEventArgs e)
        {
            Window paramWindow = new ParametersWindow();
            paramWindow.ShowDialog();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            Window paramWindow = new TrainerParametersWindow();
            paramWindow.ShowDialog();

        }
    }
}
