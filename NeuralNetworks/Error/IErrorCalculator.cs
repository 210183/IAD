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
        /// Returns sum of single value errors (appropriate to specific calculator, it may be e.g. sum of squared errors)
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        double CalculateErrorSum(Vector<double> output, Vector<double> desiredOutput);

        /// <summary>
        /// That method will calc sumaric error from given errors.
        /// </summary>
        /// <returns></returns>
        double CalculateEpochError(Vector<double> errors);
    }
}