using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks
{
    public interface IErrorCalculator
    {
        /// <summary>
        /// Returns vector with error values.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        /// <returns></returns>
        Vector<double> CalculateErrorVector(Vector<double> output, Vector<double> desiredOutput);

        /// <summary>
        /// That method will store calculated error in class implementing it.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        double CalculateSingleError(Vector<double> output, Vector<double> desiredOutput);

        /// <summary>
        /// That method will calc error from errors counted before
        /// </summary>
        /// <returns></returns>
        double CalculateEpochError();
    }
}