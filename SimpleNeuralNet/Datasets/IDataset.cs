using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    /// <summary>
    /// Simple interface for what is and isn't a dataset
    /// </summary>
    public interface IDataset
    {
        int GetDataSize();
        int GetLabelSize();

        double[][] GetDataset();
        double[][] GetLabelset();
    }
}
