using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ChaosWPF
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        static Random rnd = new Random();
        WriteableBitmap wb;
        int imgWidth, imgHeight;
        int pointsize = 1;
        int iteration = 0;

        List<Point> vertexes;
        Point tracepoint;

        // Checkbox правила приближения
        double _AdivB = 0.5;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imgWidth = (int)image.Width; imgHeight = (int)image.Height;

            wb = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null); image.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            Init();
        }

        private void Init()
        {
            tracepoint = new Point();
            vertexes = new List<Point>();
        }

        double GetDistance(Point p1, Point p2) => Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));

        // Roll the dice
        int RandomDice(int vertex) => rnd.Next(0, vertex);


        void Control()
        {
            if (vertexes.Count == 0) return;

            int chance = RandomDice(vertexes.Count); // Roll the dice and chose random number of vertex

            double divDist = GetDistance(tracepoint, vertexes[chance]) * _AdivB;
            double dX = (tracepoint.X - vertexes[chance].X) * _AdivB;
            double dY = (tracepoint.Y - vertexes[chance].Y) * _AdivB;

            Point newTracePoint = new Point(vertexes[chance].X + dX, vertexes[chance].Y + dY);

            tracepoint = newTracePoint;
            wb.FillEllipseCentered((int)tracepoint.X, (int)tracepoint.Y, pointsize, pointsize, Colors.White);

            iteration++;

            lblIter.Content = iteration.ToString();
        }


        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (vertexes.Count == 0) return;

            if (!timer.IsEnabled)
                timer.Start();
            else
                timer.Stop();

            if ((bool)chk1div2.IsChecked) _AdivB = 0.5;
            if ((bool)chk1div3.IsChecked) _AdivB = 0.3;
            if ((bool)chk2div3.IsChecked) _AdivB = 0.6;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(image);
            vertexes.Add(mousePos);

            wb.FillEllipseCentered((int)mousePos.X, (int)mousePos.Y, pointsize, pointsize, Colors.Yellow);
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tracepoint.X == 0 & tracepoint.Y == 0)
            {
                tracepoint = e.GetPosition(image);
                wb.FillEllipseCentered((int)tracepoint.X, (int)tracepoint.Y, pointsize, pointsize, Colors.White);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Init();

            wb.Clear(Colors.Black);
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            Control();

            if ((bool)chk1div2.IsChecked) _AdivB = 0.5;
            if ((bool)chk1div3.IsChecked) _AdivB = 0.3;
            if ((bool)chk2div3.IsChecked) _AdivB = 0.6;
        }

        private void timerTick(object sender, EventArgs e) => Control();
    }
}
