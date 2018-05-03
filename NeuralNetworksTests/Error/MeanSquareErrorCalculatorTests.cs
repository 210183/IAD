using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using NeuralNetworksTestsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class MeanSquareErrorCalculatorTests
    {
        public IErrorCalculator ErrorCalculator { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ErrorCalculator = new MeanSquareErrorCalculator();
        }

        [TestMethod()]
        public void CalculateErrorVectorTest()
        {
            var output = Vector<double>.Build.Dense(new Double[7] {5, 0.8, 0.4, 0, -0.2, -0.2, -2});
            var desiredOutput = Vector<double>.Build.Dense(new Double[7] { 5, 1, 0.2, 0, 0.2, -0.8, -1.4 });
            var correctErrorVector = Vector<double>.Build.Dense(new Double[7] { 0, 0.2, -0.2, 0, 0.4, -0.6, 0.6 });
            var calculatedErrorVector = ErrorCalculator.CalculateErrorVector(output, desiredOutput);
            calculatedErrorVector = calculatedErrorVector.RoundPointwise();
            Assert.IsTrue(correctErrorVector.SequenceEqual(calculatedErrorVector));
        }

        [TestMethod()]
        public void CalculateErrorSumTest()
        {
            var output = Vector<double>.Build.Dense(new Double[3] { 1, 0, 3 });
            var desiredOutput = Vector<double>.Build.Dense(new Double[3] { 2, -3, 1 });
            double correctErrorSum = 14;
            var calculatedErrorSum = ErrorCalculator.CalculateErrorSum(output, desiredOutput);
            Assert.IsTrue(correctErrorSum == calculatedErrorSum);
        }

        [TestMethod()]
        public void CalculateEpochErrorTest()
        {
            var output = Vector<double>.Build.Dense(new Double[7] { 11, -2, 2, 4, 0, 2, -10 });
            var desiredOutput = Vector<double>.Build.Dense(new Double[7] { 5, 1, -1, 2, -2, 1, -9 });
            var errors = desiredOutput - output;
            errors = errors.PointwisePower(2); 
            double correctErrorSum = 32;
            var calculatedErrorSum = ErrorCalculator.CalculateEpochError(errors);
            Assert.IsTrue(Math.Round(correctErrorSum, 6) == Math.Round(calculatedErrorSum, 6));
        }

        
    }
}