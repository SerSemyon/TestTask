using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    class Button
    {
        string text;
        EventHandler OnClick;
        int lineWidth = 80;
        int lineHeight = 20;
        StringFormat stringFormat = new StringFormat();
        Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);

        public Button(string str, EventHandler command)
        {
            text = str;
            OnClick = command;
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }

        public void Click()
        {
            OnClick(this, EventArgs.Empty);
        }

        public void Draw(Graphics g, Font font, ref Point location)
        {
            g.DrawString(text, font, new SolidBrush(Color.Black), new Point(location.X + lineWidth / 2, location.Y + lineHeight / 2), stringFormat);
            g.DrawRectangle(blackPen, location.X, location.Y, lineWidth, lineHeight);
            location.Y += lineHeight;
        }

        public bool Intersect(ref Point startingPoint, Point location)
        {
            if (location.X < startingPoint.X || location.Y < startingPoint.Y
                || location.X > startingPoint.X + lineWidth || location.Y > startingPoint.Y + lineHeight)
            {
                startingPoint.Y += lineHeight;
                return false;
            }
            startingPoint.Y += lineHeight;
            return true;
        }
    }
}
