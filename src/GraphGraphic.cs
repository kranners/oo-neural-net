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
        private double MaxValue;

        public GraphGraphic(int x, int y, int w, int h, string yLabel, string xLabel)
        {
            MaxValue = 0;
            yValues = new List<double> { 0 };
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
            SwinGame.DrawText(yLabel, Color.Black, x - 30, y - (h / 2)); // draw on the y axis label
            SwinGame.DrawText(xLabel, Color.Black, x + (w / 2), y + 30); // draw on the x axis label

            if (yValues.Count > w) yValues.RemoveAt(0);

            //drawTrendline();
            drawGraph();
            drawAverage();
        }

        // will draw on the actual graph labels
        private void drawGraph()
        {
            if(yValues.Max() > MaxValue)
            {
                MaxValue = yValues.Max();
            }

            for (int i = 0; i < yValues.Count; i++)
            {
                Point2D dataPoint = new Point2D();
                dataPoint.X = (float)(x + i);
                dataPoint.Y = (float)(y - (yValues[i] / MaxValue) * h); // if it IS the max value, then it should be at the top of the graph
                // draw each line segment
                SwinGame.DrawPixelOnScreen(Color.Red, dataPoint);
            }
        }

        // will draw a blue trendline on the graph showcasing its average value over time
        private void drawTrendline()
        {
            double xSum, ySum, xySum, xxSum;
            xSum = 0; ySum = 0; xySum = 0; xxSum = 0;
            int count = yValues.Count;

            for(int i=0; i<count; i++)
            {
                xSum += i; // sum of all x values
                ySum += yValues[i];
                xySum += i * yValues[i];
                xxSum = i * i;
            }

            double slope = ((count * xySum) - (xSum * ySum)) / ((count * xxSum) - (xSum * xSum)) * 1000;
            double intercept = (ySum - (slope * xSum)) / count;
            double last = count * slope + intercept;

            SwinGame.DrawLine(Color.Blue, x, (float) intercept, x + w, (float) last);
        }

        private void drawAverage()
        {
            SwinGame.DrawText(Convert.ToString(Math.Round(yValues.Average(), 3)), Color.Black, x + w + 15, y);
        }
    }
}
