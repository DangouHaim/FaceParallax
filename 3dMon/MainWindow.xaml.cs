using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using FaceBoxParallax;

namespace _3dMon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BG.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() +  @"\sky.jpg"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FaceController c = new FaceController();
            c.OnShift += (s, ea) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Point3D position = new Point3D(
                        Camera.Position.X + (ea.Coords.Item1 / 100), 
                        Camera.Position.Y + (ea.Coords.Item2 / 100),
                        12);
                    Vector3D direction = new Vector3D(
                        Camera.LookDirection.X + (ea.Coords.Item1 / 600) * -1,
                        Camera.LookDirection.Y + (ea.Coords.Item2 / 600) * -1,
                        Camera.LookDirection.Z
                        );
                    Camera.LookDirection = direction;
                    Camera.Position = position;
                    View.Camera = Camera;

                    Canvas.SetLeft(BG, Canvas.GetLeft(BG) + (ea.Coords.Item1 / 10));
                    Canvas.SetTop(BG, Canvas.GetTop(BG) + (ea.Coords.Item2 / 10) * -1);

                    Title = ea.Coords.Item1 + " : " + ea.Coords.Item2;
                });
            };
            c.OnReset += (s, ea) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Point3D position = new Point3D(0.5, 1.5, 12);
                    Vector3D direction = new Vector3D(0, 0, -3.5);
                    Camera.LookDirection = direction;
                    Camera.Position = position;
                    View.Camera = Camera;
                    Canvas.SetLeft(BG, -100);
                    Canvas.SetTop(BG, -100);
                });
            };
            new Thread(() =>
            {
                c.Start();
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}
