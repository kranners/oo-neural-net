using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Tanh : IFunction
    {
        public double Evaluate(double x)
        {
            return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
        }

        public double EvaluateDelta(double x)
        {
            return 1 - Math.Pow(Evaluate(x), 2);
        }
    }
}
