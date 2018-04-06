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
            TestErrorTextBlock.Text = Creator.BestTestError.ToString();

            PlotModels = new PlotModel[2]; // TODO: How big sohuld that be ?
            #region Epoch error plot (set on 0 index of PlotModels)
            var epochErrorPlotModel = new PlotModel
            {
                Title = "Error of epoch number plot",
                IsLegendVisible = true
            };
            var EpochErrorDataPoints = new List<DataPoint>();
            for (int i = 0; i < Creator.BestNetworkEpochHistory.Count; i++) // adding data
            {
                EpochErrorDataPoints.Add(new DataPoint(i, Creator.BestNetworkEpochHistory[i]));
            }
            var series = new LineSeries()
            {
                Smooth = false,
                LineJoin = LineJoin.Miter,
            };
            series.ItemsSource = EpochErrorDataPoints;
            epochErrorPlotModel.Series.Add(series);
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
                classGrid.Children.Add(new TextBlock() { Text = "Class number", FontWeight = System.Windows.FontWeights.Bold, TextAlignment=TextAlignment.Center });
                for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++) //ceate column names as class number bolded
                {
                    classGrid.Children.Add(new TextBlock() { Text = columnIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, TextAlignment = TextAlignment.Center });
                }
                for (int rowIndex = 0; rowIndex < Creator.ClassificationFullResults.RowCount; rowIndex++)
                {
                    classGrid.Children.Add(new TextBlock() { Text = rowIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, TextAlignment = TextAlignment.Center }); //add row name = class number bolded
                    for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++)
                    {
                        classGrid.Children.Add(new TextBlock() { Text = Creator.ClassificationFullResults[rowIndex, columnIndex].ToString(), TextAlignment = TextAlignment.Center });
                    }
                }
                NetworkStatsMainGrid.Children.Add(classGrid);
                Grid.SetColumn(classGrid, 3);
                Grid.SetColumnSpan(classGrid, 3);
                Grid.SetRow(classGrid, 0);
                Grid.SetRowSpan(classGrid, 3);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NetworkStatsMainPlot.Model = PlotModels[SelectedPlotIndex];
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
