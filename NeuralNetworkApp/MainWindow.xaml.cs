using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using LiveCharts.Wpf;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;

namespace Geared.Wpf.MultipleSeriesTest
{
    public class MultipleSeriesVm
    {
        public MultipleSeriesVm()
        {
            #region create and train network
            //string learningFileName = @"C:\Users\Jakub\Desktop\approximation_train_1.txt";
            //string testFileName = @"C:\Users\Jakub\Desktop\approximation_test.txt";

            string learningFileName = @"C:\Users\Lola\Desktop\approximation_train_1.txt";
            string testFileName = @"C:\Users\Lola\Desktop\approximation_test.txt";

            LayerCharacteristic[] layers = new LayerCharacteristic[2];
            layers[0] = new LayerCharacteristic(2, new SigmoidUnipolarFunction());
            layers[1] = new LayerCharacteristic(1, new IdentityFunction());
            var network = new NeuralNetwork(1, layers);

            ILearningProvider dataProvider = new LearningApproximationDataProvider(learningFileName, testFileName, 1, 1, true);

            //dataProvider.LearnSet[0] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 1));
            //dataProvider.LearnSet[1] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 0));
            var trainer = new OnlineTrainer(new MeanSquareErrorCalculator(), dataProvider, new BackPropagationAlgorithm(network, new LearningRateHandler(0.01, 0.7, 1.05, 1.04), 0.2, 1.05));

            trainer.TrainNetwork(network, 1000);

            #endregion

            Series = new SeriesCollection();

            #region adding input points loaded from perceptron's file to chart
            var errorsOfEpoch = new ChartValues<ObservablePoint>();
            for (int i = 0; i < trainer.EpochErrorHistory.Count; i++)
            {
                errorsOfEpoch.Add(new ObservablePoint(i, trainer.EpochErrorHistory[i]));
            }
            var errorsOnEpochSeries = new GLineSeries
            {
                Values = errorsOfEpoch.AsGearedValues().WithQuality(Quality.Medium),
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                PointGeometry = DefaultGeometries.Square,
                StrokeThickness = 3,
                //PointGeometry = 1,
                //PointGeometry = null //use a null geometry when you have many series
            };
            Series.Add(errorsOnEpochSeries);
            //var pointBSeries = new GScatterSeries
            //{
            //    Values = pointsB.AsGearedValues().WithQuality(Quality.Medium),
            //    Fill = Brushes.ForestGreen,
            //    Stroke = Brushes.SaddleBrown,
            //    PointGeometry = DefaultGeometries.Triangle,
            //    StrokeThickness = 3,
            //    MinPointShapeDiameter = 3,
            //    MaxPointShapeDiameter = 8,
            //    //PointGeometry = 1,
            //    //PointGeometry = null //use a null geometry when you have many series
            //};
            //Series.Add(pointBSeries);
            #endregion
            Thread.Sleep(10_000);
        }

        public SeriesCollection Series { get; set; }
    }
}
