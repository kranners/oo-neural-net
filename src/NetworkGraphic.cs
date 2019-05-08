using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNeuralNet;

namespace MyGame.src
{
    public class NetworkGraphic
    {
        private List<LayerGraphic> Graphics;
        public NetworkGraphic(int x, int y, int width, int height, Network network)
        {
            Graphics = new List<LayerGraphic> { };
            // will try to fill the width given with layers of a given height
            int layerCount = network.Layers.Length;
            for(int layer=0; layer<layerCount; layer++)
            {
                Graphics.Add(new LayerGraphic(x + (width / layerCount) * layer, y, height, network.Layers[layer]));
            }
        }
        public void Draw()
        {
            foreach(LayerGraphic layer in Graphics)
            {
                layer.Draw();
            }
        }
    }
}
