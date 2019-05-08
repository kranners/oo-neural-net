using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNeuralNet;

namespace MyGame.src
{
    /// <summary>
    /// Graphical layer, will draw a layer as a square, filling it with all neurons in the set, enlarging them if needed
    /// </summary>
    public class LayerGraphic
    {
        private List<NeuronGraphic> Graphics;
        public LayerGraphic(int x, int y, int size, Layer layer)
        {
            int dimensions = Convert.ToInt16(Math.Ceiling(Math.Sqrt(layer.NeuronCount))); // expected dimensions, eg for 16 neurons we would expect a 4x4
            int psize = Convert.ToInt16(size / dimensions); // the size of each neuron to be drawn


            Graphics = new List<NeuronGraphic> { };
            for(int col=0; col<dimensions; col++)
            {
                for (int row=0; row<dimensions; row++)
                {
                    if (col * dimensions + row < layer.NeuronCount)
                    {
                        Graphics.Add(new NeuronGraphic(x + (row * psize), y + (col * psize), psize, layer.Neurons[col * dimensions + row]));
                    }
                }
            }
        }

        public void Draw()
        {
            foreach(NeuronGraphic sq in Graphics)
            {
                sq.Draw();
            }
        }
    }
}
