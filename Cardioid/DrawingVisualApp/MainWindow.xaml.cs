using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawingVisualApp
{
    // Based on #133 — Times Tables Cardioid Visualization
    // https://thecodingtrain.com/challenges/133-time-tables-cardioid-visualization

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd = new Random();

        Brush brush;
        DrawingVisual visual;
        DrawingContext dc;
        int width, height;

        int total = 10;
        double r;
        Point mouse;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            r = width / 2 - 10; // Math.PI нужен чтобы развернуть фигуру

            Drawing();
        }


        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                int factor = 2;

                double x = width / 2;
                double y = height / 2;
                dc.DrawEllipse(null, new Pen(Brushes.Gray, 1), new Point(x, y), r, r);


                for (int i = 0; i < total; ++i)
                {
                    Point p = GetPoint(i);

                    dc.DrawEllipse(Brushes.White, null, new Point(p.X, p.Y), 3, 3);
                }


                for (int i = 0; i < total; ++i)
                {
                    Point a = GetPoint(i);
                    Point b = GetPoint(i * factor);
                    dc.DrawLine(new Pen(Brushes.Gray, 1), a, b);
                }

                    dc.Close();
                g.AddVisual(visual);
            }
        }

        private Point GetPoint(int index)
        {
            double angle = Map(index % total, 0, total, 0, 2 * Math.PI);
            Point p = new Point(r * Math.Cos(angle + Math.PI) + width / 2,  r * Math.Sin(angle + Math.PI) + height / 2);
            return p;
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = e.GetPosition(g);
            total = (int)Map(mouse.X, 0, width, 0, 200);
            Drawing();
        }

        public double Constrain(double n, double low, double high)
        {
            return Math.Max(Math.Min(n, high), low);
        }

        public double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
        {
            double num = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
            if (!withinBounds)
            {
                return num;
            }

            if (start2 < stop2)
            {
                return Constrain(num, start2, stop2);
            }

            return Constrain(num, stop2, start2);
        }
    }
}
