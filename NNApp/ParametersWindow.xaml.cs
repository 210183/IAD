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
        private MainWindow MainWindow;
        public ParametersWindow()
        {
            InitializeComponent();
            MainWindow = ((MainWindow)Application.Current.MainWindow);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NetworkParameters.NumberOfInputs = Convert.ToInt16(NumberOfInputsBox.Text);
            MainWindow.NetworkParameters.NumberOfOutputNeurons = Convert.ToInt16(NumberOfOutputsBox.Text);
            MainWindow.NetworkParameters.NumberOfRadialNeurons = Convert.ToInt16(NumberOfRadialNeuronsBox.Text);
            if (ActivationFunctionComboBox.SelectionBoxItem.ToString() == "SigmoidUnipolar")
                MainWindow.NetworkParameters.ActivationFunction = new SigmoidUnipolarFunction();
            else if (ActivationFunctionComboBox.SelectionBoxItem.ToString() == "SigmoidBipolar")
                MainWindow.NetworkParameters.ActivationFunction = new SigmoidBipolarFunction();
            else if (ActivationFunctionComboBox.SelectionBoxItem.ToString() == "IdentityFunction")
                MainWindow.NetworkParameters.ActivationFunction = new IdentityFunction();

            if (BiasComboBox.SelectionBoxItem.ToString().Equals("true"))
                MainWindow.NetworkParameters.IsBiased = true;
            else if (BiasComboBox.SelectionBoxItem.ToString().Equals("false"))
                MainWindow.NetworkParameters.IsBiased = false;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NumberOfInputsBox.Text = MainWindow.NetworkParameters.NumberOfInputs.ToString();
            NumberOfOutputsBox.Text = MainWindow.NetworkParameters.NumberOfOutputNeurons.ToString();
            NumberOfRadialNeuronsBox.Text = MainWindow.NetworkParameters.NumberOfRadialNeurons.ToString();
            if (MainWindow.NetworkParameters.IsBiased)
                BiasComboBox.SelectedIndex = 0;
            else
                BiasComboBox.SelectedIndex = 1;
        }
    }
}
