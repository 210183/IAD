using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public class ClassificationDataProvider : DataProvider
    {

        public ClassificationDataProvider(string fileName, int inputsNumber, int outputsNumber, bool isBiasOn) 
        {
            DataSet = SetData(fileName, inputsNumber, outputsNumber, isBiasOn);
        }

        protected Datum[] SetData(string fileName,int inputsNumber, int outputsNumber, bool isBiasOn)
        {
            int biasModifier = 0;
            if (isBiasOn)
            {
                biasModifier = 1;
            }

            var tempInputOutputSet = base.LoadFileToStringTable(inputsNumber, outputsNumber, fileName);
            Datum[] tempData = new Datum[tempInputOutputSet.Length];

            var singleInputData = Vector<double>.Build.Dense(inputsNumber + biasModifier, 0);
            var singleOutputData = Vector<double>.Build.Dense(outputsNumber, 0);

            string[] tempLine;

            var delimiter = ' ';

            for (int i = 0; i < tempInputOutputSet.Count(); i++)
            {
                singleInputData.Clear();
                singleOutputData.Clear();

                tempLine = tempInputOutputSet[i].Split(delimiter);
                if (tempLine.Count() == inputsNumber + 1) //1 for outputs column
                {
                    if (isBiasOn)
                        singleInputData[0] = 1;

                    for (int j = biasModifier; j < inputsNumber + biasModifier; j++)
                    {
                        singleInputData[j] = Convert.ToDouble(tempLine[j - biasModifier]);
                    }

                    int classNumber  = Convert.ToInt16(tempLine[inputsNumber]);
                    singleOutputData[classNumber - 1] = 1;

                }
                tempData[i] = new Datum(Vector<double>.Build.DenseOfVector(singleInputData), Vector<double>.Build.DenseOfVector(singleOutputData));
            }
            return tempData;
        }
    }
}
