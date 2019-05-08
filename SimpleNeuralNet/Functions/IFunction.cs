namespace SimpleNeuralNet
{
    /// <summary>
    /// Activation function interface, must be able to return its value and its derivative.
    /// </summary>
    public interface IFunction
    {
        double Evaluate(double x);
        double EvaluateDelta(double x);
    }
}