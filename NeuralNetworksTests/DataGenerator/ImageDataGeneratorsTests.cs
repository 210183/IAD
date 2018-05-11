using System;
using NeuralNetworks.DataGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNetworksTests.DataGenerator
{
    [TestClass]
    public class ImageDataGeneratorsTests
    {
        [TestMethod]
        public void MakeBlackOrWhite()
        {
            var gen = new ImageDataGenerator();
            //gen.GenerateGrayData(123, "");
            gen.GenerateData(@"F:\ImagesTests\newImage2.bmp", @"F:\ImagesTests\generatedData.txt");
        }
    }
}
