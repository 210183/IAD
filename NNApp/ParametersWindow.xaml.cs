using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
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
    /// Interaction logic for ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        public string Test{ get; set; }
        private int currentLayer = 0;
        private LayerCharacteristic[] layers;


        public ParametersWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            ((MainWindow)Application.Current.MainWindow).InputsNumber = Convert.ToInt16(NumberOfInputsBox.Text);
            ((MainWindow)Application.Current.MainWindow).OutputsNumber = Convert.ToInt16(NumberOfOutputsBox.Text);

            if (BiasComboBox.SelectionBoxItem.ToString().Equals("true"))
                ((MainWindow)Application.Current.MainWindow).IsBiasOn = true;
            else 
                if (BiasComboBox.SelectionBoxItem.ToString().Equals("false"))
                    ((MainWindow)Application.Current.MainWindow).IsBiasOn = false;

            ((MainWindow)Application.Current.MainWindow).Layers = layers;


        }

        private void AddLayer_Click(object sender, RoutedEventArgs e)
        {
            if (currentLayer < layers.Length)
            {
                if (ActivationFunctionComboBox.SelectionBoxItem.ToString() == "SigmoidUnipolar")
                    layers[currentLayer] = new LayerCharacteristic(Convert.ToInt32(CurrentLayerNeuronsBox.Text), new SigmoidUnipolarFunction());
                else if (ActivationFunctionComboBox.SelectionBoxItem.ToString() == "IdentityFunction")
                    layers[currentLayer] = new LayerCharacteristic(Convert.ToInt32(CurrentLayerNeuronsBox.Text), new IdentityFunction());

                CurrentLayerNumber.Text = (currentLayer+1).ToString();
                currentLayer++;
            }
            else
                MessageBox.Show("All layers already have description");
        }

        private void AddLayerBase_Click(object sender, RoutedEventArgs e)
        {

            int numberOfLayers = Convert.ToInt32(NumberOfLayersBox.Text);
            NumberOfLayersBox.IsEnabled = false;

            layers = new LayerCharacteristic[numberOfLayers];
        }
    }
}
