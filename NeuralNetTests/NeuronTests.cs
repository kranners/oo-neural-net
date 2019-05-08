using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNeuralNet;

namespace NeuralNetTests
{
    [TestClass]
    public class NeuronTests
    {
        const int inputLength = 5;
        const double expected = 1;
        const double givenInput = 0;
        const double learningRate = 0.01;

        Neuron neuronToTest;
        Neuron[] testInputs = new Neuron[inputLength];
        IFunction activator; // just define that it's an activation function out here, define it as whatever inside the function

        [TestInitialize]
        public void Scaffold_Test_Neuron()
        {
            activator = new ReLU();
            for(int i=0; i<testInputs.Length; i++)
            {
                testInputs[i] = new Neuron();
            }
            neuronToTest = new Neuron(testInputs, activator, 2); // Defines each weight in the inputs as 1 for testing purposes

            foreach (Neuron inputNeuron in testInputs) {
                inputNeuron.Input = 1;
                inputNeuron.Activate(); // playing the role of the neuron layer
            }

            // Act for every test, test case
            neuronToTest.Input = givenInput;
            neuronToTest.Activate();
            neuronToTest.SetError(expected);
        }

        [TestMethod]
        public void SetActivation()
        {
            Assert.AreEqual(givenInput, neuronToTest.Activation);
        }

        [TestMethod]
        public void InheritActivation()
        {
            neuronToTest.Input = null;
            neuronToTest.Activate();
            Assert.AreEqual(11, neuronToTest.Activation); // should give us the weighted sum!
        }
        [TestMethod]
        public void OutputNeuronError()
        {
            Assert.AreEqual(expected - givenInput, neuronToTest.Error);
        }
        [TestMethod]
        public void ActivatingResetsError()
        {
            neuronToTest.Activate(); // Error should now reset here after a fresh activation
            Assert.AreEqual(0, neuronToTest.Error);
        }
        [TestMethod]
        public void BackpropUpdatesInputErrors()
        {
            neuronToTest.Backprop(learningRate);
            foreach (Neuron inputNeuron in testInputs) { if (inputNeuron.Error != (expected-givenInput)*2) Assert.Fail(); } 
            // if any of our input neurons have not had their error updated, fail
        }

        [TestMethod]
        public void TrainANodeUpwards()
        {
            neuronToTest.Input = null;
            neuronToTest.SetError(20);
            for (int i = 0; i < 2000; i++)
            {
                neuronToTest.Backprop(learningRate);
                neuronToTest.Activate();
                neuronToTest.SetError(20);
            }

            Assert.AreEqual(20, neuronToTest.Activation, 0.15);
        }

        [TestMethod]
        public void TrainANodeDownwards()
        {
            neuronToTest.Input = null;
            neuronToTest.SetError(0);
            for (int i = 0; i < 2000; i++)
            {
                neuronToTest.Backprop(learningRate);
                neuronToTest.Activate();
                neuronToTest.SetError(0);
            }

            Assert.AreEqual(0, neuronToTest.Activation, 0.15);
        }
    }
}
