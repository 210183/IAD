using NeuralNetworks;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
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
    /// Interaction logic for TreinerParametersWindow.xaml
    /// </summary>
    public partial class TrainerParametersWindow : Window
    {
        public TrainerParametersWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {


            ((MainWindow)Application.Current.MainWindow).LearningRate = Convert.ToDouble(LearningRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).ReductionRate = Convert.ToDouble(ReductionRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).IncreaseRate = Convert.ToDouble(IncreaseRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).MaxErrorIncreaseRate = Convert.ToDouble(MaxErrorIncreaseRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).Momentum = Convert.ToDouble(MomentumBox.Text);
            ((MainWindow)Application.Current.MainWindow).ErrorIncreaseCoefficient = Convert.ToDouble(ErrorIncreaseCoefficientBox.Text);

            ((MainWindow)Application.Current.MainWindow).MaxEpochs = Convert.ToInt32(MaxEpochsBox.Text);
            ((MainWindow)Application.Current.MainWindow).DesiredMaxError = Convert.ToDouble(DesiredMaxErrorBox.Text);

            LearningRateHandler tempHandler = new LearningRateHandler(Convert.ToDouble(LearningRateBox.Text), Convert.ToDouble(ReductionRateBox.Text), Convert.ToDouble(IncreaseRateBox.Text), Convert.ToDouble(MaxErrorIncreaseRateBox.Text));

            if (LearningAlgorithmComboBox.Text == "Back Propagation")
                ((MainWindow)Application.Current.MainWindow).LearningAlgorithm = new BackPropagationAlgorithm(tempHandler, Convert.ToDouble(MomentumBox.Text), Convert.ToDouble(ErrorIncreaseCoefficientBox.Text));
            if (ErrorCalculatorComboBox.Text == "Mean Square Error")
                ((MainWindow)Application.Current.MainWindow).ErrorCalculator = new MeanSquareErrorCalculator();
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            if ( ((MainWindow)Application.Current.MainWindow).Layers != null)
            {
                if (((MainWindow)Application.Current.MainWindow).DataProvider is ILearningProvider)
                {
                    var creator = new CompleteNetworkCreator()
                    {
                        DataProvider = ((MainWindow)Application.Current.MainWindow).DataProvider as ILearningProvider,
                        DesiredError = ((MainWindow)Application.Current.MainWindow).DesiredMaxError,
                        ErrorCalculator = ((MainWindow)Application.Current.MainWindow).ErrorCalculator,
                        InputsNumber = ((MainWindow)Application.Current.MainWindow).InputsNumber,
                        IsBiasOn = ((MainWindow)Application.Current.MainWindow).IsBiasOn,
                        Layers = ((MainWindow)Application.Current.MainWindow).Layers,
                        LearningAlgorithm = ((MainWindow)Application.Current.MainWindow).LearningAlgorithm,
                        MaxEpochs = ((MainWindow)Application.Current.MainWindow).MaxEpochs,
                    };
                    var network = creator.CreateNetwork(GetChosenTaskType(), Convert.ToInt32(NumberOfNetworksBox.Text));
                    ((MainWindow)Application.Current.MainWindow).Creator = creator;
                    ((MainWindow)Application.Current.MainWindow).CurrentNetwork = network;
                }
                else
                {
                    MessageBox.Show("Cannot start training without data for learning and testing. Setup data sources first.");
                }
            }
            else
            {
                MessageBox.Show("Specify network layers first.");
            }
            this.Close();
        }
        private TaskType GetChosenTaskType()
        {
            var window = ((MainWindow)Application.Current.MainWindow);
            if (window.TaskChooseComboBox.SelectedItem == window.Approximation)
            {
                return TaskType.Approximation;
            }
            else if (window.TaskChooseComboBox.SelectedItem == window.Classification)
            {
                return TaskType.Classification;
            }
            else if (window.TaskChooseComboBox.SelectedItem == window.Transformation)
            {
                return TaskType.Transformation;
            }
            else
            {
                return TaskType.None;
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
            ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
        }

        private void NumberOfNetworksBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
