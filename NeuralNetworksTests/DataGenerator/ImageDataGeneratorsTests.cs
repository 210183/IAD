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
            gen.GenerateData(@"F:\ImagesTests\lenaSmall.bmp", @"F:\ImagesTests\generatedData.txt");
        }
        [TestMethod]
        public void GenerateDataForCompression()
        {
            var gen = new DataToCompressFromImageGenerator();
            //gen.GenerateData(@"F:\ImagesTests\imageForGeneratorTest.bmp", @"F:\ImagesTests\generatedDataForCompression.txt");
            gen.GenerateData(@"F:\ImagesTests\lenaFullSmall.bmp", @"F:\ImagesTests\generatedDataForCompression.txt");
        }
    }
}
