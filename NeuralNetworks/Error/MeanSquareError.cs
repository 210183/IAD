using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks
{
    public class MeanSquareErrorCalculator : IErrorCalculator
    {
        public Vector<double> Errors { get; set; } = Vector<double>.Build.Dense(1);

        /// <summary>
        /// Calculates single error squared value
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        /// <returns></returns>
        public Vector<double> CalculateErrorVector(Vector<double> output, Vector<double> desiredOutput)
        {
            if (output.Count == desiredOutput.Count)
            {
                var errorV = desiredOutput - output;
                return errorV;
            }
            else
            {
                throw new ArgumentException("Vectors have to have the same length");
            }
        }

        /// <summary>
        /// Calculates single error squared value
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        /// <returns></returns>
        public double CalculateSingleError(Vector<double> output, Vector<double> desiredOutput)
        {
            if (output.Count == desiredOutput.Count)
            {
                var errors = CalculateErrorVector(output, desiredOutput);
                errors = errors.PointwisePower(2);
                var resultError = errors.Sum();
                Errors.Add(resultError);
                return resultError;
            }
            else
            {
                throw new ArgumentException("Vectors have to have the same length");
            }
        }

        /// <summary>
        /// Calculate square root from errors counted before sum and divides it by 2.
        /// Also clears stored error values.
        /// </summary>
        /// <returns></returns>
        public double CalculateEpochError()
        {
            double meanSquaredError = 0;
            for (int i = 0; i < Errors.Count; i++) //sum already squared errors
            {
                meanSquaredError += Errors[i];
            }
            meanSquaredError = Math.Sqrt(meanSquaredError) / 2;
            return meanSquaredError;
        }
    }
}
