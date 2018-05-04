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
            gen.GenerateData(@"C:\Users\Mateusz\Desktop\ImagesTests\test2.bmp", @"C:\Users\Mateusz\Desktop\ImagesTests\generatedData.txt");
        }
    }
}
