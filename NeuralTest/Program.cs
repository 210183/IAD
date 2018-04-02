using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.Data;
using NeuralNetworks.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string learningFileName = @"C:\Users\Jakub\Desktop\approximation_train_1.txt";
            string testFileName = @"C:\Users\Jakub\Desktop\approximation_test.txt";

            LayerCharacteristic[] layers = new LayerCharacteristic[2];
            layers[0] = new LayerCharacteristic(2, new SigmoidUnipolarFunction());
            layers[1] = new LayerCharacteristic(1, new IdentityFunction());
            var network = new NeuralNetwork(1, layers);

            ILearningProvider dataProvider = new LearningApproximationDataProvider(learningFileName, testFileName, 1, 1, true);

            //dataProvider.LearnSet[0] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 1));
            //dataProvider.LearnSet[1] = new Datum(Vector<double>.Build.Dense(2, 1), Vector<double>.Build.Dense(2, 0));
            var trainer = new OnlineTrainer(new MeanSquareErrorCalculator(), dataProvider, new BackPropagationAlgorithm( new LearningRateHandler(0.01, 0.7,1.05,1.04), 0.2, 1.05));

            trainer.TrainNetwork(network, 100);
            network.ConsoleDisplay();
            Console.ReadLine();
            #region old demo
            //LayerCharacteristic[] layers = new LayerCharacteristic[2];
            //layers[0] = new LayerCharacteristic(2, new SigmoidUnipolarFunction());
            //layers[1] = new LayerCharacteristic(2, new IdentityFunction());
            //var network = new NeuralNetwork(1, layers);
            //var input = Vector<double>.Build.Dense(2);
            //input[0] = 1;
            //input[1] = 2;
            //foreach (var layer in network.Layers)
            //{
            //    Console.Write("\nNext layer:");
            //    foreach (var row in layer.Weights.ToRowArrays())
            //    {
            //        Console.WriteLine("\nrow: ");
            //        foreach (var cell in row)
            //        {
            //            Console.Write($"{cell} | ");
            //        }

            //    }
            //}
            //Console.WriteLine($"\nCalculated first: {network.CalculateOutput(input)[0]}");
            //Console.WriteLine($"\nCalculated seond: {network.CalculateOutput(input)[1]}");
            //var r = network.CalculateOutput(input);
            //Console.ReadLine();
            #endregion
            #region multiplty demo
            //var m = Matrix<double>.Build.Random(2,1);
            //var v = Vector<double>.Build.Dense(2,2);
            //Console.WriteLine($"vector: {v[0]} {v[1]}");
            //Console.Write("\nMatrix:");
            //foreach (var row in m.ToRowArrays())
            //{
            //    Console.WriteLine("\nrow: ");
            //    foreach (var cell in row)
            //    {
            //        Console.Write($"{cell} | ");
            //    }

            //}
            //var res = v * m;
            //Console.WriteLine($"res: {res.ToString()}");

            //Console.ReadLine();
            #endregion
        }
    }
}
