using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNeuralNet
{
    public class Network
    {
        public Network(int[] neuronCounts, IFunction activator, IDataset dataset, double dataRatio, double? weight=null)
        {
            // throw some errors first
            if (neuronCounts.Length < 2) throw new ArgumentOutOfRangeException("Network must have at least 2 layers");
            if (neuronCounts[0] != dataset.GetDataSize()) throw new ArgumentOutOfRangeException("Input neuron count must match 1:1 with input data size. Current neuron count: "+neuronCounts[0]+", current input size: "+dataset.GetDataSize());
            if (neuronCounts[neuronCounts.Length - 1] != dataset.GetLabelSize()) throw new ArgumentOutOfRangeException("Output neuron count must match 1:1 with output data size. Current neuron count: " + neuronCounts[neuronCounts.Length - 1] + ", current input size: " + dataset.GetLabelSize());

            // init new network based on neuroncounts
            Layers = new Layer[neuronCounts.Length];

            Layers[0] = new Layer(neuronCounts[0]);
            for(int c=1; c<neuronCounts.Length; c++)
            {
                Layers[c] = new Layer(neuronCounts[c], Layers[c - 1], activator, weight);
            }

            // create testdata and trainingdata
            double counter = Math.Round(1 / (1 - dataRatio)); // if data ratio is 0.8 then 80% of the data will be for training.
            // this means the counter should be 5, eg. every 5th entry will be test data rather than training data
            double[][] inputSet = dataset.GetDataset();
            double[][] outputSet = dataset.GetLabelset();

            List<double[]> tr_i = new List<double[]> { };
            List<double[]> tr_o = new List<double[]> { };
            List<double[]> te_i = new List<double[]> { };
            List<double[]> te_o = new List<double[]> { };


            for (int i=0; i<inputSet.Length; i++)
            {
                if(i % counter == 0) // how we test whether to add something to test data or training data
                {
                    // test data goes in here
                    te_i.Add(inputSet[i]);
                    te_o.Add(outputSet[i]);
                } else
                {
                    // training data goes in here
                    tr_i.Add(inputSet[i]);
                    tr_o.Add(outputSet[i]);
                }
            }

            TrainingInputs = tr_i.ToArray();
            TrainingOutputs = tr_o.ToArray();
            TestingInputs = te_i.ToArray();
            TestingOutputs = te_o.ToArray();
            GlobalError = 1337;
        }

        private double[][] TrainingInputs;
        private double[][] TrainingOutputs;
        private double[][] TestingInputs;
        private double[][] TestingOutputs;

        public Layer[] Layers { get; protected set; }
        public double[] Output { get
            {
                double[] output = new double[OutputLayer.Neurons.Length];
                for(int n=0; n<OutputLayer.Neurons.Length; n++)
                {
                    output[n] = OutputLayer.Neurons[n].Activation;
                }
                return output;
            }
        }
        public double GlobalError { get; protected set; }

        public Layer OutputLayer { get
            {
                return Layers[Layers.Length - 1];
            }
        }

        public void Input(double[] inputData)
        {
            Layers[0].InputToNeurons(inputData);
        }

        public void Feedforward()
        {
            for(int i=0; i<Layers.Length; i++)
            {
                Layers[i].ActivateNeurons();
            }
        }

        public void Backpropagate(double[] expectedOutput, double learningRate)
        {
            OutputLayer.SetErrorNeurons(expectedOutput);
            for(int n=Layers.Length-1; n>=1; n--)
            {
                Layers[n].BackpropAll(learningRate);
            }
        }

        public void Train(int epochs, double learningRate)
        {
            if (TrainingInputs.Length != TrainingOutputs.Length) throw new ArgumentOutOfRangeException("Length of input data must match length of output data");
            for(int e=1; e<=epochs; e++)
            {
                Console.WriteLine("epoch " + Convert.ToString(e));
                for(int i=0; i<TrainingInputs.Length; i++)
                {
                    Input(TrainingInputs[i]);
                    Feedforward();
                    Backpropagate(TrainingOutputs[i], learningRate);
                    if(i % 100 == 0)
                    {
                        double percentage = Math.Round(((double) i / (double) TrainingInputs.Length) * 100, 2);
                        Console.WriteLine(Convert.ToString(percentage) + "%");
                    }
                }
            }
        }

        public void Test()
        {
            double errorSum = 0;
            for(int i=0; i<TestingInputs.Length; i++)
            {
                Input(TestingInputs[i]);
                Feedforward();
                errorSum += OutputError(TestingOutputs[i]);
            }
            GlobalError = errorSum / TestingInputs.Length;
        }

        // should only be run after backpropping!
        public double SingleTest()
        {
            double errorSum = 0;
            for(int i=0; i<OutputLayer.NeuronCount; i++)
            {
                errorSum += Math.Abs(OutputLayer.Neurons[i].Error);
            }
            return errorSum;
        }

        private double OutputError(double[] expectedOutput)
        {
            OutputLayer.SetErrorNeurons(expectedOutput);

            double OutputErrorSum = 0;
            foreach(Neuron outputNeuron in OutputLayer.Neurons)
            {
                OutputErrorSum += outputNeuron.Error;
            }
            return OutputErrorSum;
        }

        public double[] WhatIs(double[] dataInput)
        {
            Input(dataInput);
            Feedforward();
            return Output;
        }

        public string FlavorOutput(double[] dataInput)
        {
            return String.Join(",", WhatIs(dataInput).Select(o => o.ToString()).ToArray());
            throw new Exception();
        }
    }
}
