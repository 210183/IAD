using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Data.Tests
{
    [TestClass()]
    public class ClassificationDataProviderTests
    {
        [TestMethod()]
        public void ClassificationDataProviderTest()
        {
              string clearnFileName = @"C:\Users\Jakub\Desktop\classification_train.txt";
              string ctestFileName = @"C:\Users\Jakub\Desktop\classification_test.txt";

              string alearnFileName = @"C:\Users\Jakub\Desktop\approximation_train_1.txt";
              string atestFileName = @"C:\Users\Jakub\Desktop\approximation_test.txt";


              var newProvider = new LearningClassificationDataProvider(clearnFileName, ctestFileName, 4, 3, true);

              var aproxProvider = new LearningApproximationDataProvider(alearnFileName, atestFileName, 1, 1, true);
               


            
        }
    }
}