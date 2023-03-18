using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask;

namespace TestTask
    {
    class OneLine
    {
        public Point position;
        public string text;
        public List<OneLine> lines;
        public bool minimized = false;
        private int width = 80, height = 20;
        public int xStep, yStep;
        private TestElement parent;
        public int Widht
        {
            set
            {
                width = value;
                foreach (var line in lines)
                {
                    line.Widht = value;
                }
            }
            get { return width; }
        }
        public int Height
        {
            set
            {
                height = value;
                xStep = height - 5;
                foreach (var line in lines)
                {
                    line.Height = value;
                }
            }
            get { return height; }
        }
        public OneLine(string content, int lineWidth, int lineHeight, TestElement parent)
        {
            text = content;
            lines = new List<OneLine>();
            width = lineWidth;
            height = lineHeight;
            xStep = height - 5;
            yStep = 3;
            this.parent = parent;
        }
        public void AddLine(string text)
        {
            lines.Add(new OneLine(text, width, height, parent));
        }
        public void Delete()
        {
            lines.Clear();

        }
        public Point Draw(Graphics g, Font font, Point location)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            position = location;
            g.DrawRectangle(blackPen, location.X, location.Y, height, height);
            g.DrawRectangle(blackPen, location.X + height, location.Y, width, height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(text, font, new SolidBrush(Color.Black), new Point(location.X + height + width / 2, location.Y + height / 2), stringFormat);
            for (int i = 0; i < 4; i++)
            {
                g.DrawRectangle(blackPen, location.X + height + width + i * height, location.Y, height, height);
            }
            location.Y += height + yStep;
            if (!minimized)
            {
                location.X += xStep;
                foreach (OneLine line in lines)
                {
                    location = line.Draw(g, font, location);
                }
                location.X -= xStep;
            }
            return location;
        }

        public void DrawSelect(Graphics g, Brush fillBrush)
        {
            g.FillRectangle(fillBrush, position.X + height, position.Y, width, height);
        }

        public void Click(Point point)
        {
            parent.locationSelectedLine = point;
            minimized = !minimized;
        }
        public OneLine? FindSelectedObject(ref Point startingPoint, Point location)
        {
            if (location.X < startingPoint.X || location.Y < startingPoint.Y)
                return null;
            if (location.Y - startingPoint.Y < height)
            {
                Click(startingPoint);
                return this;
            }
            startingPoint.Y += height + yStep;
            if (!minimized)
            {
                startingPoint.X += xStep;
                foreach (OneLine line in lines)
                {
                    OneLine? currenLine = line.FindSelectedObject(ref startingPoint, location);
                    if (currenLine != null)
                        return currenLine;
                }
                startingPoint.X -= xStep;
            }
            return null;
        }
    }
}
