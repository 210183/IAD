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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;

namespace Geared.Wpf.MultipleSeriesTest
{
    public class MultipleSeriesVm
    {
        public MultipleSeriesVm()
        {
            #region create and train Perceptron
            double learningRate = 0.05;
            Perceptron perceptron = new Perceptron(learningRate);
            perceptron.DesiredOutputTrainingFile.SetFileName(@"C:\Users\Lola\Desktop\zbior_do_nauki\desired_outputs.txt");
            perceptron.InputTrainingFile.SetFileName(@"C:\Users\Lola\Desktop\zbior_do_nauki\inputs.txt");
            perceptron.Train(100, 1);
            #endregion

            Series = new SeriesCollection();

            #region prepare pperceptron separating line and add to display (Series)
            var separatingLineParameters = perceptron.calculateSeparatingLineParameters();
            var values = new ChartValues<ObservablePoint>();
            for (double j = -20; j < 20; j += 0.2) // 10k points each
            {
                values.Add(new ObservablePoint(j, separatingLineParameters[0] * j + separatingLineParameters[1]));
            }
            var series = new GLineSeries
            {
                Values = values.AsGearedValues().WithQuality(Quality.Low),
                Fill = Brushes.Transparent,
                StrokeThickness = 1,
                //MinPointShapeDiameter = 0.1,
                //MaxPointShapeDiameter = 2,
                //PointGeometry = 1,
                //PointGeometry = null //use a null geometry when you have many series
            };
            Series.Add(series);
            #endregion

            #region adding input points loaded from perceptron's file to chart
            var pointsA = new ChartValues<ObservablePoint>();
            var pointsB = new ChartValues<ObservablePoint>();
            var inputPoints = perceptron.InputTrainingFile.LoadFile(SaveLoadMode.InputData); //currnently as List of double[2] with values: X and Y
            var desiredOutput = perceptron.DesiredOutputTrainingFile.LoadFile(SaveLoadMode.DesiredOutputData);
            for (int i = 0; i < inputPoints.Count; i++)
            {
                if (desiredOutput[i][0] == 1)
                {
                    pointsA.Add(new ObservablePoint(inputPoints[i][1], inputPoints[i][2]));
                }
                else if (desiredOutput[i][0] == -1)
                {
                    pointsB.Add(new ObservablePoint(inputPoints[i][1], inputPoints[i][2]));
                }
            }
            var pointASeries = new GScatterSeries
            {
                Values = pointsA.AsGearedValues().WithQuality(Quality.Medium),
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                PointGeometry = DefaultGeometries.Square,
                StrokeThickness = 3,
                MinPointShapeDiameter = 3,
                MaxPointShapeDiameter = 8,
                //PointGeometry = 1,
                //PointGeometry = null //use a null geometry when you have many series
            };
            Series.Add(pointASeries);
            var pointBSeries = new GScatterSeries
            {
                Values = pointsB.AsGearedValues().WithQuality(Quality.Medium),
                Fill = Brushes.ForestGreen,
                Stroke = Brushes.SaddleBrown,
                PointGeometry = DefaultGeometries.Triangle,
                StrokeThickness = 3,
                MinPointShapeDiameter = 3,
                MaxPointShapeDiameter = 8,
                //PointGeometry = 1,
                //PointGeometry = null //use a null geometry when you have many series
            };
            Series.Add(pointBSeries);
            #endregion
        }

        public SeriesCollection Series { get; set; }
    }
}
