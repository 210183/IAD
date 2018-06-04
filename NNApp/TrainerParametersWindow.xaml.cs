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
        public LearningParameters LearningParameters { get; set; } = ((MainWindow)Application.Current.MainWindow).LearningParameters;
        public MainWindow MainWindow { get; set; } = ((MainWindow)Application.Current.MainWindow);
        public TrainerParametersWindow()
        {
            InitializeComponent();
            LearningRateBox.Text = LearningParameters.LearningRate.ToString();
            ReductionRateBox.Text = LearningParameters.ReductionRate.ToString();
            IncreaseRateBox.Text = LearningParameters.IncreaseRate.ToString();
            MaxErrorIncreaseRateBox.Text = LearningParameters.MaxErrorIncreaseRate.ToString();
            LearningRateBox.Text = LearningParameters.LearningRate.ToString();
            NumberOfNetworksBox.Text = LearningParameters.NumberOfNetworksToTry.ToString();
            MomentumBox.Text = LearningParameters.Momentum.ToString();
            ErrorIncreaseCoefficientBox.Text = LearningParameters.ErrorIncreaseCoefficient.ToString();
            MaxEpochsBox.Text = LearningParameters.MaxEpochs.ToString();
            NumberOfNetworksBox.Text = LearningParameters.NumberOfNetworksToTry.ToString();
            DesiredMaxErrorBox.Text = LearningParameters.DesiredMaxError.ToString();
            NeighboursCountBox.Text = LearningParameters.WidthModifierAdjuster.CountedNeighbours.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            LearningParameters.LearningRate = Convert.ToDouble(LearningRateBox.Text);
            LearningParameters.LearningRate = Convert.ToDouble(LearningRateBox.Text);
            LearningParameters.ReductionRate = Convert.ToDouble(ReductionRateBox.Text);
            LearningParameters.IncreaseRate = Convert.ToDouble(IncreaseRateBox.Text);
            LearningParameters.MaxErrorIncreaseRate = Convert.ToDouble(MaxErrorIncreaseRateBox.Text);
            LearningParameters.Momentum = Convert.ToDouble(MomentumBox.Text);
            LearningParameters.ErrorIncreaseCoefficient = Convert.ToDouble(ErrorIncreaseCoefficientBox.Text);
            LearningParameters.WidthModifierAdjuster = new WidthModifierAdjuster(Convert.ToInt32(NeighboursCountBox.Text));
;           LearningParameters.NumberOfNetworksToTry = Convert.ToInt32(NumberOfNetworksBox.Text);
            LearningParameters.MaxEpochs = Convert.ToInt32(MaxEpochsBox.Text);
            LearningParameters.DesiredMaxError = Convert.ToDouble(DesiredMaxErrorBox.Text);

            LearningRateHandler tempHandler = new LearningRateHandler(Convert.ToDouble(LearningRateBox.Text), Convert.ToDouble(ReductionRateBox.Text), Convert.ToDouble(IncreaseRateBox.Text), Convert.ToDouble(MaxErrorIncreaseRateBox.Text));

            if (LearningAlgorithmComboBox.Text == "Back Propagation")
            {
                LearningParameters.LearningAlgorithm = new BackPropagationRadialAlgorithm(tempHandler, Convert.ToDouble(MomentumBox.Text), Convert.ToDouble(ErrorIncreaseCoefficientBox.Text));
            }
            if (ErrorCalculatorComboBox.Text == "Mean Square Error")
            {
                LearningParameters.ErrorCalculator = new MeanSquareErrorCalculator();
            }
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.DataProvider is ILearningProvider)
            {
                var creator = new CompleteNetworkCreator()
                {
                    DataProvider = MainWindow.DataProvider as ILearningProvider,
                    WidthModifierAdjuster = LearningParameters.WidthModifierAdjuster,
                    DesiredError = LearningParameters.DesiredMaxError,
                    ErrorCalculator = LearningParameters.ErrorCalculator,
                    NetworkParameters = MainWindow.NetworkParameters,
                    LearningAlgorithm = LearningParameters.LearningAlgorithm,
                    MaxEpochs = LearningParameters.MaxEpochs,
                };
                var network = creator.CreateNetwork( (TaskType)MainWindow.ChosenTaskType, LearningParameters.NumberOfNetworksToTry);
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
