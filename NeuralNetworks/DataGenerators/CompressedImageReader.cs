using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CC = NeuralNetworks.CompressionConstants;

namespace NeuralNetworks.DataGenerators
{
    public class CompressedImageReader
    {
        private static readonly int neuronsInFrame = CC.neuronsInFrame;
        private static readonly int stepSize = CC.stepSize;
        private int width;
        private int height;

        public CompressedImageReader(int width = 200, int height = 200)
        {
            if (width >= height) // width and height has to be the same, so choose smaller
            {
                this.height = (height / 3) * 3;
                this.width = this.height;
            }
            else
            {
                this.width = (width / 3) * 3;
                this.height = this.width;
            }
        }

        public void ReadCompressedImage(string pathToSourceData, string pathToCodeBook, string pathToSaveImage)
        {
            Dictionary<int, Color[]> codeBook = ReadCodeBook(pathToCodeBook);
            using (var destinationImage = new Bitmap(width, height)) // TODO: add flexibiity
            {
                // prepare saving results
                CheckDirectory(pathToSaveImage);
                string[] lines = System.IO.File.ReadAllLines(pathToSourceData);
                List<int> neuronNumbers = new List<int>();
                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    string line = lines[lineNumber];
                    int neuronNumber = Convert.ToInt32(line);
                    neuronNumbers.Add(neuronNumber);
                }
                int x, y;
                int frameNumber = 0;
                for (x = 0; x < width; x += stepSize)
                {
                    for (y = 0; y < height; y += stepSize)
                    {
                        SetImageFrameColors();
                        frameNumber++;
                    }
                }
                destinationImage.Save(Path.ChangeExtension(pathToSaveImage, ".bmp"));
                //local method
                void SetImageFrameColors()
                {
                    var colors = codeBook[neuronNumbers[frameNumber]];
                    for (int i = 0; i < stepSize; i++)
                    {
                        for (int j = 0; j < stepSize; j++)
                        {
                            destinationImage.SetPixel(x + i, y + j, colors[i * stepSize + j]);
                        }
                    }
                }
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

        private Dictionary<int, Color[]> ReadCodeBook(string pathToCodeBook)
        {
            var codeBook = new Dictionary<int, Color[]>(); // stores neuron index and array of its gray colors of frame (its weights)

            string[] lines = System.IO.File.ReadAllLines(pathToCodeBook);
            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string[] splittedLine = lines[lineNumber].Split(new char[] { ',' });
                if (splittedLine.Length != neuronsInFrame)
                {
                    throw new ArgumentException("Could not read that line: " + lines[lineNumber]);
                }
                Color[] frameColors = new Color[neuronsInFrame];
                for (int colorIndex = 0; colorIndex < neuronsInFrame; colorIndex++)
                {
                    int colorValue = Convert.ToInt32(splittedLine[colorIndex]);
                    frameColors[colorIndex] = Color.FromArgb(colorValue, colorValue, colorValue); // gray
                }
                codeBook.Add(lineNumber, frameColors);
            }
            return codeBook;
        }
    }
}
