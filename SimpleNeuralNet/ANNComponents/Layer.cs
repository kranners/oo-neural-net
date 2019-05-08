using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Layer
    {
        /// <summary>
        /// Base layer constructor, for input layers ONLY!
        /// </summary>
        /// <param name="neuronCount">Amount of neurons in the layer</param>
        public Layer(int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            NeuronCount = neuronCount;
            for(int i=0; i<neuronCount; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        /// <summary>
        /// Non-base case, for hidden and output layers
        /// </summary>
        /// <param name="neuronCount">Amount of neurons in the layer</param>
        /// <param name="inputLayer">The layer that it connects to</param>
        /// <param name="weight">Optional, give a set weight for testing purposes</param>
        public Layer(int neuronCount, Layer inputLayer, IFunction activator, double? weight=null)
        {
            Neurons = new Neuron[neuronCount];
            NeuronCount = neuronCount;
            for(int i=0; i<neuronCount; i++)
            {
                Neurons[i] = new Neuron(inputLayer.Neurons, activator, weight);
            }
        }

        public Neuron[] Neurons { get; protected set; }
        public int NeuronCount { get; }

        public void InputToNeurons(double[] neuronInput)
        {
            DataCheck(neuronInput, "Input");
            for(int i=0; i<NeuronCount; i++)
            {
                Neurons[i].Input = neuronInput[i];
            }
        }

        public void SetErrorNeurons(double[] expectedOutput)
        {
            DataCheck(expectedOutput, "Output");
            for (int i=0; i<NeuronCount; i++)
            {
                Neurons[i].SetError(expectedOutput[i]);
            }
        }

        public void ActivateNeurons()
        {
            foreach(Neuron neuronToActivate in Neurons) { neuronToActivate.Activate(); }
        }

        public void BackpropAll(double learningRate)
        {
            foreach(Neuron neuronToBackprop in Neurons) { neuronToBackprop.Backprop(learningRate); }
        }

        private void DataCheck(double[] inputData, string layerType)
        {
            if (inputData.Length != NeuronCount) throw new ArgumentOutOfRangeException(layerType + " must match 1:1. Size given: "+inputData.Length+", neuron count: "+NeuronCount);
        }
    }
}
