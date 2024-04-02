using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace LissajousWPF
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        WriteableBitmap wb;
        int imgWidth, imgHeight;

        double oldX;
        double oldY;
        double A = 1;
        double B = 1;
        double t = 0; // time

        double a = 5; // angular frequency Х
        double b = 6;   // angular frequency Y
        double phi = Math.PI / 2;

        public MainWindow() => InitializeComponent();

        private void Window_Initialized(object sender, EventArgs e)
        {
            imgWidth = (int)image.Width; imgHeight = (int)image.Height;

            wb = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null); image.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Init();
        }


        void Control()
        {
            if (a > b)
            {
                if (t >= a * Math.PI)
                {
                    wb.Clear(Colors.Black);
                    DrawAxis();
                    t = 0;
                }
            }
            else
            {
                if (t >= b * Math.PI)
                {
                    wb.Clear(Colors.Black);
                    DrawAxis();
                    t = 0;
                }
            }

            double x = A * Math.Sin(a * t + phi);
            double y = B * Math.Sin(b * t);

            int x1, x2, y1, y2;
            ToCentricAxis(x, y, out x1, out y1);
            ToCentricAxis(oldX, oldY, out x2, out y2);

            // we specifically check the time for > 0 so that a white line from the center of the axes (0, 0) is not drawn on the first cycle
            if (t > 0)
                wb.DrawLine(x1, y1, x2, y2, Colors.White);

            oldX = x;
            oldY = y;

            t = t + 0.01 * Math.PI;
        }

        private void Init()
        {
            wb.Clear(Colors.Black);
            t = 0;
            oldX = oldY = 0;
            DrawAxis();
        }

        private void DrawAxis()
        {
            int x1, x2, y1, y2;
            ToCentricAxis(0, -2, out x1, out y1);
            ToCentricAxis(0, 2, out x2, out y2);
            wb.DrawLine(x1, y1, x2, y2, Colors.Yellow);

            ToCentricAxis(-2, 0, out x1, out y1);
            ToCentricAxis(2, 0, out x2, out y2);
            wb.DrawLine(x1, y1, x2, y2, Colors.Yellow);
        }

        // Converting coordinates to a centric coordinate system
        private void ToCentricAxis(double oldX, double oldY, out int newX, out int newY)
        {
            double midScreenX = imgWidth / 2;
            double midScreenY = imgHeight / 2;

            newX = newY = 0;
            oldX *= 100;
            oldY *= 100;

            if (oldY < midScreenY)
            {
                newY = (int)(midScreenY - oldY);
            }
            if (oldX < midScreenX)
            {
                newX = (int)(midScreenX + oldX);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => Init();
        private void btnStep_Click(object sender, RoutedEventArgs e) => Control();
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
                timer.Start();
            else
                timer.Stop();
        }
        private void timerTick(object sender, EventArgs e) => Control();
    }
}
