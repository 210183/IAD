using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Error.Extensions
{
    public static class ClassificationMetrics
    {
        /// <summary>
        /// Calculates whole classifier accuracy (all proper / all)
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns>Accuracy [0,1] or -1 if no values in matrix</returns>
        public static double CalculateAccuracy(this Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");
            double dataCount = classificationResults.RowSums().Sum();
            if (dataCount <= 0) // no elements in calssification history
                return -1;
            double properlyClassified = classificationResults.Diagonal().Sum();
            double accuracy = properlyClassified / dataCount; //TODO: Test that 
            return accuracy;
        }

        /// <summary>
        /// Calculates mean precision for all classes
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns>Precision [0,1] or -1 if no values in matrix</returns>
        public static double CalculatePrecision(this Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");

            double sumaricPrecision = 0;
            for (int classNumber = 0; classNumber < classificationResults.RowCount; classNumber++) //here classes are enumerated from 0
            {
                double classPrecision = 0;
                double classTruePositive = classificationResults[classNumber, classNumber]; //on diagonal
                double classAll = 0;
                for (int rowIndex = 0; rowIndex < classificationResults.RowCount; rowIndex++)
                {
                    classAll += classificationResults[rowIndex, classNumber];
                }
                if (classAll == 0) // that means network never classified object as that class
                    classPrecision = 0;
                else 
                    classPrecision = classTruePositive / classAll;
                sumaricPrecision += classPrecision;
            }
            double meanPrecision = sumaricPrecision / classificationResults.RowCount;
            return meanPrecision;
        }

        /// <summary>
        /// Calculates arithmetic mean sensitivity
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        public static double CalculateSensitivity(this Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");
            int numberOfClasses = classificationResults.RowCount;
            double sumaricSensitivity = 0;
            for (int classNumber = 0; classNumber < classificationResults.ColumnCount; classNumber++) //here classes are enumerated from 0
            {
                double classSensitivity = 0;
                double classTruePositive = classificationResults[classNumber, classNumber]; //on diagonal
                double classAll = 0;
                for (int columnIndex = 0; columnIndex < classificationResults.ColumnCount; columnIndex++)
                {
                    classAll += classificationResults[classNumber, columnIndex];
                }
                if (classAll == 0) // that means there were no elements of given class, so don't count it at all
                {
                    classSensitivity = 0;
                    numberOfClasses--;
                }
                else
                    classSensitivity = classTruePositive / classAll;
                sumaricSensitivity += classSensitivity;
            }
            double meanSensitivity = sumaricSensitivity / numberOfClasses;
            return meanSensitivity;
        }

        /// <summary>
        /// Calculates arithmetic mean Specificity
        /// </summary>
        /// <param name="classificationResults"></param>
        /// <returns></returns>
        public static double CalculateSpecificity(this Matrix<double> classificationResults)
        {
            if (classificationResults.RowCount != classificationResults.ColumnCount)
                throw new ArgumentException("Matrix has to be square");
            Matrix<double> tempClassMatrix;
            #region check for empty rows. If happens, remove them temporary for calculating
            var emptyRowFlags = Vector<double>.Build.Dense(classificationResults.RowCount, 0);
            for (int rowIndex = 0; rowIndex < classificationResults.RowCount; rowIndex++)
            {
                if (classificationResults.Row(rowIndex).Sum() == 0)
                    emptyRowFlags[rowIndex] = 1;
            }
            if (emptyRowFlags.Sum() != 0) //empty rows existing
            {
                tempClassMatrix = Matrix<double>.Build.Dense((int)(classificationResults.RowCount - emptyRowFlags.Sum()), classificationResults.ColumnCount);
                int tempMatrixRowIndex = 0;
                for (int resultsMatrixRowIndex = 0; resultsMatrixRowIndex < classificationResults.RowCount; resultsMatrixRowIndex++)
                {
                    if (emptyRowFlags[resultsMatrixRowIndex] == 1 )
                    {
                        tempMatrixRowIndex++;
                        break;
                    }
                    for (int columnIndex = 0; columnIndex < classificationResults.ColumnCount; columnIndex++) //copy column
                    {
                        tempClassMatrix[tempMatrixRowIndex, columnIndex] = tempClassMatrix[resultsMatrixRowIndex, columnIndex];
                    }
                    tempMatrixRowIndex++;
                }
            }
            else
                tempClassMatrix = classificationResults;
            #endregion
            double sumaricSpecificity = 0;
            for (int classNumber = 0; classNumber < classificationResults.RowCount; classNumber++) //here classes are enumerated from 0
            {
                double classSpecificity = 0;
                double classTrueNegative = 0;
                for (int diagonalIndex = 0; diagonalIndex < classificationResults.RowCount; diagonalIndex++)
                {
                    if (diagonalIndex != classNumber) // all positives other than current class 
                        classTrueNegative += classificationResults[diagonalIndex, diagonalIndex];
                }
                double classAll = classTrueNegative; //first ingredient, second added below in parts
                for (int rowIndex = 0; rowIndex < classificationResults.RowCount; rowIndex++) // False Positive
                {
                    if(rowIndex != classNumber)
                        classAll += classificationResults[rowIndex, classNumber];
                }
                for (int columnIndex = 0; columnIndex < classificationResults.ColumnCount; columnIndex++) // False Negative
                {
                    if (columnIndex != classNumber)
                        classAll += classificationResults[classNumber, columnIndex];
                }
                classSpecificity = classTrueNegative / classAll;
                sumaricSpecificity += classSpecificity;
            }
            double meanSpecificity = sumaricSpecificity / classificationResults.ColumnCount;
            return meanSpecificity;
        }
    }
}
