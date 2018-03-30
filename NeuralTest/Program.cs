﻿using MathNet.Numerics.LinearAlgebra;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
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
            LayerCharacteristic[] layers = new LayerCharacteristic[2];
            layers[0] = new LayerCharacteristic(2, new SigmoidUnipolarFunction());
            layers[1] = new LayerCharacteristic(2, new IdentityFunction());
            var network = new NeuralNetwork(1, layers);
            var input = Vector<double>.Build.Dense(2);
            input[0] = 1;
            input[1] = 2;
            foreach (var layer in network.Layers)
            {
                Console.Write("\nNext layer:");
                foreach (var row in layer.Weights.ToRowArrays())
                {
                    Console.WriteLine("\nrow: ");
                    foreach (var cell in row)
                    {
                        Console.Write($"{cell} | ");
                    }

                }
            }
            Console.WriteLine($"\nCalculated first: {network.CalculateOutput(input)[0]}");
            Console.WriteLine($"\nCalculated seond: {network.CalculateOutput(input)[1]}");
            var r = network.CalculateOutput(input);
            Console.ReadLine();

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