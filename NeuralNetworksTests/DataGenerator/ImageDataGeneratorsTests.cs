﻿using System;
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
            gen.GenerateData(@"F:\ImagesTests\kola.bmp", @"F:\ImagesTests\generatedDatakola.txt");
        }
        [TestMethod]
        public void GenerateDataForCompression()
        {
            var gen = new DataToCompressGenerator();
            //gen.GenerateData(@"F:\ImagesTests\imageForGeneratorTest.bmp", @"F:\ImagesTests\generatedDataForCompression.txt");
            gen.GenerateData(@"F:\ImagesTests\owoc.jpg", @"F:\ImagesTests\generatedDataForCompression.txt");
        }
    }
}
