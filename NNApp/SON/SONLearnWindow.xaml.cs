using NeuralNetworks;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Error;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for SONLearnWindow.xaml
    /// </summary>
    public partial class SONLearnWindow : Window
    {
        public IOnGoingTrainer Trainer { get; set; }
        public LearningHistoryObserver Observer { get; set; }
        public List<PlotModel> PlotModels { get; set; } = new List<PlotModel>();
        public int GeneratedPlotIndex { get; set; } = 0;
        public int DisplayedPlotIndex { get; set; } = 0;

        public List<List<ScatterPoint>> NeuronsData { get; set; } = new List<List<ScatterPoint>>();

        public ScatterSeries NeuronsSeries { get; set; }
        public ScatterSeries LearningSeries { get; set; }
        public QuantizationCalculator QuantizationCalculator { get; set; }

        private NeuralNetwork network;

        public BackgroundWorker AnimationWorker { get; set; }

        public SONLearnWindow(IOnGoingTrainer trainer, NeuralNetwork network, ILengthCalculator lengthCalculator, LearningHistoryObserver observer)
        {
            InitializeComponent();
            Trainer = trainer;
            Observer = observer;
            QuantizationCalculator = new QuantizationCalculator(lengthCalculator);
            this.network = network;
            CreatePlot();
            AddPlotModel();
            UpdateControlNumbers();
        }

        public void CreatePlot()
        {
            var SONModel = new PlotModel
            {
                Title = "SON Number: " + GeneratedPlotIndex.ToString(),
                IsLegendVisible = true,
            };

            NeuronsSeries = new ScatterSeries()
            {
                Title = "Neurons",
            };

            LearningSeries = new ScatterSeries()
            {
                Title = "Learning Data",
            };

            var learningPoints = new List<ScatterPoint>();
            for (int dataIndex = 0; dataIndex < Trainer.DataSet.Length; dataIndex++)
            {
                learningPoints.Add(new ScatterPoint(
                    Trainer.DataSet[dataIndex].X[0], //x
                    Trainer.DataSet[dataIndex].X[1], //y
                    1                        //point size
                    ));
            }
            LearningSeries.ItemsSource = learningPoints;

            SONModel.Series.Add(NeuronsSeries);
            SONModel.Series.Add(LearningSeries);

            SONMainPlot.Model = SONModel;
        }

        private void AddPlotModel()
        {
            int trainedDataCount = Observer.NetworkStatesHistory.Count();
            while (GeneratedPlotIndex < trainedDataCount)
            {
                var network = Observer.NetworkStatesHistory[GeneratedPlotIndex];
                var weights = network.Layers[0].Weights;
                
                var neuronPoints = new List<ScatterPoint>();
                for (int neuronIndex = 0; neuronIndex < weights.ColumnCount; neuronIndex++)
                {
                    neuronPoints.Add(new ScatterPoint(
                        weights[0, neuronIndex], //x
                        weights[1, neuronIndex], //y
                        2                        //point size
                        ));
                }

                NeuronsData.Add(neuronPoints);
        
                GeneratedPlotIndex++;
            }
            DisplayedPlotIndex = GeneratedPlotIndex;
            UpdateDisplayedPlot();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            int previousAmount = Convert.ToInt32(HowMuchDataInStepBox.Text);
            if (DisplayedPlotIndex - previousAmount >=0)
            {
                UpdateDisplayedPlot(DisplayedPlotIndex - previousAmount);
            }
            else
            {
                UpdateDisplayedPlot(1);
            }
            UpdateControlNumbers();
        }
        private void Nextbutton_Click(object sender, RoutedEventArgs e)
        {
            int nextAmount = Convert.ToInt32(HowMuchDataInStepBox.Text);
            if (DisplayedPlotIndex + nextAmount < GeneratedPlotIndex)
            {
                UpdateDisplayedPlot(DisplayedPlotIndex + nextAmount);
            }
            else
            {
                int indexDifference = GeneratedPlotIndex - DisplayedPlotIndex;
                UpdateDisplayedPlot(DisplayedPlotIndex + indexDifference);
                int toLearnCount = nextAmount - indexDifference; // how many time sshould still learn
                Trainer.TrainNetwork(ref network, toLearnCount);
                AddPlotModel();
            }
            UpdateControlNumbers();
            CalculateErrorButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void UpdateDisplayedPlot()
        {
            NeuronsSeries.ItemsSource = NeuronsData[DisplayedPlotIndex - 1];
            SONMainPlot.Model.Title = "SON Number: " + DisplayedPlotIndex.ToString();
            SONMainPlot.Model.InvalidatePlot(true);
        }
        private void UpdateDisplayedPlot(int index)
        {
            if (index <= 1) // more safe
                index = 1;
            DisplayedPlotIndex = index;
            NeuronsSeries.ItemsSource = NeuronsData[DisplayedPlotIndex - 1];
            SONMainPlot.Model.Title = "SON Number: " + DisplayedPlotIndex.ToString();
            SONMainPlot.Model.InvalidatePlot(true);
        }

        private void UpdateControlNumbers()
        {
            CurrentEpochNumber.Text = (DisplayedPlotIndex / Trainer.DataSet.Length).ToString();//Trainer.EpochNumber.ToString();
            EpochSizeNumber.Text = Trainer.DataSet.Length.ToString();
            CurrentDataNumber.Text = (DisplayedPlotIndex-(Convert.ToInt32(CurrentEpochNumber.Text) * Convert.ToInt32(EpochSizeNumber.Text))).ToString();
        }
        #region WindowService
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
        #endregion

        private void CalculateErrorButton_Click(object sender, RoutedEventArgs e)
        {
            double error = QuantizationCalculator.CalculateError(network, Trainer.DataSet);
            ErrorTextBox.Text = error.ToString("e6");
        }

        private void AnimationButton_Click(object sender, RoutedEventArgs e)
        {
            AnimationWorker = new BackgroundWorker();
            AnimationWorker.DoWork += AnimationWorker_DoWork;
            AnimationWorker.WorkerReportsProgress = true;
            AnimationWorker.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
            AnimationWorker.ProgressChanged += AnimationWorker_ProgressChanged;
            AnimationWorker.RunWorkerAsync();
        }

        private void AnimationWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (DisplayedPlotIndex < GeneratedPlotIndex)
            {
                DisplayedPlotIndex++;
                Thread.Sleep(120);
                AnimationWorker.ReportProgress(0);
            }
            AnimationWorker.ReportProgress(100);
        }
        private void AnimationWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                UpdateDisplayedPlot();
            }
            else if(e.ProgressPercentage == 100)
            {
                AnimationWorker.CancelAsync();
            }
        }

    }
}
