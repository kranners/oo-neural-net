using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNeuralNet;

namespace NeuralNetTests
{
    [TestClass]
    public class LayerTests
    {
        Layer layerToTest, testOutputLayer;
        double[] neuronInput = new double[5] { 1, 1, 1, 1, 1 };
        double[] expectedOutput = new double[5] { 0, 0, 0, 0, 0 };
        IFunction activator;
        double learningRate;

        [TestInitialize]
        public void Scaffold_Testing_Layer()
        {
            activator = new ReLU();
            layerToTest = new Layer(5);
            learningRate = 0.01;

            layerToTest.InputToNeurons(neuronInput);


            testOutputLayer = new Layer(5, layerToTest, activator, 2); // generates a layer with all its neurons taking in the previous layer as inputs!
            // Essentially, this layer becomes the output to the previous layer

            // If this creation of the output layer worked, then by backpropping one of the neurons, all the previous neurons' errors should be 2
            testOutputLayer.InputToNeurons(neuronInput);
            testOutputLayer.ActivateNeurons();
            testOutputLayer.SetErrorNeurons(expectedOutput);
        }

        [TestMethod]
        public void InputLayer()
        {
            foreach(Neuron inputNeuron in layerToTest.Neurons) { Assert.AreEqual(1, inputNeuron.Input); }
        }

        [TestMethod]
        public void ActivateAll()
        {
            layerToTest.ActivateNeurons();
            foreach(Neuron inputNeuron in layerToTest.Neurons) { Assert.AreEqual(1, inputNeuron.Activation); }
        }

        [TestMethod]
        public void OutputLayerError()
        {
            layerToTest.ActivateNeurons();
            layerToTest.SetErrorNeurons(expectedOutput);
            foreach(Neuron outputNeuron in layerToTest.Neurons) { Assert.AreEqual(-1, outputNeuron.Error); }
        }

        [TestMethod]
        public void CreateOutputLayer()
        {
            testOutputLayer.Neurons[0].Backprop(learningRate); // if this works in NeuronTests, then it is behaving as expected
            foreach (Neuron inputNeuron in layerToTest.Neurons) { Assert.AreEqual(-2, inputNeuron.Error); }
        }

        [TestMethod]
        public void BackpropAll()
        {
            testOutputLayer.BackpropAll(learningRate);
            foreach(Neuron inputNeuron in layerToTest.Neurons) { Assert.AreEqual(-10, inputNeuron.Error); }
        }
    }
}
