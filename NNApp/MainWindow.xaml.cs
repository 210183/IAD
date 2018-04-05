using Microsoft.Win32;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace NNApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties
        private string learnFileName = @"C:\Users\Jakub\Desktop\approximation_train_1.txt";
        private string testFileName = @"C:\Users\Jakub\Desktop\approximation_test.txt";

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

        public IDataProvider DataProvider { get; set; }

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

        public CompleteNetworkCreator Creator { get; set; }
        public NeuralNetwork CurrentNetwork { get; set; }
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
            //create data provider before moving further
            if (TaskChooseComboBox.SelectedIndex > -1) // if any item was chosen 
            {
                if (File.Exists(learnFileName) && File.Exists(testFileName))
                {
                    if (TaskChooseComboBox.SelectedItem == Approximation || TaskChooseComboBox.SelectedItem == Transformation) // the same provider ffor both
                    {
                        DataProvider = new LearningApproximationDataProvider(learnFileName, testFileName, inputsNumber, outputsNumber, isBiasOn);
                    }
                    else if (TaskChooseComboBox.SelectedItem == Classification)
                    {
                        DataProvider = new LearningClassificationDataProvider(learnFileName, testFileName, inputsNumber, outputsNumber, isBiasOn);
                    }
                    Window paramWindow = new TrainerParametersWindow();
                    paramWindow.ShowDialog();
                } //create learning provdier
                else if (File.Exists(testFileName))
                {
                    if (TaskChooseComboBox.SelectedItem == Approximation || TaskChooseComboBox.SelectedItem == Transformation) // the same provider ffor both
                    {
                        DataProvider = new ApproximationDataProvider(learnFileName , inputsNumber, outputsNumber, isBiasOn);
                    }
                    else if (TaskChooseComboBox.SelectedItem == Classification)
                    {
                        DataProvider = new ClassificationDataProvider(learnFileName, inputsNumber, outputsNumber, isBiasOn);
                    }
                    Window paramWindow = new TrainerParametersWindow();
                    paramWindow.ShowDialog();
                }   //create provider for test only purposes
                else
                {
                    MessageBox.Show("Could not find any files under specified paths.");
                }
            }
            else
            {
                MessageBox.Show("You have to choose the type of the task first.");
            }
 
        }
    }
}
