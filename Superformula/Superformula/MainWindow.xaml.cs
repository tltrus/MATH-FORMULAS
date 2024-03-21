using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Superformula
{
    public partial class MainWindow : Window
    {
        Brush brush;
        Pen pen;
        DrawingVisual visual;
        DrawingContext dc;
        int cellSize = 10;
        Point mousePos = new Point();


        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            PaintDrawingVisual();
        }

        double R(double theta, double a, double b, double m, double n1, float n2, float n3)
        {
            return Math.Pow(Math.Pow(Math.Abs(Math.Cos(m * theta / 4) / a), n2) + Math.Pow(Math.Abs(Math.Sin(m * theta / 4) / b), n3), -1 / n1);
        }

        private void PaintDrawingVisual()
        {
            Point p1, p2;
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                // Axis ortha
                pen = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#000000")), 1);
                // Х
                p1 = new Point(0, g.Height / 2);
                p2 = new Point(g.Width, g.Height / 2);
                dc.DrawLine(pen, p1, p2);
                // Y
                p1 = new Point(g.Width / 2, 0);
                p2 = new Point(g.Width / 2, g.Height);
                dc.DrawLine(pen, p1, p2);


                StreamGeometry geom = new StreamGeometry();
                using (StreamGeometryContext gc = geom.Open())
                {
                    double rad = R(0,
                                    mousePos.X / 100, // a
                                    mousePos.Y / 100, // b
                                    10, // m
                                    1, // n1
                                    1, // n2
                                    1  // n3
                                    );
                    double x = 6 * rad * Math.Cos(0);
                    double y = 6 * rad * Math.Sin(0);
                    p1 = ConvertCentrAxisToScreen(new Point(x, y));

                    gc.BeginFigure(p1, true, true);

                    // Superformula
                    for (double theta = 0; theta <= 2 * Math.PI; theta += 0.01)
                    {
                        rad = R(theta,
                                        mousePos.X / 100, // a
                                        mousePos.Y / 100, // b
                                        10, // m
                                        1, // n1
                                        1, // n2
                                        1  // n3
                                        );
                        x = 6 * rad * Math.Cos(theta);
                        y = 6 * rad * Math.Sin(theta);
                        brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#fb8c00"));
                        pen = new Pen((SolidColorBrush)(new BrushConverter().ConvertFrom("#008cFF")), 1);
                        p1 = ConvertCentrAxisToScreen(new Point(x, y));

                        // isStroked=true, isSmoothJoin=true
                        gc.LineTo(p1, true, true);
                    }
                }
                dc.DrawGeometry(brush, pen, geom);

                dc.Close();
                g.AddVisual(visual);
            }
        }

        public Point ConvertCentrAxisToScreen(Point point, int width = 1)
        {
            double r = width / 2;
            // Х
            double X = 0;
            X = point.X * cellSize + g.Width / 2 - r;

            // Y
            double Y = 0;
            Y = -point.Y * cellSize + g.Height / 2 - r;

            return new Point(X, Y);
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.GetPosition(g);
            PaintDrawingVisual();
        }
    }
}
