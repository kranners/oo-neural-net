using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Linear : IFunction
    {
        public double Evaluate(double x)
        {
            return x;
        }

        public double EvaluateDelta(double x)
        {
            return 1;
        }
    }
}
