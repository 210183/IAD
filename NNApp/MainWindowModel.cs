using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNApp
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            #region create and train network
            //string learningFileName = @"C:\Users\Jakub\Desktop\approximation_train_1.txt";
            //string testFileName = @"C:\Users\Jakub\Desktop\approximation_test.txt";

            string learningFileName = @"C:\Users\Lola\Desktop\classification_train.txt";
            string testFileName = @"C:\Users\Lola\Desktop\classification_train.txt";

            int inputsNumber = 4;

            LayerCharacteristic[] layers = new LayerCharacteristic[2];
            layers[0] = new LayerCharacteristic(7, new SigmoidUnipolarFunction());
            layers[1] = new LayerCharacteristic(3, new IdentityFunction());
            var network = new NeuralNetwork(inputsNumber, layers);

            ILearningProvider dataProvider = new LearningApproximationDataProvider(learningFileName, testFileName, inputsNumber, 3, true);

            //dataProvider.LearnSet[0] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 1));
            //dataProvider.LearnSet[1] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 0));
            var trainer = new OnlineTrainer(new MeanSquareErrorCalculator(), dataProvider, new BackPropagationAlgorithm(network, new LearningRateHandler(0.001, 0.8, 1.1, 1.05), 0.02, 1.05));

            trainer.TrainNetwork(network, 100);

            #endregion

            #region 
            MyModel = new PlotModel { Title = "Main Network" };
            var FirstLineSeriesPoints = new List<DataPoint>();
            for (int i = 0; i < trainer.EpochErrorHistory.Count; i++) // adding data
            {
                FirstLineSeriesPoints.Add(new DataPoint(i, trainer.EpochErrorHistory[i]));
            }
            var series = new LineSeries();
            series.ItemsSource = FirstLineSeriesPoints;
            MyModel.Series.Add(series);
            MyModel.InvalidatePlot(true);
            #endregion
            
        }
        public PlotModel MyModel { get; private set; }
    }
}
