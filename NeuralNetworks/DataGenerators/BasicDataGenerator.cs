using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.DataGenerators
{
    public abstract class BasicDataGenerator : IDataGenerator
    {
        public abstract void GenerateData(string pathToSourceImage, string pathToSaveData, bool shouldReverseColors);


        protected Color MakeGray(Color color)
        {
            int max = MaxFromColor(color);
            var newColor = Color.FromArgb(max, max, max);
            return newColor;
        }
        protected Color MakeMeanGray(Color color)
        {
            int mean = MeanFromColor(color);
            var newColor = Color.FromArgb(mean, mean, mean);
            return newColor;
        }
        /// <summary>
        /// Sets colors in given array to gray 
        /// </summary>
        /// <param name="color"></param>
        protected void MakeMeanGray(Color[] color)
        {
            if(color != null)
            {
                for (int i = 0; i < color.Length; i++)
                {
                    int mean = MeanFromColor(color[i]);
                    var newColor = Color.FromArgb(mean, mean, mean);
                    color[i] = newColor;
                }
            }
        }
        protected Color MakeBlackOrWhiteFromMax(Color color)
        {
            int max = MaxFromColor(color);
            if (max > 160) //make white
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
        protected Color MakeBlackOrWhiteFromMean(Color color)
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
        protected Color MakeReverseBlackOrWhiteFromMean(Color color)
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
        protected int MaxFromColor(Color color)
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
        protected int MeanFromColor(Color color)
        {
            int sum = color.R + color.G + color.B;
            return sum / 3;
        }
    }
}
