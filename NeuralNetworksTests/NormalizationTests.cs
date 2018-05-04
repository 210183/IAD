using System;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.Data;
using NeuralNetworks.DistanceMetrics;
using NeuralNetworks.Normalization;

namespace NeuralNetworksTests
{
    [TestClass]
    public class NormalizationTests
    {
        [TestMethod]
        public void EuclideanNormalization()
        {
            var distCalc = new EuclideanLength();
            var norm = new EuclideanNormalizator();
            var points = new Datum[4];
            points[0] = new Datum(Vector<double>.Build.Dense(new double[] {
                1.2342,
                434
            }));
            points[1] = new Datum(Vector<double>.Build.Dense(new double[] {
                -12332,
                0.01231
            }));
            points[2] = new Datum(Vector<double>.Build.Dense(new double[] {
                1123,
                13
            }));
            points[3] = new Datum(Vector<double>.Build.Dense(new double[] {
                1.001,
                -0.123
            }));
            norm.Normalize(points);
            foreach(var p in points)
            {
                var length = distCalc.Length(p.X);
                Assert.AreEqual(1, length, 0.001);
            }
        }
    }
}
