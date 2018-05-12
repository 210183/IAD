using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC = NeuralNetworks.CompressionConstants;

namespace NeuralNetworks.DataGenerators
{
    public class DataToCompressFromImageGenerator : BasicDataGenerator, IDataGenerator
    {
        private static readonly int neuronsInFrame = CC.neuronsInFrame;
        private static readonly int stepSize = CC.stepSize;
        public override void GenerateData(string pathToSourceImage, string pathToSaveData, bool shouldReverseColors = false)
        {
            try
            {
                // Retrieve the image. TODO: ADD SOURCE VALIDATION HERE
                using (var sourceImage = new Bitmap(pathToSourceImage, true))
                {
                    using (var destinationImage = new Bitmap(sourceImage.Width, sourceImage.Height))
                    {
                        // prepare saving results results
                        CheckDirectory(pathToSaveData);
                        using (var file = File.Create(Path.ChangeExtension(pathToSaveData, ".txt"))) //to create file
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                int width, height;
                                if (sourceImage.Width >= sourceImage.Height) // width and height has to be the same, so choose smaller
                                {
                                    height = (sourceImage.Height / 3) * 3;
                                    width = height;
                                }
                                else
                                {
                                    width = (sourceImage.Width / 3) * 3;
                                    height = width;
                                }

                                Color[] frameColors = new Color[neuronsInFrame];
                                int x, y; // to move through image
                                          // Loop through the images pixels to reset color.
                                for (x = 0; x < width; x += stepSize)
                                {
                                    for (y = 0; y < height; y += stepSize)
                                    {
                                        frameColors = ReadFrameColors(sourceImage, x, y);
                                        if (shouldReverseColors)
                                        {
                                            MakeMeanGray(frameColors); // TODO: Add reverse gray ??
                                        }
                                        else
                                        {
                                            MakeMeanGray(frameColors);
                                        }
                                        SaveFrameData(writer, frameColors);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(ArgumentException)
            {
                throw;
            }
        }

        private void CheckDirectory(string pathToSaveData)
        {
            if (!Directory.Exists(Path.GetDirectoryName(pathToSaveData)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pathToSaveData));
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }

        private Color[] ReadFrameColors(Bitmap image, int x, int y)
        {
            Color[] frameColors = new Color[neuronsInFrame];
            for (int i = 0; i < stepSize; i++)
            {
                for (int j = 0; j < stepSize; j++)
                {
                    frameColors[i * stepSize + j] = image.GetPixel(x + i, y + j);
                }
            }
            return frameColors;
        }

        private void SaveFrameData(StreamWriter writer, Color[] colors)
        {
            if (colors?.Length != neuronsInFrame)
            {
                throw new ArgumentException("Invalid array size");
            }
            for (int i = 0; i < neuronsInFrame - 1; i++)
            {
                writer.Write(((int)colors[i].R).ToString() + ",");
            }
            writer.WriteLine(((int)colors[neuronsInFrame - 1].R).ToString()); // write last value and next line
        }
    }
}
