using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwinGameSDK;

namespace MyGame.src
{
    public class NeuronGraphic
    {
        private Color fillColor;
        private Int32 alpha;
        private Rectangle nRect;
        private SimpleNeuralNet.Neuron reference;

        public NeuronGraphic(int x, int y, int size, SimpleNeuralNet.Neuron neuron)
        {
            reference = neuron;
            nRect = new Rectangle();
            nRect.X = x; nRect.Y = y; nRect.Width = size; nRect.Height = size;
        }

        public void Draw()
        {
            try
            {
                alpha = Convert.ToInt32(reference.Activation * 9);
            } catch (Exception)
            {
                alpha = 1;
            }
            fillColor = Color.FromArgb(-alpha * 11111111);
            SwinGame.DrawRectangle(fillColor, true, nRect);
        }
    }
}
