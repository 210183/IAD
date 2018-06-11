using NeuralNetworks;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Learning;
using NeuralNetworks.Networks;
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
            MinLearningRateBox.Text = LearningParameters.MinLearningRate.ToString();
            MaxLearningRateBox.Text = LearningParameters.MaxLearningRate.ToString();
            //Momentum
            MomentumBox.Text = LearningParameters.Momentum.ToString();
            ErrorIncreaseCoefficientBox.Text = LearningParameters.ErrorIncreaseCoefficient.ToString();
            //SON
            NeighboursCountBox.Text = LearningParameters.WidthModifierAdjuster.CountedNeighbours.ToString();
            MinPotentialBox.Text = LearningParameters.MinimalPotential.ToString();
            LambdaMinBox.Text = LearningParameters.MinLambda.ToString();
            LambdaMaxBox.Text = LearningParameters.MaxLambda.ToString();
            LambdaIterationsBox.Text = LearningParameters.LambdaIterations.ToString();
            MinPotentialBox.Text = LearningParameters.MinimalPotential.ToString();
            //trainer
            NumberOfNetworksBox.Text = LearningParameters.NumberOfNetworksToTry.ToString();
            MaxEpochsBox.Text = LearningParameters.MaxEpochs.ToString();
            NumberOfNetworksBox.Text = LearningParameters.NumberOfNetworksToTry.ToString();
            DesiredMaxErrorBox.Text = LearningParameters.DesiredMaxError.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(NeighboursCountBox.Text) >= MainWindow.NetworkParameters.NumberOfRadialNeurons)
                {
                    throw new ArgumentException("Neighbour count cannot be greater than or equal neurons count");
                }
                LearningParameters.MinLearningRate = Convert.ToDouble(MinLearningRateBox.Text);
                LearningParameters.MaxLearningRate = Convert.ToDouble(MaxLearningRateBox.Text);

                LearningParameters.Momentum = Convert.ToDouble(MomentumBox.Text);
                LearningParameters.ErrorIncreaseCoefficient = Convert.ToDouble(ErrorIncreaseCoefficientBox.Text);
                //SON
                LearningParameters.MaxLambda = Convert.ToDouble(LambdaMaxBox.Text);
                LearningParameters.MinLambda = Convert.ToDouble(LambdaMinBox.Text);
                LearningParameters.LambdaIterations = Convert.ToInt32(LambdaIterationsBox.Text);
                LearningParameters.MinimalPotential = Convert.ToDouble(MinPotentialBox.Text);
                LearningParameters.WidthModifierAdjuster = new WidthModifierAdjuster(Convert.ToInt32(NeighboursCountBox.Text));
                //trainer
                LearningParameters.NumberOfNetworksToTry = Convert.ToInt32(NumberOfNetworksBox.Text);
                LearningParameters.MaxEpochs = Convert.ToInt32(MaxEpochsBox.Text);
                LearningParameters.DesiredMaxError = Convert.ToDouble(DesiredMaxErrorBox.Text);
                LearningParameters.IterationsNumber = LearningParameters.MaxEpochs * (MainWindow.DataProvider as ILearningProvider).LearnSet.Length;

                CreateComplexParameters();

                if (LearningAlgorithmComboBox.Text == "Back Propagation")
                {
                    LearningParameters.LearningAlgorithm = new BackPropagationRadialAlgorithm(Convert.ToDouble(MomentumBox.Text), Convert.ToDouble(ErrorIncreaseCoefficientBox.Text));
                }
                if (ErrorCalculatorComboBox.Text == "Mean Square Error")
                {
                    LearningParameters.ErrorCalculator = new MeanSquareErrorCalculator();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //creator methods
        private void CreateComplexParameters()
        {
            CreateLearningRateHandler();
            CreateLambda();
            CreateConscience();
            CreateSONLearningAlgorithm();
            CreateCenterAdapter();
        }
        private void CreateLearningRateHandler()
        {
            LearningParameters.LearningRateHandler = new LearningRateHandler(LearningParameters.MaxLearningRate,
                                                                      LearningParameters.MinLearningRate,
                                                                      LearningParameters.IterationsNumber);
        }
        private void CreateConscience()
        {
            int iterationsNumber = 3 * (MainWindow.DataProvider as ILearningProvider).LearnSet.Length;
            LearningParameters.Conscience = new ConscienceWithPotential(LearningParameters.MinimalPotential,
                                                        MainWindow.NetworkParameters.NumberOfRadialNeurons,
                                                        iterationsNumber);
        }
        private void CreateLambda()
        {
            LearningParameters.Lambda = new Lambda(LearningParameters.MaxLambda,
                                                   LearningParameters.MinLambda,
                                                   LearningParameters.LambdaIterations);
        }
        private void CreateSONLearningAlgorithm()
        {
            LearningParameters.SONLearningAlgorithm = new GasAlgorithm(LearningParameters.Lambda);
        }
        private void CreateCenterAdapter()
        {
            LearningParameters.CenterAdapter = new SONAdapter(LearningParameters.SONLearningAlgorithm,
                                                              LearningParameters.LearningRateHandler,
                                                              new EuclideanLength(),
                                                              LearningParameters.Conscience);
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.DataProvider is ILearningProvider)
                {
                    var creator = new CompleteNetworkCreator()
                    {
                        DataProvider = MainWindow.DataProvider as ILearningProvider,
                        WidthModifierAdjuster = LearningParameters.WidthModifierAdjuster,
                        ErrorCalculator = LearningParameters.ErrorCalculator,
                        NetworkParameters = MainWindow.NetworkParameters,
                        LearningParameters = MainWindow.LearningParameters
                    };
                    var network = creator.CreateNetwork((TaskType)MainWindow.ChosenTaskType, LearningParameters.NumberOfNetworksToTry);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void NumberOfNetworksBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
