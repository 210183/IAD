using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.Error.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Error.Extensions.Tests
{
    [TestClass()]
    public class ClassificationMetricsTests
    {
        Matrix<double> DiagonalMatrix;
        Matrix<double> Matrix;

        [TestInitialize]
        public void Initialize()
        {
            DiagonalMatrix = Matrix<double>.Build.Diagonal(new Double[4] {2, 3, 5, 17 });
            Matrix = Matrix<double>.Build.DenseOfArray(new Double[3,3] { { 3, 1, 2 }, {2, 5, 1 }, {3, 0, 10 } });
        }

        [TestMethod()]
        public void CalculateAccuracyTest()
        {
            var diagAcc = DiagonalMatrix.CalculateAccuracy();
            Assert.AreEqual(diagAcc, 1);
            var acc = Matrix.CalculateAccuracy();
            Assert.IsTrue(0.66 <= acc && acc <= 0.67);
        }

        [TestMethod()]
        public void CalculatePrecisionTest()
        {
            var diagPrec = DiagonalMatrix.CalculatePrecision();
            Assert.AreEqual(diagPrec, 1);
            var prec = Matrix.CalculatePrecision();
            Assert.IsTrue(0.65 <= prec && prec <= 0.66);
        }

        [TestMethod()]
        public void CalculateSensitivityTest()
        {
            var diagSens = DiagonalMatrix.CalculateSensitivity();
            Assert.AreEqual(diagSens, 1);
            var sens = Matrix.CalculateSensitivity();
            Assert.IsTrue(0.63 <= sens && sens <= 0.64);
        }

        [TestMethod()]
        public void CalculateSpecificityTest()
        {
            var diagSpec = DiagonalMatrix.CalculateSpecificity();
            Assert.AreEqual(diagSpec, 1);
            var spec = Matrix.CalculateSpecificity();
            Assert.IsTrue(0.66 <= spec && spec <= 0.67);
        }
    }
}