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
    enum TestElemenCommand
    {
        add, delete, incWidth, decWidth, incHeight, decHeight, none
    }
    enum Status
    {
        list, contextMenu, enterText
    }
    class OneLine
    {
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
        public Point Draw(Graphics g, Font font, Point location)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            g.DrawRectangle(blackPen, location.X, location.Y, height, height);
            g.DrawRectangle(blackPen, location.X + height, location.Y, width, height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(text, font, new SolidBrush(Color.Black), new Point(location.X + height + width/2, location.Y+height/2), stringFormat);
            for (int i = 0; i < 4; i++)
            {
                g.DrawRectangle(blackPen, location.X + height + width + i*height, location.Y, height, height);
            }
            location.Y += height+yStep;
            if (!minimized)
            {
                location.X += xStep;
                foreach (OneLine line in lines)
                {
                    location=line.Draw(g, font, location);
                }
                location.X -= xStep;
            }
            return location;
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
            if (location.Y-startingPoint.Y < height)
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
                    OneLine currenLine = line.FindSelectedObject(ref startingPoint, location);
                    if (currenLine != null)
                        return currenLine;
                }
                startingPoint.X -= xStep;
            }
            return null;
        }
    }
    class ContextMenu
    {
        int lineWidth = 80;
        int lineHeight = 20;
        public Point location;
        bool hide = true;
        string[] elements = { "Добавить", "Удалить", "Задать ширину","Задать высоту"};

        public ContextMenu() 
        {
            location = new Point(0, 0);
        }
        public Point Draw(Graphics g, Font font)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            g.DrawRectangle(blackPen, location.X, location.Y, lineWidth, lineHeight);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            Brush fillPen = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            g.FillRectangle(fillPen, location.X, location.Y, lineWidth, lineHeight*elements.Length);

            for (int i  = 0; i < elements.Length; i++)
            {
                g.DrawString(elements[i], font, new SolidBrush(Color.Black), new Point(location.X + lineWidth / 2, location.Y + i*lineHeight + lineHeight / 2), stringFormat);
                g.DrawRectangle(blackPen, location.X, location.Y + i * lineHeight, lineWidth, lineHeight);
            }
            return location;
        }
        public void Click(Point point)
        {

        }
        public TestElemenCommand FindSelectedObject(ref Point startingPoint, Point location)
        {
            if (hide)
                return TestElemenCommand.none;
            if (location.X < startingPoint.X || location.Y < startingPoint.Y)
                return TestElemenCommand.none;
            if (location.Y - startingPoint.Y < lineHeight)
            {
                Click(startingPoint);
                return TestElemenCommand.add;
            }
            
            return TestElemenCommand.none;
        }
    }
    class EntryField
    {
        Point position;
        int height = 60;
        int width = 200;
        string text = "";
        Graphics g;
        Font font;
        Point location;

        public EntryField(Graphics g, Font font, Point location)
        {
            g = g;
            font = font;
            location = location;
        }

        public Point Draw(Graphics g, Font font, Point location)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            g.DrawRectangle(blackPen, location.X, location.Y, height, height);
            g.DrawRectangle(blackPen, location.X + height, location.Y, width, height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            Brush fillPen = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            g.FillRectangle(fillPen, location.X + height, location.Y, width, height);
            g.DrawString(text, font, new SolidBrush(Color.Black), new Point(location.X + height + width / 2, location.Y + height / 2), stringFormat);
            for (int i = 0; i < 4; i++)
            {
                g.DrawRectangle(blackPen, location.X + height + width + i * height, location.Y, height, height);
            }
            return location;
        }
    }
    class TestElement : PictureBox
    {
        public int lineHeight;
        public int lineWidth;
        public Font font;
        public List <OneLine> lines;
        public Point locationSelectedLine;
        public OneLine? selectedLine;
        private ContextMenu contextMenu;
        private Status status = Status.list;
        private int minWidth = 60, maxWidth = 200;
        private int minHeight = 20, maxHeight = 60;
        public EntryField entryField;
        public TestElement() : base() 
        {
            font = SystemFonts.DefaultFont;
            lineHeight = 20;
            lineWidth = 80;
            lines = new List<OneLine>();
            lines.Add(new OneLine("String",lineWidth, lineHeight,this));
            lines[0].AddLine("Блабла");
            lines[0].lines[0].AddLine("Ещё строка");
            lines[0].AddLine("КААК");
            lines[0].lines[1].AddLine("dfdsaf");
            lines[0].AddLine("Sggsg");
            MouseClick += OnClick;
            contextMenu = new ContextMenu();
            Point position = new Point(0, 0);
            Graphics g = CreateGraphics();
            entryField = new EntryField(g, font, position);
            KeyDown += OnKeyDown;
            //SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            foreach (OneLine oneLine in lines)
            {
                oneLine.Widht = +3;
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            Graphics g = CreateGraphics();
            Brush fillPen = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            g.FillEllipse(fillPen, 0,0,100,100);
            entryField.Draw(g, font, new Point(0,0));
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            Point startingPoint = new Point(0, 0);
            Point location = e.Location;
            if (status == Status.list)
            {
                bool isFind = false;
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
            if (e.Button == MouseButtons.Left || status == Status.contextMenu)
            {
                TestElemenCommand command = contextMenu.FindSelectedObject(ref startingPoint, location);
                switch (command)
                {
                    case TestElemenCommand.add:
                        lineWidth += 3;
                        foreach (OneLine line in lines)
                        {
                            line.Widht = lineWidth;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (status == Status.contextMenu)
                    status = Status.list;
                else if (status == Status.list)
                {
                    status = Status.contextMenu;
                    contextMenu.location = e.Location;
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
                g.FillRectangle(fillPen, locationSelectedLine.X + lineHeight, locationSelectedLine.Y, lineWidth, lineHeight);
            }
            foreach (OneLine line in lines)
            {
                line.Draw(g, font, position);
            }
            if (status == Status.contextMenu)
            {
                contextMenu.Draw(g, font);
            }
            //entryField.Draw(g, font, position);
        }
    }
}
