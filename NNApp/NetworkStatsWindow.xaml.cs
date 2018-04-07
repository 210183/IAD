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
using OxyPlot.Wpf;
using Microsoft.Win32;
using NeuralNetworks.Data;
using System.IO;

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
            var learningSeries = new OxyPlot.Series.LineSeries()
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
                var testSeries = new OxyPlot.Series.LineSeries()
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
                var approximationFunctionPlotModel = new PlotModel 
                {
                    Title = "Network approximation function",
                    IsLegendVisible = true,
                };
                var approximationFunctionDataPoints = new List<DataPoint>();    //network outputs approximation
                for (int i = 0; i < Creator.ApproximationFunctionPoints.GetLength(0); i++) // adding data
                {
                    approximationFunctionDataPoints.Add(new DataPoint(Creator.ApproximationFunctionPoints[i, 0], Creator.ApproximationFunctionPoints[i, 1]));
                }
                var approximationSeries = new OxyPlot.Series.LineSeries()
                {
                    Title = "Network approximation",
                    Selectable = false
                };
                approximationSeries.ItemsSource = approximationFunctionDataPoints;
                approximationFunctionPlotModel.Series.Add(approximationSeries);
                //set of 'real' points
                var testSet = Creator.DataProvider.DataSet;
                var testPoints = new List<ScatterPoint>();    //network outputs approximation
                for (int i = 0; i < testSet.GetLength(0); i++) // adding data
                {
                    if (CurrentNetwork.IsBiasExisting)
                        testPoints.Add(new ScatterPoint(testSet[i].X[1], testSet[i].D[0]));
                    else
                        testPoints.Add(new ScatterPoint(testSet[i].X[0], testSet[i].D[0]));
                }
                var testSeries = new OxyPlot.Series.ScatterSeries()
                {
                    Title = "Test points",
                };
                testSeries.ItemsSource = testPoints;
                approximationFunctionPlotModel.Series.Add(testSeries);
                //save created plot to plot models 
                PlotModels[1] = approximationFunctionPlotModel;
            }
            else if (taskType == TaskType.Classification)
            {
                var newBorder = new Border();
                newBorder.BorderThickness = new Thickness(3);
                newBorder.BorderBrush = Brushes.Black;

                var classGrid = new UniformGrid
                {
                    Rows = Creator.ClassificationFullResults.RowCount + 1,
                    Columns = Creator.ClassificationFullResults.ColumnCount + 1,
                    
                };

                //No way to set Border on textblock defined in code behind; changing for TextBox is a way to make border but still it doesn't look like a table

                classGrid.Children.Add(new TextBox() { BorderThickness = new Thickness(1), Text = "Class number", FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center, MinHeight = 50});
                for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++) //ceate column names as class number bolded
                {
                    classGrid.Children.Add(new TextBox() { BorderThickness = new Thickness(1), Text = columnIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center, MinHeight = 50 });
                }
                for (int rowIndex = 0; rowIndex < Creator.ClassificationFullResults.RowCount; rowIndex++)
                {
                    classGrid.Children.Add( new TextBox() { BorderThickness = new Thickness(1), Text = rowIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center, MinHeight = 50 }); //add row name = class number bolded
                    for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++)
                    {
                        classGrid.Children.Add( new TextBox() { BorderThickness = new Thickness(1), Text = Creator.ClassificationFullResults[rowIndex, columnIndex].ToString(), VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center, MinHeight = 50 });
                        
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

        private void ScreenShotbutton_Click(object sender, RoutedEventArgs e)
        {
            var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(PlotModels[0]);
            Clipboard.SetImage(bitmap);
        }

        private void Testbutton_Click(object sender, RoutedEventArgs e)
        {
            
            string testFileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Test input file";
            if (openFileDialog.ShowDialog() == true)
               testFileName = openFileDialog.FileName;
            if (File.Exists(testFileName))
            {
                try
                {
                    int numberOfOutputs = Creator.BestNetwork.Layers[Creator.BestNetwork.Layers.Length-1].Weights.ColumnCount;
                    IDataProvider dataProvider;

                    if (ChosenTask == TaskType.Classification)
                        dataProvider = new ClassificationDataProvider(testFileName, Creator.InputsNumber, numberOfOutputs, Creator.IsBiasOn);
                    else if (ChosenTask == TaskType.Approximation || ChosenTask == TaskType.Transformation)
                        dataProvider = new ApproximationDataProvider(testFileName, Creator.InputsNumber, numberOfOutputs, Creator.IsBiasOn);
                    else
                    {
                        MessageBox.Show("Choose task type!");
                        return;
                    }

                    string pathToLogFile = Directory.GetCurrentDirectory() + @"\OutputFile.txt";

                    if (!File.Exists(pathToLogFile))
                    {
                        using (var loggerFile = File.Create(pathToLogFile))
                        {
                            //Do nothing
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(pathToLogFile))
                    {
                        var dataSet = dataProvider.DataSet;
                        var output = Vector<double>.Build.Dense(dataSet.Length);

                        for (int dataIndex = 0; dataIndex < dataSet.Length; dataIndex++)
                        {
                            output = CurrentNetwork.CalculateOutput(dataSet[dataIndex].X);
                            writer.WriteLine(output.ToString());
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Incorrect file path!");
                return;
            }
        }

    }
}
