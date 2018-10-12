using System;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FaceController c = new FaceController();
            c.OnShift += (s, ea) =>
            {
                Dispatcher.Invoke(() =>
                {
                    //Vector3D direction = new Vector3D(0, 1, 0);
                    Point3D position = new Point3D(
                        Camera.Position.X + (ea.Coords.Item1 / 150), 
                        Camera.Position.Y + (ea.Coords.Item2 / 150), 
                        8);
                    //Camera.LookDirection = direction;
                    Camera.Position = position;
                    View.Camera = Camera;

                    Title = ea.Coords.Item1 + " : " + ea.Coords.Item2;
                });
            };
            c.OnReset += (s, ea) =>
            {
                Dispatcher.Invoke(() =>
                {
                    //Vector3D direction = new Vector3D(0, 1, 0);
                    Point3D position = new Point3D(0.5, 0.5, 8);
                    //Camera.LookDirection = direction;
                    Camera.Position = position;
                    View.Camera = Camera;
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
