using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data
{
    public class ClassificationDataProvider : DataWithDProvider
    {

        public ClassificationDataProvider(string fileName, int inputsNumber, int outputsNumber, bool isBiasOn) 
        {
            DataSet = SetData(fileName, inputsNumber, outputsNumber, isBiasOn);
        }

        protected DatumWithD[] SetData(string fileName,int inputsNumber, int outputsNumber, bool isBiasOn)
        {
            int biasModifier = 0;
            if (isBiasOn)
            {
                biasModifier = 1;
            }

            var tempInputOutputSet = base.LoadFileToStringTable(fileName);
            DatumWithD[] tempData = new DatumWithD[tempInputOutputSet.Length];

            var singleInputData = Vector<double>.Build.Dense(inputsNumber + biasModifier, 0);
            var singleOutputData = Vector<double>.Build.Dense(outputsNumber, 0);

            string[] tempLine;
            int numberOfAttributesInLine;
            var delimiter = ' ';

            for (int i = 0; i < tempInputOutputSet.Count(); i++)
            {
                singleInputData.Clear();
                singleOutputData.Clear();

                tempLine = tempInputOutputSet[i].Split(delimiter);
                numberOfAttributesInLine = tempLine.Length - 1;
                if (tempLine.Count() >= inputsNumber + 1) //1 for outputs column
                {
                    if (isBiasOn)
                        singleInputData[0] = 1;

                    for (int j = biasModifier; j < inputsNumber + biasModifier; j++)
                    {
                        singleInputData[j] = Convert.ToDouble(tempLine[j - biasModifier]);
                    }
                    
                    int classNumber  = Convert.ToInt16(tempLine[inputsNumber + (numberOfAttributesInLine - inputsNumber)]); //skip unused attribute
                    singleOutputData[classNumber - 1] = 1;

                }
                tempData[i] = new DatumWithD(Vector<double>.Build.DenseOfVector(singleInputData), Vector<double>.Build.DenseOfVector(singleOutputData));
            }
            return tempData;
        }
    }
}
