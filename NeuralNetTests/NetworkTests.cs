using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNeuralNet;

namespace NeuralNetTests
{
    [TestClass]
    public class NetworkTests
    {
        Network networkToTest, trainedNetwork;
        int[] neuronCounts;
        IFunction activator;
        double givenWeight, learningRate, dataRatio;
        IDataset XORData;

        [TestInitialize]
        public void Scaffold_Network_Case()
        {
            // Lets define some data!
            XORData = new XORSet();
            dataRatio = 0.5;

            // Our test case is just to have a network that matches an XOR gate nearly completely well

            activator = new ReLU();
            givenWeight = 3;
            learningRate = 0.1;
            neuronCounts = new int[] { 2, 2, 1 };
            networkToTest = new Network(neuronCounts, activator, XORData, dataRatio, givenWeight); // neuronCounts specifies the number of neurons in each layer, read left to right, input to output
            networkToTest.Input(new double[] { 1, 1 });
        }

        [TestMethod]
        public void ConstructorNeuronLinks()
        {
            // On initializing, the neurons should all connect to eachother
            networkToTest.Layers[1].InputToNeurons(new double[] { 1, 1 });
            networkToTest.Layers[1].ActivateNeurons(); // Sets activation to 1,1
            networkToTest.Layers[1].SetErrorNeurons(new double[] { 0, 0 }); // Sets error to 0,0
            networkToTest.Layers[1].BackpropAll(learningRate); // Backprops the error to the previous layer
            foreach(Neuron inputLayerNeuron in networkToTest.Layers[0].Neurons) { Assert.AreEqual(-2*givenWeight, inputLayerNeuron.Error); }
            // Every neuron in the first layer should now have an error of -2
        }

        [TestMethod]
        public void ConstructorNeuronLinksBackForReal()
        {
            // Checks that the inputs leading into the second layer are identical to the neurons in the input layer
            CollectionAssert.AreEqual(networkToTest.Layers[1].Neurons[0].Inputs, networkToTest.Layers[0].Neurons);
            CollectionAssert.AreEqual(networkToTest.Layers[2].Neurons[0].Inputs, networkToTest.Layers[1].Neurons);
        }

        [TestMethod]
        public void InputToInputLayer()
        {
            foreach(Neuron inputLayerNeuron in networkToTest.Layers[0].Neurons) { Assert.AreEqual(1, inputLayerNeuron.Input); }
        }

        [TestMethod]
        public void FeedforwardProper()
        {
            networkToTest.Feedforward();
            foreach (Neuron outputLayerNeuron in networkToTest.OutputLayer.Neurons) { Assert.AreEqual((givenWeight*2)*givenWeight*2 + 7, outputLayerNeuron.Activation); }
        }

        [TestMethod]
        public void WhatIsShorthand()
        {
            Assert.AreEqual((givenWeight * 2) * givenWeight * 2 + 7, networkToTest.WhatIs(new double[] { 1, 1 })[0]);
        }

        [TestMethod]
        public void OutputIsEqual()
        {
            networkToTest.Feedforward();
            double[] outputLayerTest = new double[1];
            for(int n=0; n<1; n++)
            {
                outputLayerTest[n] = networkToTest.Layers[2].Neurons[n].Activation;
            }
            CollectionAssert.AreEqual(outputLayerTest, networkToTest.Output);
        }

        [TestMethod]
        public void Backpropagate()
        {
            networkToTest.Input(new double[] { 1, 1 });
            networkToTest.Feedforward();
            networkToTest.Backpropagate(new double[] { 0 }, learningRate);
            Assert.AreEqual(-(networkToTest.Output[0]+1)*3+3, networkToTest.Layers[1].Neurons[0].Error);
        }

        [TestMethod]
        public void FeedforwardTwice()
        {
            networkToTest.Input(new double[] { 1, 1 });
            networkToTest.Feedforward();
            networkToTest.Feedforward();

            foreach (Neuron outputLayerNeuron in networkToTest.OutputLayer.Neurons) { Assert.AreEqual((givenWeight * 2) * givenWeight * 2 + 7, outputLayerNeuron.Activation); }
        }

        [TestMethod]
        public void FeedThenBackThenFeed()
        {
            networkToTest.Feedforward();
            networkToTest.Backpropagate(new double[] { 0 }, learningRate);
            networkToTest.Feedforward();


            foreach (Neuron outputLayerNeuron in networkToTest.OutputLayer.Neurons) { Assert.AreEqual(0, outputLayerNeuron.Activation, 5); }
        }

        [TestMethod]
        public void Train_On_Data()
        {
            int epochs = 200;
            networkToTest.Train(epochs, learningRate);
            networkToTest.Test();
            Assert.AreEqual(0, networkToTest.GlobalError, 0.15);
        }

        [TestMethod]
        public void Train_An_XORGate()
        {
            IFunction activator = new Softplus();
            trainedNetwork = new Network(neuronCounts, activator, XORData, dataRatio);
            int epochs = 2000;
            trainedNetwork.Train(epochs, learningRate);
            Assert.AreEqual(trainedNetwork.WhatIs(new double[] { 0, 0 })[0], 0, 0.1); // giving an XORgate 0,0 should return 0
            Assert.AreEqual(trainedNetwork.WhatIs(new double[] { 1, 0 })[0], 1, 0.1); // giving an XORgate 1,0 should return 1
            Assert.AreEqual(trainedNetwork.WhatIs(new double[] { 0, 1 })[0], 1, 0.1); // giving an XORgate 0,1 should return 1
            Assert.AreEqual(trainedNetwork.WhatIs(new double[] { 1, 1 })[0], 0, 0.1); // giving an XORgate 1,1 should return 0
        }
    }
}
