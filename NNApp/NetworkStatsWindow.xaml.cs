using NeuralNetworks;
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
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Controls.Primitives;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks.Error.Extensions;

namespace NNApp
{
    /// <summary>
    /// Interaction logic for NetworkStatsWindow.xaml
    /// </summary>
    public partial class NetworkStatsWindow : Window
    {
        public NeuralNetwork CurrentNetwork { get; set; } = ((MainWindow)Application.Current.MainWindow).CurrentNetwork;
        public CompleteNetworkCreator Creator { get; set; } = ((MainWindow)Application.Current.MainWindow).Creator;
        public TaskType ChosenTask { get; set; }
        public int SelectedPlotIndex { get; set; } = 0;
        public PlotModel[] PlotModels { get; set; }
        public NetworkStatsWindow(TaskType taskType)
        {
            InitializeComponent();
            ChosenTask = taskType;
            TestErrorTextBlock.Text = Math.Round(Creator.BestTestError).ToString();

            if (taskType == TaskType.Approximation) // number of plots is dependent on type of task
                PlotModels = new PlotModel[2];
            else
                PlotModels = new PlotModel[1];
            #region error of epochs plot (two series - learning and test error) (set on 0 index of PlotModels)
            var epochErrorPlotModel = new PlotModel
            {
                Title = "Error of epoch",
                IsLegendVisible = true,
            };
            //create learniing erro series
            var learningEpochErrorDataPoints = new List<DataPoint>();
            for (int i = 0; i < Creator.BestNetworkEpochHistory.Count; i++) // adding data
            {
                learningEpochErrorDataPoints.Add(new DataPoint(i, Creator.BestNetworkEpochHistory[i]));
            }
            var learningSeries = new LineSeries()
            {
                Title = "Learning set error",
                Smooth = false,
                LineJoin = LineJoin.Bevel,
            };
            learningSeries.ItemsSource = learningEpochErrorDataPoints;
            epochErrorPlotModel.Series.Add(learningSeries);
            //create test error series
            if (Creator.BestNetworkTestHistory != null)
            {
                var testEpochErrorDataPoints = new List<DataPoint>();
                for (int i = 0; i < Creator.BestNetworkTestHistory.Count; i++) // adding data
                {
                    testEpochErrorDataPoints.Add(new DataPoint(i, Creator.BestNetworkTestHistory[i]));
                }
                var testSeries = new LineSeries()
                {
                    Title = "Test set error",
                    Smooth = false,
                    LineJoin = LineJoin.Bevel,
                };
                testSeries.ItemsSource = testEpochErrorDataPoints;
                epochErrorPlotModel.Series.Add(testSeries);
            }
            //set plot
            PlotModels[0] = epochErrorPlotModel;
            #endregion 

            if (taskType == TaskType.Approximation)
            {
                var ApproximationFunctionPlotModel = new PlotModel
                {
                    Title = "Network approximation function",
                    IsLegendVisible = true
                };
                var ApproximationFunctionDataPoints = new List<DataPoint>();
                for (int i = 0; i < Creator.ApproximationFunctionPoints.Count; i++) // adding data
                {
                    ApproximationFunctionDataPoints.Add(new DataPoint(i, Creator.ApproximationFunctionPoints[i]));
                }
                var ApproximationSeries = new LineSeries()
                {
                    Smooth = true,
                    LineJoin = LineJoin.Miter,
                };
                ApproximationSeries.ItemsSource = ApproximationFunctionDataPoints;
                ApproximationFunctionPlotModel.Series.Add(ApproximationSeries);
                PlotModels[1] = ApproximationFunctionPlotModel;
            }
            else if (taskType == TaskType.Classification)
            {
                var classGrid = new UniformGrid
                {
                    Rows = Creator.ClassificationFullResults.RowCount + 1,
                    Columns = Creator.ClassificationFullResults.ColumnCount + 1,                   
                    
                };
                classGrid.Children.Add(new TextBlock() { Text = "Class number", FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center});
                for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++) //ceate column names as class number bolded
                {
                    classGrid.Children.Add(new TextBlock() { Text = columnIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center });
                }
                for (int rowIndex = 0; rowIndex < Creator.ClassificationFullResults.RowCount; rowIndex++)
                {
                    classGrid.Children.Add(new TextBlock() { Text = rowIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center }); //add row name = class number bolded
                    for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++)
                    {
                        classGrid.Children.Add(new TextBlock() { Text = Creator.ClassificationFullResults[rowIndex, columnIndex].ToString(), VerticalAlignment = System.Windows.VerticalAlignment.Center });
                    }
                }
                NetworkStatsMainGrid.Children.Add(classGrid);
                Grid.SetColumn(classGrid, 4);
                Grid.SetColumnSpan(classGrid, 4);
                Grid.SetRow(classGrid, 0);
                Grid.SetRowSpan(classGrid, 3);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NetworkStatsMainPlot.Model = PlotModels[SelectedPlotIndex];
            if(Creator.ClassificationFullResults != null)
            {
                PrecisionValueTextBlock.Text = Math.Round(Creator.ClassificationFullResults.CalculatePrecision(), 4 ).ToString();
                AccuracyValueTextBlock.Text = Math.Round(Creator.ClassificationFullResults.CalculateAccuracy(), 4).ToString();
                SpecificityValueTextBlock.Text = Math.Round(Creator.ClassificationFullResults.CalculateSpecificity(), 4).ToString();
                SensitivityValueTextBlock.Text = Math.Round(Creator.ClassificationFullResults.CalculateSensitivity(), 4).ToString();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPlotIndex > 0)
            {
                SelectedPlotIndex -= 1;
                NetworkStatsMainPlot.Model = PlotModels[SelectedPlotIndex];
            }
        }

        private void Nextbutton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPlotIndex < PlotModels.Length - 1)
            {
                SelectedPlotIndex += 1;
                NetworkStatsMainPlot.Model = PlotModels[SelectedPlotIndex];
            }
        }

    }
}
