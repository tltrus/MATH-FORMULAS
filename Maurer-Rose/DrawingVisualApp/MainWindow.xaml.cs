using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    // Based on https://thecodingtrain.com/challenges/c1-maurer-rose
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();

        Brush brush;
        DrawingVisual visual;
        DrawingContext dc;
        StreamGeometry streamGeometry_1, streamGeometry_2;
        double x, y;
        List<Point> points_1, points_2;
        double d = 0, n = 0, k = 0;
        int width, height;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            streamGeometry_1 = new StreamGeometry();
            streamGeometry_2 = new StreamGeometry();
            points_1 = new List<Point>();
            points_2 = new List<Point>();

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 70);

            timer.Start();
        }

        private void Control()
        {
            using (StreamGeometryContext geometryContext = streamGeometry_1.Open())
            {
                for (double i = 0; i < 361; i++)
                {
                    k = i * d * Math.PI / 180;
                    var r = 250 * Math.Sin(n * k);
                    x = r * Math.Cos(k);
                    y = r * Math.Sin(k);

                    // Пересчет координат в центр экрана
                    x += width / 2;
                    y += height / 2;

                    if (i == 0) geometryContext.BeginFigure(new Point(x, y), false, false);

                    points_1.Add(new Point(x, y));
                }
                geometryContext.PolyLineTo(points_1, true, true);
            }

            using (StreamGeometryContext geometryContext = streamGeometry_2.Open())
            {
                for (double i = 0; i < 361; i++)
                {
                    k = i * Math.PI / 180;
                    var r = 250 * Math.Sin(n * k);
                    x = r * Math.Cos(k);
                    y = r * Math.Sin(k);

                    // Пересчет координат в центр экрана
                    x += width / 2;
                    y += height / 2;

                    if (i == 0) geometryContext.BeginFigure(new Point(x, y), false, false);

                    points_2.Add(new Point(x, y));
                }
                geometryContext.PolyLineTo(points_2, true, true);
            }

            n += 0.01;
            d += 0.03;
        }

        private void timerTick(object sender, EventArgs e)
        {
            Control();
            Drawing();
            points_1.Clear();
            points_2.Clear();
        }


        private void Drawing()
        {
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                dc.DrawGeometry(null, new Pen(Brushes.White, 1), streamGeometry_1);
                dc.DrawGeometry(null, new Pen(Brushes.DeepPink, 3), streamGeometry_2);
                

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
