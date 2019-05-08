using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    /// <summary>
    /// Simple test dataset of the XOR gate, with expected inputs and outputs. Repeated twice for equal training and testing data
    /// </summary>
    public class XORSet : IDataset
    {
        private double[][] dataSet = new double[][] {
            new double[] { 0, 0 }, new double[] { 0, 0 },
            new double[] { 0, 1 }, new double[] { 0, 1 },
            new double[] { 1, 0 }, new double[] { 1, 0 },
            new double[] { 1, 1 }, new double[] { 1, 1 }
        };

        private double[][] labelSet = new double[][]
        {
            new double[] { 0 }, new double[] { 0 },
            new double[] { 1 }, new double[] { 1 },
            new double[] { 1 }, new double[] { 1 },
            new double[] { 0 }, new double[] { 0 }
        };
        public double[][] GetDataset()
        {
            return dataSet;
        }

        public int GetDataSize()
        {
            return 2;
        }

        public double[][] GetLabelset()
        {
            return labelSet;
        }

        public int GetLabelSize()
        {
            return 1;
        }
    }
}
