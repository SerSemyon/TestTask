using System.Drawing;

namespace TestTask
{
    public partial class Form1 : Form
    {
        static Bitmap SquareBM = new Bitmap(10, 10);
        public Form1()
        {
            InitializeComponent();
        }

        private void testElement1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void testElement1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void testElement1_Click(object sender, EventArgs e)
        {

        }

        private void testElement1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Graphics g = testElement1.CreateGraphics();
            Brush fillPen = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
            g.FillEllipse(fillPen, 0, 0, 100, 100);
            testElement1.entryField.Draw(g, testElement1.font, new Point(0, 0));
        }

        private void testElement1_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}