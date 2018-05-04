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
    public class ImageDataGenerator : ITwoDimensionalDataGenerator
    {
        public void GenerateData(string pathToSourceImage, string pathToSaveData, bool shouldReverseColors = false)
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

        public void GenerateGrayData(int numberOfPoints, string fileToSaveData)
        {
            try
            {
                // Retrieve the image.
                var sourceImage = new Bitmap(@"C:\Users\Mateusz\Desktop\ImagesTests\lenaFull.bmp", true);
                var destinationImage = new Bitmap(sourceImage.Width, sourceImage.Height);
                int x, y;

                // Loop through the images pixels to reset color.
                for (x = 0; x < sourceImage.Width; x++)
                {
                    for (y = 0; y < sourceImage.Height; y++)
                    {
                        Color pixelColor = sourceImage.GetPixel(x, y);
                        Color newColor = MakeMeanGray(pixelColor);
                        destinationImage.SetPixel(x, y, newColor);
                    }
                }
                destinationImage.Save(@"C:\Users\Mateusz\Desktop\ImagesTests\newGrayImage2.bmp");
                destinationImage.Dispose();
                sourceImage.Dispose();
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        private Color MakeGray(Color color)
        {
            int max = MaxFromColor(color);
            var newColor = Color.FromArgb(max, max, max);
            return newColor;
        }
        private Color MakeMeanGray(Color color)
        {
            int mean = MeanFromColor(color);
            var newColor = Color.FromArgb(mean, mean, mean);
            return newColor;
        }
        private Color MakeBlackOrWhiteFromMax(Color color)
        {
            int max = MaxFromColor(color);
            if(max > 160) //make white
            {
                var newColor = Color.FromArgb(255, 255, 255);
                return newColor;
            }
            else // make dark
            {
                var newColor = Color.FromArgb(0,0,0);
                return newColor;
            }
        }
        private Color MakeBlackOrWhiteFromMean(Color color)
        {
            int mean = MeanFromColor(color);
            if (mean > 220) //make white
            {
                var newColor = Color.FromArgb(255, 255, 255);
                return newColor;
            }
            else // make dark
            {
                var newColor = Color.FromArgb(0, 0, 0);
                return newColor;
            }
        }
        private Color MakeReverseBlackOrWhiteFromMean(Color color)
        {
            int mean = MeanFromColor(color);
            if (mean < 112) //make white
            {
                var newColor = Color.FromArgb(255, 255, 255);
                return newColor;
            }
            else // make dark
            {
                var newColor = Color.FromArgb(0, 0, 0);
                return newColor;
            }
        }
        private int MaxFromColor(Color color)
        {
            int max = color.R;
            if (color.G > max)
            {
                max = color.G;
            }
            if (color.B > max)
            {
                max = color.B;
            }
            return max;
        }
        private int MeanFromColor(Color color)
        {
            int sum = color.R + color.G + color.B;
            return sum / 3;
        }
    }
}
