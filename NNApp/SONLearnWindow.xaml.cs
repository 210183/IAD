using NeuralNetworks;
using NeuralNetworks.Trainer;
using OxyPlot;
using OxyPlot.Series;
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
    /// Interaction logic for SONLearnWindow.xaml
    /// </summary>
    public partial class SONLearnWindow : Window
    {
        public SONTrainer Trainer { get; set; }
        public List<PlotModel> PlotModels { get; set; } = new List<PlotModel>();
        public int PlotModelIndex { get; set; } = 0;

        private NeuralNetwork network;

        public SONLearnWindow(SONTrainer trainer, NeuralNetwork network)
        {
            InitializeComponent();
            Trainer = trainer;
            this.network = network;
            AddPlotModel();
        }

        private void AddPlotModel()
        {
            var network = Trainer.NetworkStatesHistory[PlotModelIndex];
            var weights = network.Layers[0].Weights;
            var SONModel = new PlotModel
            {
                Title = "SON Number: " + PlotModelIndex.ToString(),
                IsLegendVisible = true,
            };
            #region neurons series
            var neuronPoints = new List<ScatterPoint>();
            for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
            {
                neuronPoints.Add(new ScatterPoint(
                    weights[0, neuronIndex], //x
                    weights[1, neuronIndex], //y
                    2                        //point size
                    ));
            }
            var neuronSeries = new ScatterSeries()
            {
                Title = "Neurons",
            };
            neuronSeries.ItemsSource = neuronPoints;
            SONModel.Series.Add(neuronSeries);
            #endregion
            #region learning data series
            var learningPoints = new List<ScatterPoint>();
            for (int dataIndex = 0; dataIndex < Trainer.DataSet.Length; dataIndex++)
            {
                learningPoints.Add(new ScatterPoint(
                    Trainer.DataSet[dataIndex].X[0], //x
                    Trainer.DataSet[dataIndex].X[1], //y
                    1                        //point size
                    ));
            }
            var learningSeries = new ScatterSeries()
            {
                Title = "Learning Data",
            };
            learningSeries.ItemsSource = learningPoints;
            SONModel.Series.Add(learningSeries);
            #endregion
            // add completed plot model
            PlotModels.Add(SONModel);
            SONMainPlot.Model = PlotModels[PlotModelIndex]; // change currently viewed plot
            PlotModelIndex++;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Nextbutton_Click(object sender, RoutedEventArgs e)
        {
            Trainer.TrainNetwork(ref network, Convert.ToInt32(HowMuchDataInStepBox.Text));
            AddPlotModel();
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
        }
    }
}
