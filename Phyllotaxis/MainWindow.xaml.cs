using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace WpfApp
{
    // Based on #30 — Phyllotaxis / https://thecodingtrain.com/challenges/30-phyllotaxis
	
	public partial class MainWindow : Window
    {
        WriteableBitmap wb;
        int width, height;

        System.Windows.Threading.DispatcherTimer timer;

        int n = 0;
        double c = 6;
        double a, start, coeff;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            width = (int)image1.Width; height = (int)image1.Height;

            // Для Битмапа
            wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null); image1.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            timer.Start();

            Init();
        }

        private void Init()
        {
            n = 0;
            c = 4;
            a = 0;
            coeff = 137.5;
            start = 0;

            if (lb != null)
            {
                lb.Content = String.Format("{0:f1}", slider.Value);
                coeff = slider.Value;
            }
            if (wb != null)
                wb.Clear();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => Init();

        private void Drawing()
        {
            for (int i = 0; i < n; ++i)
            {
                a = i * coeff;
                var r = c * Math.Sqrt(i);

                var x = r * Math.Cos(a) * 1.05 + width / 2;
                var y = r * Math.Sin(a) * 1.05 + height / 2;

                var color = Colors.Black;

                wb.FillEllipseCentered((int)x, (int)y, 1, 1, color);
            }

            n += 5;
            start += 0.1;
        }

        private void timerTick(object sender, EventArgs e) => Drawing();
    }
}
