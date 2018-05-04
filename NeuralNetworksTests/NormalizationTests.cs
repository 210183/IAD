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
            var provider = new PointsDataProvider
            {
                Points = new Datum[4]
            };
            provider.Points[0] = new Datum(Vector<double>.Build.Dense(new double[] {
                1.2342,
                434
            }));
            provider.Points[1] = new Datum(Vector<double>.Build.Dense(new double[] {
                -12332,
                0.01231
            }));
            provider.Points[2] = new Datum(Vector<double>.Build.Dense(new double[] {
                1123,
                13
            }));
            provider.Points[3] = new Datum(Vector<double>.Build.Dense(new double[] {
                1.001,
                -0.123
            }));
            norm.Normalize(provider);
            foreach(var p in provider.Points)
            {
                var length = distCalc.Length(p.X);
                Assert.AreEqual(1, length, 0.001);
            }
        }
    }
}
