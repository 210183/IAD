using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNetworks.Data
{
    public class PointsDataProvider : BasicDataProvider, IDataProvider
    {
        public Datum[] Points { get; set; }

        PointsDataProvider(string fileName, int inputsNumber)
        {
            Points = SetData(fileName, inputsNumber);
        }

        private Datum[] SetData(string fileName, int inputsNumber)
        {

            var tempInputOutputSet = base.LoadFileToStringTable(fileName);
            Datum[] tempData = new Datum[tempInputOutputSet.Length];

            var singleInputData = Vector<double>.Build.Dense(inputsNumber, 0);

            string[] tempLine;

            var delimiter = ',';

            for (int i = 0; i < tempInputOutputSet.Count(); i++)
            {
                singleInputData.Clear();
             

                tempLine = tempInputOutputSet[i].Split(delimiter);
                if (tempLine.Count() == inputsNumber )
                {

                    for (int j = 0; j < inputsNumber ; j++)
                    {
                        singleInputData[j] = Convert.ToDouble(tempLine[j]);
                    }
                    
                }
                tempData[i] = new Datum(Vector<double>.Build.DenseOfVector(singleInputData));
            }
            return tempData;
        }

    }
}
