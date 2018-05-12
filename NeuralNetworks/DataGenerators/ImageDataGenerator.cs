using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.DataGenerators
{
    public class ImageDataGenerator : BasicDataGenerator, ITwoDimensionalDataGenerator
    {
        public override void GenerateData(string pathToSourceImage, string pathToSaveData, bool shouldReverseColors = false)
        {
            try
            {
                // Retrieve the image. TODO: ADD SOURCE VALIDATION HERE
                using (var sourceImage = new Bitmap(pathToSourceImage, true))
                {
                    using (var destinationImage = new Bitmap(sourceImage.Width, sourceImage.Height))
                    {
                        int x, y;
                        List<Point> points = new List<Point>();
                        // Loop through the images pixels to reset color.
                        for (x = 0; x < sourceImage.Width; x++)
                        {
                            for (y = 0; y < sourceImage.Height; y++)
                            {
                                Color pixelColor = sourceImage.GetPixel(x, y);
                                Color newColor;
                                if (shouldReverseColors)
                                {
                                    newColor = MakeReverseBlackOrWhiteFromMean(pixelColor);
                                }
                                else
                                {
                                    newColor = MakeBlackOrWhiteFromMean(pixelColor);
                                }
                                if(newColor.R == 0) //all color components should be the same so can take R
                                {
                                    points.Add(new Point()
                                    {
                                        X = x,
                                        Y = y
                                    });
                                }
                                destinationImage.SetPixel(x, y, newColor);
                            }
                        }
                        #region save results
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
                        using(var file = File.Create(Path.ChangeExtension(pathToSaveData, ".txt"))) //to create file
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                foreach (var p in points)
                                {
                                    writer.WriteLine(p.X.ToString() + "," + p.Y.ToString());
                                }
                            }
                        }
                        destinationImage.Save(Path.ChangeExtension(pathToSaveData, ".bmp"));
                        #endregion
                    }
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
        }
    }
}
