using Microsoft.Win32;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using NeuralNetworks.Networks.NetworkFactory;
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
        private string learnFileName = @"C:\Users\Mateusz\Desktop\approximation_train_1.txt";
        private string testFileName = @"C:\Users\Mateusz\Desktop\approximation_test.txt";

        public IDataProvider DataProvider { get; set; }

        public TaskType? ChosenTaskType { get; set; }
        public bool IsTaskTypeSaved { get; set; } = false;

        public RadialNetworkParameters NetworkParameters { get; set; } = new RadialNetworkParameters();
        public LearningParameters LearningParameters { get; set; } = new LearningParameters();
        public CompleteNetworkCreator Creator { get; set; }
        public NeuralNetworkRadial CurrentNetwork { get; set; }
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

            UnlockParameters();
        }

        private void SetParameters_Click(object sender, RoutedEventArgs e)
        {
            SaveParameters();
            Window paramWindow = new ParametersWindow();
            paramWindow.ShowDialog();
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
                else
                {
                    MessageBox.Show("You have to choose the type of the task first.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            NetworkResultsButton.IsEnabled = true;
        }

        private void NetworkResults_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("You need to use some network creator first");
                }
            }
            else
            {
                MessageBox.Show("You need to train some network first");
            }
        }

        #region helper methods

        private void SaveParameters()
        {
            if(! IsTaskTypeSaved)
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
            else
                return TaskType.None;
        }
        
        private void CreateLearningDataProvider()
        {
            if (ShouldCreateApproximationDataProvider()) // the same provider ffor both
            {
                DataProvider = new LearningApproximationDataProvider(
                    learnFileName,
                    testFileName,
                    NetworkParameters.NumberOfInputs,
                    NetworkParameters.NumberOfOutputNeurons,
                    NetworkParameters.IsBiased);
            }
            else if (ShouldCreateClassificationDataProvider())
            {
                DataProvider = new LearningClassificationDataProvider(
                    learnFileName,
                    testFileName,
                    NetworkParameters.NumberOfInputs,
                    NetworkParameters.NumberOfOutputNeurons,
                    NetworkParameters.IsBiased);
            }
        }
        private void CreateTestDataProvider()
        {
            if (ShouldCreateApproximationDataProvider()) // the same provider ffor both
            {
                DataProvider = new ApproximationDataProvider(
                    testFileName,
                    NetworkParameters.NumberOfInputs,
                    NetworkParameters.NumberOfOutputNeurons,
                    NetworkParameters.IsBiased);
            }
            else if (ShouldCreateClassificationDataProvider())
            {
                DataProvider = new ClassificationDataProvider(
                    testFileName,
                    NetworkParameters.NumberOfInputs,
                    NetworkParameters.NumberOfOutputNeurons,
                    NetworkParameters.IsBiased);
            }
        }
        private bool ShouldCreateApproximationDataProvider()
        {
            if(ChosenTaskType == TaskType.Approximation || ChosenTaskType == TaskType.Transformation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ShouldCreateClassificationDataProvider( )
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

        #endregion

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
    }
}
