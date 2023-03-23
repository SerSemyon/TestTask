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
        List <OneLine> lines;
        public Point locationSelectedLine;
        public OneLine? selectedLine;
        private ContextMenu contextMenu;
        BufferedGraphics bufferGraphics;
        BufferedGraphicsContext bufferContext;
        public TestElement() : base() 
        {
            font = SystemFonts.DefaultFont;
            lineHeight = 25;
            lineWidth = 130;
            lines = new List<OneLine>();
            MouseClick += OnClick;
            SizeChanged += OnSizeChanged;
            contextMenu = new ContextMenu(this);
            bufferContext = new BufferedGraphicsContext();
            Draw();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
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
            contextMenu.hide = true;
        }

        public void ClearLine(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                selectedLine.Clear();
            }
            else
            {
                foreach (OneLine line in lines)
                {
                    line.Clear();
                }
                lines.Clear();
            }
            contextMenu.hide = true;
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

        private void FindSelectedLine(Point startingPoint, Point location)
        {
            OneLine? currentLine = null;
            if (contextMenu.hide)
            {
                foreach (OneLine line in lines)
                {
                    currentLine = line.FindSelectedObject(ref startingPoint, location);
                    if (currentLine != null)
                    {
                        break;
                    }
                }
                selectedLine = currentLine;
            }
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            Point startingPoint = new Point(0, 0);
            Point location = e.Location;
            FindSelectedLine(startingPoint, location);
            contextMenu.Click(this,e);
            Draw();
        }

        public void Draw()
        {
            Point position=new Point(0, 0);
            Graphics graphicsToShow = CreateGraphics();
            bufferGraphics = bufferContext.Allocate(graphicsToShow, new Rectangle(0, 0, Width, Height));
            Graphics graphicsInBuffer = bufferGraphics.Graphics;
            graphicsInBuffer.Clear(Color.FromArgb(255, 192,192,192));
            if (selectedLine != null)
            {
                Brush fillBrush = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
                selectedLine.DrawSelect(graphicsInBuffer, fillBrush);
            }
            foreach (OneLine line in lines)
            {
                line.Draw(graphicsInBuffer, font, ref position);
            }
            contextMenu.Draw(graphicsInBuffer, font);
            bufferGraphics.Render(graphicsToShow);
        }
    }
}
