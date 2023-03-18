using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    class ContextMenu
    {
        int lineWidth = 80;
        int lineHeight = 20;
        public Point position;
        public bool hide = true;
        Button[] buttons = new Button[6];

        public ContextMenu()
        {
            position = new Point(0, 0);
            buttons[0] = new Button("Добавить", TestElemenCommand.add);
            buttons[1] = new Button("Удалить", TestElemenCommand.delete);
            buttons[2] = new Button("+ ширина", TestElemenCommand.incWidth);
            buttons[3] = new Button("- ширина", TestElemenCommand.decWidth);
            buttons[4] = new Button("+ высота", TestElemenCommand.incHeight);
            buttons[5] = new Button("- высота", TestElemenCommand.decHeight);
        }
        public Point Draw(Graphics g, Font font)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            g.DrawRectangle(blackPen, position.X, position.Y, lineWidth, lineHeight);

            Brush fillPen = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            g.FillRectangle(fillPen, position.X, position.Y, lineWidth, lineHeight * buttons.Length);

            Point buttonLocation = position;
            foreach (Button button in buttons)
            {
                button.Draw(g, font, ref buttonLocation);
            }
            return position;
        }
        public void Click(Point point)
        {

        }
        public TestElemenCommand FindSelectedObject(Point location)
        {
            if (hide)
                return TestElemenCommand.none;
            if (location.X < position.X || location.Y < position.Y
                || location.X > position.X + lineWidth || location.Y > position.Y + lineHeight * buttons.Length)
                return TestElemenCommand.none;
            Point buttonLocation = position;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Intersect(ref buttonLocation, location))
                    return buttons[i].Command;
            }
            return TestElemenCommand.none;
        }
    }
}
