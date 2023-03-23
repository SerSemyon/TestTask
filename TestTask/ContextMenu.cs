using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestTask
{
    class ContextMenu
    {
        int lineWidth = 80;
        int lineHeight = 20;
        public Point position;
        public bool hide = true;
        Button[] buttons = new Button[6];
        Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        Brush fillPen = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

        public ContextMenu(TestElement testElement)
        {
            position = new Point(0, 0);
            buttons[0] = new Button("Добавить", testElement.AddLine);
            buttons[1] = new Button("Очистить", testElement.ClearLine);
            buttons[2] = new Button("+ ширина", testElement.IncWidth);
            buttons[3] = new Button("- ширина", testElement.DecWidth);
            buttons[4] = new Button("+ высота", testElement.IncHeight);
            buttons[5] = new Button("- высота", testElement.DecHeight);
        }
        public void Draw(Graphics g, Font font)
        {
            if (!hide)
            {
                g.FillRectangle(fillPen, position.X, position.Y, lineWidth, lineHeight * buttons.Length);

                Point buttonLocation = position;
                foreach (Button button in buttons)
                {
                    button.Draw(g, font, ref buttonLocation);
                }
            }
        }
        public void Click(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            if (!hide)
            {
                if (location.X < position.X || location.Y < position.Y
                    || location.X > position.X + lineWidth || location.Y > position.Y + lineHeight * buttons.Length)
                {
                    hide = true;
                    return;
                }
                Point buttonLocation = position;
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i].Intersect(ref buttonLocation, location))
                    {
                        buttons[i].Click();
                        return;
                    }
                }
            }
            if (e.Button == MouseButtons.Right && hide)
            {
                hide = false;
                position = location;
            }
        }
    }
}
