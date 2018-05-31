using NeuralNetworks;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using NeuralNetworks.Networks.NetworkFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        public MainWindow MainWindow { get; set; } = ((MainWindow)Application.Current.MainWindow);

        public TrainerParametersWindow()
        {
            InitializeComponent();
            LearningRateBox.Text = MainWindow.LearningRate.ToString();
            ReductionRateBox.Text = MainWindow.ReductionRate.ToString();
            IncreaseRateBox.Text = MainWindow.IncreaseRate.ToString();
            MaxErrorIncreaseRateBox.Text = MainWindow.MaxErrorIncreaseRate.ToString();
            LearningRateBox.Text = MainWindow.LearningRate.ToString();
            NumberOfNetworksBox.Text = MainWindow.NumberOfNetworksToTry.ToString();
            MomentumBox.Text = MainWindow.Momentum.ToString();
            ErrorIncreaseCoefficientBox.Text = MainWindow.ErrorIncreaseCoefficient.ToString();
            MaxEpochsBox.Text = MainWindow.MaxEpochs.ToString();
            NumberOfNetworksBox.Text = MainWindow.NumberOfNetworksToTry.ToString();
            DesiredMaxErrorBox.Text = MainWindow.DesiredMaxError.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LearningRate = Convert.ToDouble(LearningRateBox.Text);
            MainWindow.LearningRate = Convert.ToDouble(LearningRateBox.Text);
            MainWindow.ReductionRate = Convert.ToDouble(ReductionRateBox.Text);
            MainWindow.IncreaseRate = Convert.ToDouble(IncreaseRateBox.Text);
            MainWindow.MaxErrorIncreaseRate = Convert.ToDouble(MaxErrorIncreaseRateBox.Text);
            MainWindow.Momentum = Convert.ToDouble(MomentumBox.Text);
            MainWindow.ErrorIncreaseCoefficient = Convert.ToDouble(ErrorIncreaseCoefficientBox.Text);
            MainWindow.NumberOfNetworksToTry = Convert.ToInt32(NumberOfNetworksBox.Text);
            MainWindow.MaxEpochs = Convert.ToInt32(MaxEpochsBox.Text);
            MainWindow.DesiredMaxError = Convert.ToDouble(DesiredMaxErrorBox.Text);

            LearningRateHandler tempHandler = new LearningRateHandler(Convert.ToDouble(LearningRateBox.Text), Convert.ToDouble(ReductionRateBox.Text), Convert.ToDouble(IncreaseRateBox.Text), Convert.ToDouble(MaxErrorIncreaseRateBox.Text));

            if (LearningAlgorithmComboBox.Text == "Back Propagation")
            {
                MainWindow.LearningAlgorithm = new BackPropagationRadialAlgorithm(tempHandler, Convert.ToDouble(MomentumBox.Text), Convert.ToDouble(ErrorIncreaseCoefficientBox.Text));
            }
            if (ErrorCalculatorComboBox.Text == "Mean Square Error")
            {
                MainWindow.ErrorCalculator = new MeanSquareErrorCalculator();
            }
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.DataProvider is ILearningProvider)
            {
                var creator = new CompleteNetworkCreator()
                {
                    DataProvider = MainWindow.DataProvider as ILearningProvider,
                    DesiredError = MainWindow.DesiredMaxError,
                    ErrorCalculator = MainWindow.ErrorCalculator,
                    NetworkParameters = MainWindow.NetworkParameters,
                    LearningAlgorithm = MainWindow.LearningAlgorithm,
                    MaxEpochs = MainWindow.MaxEpochs,
                };
                var network = creator.CreateNetwork( (TaskType)MainWindow.ChosenTaskType, MainWindow.NumberOfNetworksToTry);
                MainWindow.Creator = creator;
                MainWindow.CurrentNetwork = network;
            }
            else
            {
                MessageBox.Show("Cannot start training without data for learning and testing. Setup data sources first.");
            }
            SystemSounds.Beep.Play();
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

        private void NumberOfNetworksBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
