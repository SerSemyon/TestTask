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
        StringFormat stringFormat = new StringFormat();
        Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        Pen whitePen = new Pen(Color.FromArgb(255, 255, 255, 255), 1);
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
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }

        public void AddLine(string text)
        {
            lines.Add(new OneLine(text, width, height, parent));
        }
        public void Clear()
        {
            lines.Clear();
        }

        public Point Draw(Graphics g, Font font, Point location)
        {
            position = location;
            g.DrawLine(whitePen, location.X+1, location.Y+1, location.X + height*5 + width - 1, location.Y+1);
            g.DrawLine(whitePen, location.X + 1, location.Y + 1, location.X + 1, location.Y + height - 1);
            g.DrawRectangle(blackPen, location.X, location.Y, height, height);
            g.DrawRectangle(blackPen, location.X + height, location.Y, width, height);
            g.DrawLine(whitePen, location.X + height + 1, location.Y + 1, location.X + height + 1, location.Y + height - 1);

            g.DrawString(text, font, new SolidBrush(Color.Black), new Point(location.X + height + width / 2, location.Y + height / 2), stringFormat);
            for (int i = 0; i < 4; i++)
            {
                g.DrawRectangle(blackPen, location.X + height + width + i * height, location.Y, height, height);
                g.DrawLine(whitePen, location.X + width + height*(i+1) + 1, location.Y + 1, location.X + width + height * (i + 1) + 1, location.Y + height - 1);
            }
            location.Y += height + yStep;
            if (lines.Count > 0)
            {
                if (!minimized)
                {
                    location.X += xStep;
                    Point lastElementPosition = location;
                    foreach (OneLine line in lines)
                    {
                        lastElementPosition = location;
                        g.DrawLine(whitePen, position.X + height / 2, location.Y + height / 2, location.X, location.Y + height / 2);
                        location = line.Draw(g, font, location);
                    }
                    location.X -= xStep;
                    g.DrawLine(whitePen, position.X + height / 2, position.Y + height + 1, position.X + height / 2, lastElementPosition.Y + height / 2);
                    g.DrawString("-", font, new SolidBrush(Color.Black), new Point(position.X + height / 2, position.Y + height / 2), stringFormat);
                }
                else
                {
                    g.DrawString("+", font, new SolidBrush(Color.Black), new Point(position.X + height / 2, position.Y + height / 2), stringFormat);
                }
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

        public void MovePointToEndElement(ref Point point)
        {
            point.Y += height + yStep;
            if (!minimized)
            {
                point.X += xStep;
                foreach (OneLine line in lines)
                {
                    line.MovePointToEndElement(ref point);
                }
                point.X -= xStep;
            }
        }

        public OneLine? FindSelectedObject(ref Point startingPoint, Point location)
        {
            if (location.X < startingPoint.X || location.Y < startingPoint.Y)
            {
                MovePointToEndElement(ref startingPoint);
                return null;
            }
            if (location.Y < startingPoint.Y + height && location.X < (startingPoint.X + width + height * 5))
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
                    {
                        startingPoint.X -= xStep;
                        return currenLine;
                    }
                }
                startingPoint.X -= xStep;
            }
            return null;
        }
    }
}
