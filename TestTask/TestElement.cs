using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Security.Permissions;
using System.Reflection;

namespace TestTask
{
    enum Status
    {
        list, contextMenu, enterText
    }

    class TestElement : PictureBox
    {
        private int lineHeight;
        private int lineWidth;
        private int minWidth = 80, maxWidth = 200;
        private int minHeight = 15, maxHeight = 30;
        public int LineHeight
        {
            get { return lineHeight; }
            set 
            { 
                lineHeight = value; 
                if (lineHeight< minHeight) { lineHeight= minHeight; }
                if (lineHeight> maxHeight) { lineHeight= maxHeight; }
            }
        }
        public int LineWidth
        {
            get { return lineWidth; }
            set
            {
                lineWidth = value;
                if (lineWidth < minWidth) { lineWidth = minWidth; }
                if (lineWidth > maxWidth) { lineWidth = maxWidth; }
            }
        }
        public Font font;
        public List <OneLine> lines;
        public Point locationSelectedLine;
        public OneLine? selectedLine;
        private ContextMenu contextMenu;
        private Status status = Status.list;
        public TestElement() : base() 
        {
            font = SystemFonts.DefaultFont;
            lineHeight = 25;
            lineWidth = 130;
            lines = new List<OneLine>();
            lines.Add(new OneLine("Ели",lineWidth, lineHeight,this));
            lines[0].AddLine("мясо");
            lines[0].lines[0].AddLine("мужики");
            lines[0].AddLine("пивом");
            lines[0].lines[1].AddLine("запивали");
            lines.Add(new OneLine("О чем", lineWidth, lineHeight, this));
            lines[1].AddLine("конюх");
            lines[1].lines[0].AddLine("говорил");
            lines[1].AddLine("они");
            lines[1].lines[1].AddLine("не понимали");
            MouseClick += OnClick;
            contextMenu = new ContextMenu(this);
            Point position = new Point(0, 0);
            Graphics g = CreateGraphics();
            Draw();
        }

        public void AddLine(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                selectedLine.AddLine("Новая");
            }
            else
            {
                lines.Add(new OneLine("строчка", lineWidth, lineHeight, this));
            }
            HideContextMenu();
        }

        public void ClearLine(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                selectedLine.Clear();
                HideContextMenu();
            }
        }

        public void IncWidth(object sender, EventArgs e)
        {
            LineWidth += 3;
            foreach (OneLine line in lines)
            {
                line.Widht = LineWidth;
            }
        }

        public void DecWidth(object sender, EventArgs e)
        {
            LineWidth -= 3;
            foreach (OneLine line in lines)
            {
                line.Widht = LineWidth;
            }
        }
        
        public void IncHeight(object sender, EventArgs e)
        {
            LineHeight += 1;
            foreach (OneLine line in lines)
            {
                line.Height = LineHeight;
            }
        }

        public void DecHeight(object sender, EventArgs e)
        {
            LineHeight -= 1;
            foreach (OneLine line in lines)
            {
                line.Height = LineHeight;
            }
        }

        public void HideContextMenu()
        {
            contextMenu.hide = true;
            status = Status.list;
        }

        public void ShowContextMenu(Point position)
        {
            contextMenu.hide = false;
            status = Status.contextMenu;
            contextMenu.position = position;
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            Point startingPoint = new Point(0, 0);
            Point location = e.Location;
            if (status == Status.list)
            {
                OneLine? currentLine = null;
                foreach (OneLine line in lines)
                {
                    currentLine = line.FindSelectedObject(ref startingPoint, location);
                    if (currentLine != null)
                    {
                        selectedLine = currentLine;
                        break;
                    }
                }
                selectedLine = currentLine;
            }
            if (e.Button == MouseButtons.Left && status == Status.contextMenu)
            {
                contextMenu.Click(location);
            }
            if (e.Button == MouseButtons.Right)
            {
                if (status == Status.contextMenu)
                {
                    HideContextMenu();
                }
                else if (status == Status.list)
                {
                    ShowContextMenu(e.Location);
                }
            }
            Draw();
        }

        public void Draw()
        {
            Point position=new Point(0, 0);
            Graphics g = CreateGraphics();
            g.Clear(Color.FromArgb(255, 192,192,192));
            Brush fillPen = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            if (selectedLine != null)
            {
                selectedLine.DrawSelect(g, fillPen);
            }
            foreach (OneLine line in lines)
            {
                position = line.Draw(g, font, position);
            }
            if (status == Status.contextMenu)
            {
                contextMenu.Draw(g, font);
            }
        }
    }
}
