using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwinGameSDK;

namespace MyGame.src
{
    public class GraphGraphic
    {
        private List<double> yValues;
        private int xValue;
        private int x, y, w, h;
        private string yLabel, xLabel;

        public GraphGraphic(int x, int y, int w, int h, string yLabel, string xLabel)
        {
            yValues = new List<double> { };
            xValue = 0;
            this.x = x; this.y = y;
            this.w = w; this.h = h;
            this.yLabel = yLabel;
            this.xLabel = xLabel;
        }

        public void pushValue (double y)
        {
            yValues.Add(y); // add a new value to the graph
            xValue++; // increment the x
        }

        public void draw()
        {
            SwinGame.DrawLine(Color.Black, x, y, x + w, y); // draw the horizontal axis
            SwinGame.DrawLine(Color.Black, x, y - h, x, y); // draw the vertical axis
            SwinGame.DrawText(yLabel, Color.Black, "yLabel", 15, x - 30, y - (h / 2)); // draw on the y axis label
            SwinGame.DrawText(xLabel, Color.Black, "xLabel", 15, x + (w / 2), y + 30); // draw on the x axis label

            drawGraph();
        }

        // will draw on the actual graph labels
        private void drawGraph()
        {
            List<double> dataToUse;
            if (yValues.Count > w) // if the data we have is longer than the graph can display
            {
                dataToUse = yValues.GetRange(yValues.Count - w, w); // gets the w most recent values
            } else
            {
                dataToUse = yValues; // otherwise sets the values to just be it
            }
            double maxValue = dataToUse.Max(); // maximum value, will be displayed at the top

            for(int i=0; i<dataToUse.Count; i++)
            {
                Point2D dataPoint = new Point2D();
                dataPoint.X = (float) (x + i);
                dataPoint.Y = (float) (y - (dataToUse[i] / maxValue) * h); // if it IS the max value, then it should be at the top of the graph
                // draw each line segment
                SwinGame.DrawPixelOnScreen(Color.Red, dataPoint);
            }
        }
    }
}
