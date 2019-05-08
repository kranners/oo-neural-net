using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Softplus : IFunction
    {
        public double Evaluate(double x)
        {
            return Math.Log(1 + Math.Exp(x));
        }

        public double EvaluateDelta(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}
