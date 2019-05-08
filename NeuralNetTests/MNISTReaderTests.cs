using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNeuralNet;

namespace NeuralNetTests
{
    [TestClass]
    public class MNISTReaderTests
    {
        MNISTReader reader;
        int sizelimit;

        [TestInitialize]
        public void Scaffold_Reader()
        {
            sizelimit = 100;
            reader = new MNISTReader("Data/train-images", "Data/train-labels", sizelimit);
        }

        [TestMethod]
        public void CleanLabels()
        {
            double[][] labelSet = reader.GetLabelset();
            Assert.AreEqual(sizelimit, labelSet.Length);
            Assert.AreEqual(10, labelSet[0].Length);
        }

        [TestMethod]
        public void CleanImages()
        {
            double[][] imageSet = reader.GetDataset();
            Assert.AreEqual(sizelimit, imageSet.Length);
            Assert.AreEqual(784, imageSet[0].Length);
        }
    }
}
