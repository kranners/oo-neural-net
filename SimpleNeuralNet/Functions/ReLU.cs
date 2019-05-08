using System;

namespace SimpleNeuralNet
{
    public class ReLU : IFunction
    {
        public double Evaluate(double x)
        {
            return Math.Max(0, x);
        }

        public double EvaluateDelta(double x)
        {
            // If x=0, arbitrary. x>0, 1. x<0,0.
            //return x == 0 ? 0.5 : x > 0 ? 1 : 0;
            return x > 0 ? 1 : 0;
        }
    }
}
