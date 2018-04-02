using NeuralNetworks;
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
    public partial class TreinerParametersWindow : Window
    {
        public TreinerParametersWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        
            ((MainWindow)Application.Current.MainWindow).LearningRate = Convert.ToInt32(LearningRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).ReductionRate = Convert.ToInt32(ReductionRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).IncreaseRate = Convert.ToInt32(IncreaseRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).MaxErrorIncreaseRate = Convert.ToInt32(MaxErrorIncreaseRateBox.Text);
            ((MainWindow)Application.Current.MainWindow).Momentum = Convert.ToInt32(MomentumBox.Text);
            ((MainWindow)Application.Current.MainWindow).ErrorIncreaseCoefficient = Convert.ToInt32(ErrorIncreaseCoefficientBox.Text);

            ((MainWindow)Application.Current.MainWindow).MaxEpochs = Convert.ToInt32(MaxEpochsBox.Text);
            ((MainWindow)Application.Current.MainWindow).DesiredMaxError = Convert.ToInt32(DesiredMaxErrorBox.Text);

            LearningRateHandler tempHandler = new LearningRateHandler(Convert.ToInt32(LearningRateBox.Text), Convert.ToInt32(ReductionRateBox.Text), Convert.ToInt32(IncreaseRateBox.Text), Convert.ToInt32(MaxErrorIncreaseRateBox.Text));

            //if (LearningAlgorithmComboBox.Text == "Back Propagation")
            //((MainWindow)Application.Current.MainWindow).LearningAlgorithm = new BackPropagationAlgorithm();
            if (ErrorCalculatorComboBox.Text == "Mean Square Error")
                ((MainWindow)Application.Current.MainWindow).ErrorCalculator = new MeanSquareErrorCalculator();
        }
    }
}
