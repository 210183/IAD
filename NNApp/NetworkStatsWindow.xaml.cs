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

            if (taskType == TaskType.Approximation)
                PlotModels = new PlotModel[2];
            else
                PlotModels = new PlotModel[1];
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
                var newBorder = new Border();
                newBorder.BorderThickness = new Thickness(3);
                newBorder.BorderBrush = Brushes.Black;

               


                var classGrid = new UniformGrid
                {
                    Rows = Creator.ClassificationFullResults.RowCount + 1,
                    Columns = Creator.ClassificationFullResults.ColumnCount + 1,
                    
                };

                //No way to set Border on textblock defined in code behind; changing for TextBox is a way to make border but still it doesn't look like a table

                classGrid.Children.Add(new TextBlock() { Text = "Class number", FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center});
                for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++) //ceate column names as class number bolded
                {
                    classGrid.Children.Add(new TextBlock() { Text = columnIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center });
                }
                for (int rowIndex = 0; rowIndex < Creator.ClassificationFullResults.RowCount; rowIndex++)
                {
                    classGrid.Children.Add( new TextBlock() { Text = rowIndex.ToString(), FontWeight = System.Windows.FontWeights.Bold, VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center }); //add row name = class number bolded
                    for (int columnIndex = 0; columnIndex < Creator.ClassificationFullResults.ColumnCount; columnIndex++)
                    {
                        classGrid.Children.Add( new TextBlock() { Text = Creator.ClassificationFullResults[rowIndex, columnIndex].ToString(), VerticalAlignment = System.Windows.VerticalAlignment.Center, TextAlignment = System.Windows.TextAlignment.Center });
                        
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
                PrecisionValueTextBlock.Text = Math.Round( CalculatePrecision(Creator.ClassificationFullResults), 4 ).ToString();
                AccuracyValueTextBlock.Text = Math.Round(CalculateAccuracy(Creator.ClassificationFullResults), 4).ToString();
                SpecificityValueTextBlock.Text = Math.Round(CalculateSpecificity(Creator.ClassificationFullResults), 4).ToString();
                SensitivityValueTextBlock.Text = Math.Round(CalculateSensitivity(Creator.ClassificationFullResults), 4).ToString();
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

        #region helper methods
        /// <summary>
        /// Calculates whole classifier accuracy (all proper / all)
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        private double CalculateAccuracy(Matrix<double> classificationResults)
        {
            double properlyClassified = 0;
            for (int i = 0; i < classificationResults.RowCount; i++)
            {
                for (int j = 0; j < classificationResults.ColumnCount; j++)
                {
                    properlyClassified += classificationResults[i, j];
                }
            }
            double accuracy = properlyClassified / classificationResults.RowSums().Sum(); //TODO: Test that 
            return accuracy;
        }

        /// <summary>
        /// Calculates mean precision for all classes
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        private double CalculatePrecision(Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");

            double sumaricPrecision = 0;
            for (int classNumber = 0; classNumber < classificationResults.RowCount; classNumber++) //here classes are enumerated from 0
            {
                double classPrecision = 0;
                double classTruePositive = classificationResults[classNumber, classNumber]; //on diagonal
                double classAll = 0;
                for (int rowIndex = 0; rowIndex < classificationResults.RowCount; rowIndex++) 
                {
                    classAll += classificationResults[rowIndex, classNumber];
                }
                classPrecision = classTruePositive / classAll;
                sumaricPrecision += classPrecision;
            }
            double meanPrecision = sumaricPrecision / classificationResults.RowCount;
            return meanPrecision;
        }

        /// <summary>
        /// Calculates arithmetic mean sensitivity
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        private double CalculateSensitivity(Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");

            double sumaricSensitivity = 0;
            for (int classNumber = 0; classNumber < classificationResults.ColumnCount; classNumber++) //here classes are enumerated from 0
            {
                double classSensitivity = 0;
                double classTruePositive = classificationResults[classNumber, classNumber]; //on diagonal
                double classAll = 0;
                for (int columnIndex = 0; columnIndex < classificationResults.ColumnCount; columnIndex++)
                {
                    classAll += classificationResults[classNumber, columnIndex];
                }
                classSensitivity = classTruePositive / classAll;
                sumaricSensitivity += classSensitivity;
            }
            double meanSensitivity = sumaricSensitivity / classificationResults.ColumnCount;
            return meanSensitivity;
        }

        /// <summary>
        /// Calculates arithmetic mean Specificity
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        private double CalculateSpecificity(Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");

            double sumaricSpecificity = 0;
            for (int classNumber = 0; classNumber < classificationResults.RowCount; classNumber++) //here classes are enumerated from 0
            {
                double classSpecificity = 0;
                double classTrueNegative = 0;
                for (int diagonalIndex = 0; diagonalIndex < classificationResults.RowCount; diagonalIndex++)
                {
                    if (diagonalIndex != classNumber) // all positives other than current class 
                        classTrueNegative += classificationResults[diagonalIndex, diagonalIndex]; 
                }
                double classAll = classTrueNegative; //first ingredient, second added below in parts
                for (int rowIndex = 0; rowIndex < classificationResults.ColumnCount; rowIndex++) // False Positive
                {
                    classAll += classificationResults[rowIndex, classNumber];
                }
                classSpecificity = classTrueNegative / classAll;
                sumaricSpecificity += classSpecificity;
            }
            double meanSpecificity = sumaricSpecificity / classificationResults.ColumnCount;
            return meanSpecificity;
        }
        #endregion
    }
}
