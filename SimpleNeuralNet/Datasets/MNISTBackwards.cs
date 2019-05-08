using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpleNeuralNet
{
    /// <summary>
    /// Dataset class used to read in the MNIST dataset
    /// </summary>
    public class MNISTBackwards : IDataset
    {
        private readonly int _sizeLimit;
        public byte[] trainingImages;
        public byte[] trainingLabels;
        public double[][] labelProperty;

        public double[][] imageProperty { get; private set; }

        public MNISTBackwards(string datapath, string labelpath, int sizelimit=2000)
        {
            trainingImages = File.ReadAllBytes(datapath);
            trainingLabels = File.ReadAllBytes(labelpath);
            if (sizelimit + 16 > trainingImages.Length) throw new ArgumentOutOfRangeException("Size limit + 16 (offset) cannot exceed set size.");
            _sizeLimit = sizelimit;
        }

        public double[][] GetLabelset()
        {
            List<double[]> imageSet = new List<double[]> { };
            // Very similar to getlabelset, offset by 16 this time
            for (int i = 0; i<_sizeLimit; i++){
                imageSet.Add(MakeImage(16 + (i * 784)));
            }
            imageProperty = imageSet.ToArray();
            return imageSet.ToArray();
        }

        public int GetLabelSize()
        {
            return 784; // this is needed for IDataSet to implement! Given input size of the MNIST database
        }

        public double[][] GetDataset()
        {
            List<double[]> labelSet = new List<double[]> { };
            // skip the first 8 (offset baked into the dataset), read the rest as ints 0-9
            for(int i=8; i<_sizeLimit+8; i++)
            {
                labelSet.Add(MakeLabel(i));
            }
            labelProperty = labelSet.ToArray();
            return labelSet.ToArray();
        }

        public int GetDataSize()
        {
            return 10;
        }

        private double[] MakeLabel(int index)
        {
            double[] label = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            label[trainingLabels[index]] = 1;

            return label;
        }

        private double[] MakeImage(int index)
        {
            double[] image = new double[784];
            for(int n=0; n<784; n++)
            {
                double pixelDouble = Convert.ToDouble(trainingImages[n + index]);
                image[n] = pixelDouble / 255; // cleans 0-255 to 0-1
            }
            return image;
        }
    }
}
