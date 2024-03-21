using System;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        Brush brush;
        DrawingVisual visual;
        DrawingContext dc;
        int numberofseeds = 2000;

        double c;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            c = (Math.Sqrt(5) + 1) / 2; // Золотое сечение 1.6180339887

            brush = new SolidColorBrush(Color.FromRgb((byte)(rnd.Next(0, 256)), (byte)(rnd.Next(0, 256)), (byte)(rnd.Next(0, 256))));

            Drawing();
        }

        private void Drawing()
        {
            double angle, r, x, y;
            
            g.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                for (int i = 0; i < numberofseeds; ++i)
                {
                    // ВАРИАНТ ПОДСОЛНУХА С какого-то САЙТА

                    r = Math.Pow(i, c) / (numberofseeds / 2);
                    angle = 2 * Math.PI * c * i;
                    x = r * Math.Sin(angle) + 250;
                    y = r * Math.Cos(angle) + 250;

                    dc.DrawEllipse(brush, null, new Point(x, y), i / (numberofseeds / 5), i / (numberofseeds / 5));


                    // ОРИГИНАЛЬНЫЙ ВАРИАНТ ПОДСОЛНУХА ФОГЕЛЯ

                    r = Math.Sqrt(i) * c;
                    angle = i * 2.4; // angle = i * 137,5 градусов (2,4 радиан)
                    x = r * Math.Sin(angle) + 600;
                    y = r * Math.Cos(angle) + 250;

                    dc.DrawEllipse(brush, null, new Point(x, y), 1, 1);


                    // СПИРАЛЬ ФЕРМА

                    //r = Math.Sqrt(i) * c;
                    angle = i * 360 / Math.PI * 2;
                    double a = -0.25; // шаг спирали
                    x = Math.Sign(a) * Math.Abs(a) * Math.Pow(angle, 0.5) * Math.Cos(angle) + 850;
                    y = Math.Sign(a) * Math.Abs(a) * Math.Pow(angle, 0.5) * Math.Sin(angle) + 250;

                    dc.DrawEllipse(brush, null, new Point(x, y), 1, 1);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
