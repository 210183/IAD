﻿using Microsoft.Win32;
using NeuralNetworks;
using NeuralNetworks.Data;
using NeuralNetworks.DataGenerators;
using NeuralNetworks.Learning;
using NeuralNetworks.Normalization;
using NeuralNetworks.Trainer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties
        //private string learnFileName = @"C:\Users\Mateusz\Desktop\approximation_train_1.txt";
        //private string testFileName = @"C:\Users\Mateusz\Desktop\approximation_test.txt";

        //private string learnFileName = @"C:\Users\Mateusz\Desktop\classification_train.txt";
        //private string testFileName = @"C:\Users\Mateusz\Desktop\classification_test.txt";

        //private string learnFileName = @"C:\Users\Mateusz\Desktop\transformation.txt";
        //private string testFileName = @"C:\Users\Mateusz\Desktop\transformation.txt";

        private string learnFileName = @"F:\ImagesTests\attract.txt";
        private string testFileName = @"C:\Users\Mateusz\Desktop\approximation_test.txt";

        private int inputsNumber = 4;
        private int outputsNumber = 4;
        private bool isBiasOn = true;
        private LayerCharacteristic[] layers;

        private double learningRate = 0.01;
        private double reductionRate = 0.8;
        private double increaseRate = 1.1;
        private double maxErrorIncreaseRate = 1.04;
        private double momentum = 0.7;
        private double errorIncreaseCoefficient = 1.04;
        private IErrorCalculator errorCalculator;
        private int maxEpochs = 1000;
        private double desiredMaxError = 0;
        private ILearningAlgorithm learningAlgorithm;
        private NeuralNetwork _currentNetwork;

        public IBasicDataProvider DataProvider { get; set; }

        public TaskType? ChosenTaskType { get; set; }
        public bool IsTaskTypeSaved { get; set; } = false;

        public int InputsNumber { get => inputsNumber; set => inputsNumber = value; }
        public int OutputsNumber { get => outputsNumber; set => outputsNumber = value; }
        public bool IsBiasOn { get => isBiasOn; set => isBiasOn = value; }
        public int NumberOfLayers { get; set; } = 2;

        public LayerCharacteristic[] Layers { get => layers; set => layers = value; }
        public double LearningRate { get => learningRate; set => learningRate = value; }
        public double ReductionRate { get => reductionRate; set => reductionRate = value; }
        public double IncreaseRate { get => increaseRate; set => increaseRate = value; }
        public double MaxErrorIncreaseRate { get => maxErrorIncreaseRate; set => maxErrorIncreaseRate = value; }
        public double Momentum { get => momentum; set => momentum = value; }
        public double ErrorIncreaseCoefficient { get => errorIncreaseCoefficient; set => errorIncreaseCoefficient = value; }

        public ILearningAlgorithm LearningAlgorithm { get => learningAlgorithm; set => learningAlgorithm = value; }
        public IErrorCalculator ErrorCalculator { get => errorCalculator; set => errorCalculator = value; }
        public int MaxEpochs { get => maxEpochs; set => maxEpochs = value; }
        public double DesiredMaxError { get => desiredMaxError; set => desiredMaxError = value; }

        public int NumberOfNetworksToTry { get; set; } = 1;
        public CompleteNetworkCreator Creator { get; set; }
        public NeuralNetwork CurrentNetwork { get => _currentNetwork; set => _currentNetwork = value; }

        private int ComboBoxMinIndexWithTest { get; set; } = 0;
        private int ComboBoxMaxIndexWithTest { get; set; } = 2;

        public SonParameters SonParameters { get; set; } = new SonParameters();
        public IOnGoingTrainer Trainer { get; set; }
        public LearningHistoryObserver Observer { get; set; } = new LearningHistoryObserver();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Buttons
        private void DataFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsTestWindowNeeded())
            {
                learnFileName = ShowFileWindow("Learn File", "Text File", "*.txt");
                testFileName = ShowFileWindow("Test File", "Text File", "*.txt");
            }
            else
            {

                learnFileName = ShowFileWindow("Learn File", "Bitmap", "*.bmp");
                if (learnFileName == "")
                {
                    learnFileName = ShowFileWindow("Learn File", "Text File", "*.txt");
                }
            }
        }

        private void SetParameters_Click(object sender, RoutedEventArgs e)
        {
            SaveParameters();
            if (TaskType.AnySON.HasFlag(ChosenTaskType))
            {
                CreateSONDataProvider();
                Window paramWindow = new SonParametersWindow();
                paramWindow.ShowDialog();
            }
            if (TaskType.AnyMLP.HasFlag(ChosenTaskType))
            {
                Window paramWindow = new ParametersWindow();
                paramWindow.ShowDialog();
            }
            if (ChosenTaskType == TaskType.PictureCompression)
            {
                Window paramWindow = new SonParametersWindow();
                paramWindow.ShowDialog();
            }
            LearnButton.IsEnabled = true;
            
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            SaveParameters();
            try
            {
                //create data provider before moving further
                if (ChosenTaskType != null) // if any item was chosen 
                {
                    if (TaskType.AnySON.HasFlag(ChosenTaskType) || ChosenTaskType == TaskType.PictureCompression)
                    {
                        if (ChosenTaskType == TaskType.PictureCompression)
                        {
                            Window compressionWindow = new PictureCompressionWindow();
                            compressionWindow.ShowDialog();
                        }
                        else
                        {
                            if(ChosenTaskType == TaskType.KMeans)
                            {
                                Trainer = new KMeansTrainer((IDataProvider)DataProvider, CurrentNetwork, SonParameters.LengthCalculator, Observer);
                            }
                            Window SONLearnWindow = new SONLearnWindow(Trainer, CurrentNetwork, SonParameters.LengthCalculator, Observer);
                            SONLearnWindow.Show();
                        }
                    }
                    else
                    {
                        
                            if (File.Exists(learnFileName) && File.Exists(testFileName)) //create learning provdier
                            {
                                CreateLearningDataProvider();
                                Window paramWindow = new TrainerParametersWindow();
                                paramWindow.Show();
                            }
                            else if (File.Exists(testFileName))
                            {
                                CreateTestDataProvider();
                                Window paramWindow = new TrainerParametersWindow(); //create provider for test only purposes
                                paramWindow.Show();
                            }
                            else
                            {
                                MessageBox.Show("Could not find any files under specified paths.");
                            }
                        
                    }
                }
                else
                {
                    MessageBox.Show("You have to choose the type of the task first.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            NetworkResultsButton.IsEnabled = true;
        }

        private void NetworkResults_Click(object sender, RoutedEventArgs e)
        {
            if (TaskType.AnySON.HasFlag(ChosenTaskType))
            {
                //TODO: Add SON network Window
            }
            else
            {
                if (CurrentNetwork != null)
                {
                    if (Creator != null)
                    {
                        Window networkWindow = new NetworkStatsWindow();
                        networkWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You need to use some network creater first");
                    }
                }
                else
                {
                    MessageBox.Show("You need to train some network first");
                }
            }
        }
        #endregion

        #region helper methods
        private void SaveParameters()
        {
            if (!IsTaskTypeSaved)
            {
                ChosenTaskType = GetCurrentTaskType();
                IsTaskTypeSaved = true;
                TaskChooseComboBox.IsEnabled = false;
            }
        }

        private void UnlockParameters()
        {
            if (IsTaskTypeSaved)
            {
                ChosenTaskType = null;
                IsTaskTypeSaved = false;
                TaskChooseComboBox.IsEnabled = true;
                LearnButton.IsEnabled = false;
                NetworkResultsButton.IsEnabled = false;
            }
        }

        private TaskType GetCurrentTaskType()
        {
            if (TaskChooseComboBox.SelectedItem == Approximation)
                return TaskType.Approximation;
            else if (TaskChooseComboBox.SelectedItem == Classification)
                return TaskType.Classification;
            else if (TaskChooseComboBox.SelectedItem == Transformation)
                return TaskType.Transformation;
            else if (TaskChooseComboBox.SelectedItem == SONWTA)
                return TaskType.WTA;
            else if (TaskChooseComboBox.SelectedItem == KohonenAlgorithm)
                return TaskType.Kohonen;
            else if (TaskChooseComboBox.SelectedItem == NeuralGas)
                return TaskType.Gas;
            else if (TaskChooseComboBox.SelectedItem == Kmeans)
                return TaskType.KMeans;
            else if (TaskChooseComboBox.SelectedItem == PictureCompression)
                return TaskType.PictureCompression;
            else
                return TaskType.None;
        }

        private void CreateSONDataProvider()
        {
            if(ChosenTaskType == TaskType.PictureCompression)
            {
                DataProvider = new PointsDataProvider(learnFileName, CompressionConstants.neuronsInFrame);
            }
            else
            {
                DataProvider = new PointsDataProvider(learnFileName, 2);
                var normalizer = new MinMaxNormalizator(1, -1);       //new EuclideanNormalizator()    
                normalizer.Normalize(((IDataProvider)DataProvider).Points);
            }
        }
        private void CreateLearningDataProvider()
        {
            if (ShouldCreateApproximationDataProvider()) // the same provider ffor both
            {
                DataProvider = new LearningApproximationDataProvider(learnFileName, testFileName, inputsNumber, outputsNumber, isBiasOn);
            }
            else if (ShouldCreateClassificationDataProvider())
            {
                DataProvider = new LearningClassificationDataProvider(learnFileName, testFileName, inputsNumber, outputsNumber, isBiasOn);
            }
        }
        private void CreateTestDataProvider()
        {
            if (ShouldCreateApproximationDataProvider()) // the same provider ffor both
            {
                DataProvider = new ApproximationDataProvider(learnFileName, inputsNumber, outputsNumber, isBiasOn);
            }
            else if (ShouldCreateClassificationDataProvider())
            {
                DataProvider = new ClassificationDataProvider(learnFileName, inputsNumber, outputsNumber, isBiasOn);
            }
        }
        private bool ShouldCreateApproximationDataProvider()
        {
            if (ChosenTaskType == TaskType.Approximation || ChosenTaskType == TaskType.Transformation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ShouldCreateClassificationDataProvider()
        {
            if (ChosenTaskType == TaskType.Classification)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTestWindowNeeded()
        {
            if (TaskChooseComboBox.SelectedIndex >= ComboBoxMinIndexWithTest && TaskChooseComboBox.SelectedIndex <= ComboBoxMaxIndexWithTest)
                return true;
            else
                return false;
        }

        private string ShowFileWindow(string WindowName, string ExtensionName, string Extension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ExtensionName + " (" + Extension + ")" + "|" + Extension + "|All files (*.*)|*.*"; //"Text files (*.txt)|*.txt" "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = WindowName;
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else
                return "";
        }
        #endregion

        #region Window service
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LearnButton.IsEnabled = false;
            NetworkResultsButton.IsEnabled = false;
        }
        #endregion

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            TaskChooseComboBox.IsEnabled = true;
        }
    }
}
