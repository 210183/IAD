using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;
using NeuralNetworks.DataGenerators;
using NeuralNetworks.DistanceMetrics;

namespace NeuralNetworksTests
{
    [TestClass]
    public class CompressorTests
    {
        [TestMethod]
        public void CompressTest()
        {
            var network = new NeuralNetwork
            (
               9,
               new LayerCharacteristic[1] { new LayerCharacteristic(2000, new IdentityFunction()) },
               false
            );
            var compressor = new DataCompressor(new EuclideanLength());
            compressor.CompressData(network, @"F:\ImagesTests\generatedDataForCompression.txt", @"F:\ImagesTests\compressed\data.txt", @"F:\ImagesTests\compressed\codebook.txt");
        }
        [TestMethod]
        public void DecompressTest()
        {
            var decompressor = new CompressedImageReader();
            decompressor.ReadCompressedImage(@"F:\ImagesTests\compressed\data.txt", @"F:\ImagesTests\compressed\codebook.txt", @"F:\ImagesTests\compressed\decompressed.bmp");
        }
    }
}
