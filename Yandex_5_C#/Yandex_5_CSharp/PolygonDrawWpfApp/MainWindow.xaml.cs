using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolygonDrawWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyPolygon curPoligon;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadNextPolugon_Click(object sender, RoutedEventArgs e)
        {
            freeCanvas.Children.Clear();

            curPoligon = new MyPolygon();

            LinkedList<Node> indexNode = MyPolygon.initializationNodes();

            Point[] curvePointsXY = new Point[4];
            for (int i = 0; i < indexNode.Count; i++)
            {
                curvePointsXY[i] =
                    new Point(curPoligon.point3Ds[i].X * 200,
                    curPoligon.point3Ds[i].Y * 20
                    );
            };
            var pointCollectionXY = new PointCollection(curvePointsXY);
            //pol
            Polygon p = new Polygon();
            p.Stroke = Brushes.Black;
            p.Fill = Brushes.LightBlue;
            p.StrokeThickness = 1;
            p.HorizontalAlignment = HorizontalAlignment.Left;
            p.VerticalAlignment = VerticalAlignment.Center;
            p.Points = pointCollectionXY;
            freeCanvas.Children.Add(p);


            Point[] curvePointsYZ = new Point[4];
            for (int i = 0; i < indexNode.Count; i++)
            {
                curvePointsYZ[i] =
                    new Point(curPoligon.point3Ds[i].Y * 20,
                    curPoligon.point3Ds[i].Z * 2
                    );
            };
            var pointCollectionYZ = new PointCollection(curvePointsYZ);
            Polygon p1 = new Polygon();
            p1.Stroke = Brushes.Black;
            p1.Fill = Brushes.Blue;
            p1.StrokeThickness = 1;
            p1.HorizontalAlignment = HorizontalAlignment.Left;
            p1.VerticalAlignment = VerticalAlignment.Center;
            p1.Points = pointCollectionYZ;
            freeCanvas.Children.Add(p1);

            Point[] curvePointsZX = new Point[4];
            for (int i = 0; i < indexNode.Count; i++)
            {
                curvePointsZX[i] =
                    new Point(curPoligon.point3Ds[i].Z * 2,
                    curPoligon.point3Ds[i].X * 40
                    );
            };
            var pointCollectionZX = new PointCollection(curvePointsZX);
            Polygon p2 = new Polygon();
            p2.Stroke = Brushes.Black;
            p2.Fill = Brushes.Aquamarine;
            p2.StrokeThickness = 1;
            p2.HorizontalAlignment = HorizontalAlignment.Left;
            p2.VerticalAlignment = VerticalAlignment.Center;
            p2.Points = pointCollectionZX;
            freeCanvas.Children.Add(p2);

            #region 
            //LinkedList<Point3D> indexPoint3D = MyPolygon.initializePoint3D();
            //for (int i = 1; i <= 3; i++)
            //{
            //    Point[] curvePoints = new Point[MyPolygon.size_curPoligon];
            //    for (int j = 0; j < indexNode.Count; j++)
            //    {
            //        curvePoints[j] =
            //            new Point(curPoligon.point3Ds[j].Value * 2,
            //            curPoligon.point3Ds[j].Next.Value * 40
            //            );
            //    };
            //    var pointCollection = new PointCollection(curvePoints);
            //    Polygon pol = new Polygon();
            //    pol.Stroke = Brushes.Black;
            //    pol.Fill = Brushes.Aquamarine;
            //    pol.StrokeThickness = 1;
            //    pol.HorizontalAlignment = HorizontalAlignment.Left;
            //    pol.VerticalAlignment = VerticalAlignment.Center;
            //    pol.Points = pointCollection;
            //    freeCanvas.Children.Add(pol);
            //}
            #endregion

            namePolygon.Text = String.Format(
                "{0} {1} {2} {3}",
                MyPolygon.evenCombinations.ToArray()[MyPolygon.currentCombination].ElementAt(0).Key,
                MyPolygon.evenCombinations.ToArray()[MyPolygon.currentCombination].ElementAt(1).Key,
                MyPolygon.evenCombinations.ToArray()[MyPolygon.currentCombination].ElementAt(2).Key,
                MyPolygon.evenCombinations.ToArray()[MyPolygon.currentCombination].ElementAt(3).Key);

            #region pol
            //var polygonXY = new Polygon
            //{
            //    Stroke = Brushes.Black,
            //    StrokeThickness = 0.05,
            //    //Fill = Brushes.Blue,
            //    Points = pointCollectionXY,
            //};
            //const int cx = 800;
            //const int cy = 1200;
            //polygonXY.Measure(new Size(cx, cy));
            //polygonXY.Arrange(new Rect(0, 0, cx, cy));
            //RenderTargetBitmap bmp = new RenderTargetBitmap(cx, cy, 2400, 1920, PixelFormats.Pbgra32);
            //bmp.Render(polygonXY);
            //_image.Source = bmp;
            #endregion pol


        }
    }
}
