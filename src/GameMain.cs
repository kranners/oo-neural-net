using System;
using SwinGameSDK;
using SimpleNeuralNet;
using MyGame.src;
using System.Linq;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            int count = 0;
            int dataSize = 10000;
            IFunction activator = new Sigmoid();
            IDataset dataset = new MNISTReader("Data/train-images", "Data/train-labels", dataSize);
            double[][] inputTests = dataset.GetDataset();
            double[][] outputTests = dataset.GetLabelset();
            int[] neuronCounts = new int[] { dataset.GetDataSize(), 50, dataset.GetLabelSize() };
            double dataRatio = 1;

            Network magicBox = new Network(neuronCounts, activator, dataset, dataRatio);
            NetworkGraphic graphicMagic = new NetworkGraphic(0, 250, 800, 100, magicBox);
            bool paused = true;
            bool drawstuff = true;

            GraphGraphic networkGraph = new GraphGraphic(250, 200, 400, 100, "Error", "Iteration");

            //Open the game window
            SwinGame.OpenGraphicsWindow("Neuralnet Project", 800, 600);


            //Run the game loop
            while (false == SwinGame.WindowCloseRequested())
            {

                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();

                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.White);
                SwinGame.DrawFramerate(0, 0);
                if (drawstuff)
                {
                    networkGraph.draw();
                    graphicMagic.Draw();

                    // Going to find the highest value (the one the neural net thinks it is) and print it!
                    double maxValue = magicBox.Output.Max();
                    int maxIndex = magicBox.Output.ToList().IndexOf(maxValue);
                    SwinGame.DrawText(Convert.ToString(maxIndex), Color.Black, 650, 225);
                }
                else
                { 
                    SwinGame.DrawText("Busy training...", Color.Black, 250, 250);
                }
                if (SwinGame.KeyReleased(KeyCode.vk_p))
                {
                    drawstuff = !drawstuff;
                }
                if (SwinGame.KeyReleased(KeyCode.vk_n))
                {
                    magicBox.Train(1, 0.05);
                }
                if (!paused || SwinGame.KeyReleased(KeyCode.vk_RIGHT))
                {
                    count++;
                    if (count >= dataSize)
                    {
                        count = 0;
                    }
                    magicBox.Input(inputTests[count]);
                    magicBox.Feedforward();
                    magicBox.Backpropagate(outputTests[count], 0.05);

                    networkGraph.pushValue(magicBox.SingleTest());
                }

                if (SwinGame.KeyReleased(KeyCode.vk_SPACE))
                {
                    paused = !paused;
                }

                SwinGame.DrawText(Convert.ToString(count), Color.Black, 50, 225);

                //Draw onto the screen
                SwinGame.RefreshScreen(60);
            }
        }
    }
}