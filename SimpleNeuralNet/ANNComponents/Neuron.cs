using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Neuron
    {
        public  Neuron[] Inputs;
        private double[] Weights;

        /// <summary>
        /// Base constructor for input neurons ONLY!
        /// </summary>
        public Neuron() {
            Error = 0;
        }

        /// <summary>
        /// Constructor for hidden and output neurons
        /// </summary>
        /// <param name="inputs">Array of all neurons in its previous layer</param>
        /// <param name="weight">Optionally define all the weights, used for testing</param>
        public Neuron(Neuron[] inputs, IFunction activator, double? weight=null)
        {
            Inputs = inputs;
            Error = 0;
            ActivationFunction = activator;
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Random rand2 = new Random(Guid.NewGuid().GetHashCode());

            Bias = weight.HasValue ? 1 : (rand.NextDouble()-rand2.NextDouble());

            Weights = new double[inputs.Length];
            for (int i=0; i<inputs.Length; i++)
            {
                // Initializes all the weights, if there is no given weight then make a random one between -1 and 1
                Weights[i] = weight.HasValue ? weight.Value : (rand.NextDouble() - rand2.NextDouble())*2;
            }
        }

        public double? Input { get; set; }
        public double Error { get; protected set; }
        public double Activation { get; protected set; }
        private IFunction ActivationFunction { get; set; }

        public double Bias;

        public void SetError(double expected)
        {
            Error = expected - Activation; // How much should the activation change to match the expected value?
        }

        public void Activate()
        {
            Error = 0;
            if (Input.HasValue)
            {
                Activation = Input.Value;
            }
            else
            {
                double sum = 0;
                for(int i=0; i<Inputs.Length; i++)
                {
                    sum = sum + Inputs[i].Activation * Weights[i]; // weighted sum
                }
                Activation = ActivationFunction.Evaluate(sum + Bias); // activation function of the weighted sum
            }
        }

        public void Backprop(double learningRate)
        {
            double delta = ActivationFunction.EvaluateDelta(Activation) * Error * learningRate;
            Bias += delta;
            for(int i=0; i<Inputs.Length; i++)
            {
                Inputs[i].Error += Error * Weights[i];
                Weights[i] += Inputs[i].Activation * delta;
            }
        }
    }
}
