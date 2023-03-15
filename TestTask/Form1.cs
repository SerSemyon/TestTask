namespace TestTask
{
    public partial class Form1 : Form
    {
        static Bitmap SquareBM = new Bitmap(10,10);
        public Form1()
        {
            InitializeComponent();
        }

        private void testElement1_Click(object sender, EventArgs e)
        {
            testElement1.CreateGraphics();
            Bitmap background = new Bitmap(testElement1.Width, testElement1.Height);
            Point position = ((System.Windows.Forms.MouseEventArgs)e).Location;
            int x = position.X - testElement1.Location.X;
            int y = position.Y - testElement1.Location.Y;
            for (int i = 0; i < testElement1.Width; i++)
            {
                background.SetPixel(i,y, Color.Black);
            }
            for (int i = 0; i < testElement1.Height; i++)
            {
                background.SetPixel(x,i, Color.Black);
            }
            testElement1.Image = background;
        }
    }
}