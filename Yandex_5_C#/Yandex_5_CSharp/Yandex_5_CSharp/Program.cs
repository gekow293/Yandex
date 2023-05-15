using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yandex_5_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = true;
            
            while (success)
            {
                Polygon curPoligon = new Polygon();//создается новый полигон,при этом в конструкторе каждый раз принимается новая комбинация (конфигурация) вершин/узлов, которая там уже хранится в листе "arrCombinations" (заполненным после создания первого полигона)

                //success == true - полигон выпуклый(тривиальный), иначе (success == false) - вогнутый(нетривиальный)
                success = GetConvex(curPoligon);
                if (success == false)
                {
                    Console.Write(curPoligon.ToString());
                    using (StreamWriter sw = new StreamWriter("output.txt"))
                    {
                        sw.WriteLine(curPoligon.ToString());//выписываем результирующую комбинацию вогнутого полигона
                    }
                }
            }
            Console.ReadKey();
        }

        #region methods
        /// <summary>
        /// Выполняет проверку всех трёх проекций полигона на выпуклость/невыпуклость
        /// </summary>
        /// <param name="curPoligon">Полигон с конкретной перестановкой узлов</param>
        /// <returns>Возвращает определение выпуклости полигона</returns>
        private static bool GetConvex(Polygon curPoligon)
        {
            List<Tuple<int, IList<double>>> collectRotations = GetCollectRotations(curPoligon);//собираем все повороты возле каждого угла каждой проекции

            bool convex = CheckingConvexity(collectRotations);//по итогу определяем и весь полигон

            return convex;//вогнутость выпуклость всего полигона
        }

        /// <summary>
        /// Выполняет проверку всех трёх проекций на выпуклость/вогнутость
        /// </summary>
        /// <param name="collRotations">Список трёх проекций с набором величин поворотов для них</param>
        /// <returns>Возвращает определение выпуклости многоугольника для данной перестановки узлов. Если результат false, то многоульник невыпуклый</returns>
        public static bool CheckingConvexity(List<Tuple<int, IList<double>>> collectRotations)//в листе collectRotations три проекции полигона
        {
            bool convex = false;
            bool convexOfSinglePtojection = false;
            List<bool> listAllConv = new List<bool>();//для трёх проекций

            foreach (var rotationOfSingleProj in collectRotations)//смотрим по одной проекции
            {
                IList<double> positiveRotations = new List<double>();
                IList<double> negativeRotations = new List<double>();

                foreach (var singleRot in rotationOfSingleProj.Item2)//в каждой проекции смотрим знак поворота около каждого узла
                {
                    if (singleRot > 0) positiveRotations.Add(singleRot);//и добавляем в свой лист по знаку
                    if (singleRot < 0) negativeRotations.Add(singleRot);//

                }
                //если все повороты по каждой проекции одного знака, то записываем true(выпуклая) для этой проекции
                if (positiveRotations.Count == rotationOfSingleProj.Item2.Count || negativeRotations.Count == rotationOfSingleProj.Item2.Count) convexOfSinglePtojection = true;
                else convexOfSinglePtojection = false;//иначе проекция (false) вогнутая
                listAllConv.Add(convexOfSinglePtojection);

            }

            //проверяем на вогнутость / выпуклость всего полигона по трём проекциям
            List<int> conditionCombinations = new List<int>();

            for (int i = 0; i < listAllConv.Count; i++)
            {
                if (listAllConv[i] == true) conditionCombinations.Add(i);
            }

            if (conditionCombinations.Count == listAllConv.Count) convex = true;//если каждая true - выпуклая, то весь вупуклый и весь полигон
            else convex = false;//если хотябы одна проекция вогнутая, то и вогнутый весь полигон 

            return convex;//возвращаем вогнутость/выпуклость всего полигона
        }

        /// <summary>
        /// Выполняет подсчет величин поворотов на всех узлах для трех проекций многоугольника для конкретной перестановки узлов
        /// </summary>
        /// <param name="curPoligon">Полигон с конкретной перестановкой узлов</param>
        /// <returns>Возвращает список трёх проекций с набором величин поворотов для них</returns>
        private static List<Tuple<int, IList<double>>> GetCollectRotations(Polygon curPoligon)
        {
            List<Tuple<int, IList<double>>> collectRotationsForEachProjections = new List<Tuple<int, IList<double>>>();

            LinkedList<Node> indexNode = initializationNodes();

            //Graphics.DrawPolygon();

            IList<double> collRotationsXY = new List<double>();//повороты в плоскости XY
            foreach (Node node in indexNode)//каждого узла
            {
                ////кинул в ВК ссылку этой формулы
                //Point3D ab = new Point3D();
                //ab.X = curPoligon.point3Ds[node.Value].X - curPoligon.point3Ds[node.PreviousNode().Value].X;//смотрим по текущему узлу и предыдущему
                //ab.Y = curPoligon.point3Ds[node.Value].Y - curPoligon.point3Ds[node.PreviousNode().Value].Y;
                //Point3D bc = new Point3D();
                //bc.X = curPoligon.point3Ds[node.NextNode().Value].X - curPoligon.point3Ds[node.Value].X;//по по текущему узлу и следующему
                //bc.Y = curPoligon.point3Ds[node.NextNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                //int rotation = (ab.X * bc.Y) - (ab.Y * bc.X);//формула для определения величины(и его знака) поворота возле угла
                int v1x = curPoligon.point3Ds[node.PreviousNode().Value].X - curPoligon.point3Ds[node.Value].X;
                int v1y = curPoligon.point3Ds[node.PreviousNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                int v2x = curPoligon.point3Ds[node.NextNode().Value].X - curPoligon.point3Ds[node.Value].X;
                int v2y = curPoligon.point3Ds[node.NextNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                Vector v1 = new Vector(v1x,v1y);
                Vector v2 = new Vector(v2x, v2y);
                double angle = Vector.AngleBetween(v2, v1);
                if (angle < 0) { angle = angle = 360 + angle; }
                angle = angle * (Math.PI / 180);
                double sinAngle = Math.Sin(angle);
                collRotationsXY.Add(sinAngle);//добавляем 4 величины поворотов в этой проекции в лист
            }
            collectRotationsForEachProjections.Add(new Tuple<int, IList<double>>(0, collRotationsXY));//и добавляем этот лист проекции в общую колекцию 

            IList<double> collRotationsYZ = new List<double>();//проекция YZ ... то же самое
            foreach (Node node in indexNode)
            {
                //Point3D ab = new Point3D();
                //ab.Y = curPoligon.point3Ds[node.Value].Y - curPoligon.point3Ds[node.PreviousNode().Value].Y;
                //ab.Z = curPoligon.point3Ds[node.Value].Z - curPoligon.point3Ds[node.PreviousNode().Value].Z;
                //Point3D bc = new Point3D();
                //bc.Y = curPoligon.point3Ds[node.NextNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                //bc.Z = curPoligon.point3Ds[node.NextNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                //int rotation = (ab.Y * bc.Z) - (ab.Z * bc.Y);
                int v1y = curPoligon.point3Ds[node.PreviousNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                int v1z = curPoligon.point3Ds[node.PreviousNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                int v2y = curPoligon.point3Ds[node.NextNode().Value].Y - curPoligon.point3Ds[node.Value].Y;
                int v2z = curPoligon.point3Ds[node.NextNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                Vector v1 = new Vector(v1y, v1z);
                Vector v2 = new Vector(v2y, v2z);
                double angle = Vector.AngleBetween(v2, v1);
                if (angle < 0) { angle = angle = 360 + angle; }
                angle = angle * (Math.PI / 180);
                double sinAngle = Math.Sin(angle);
                collRotationsYZ.Add(sinAngle);
            }
            collectRotationsForEachProjections.Add(new Tuple<int, IList<double>>(1, collRotationsYZ));

            IList<double> collRotationsZX = new List<double>();//ZX ... то же самое
            foreach (Node node in indexNode)
            {
                //Point3D ab = new Point3D();
                //ab.Z = curPoligon.point3Ds[node.Value].Z - curPoligon.point3Ds[node.PreviousNode().Value].Z;
                //ab.X = curPoligon.point3Ds[node.Value].X - curPoligon.point3Ds[node.PreviousNode().Value].X;
                //Point3D bc = new Point3D();
                //bc.Z = curPoligon.point3Ds[node.NextNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                //bc.X = curPoligon.point3Ds[node.NextNode().Value].X - curPoligon.point3Ds[node.Value].X;
                //int rotation = (ab.Z * bc.X) - (ab.X * bc.Z);
                int v1z = curPoligon.point3Ds[node.PreviousNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                int v1x = curPoligon.point3Ds[node.PreviousNode().Value].X - curPoligon.point3Ds[node.Value].X;
                int v2z = curPoligon.point3Ds[node.NextNode().Value].Z - curPoligon.point3Ds[node.Value].Z;
                int v2x = curPoligon.point3Ds[node.NextNode().Value].X - curPoligon.point3Ds[node.Value].X;
                Vector v1 = new Vector(v1z, v1x);
                Vector v2 = new Vector(v2z, v2x);
                double angle = Vector.AngleBetween(v2, v1);
                if (angle < 0) { angle = angle = 360 + angle; }
                angle = angle * (Math.PI / 180);
                double sinAngle = Math.Sin(angle);
                collRotationsZX.Add(sinAngle);
            }
            collectRotationsForEachProjections.Add(new Tuple<int, IList<double>>(2, collRotationsZX));
            return collectRotationsForEachProjections;
        }

        /// <summary>
        /// Выполняет создание кругового списка узлов многоугольника (с круговым доступом к ним)
        /// </summary>
        /// <returns>Возвращает связный список узлов многоульника</returns>
        private static LinkedList<Node> initializationNodes()
        {
            LinkedList<Node> indexNode = new LinkedList<Node>();
            Node nodeFirst = new Node(0, new Node(1), new Node(Polygon.size_curPoligon - 1));
            indexNode.AddFirst(nodeFirst);
            for (int i = 1; i < Polygon.size_curPoligon - 1; i++)
            {
                indexNode.AddLast(new Node(i, new Node(i + 1), new Node(i - 1)));
            }
            Node nodeLast = new Node(Polygon.size_curPoligon - 1, nodeFirst, new Node(Polygon.size_curPoligon - 2));
            indexNode.AddLast(nodeLast);
            return indexNode;
        }

        #endregion methods

        #region class Polygon
        public class Polygon
        {
            public static Dictionary<int, string> coordNode;

            public static int size_curPoligon;

            public static List<IDictionary<int, string>> arrCombinations = new List<IDictionary<int, string>>();

            public Point3D[] point3Ds { get; set; }

            private static List<int> Even = new List<int>();//чет
            private static int count = 0;
            public static int currentCombination = -1;

            int k = 0;
            public Polygon()
            {
                if (currentCombination == -1) GetDataFromFileForCombinations();

                currentCombination = currentCombination + 1;

                point3Ds = new Point3D[size_curPoligon];

                for (int i = 0; i <= size_curPoligon - 1; i++)
                {
                    if (size_curPoligon < 3) continue;

                    String[] tokens = arrCombinations[currentCombination].ElementAt(i).Value.Split(' ');

                    point3Ds[i].X = int.Parse(tokens[0]);
                    point3Ds[i].Y = int.Parse(tokens[1]);
                    point3Ds[i].Z = int.Parse(tokens[2]);
                }
            }

            private void GetDataFromFileForCombinations()
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    size_curPoligon = int.Parse(sr.ReadLine());
                    coordNode = new Dictionary<int, string>();
                    IDictionary<int, string> current = new Dictionary<int, string>(size_curPoligon);

                    for (int i = 0; i < size_curPoligon; i++)
                    {
                        String line = sr.ReadLine();
                        coordNode.Add(i + 1, line);
                    }
                    GetCombinationsNewPoints(coordNode, current);
                }

            }

            private void GetCombinationsNewPoints(IDictionary<int, string> coordNode, IDictionary<int, string> current)
            {
                if (coordNode.Values.Count == 0) //если все координаты использованы, получаем новую комбинацию
                {
                    k++;
                    count++;
                    if (count % 2 == 0) Even.Add(count);   //чет
                    int j = Even.Count % 2;//чет

                    if (j == 0) arrCombinations.Add(current);//добавляем четную перестановку в коллекцию. (если j != 0 то нечетная)
                    return;
                }

                foreach (var node in coordNode) //в цикле для каждого элемента прибавляем его к итоговой строке, создаем новый список из оставшихся элементов, и вызываем эту же функцию рекурсивно с новыми параметрами.
                {
                    IDictionary<int, string> dict = new Dictionary<int, string>(coordNode);
                    dict.Remove(node);
                    GetCombinationsNewPoints(dict, current.Append(node).ToDictionary(x => x.Key, y => y.Value));
                }
            }


            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("{0} {1} {2} {3}",
                  arrCombinations.ToArray()[currentCombination].ElementAt(0).Key,
                  arrCombinations.ToArray()[currentCombination].ElementAt(1).Key,
                  arrCombinations.ToArray()[currentCombination].ElementAt(2).Key,
                  arrCombinations.ToArray()[currentCombination].ElementAt(3).Key);

                return sb.ToString();
            }
        }

        #endregion class Polygon

        #region struct Point3D and class Node<T>

        public struct Point3D
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }

        public class Node
        {
            public int Value { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node(int value)
            {
                Value = value;
            }

            public Node(int value, Node next, Node previous)
            {
                Value = value;
                Previous = previous;
                Next = next;
            }

            public Node NextNode()
            {
                return Next;
            }

            public Node PreviousNode()
            {
                return Previous;
            }
        }
        #endregion struct Point3D and class Node<T>

        //public static bool CheckingConvexity(List<Tuple<int, IList<double>>> collectRotations)//в листе collectRotations три проекции полигона
        //{
        //    bool convex = false;
        //    //bool convexOfSinglePtojection = false;
        //    //List<bool> listAllConv = new List<bool>();//для трёх проекций
        //    List<double> sumSingleProj = new List<double>();
        //    foreach (var rotationOfSingleProj in collectRotations)//смотрим по одной проекции
        //    {
        //        //IList<int> positiveRotations = new List<int>();
        //        //IList<int> negativeRotations = new List<int>();

        //        //foreach (var singleRot in rotationOfSingleProj.Item2)//в каждой проекции смотрим знак поворота около каждого узла
        //        //{
        //        //if (singleRot > 0) positiveRotations.Add(singleRot);//и добавляем в свой лист по знаку
        //        //if (singleRot < 0) negativeRotations.Add(singleRot);//

        //        //}
        //        //если все повороты по каждой проекции одного знака, то записываем true (выпуклая) для этой проекции
        //        //if (positiveRotations.Count == rotationOfSingleProj.Item2.Count || negativeRotations.Count == rotationOfSingleProj.Item2.Count) convexOfSinglePtojection = true;
        //        //else convexOfSinglePtojection = false;//иначе проекция (false) вогнутая
        //        //listAllConv.Add(convexOfSinglePtojection);
        //        double sum = rotationOfSingleProj.Item2.ToArray().Sum();
        //        double[] posNum = rotationOfSingleProj.Item2.ToArray();
        //        for (int i = 0; i < rotationOfSingleProj.Item2.Count; i++)
        //        {
        //            if (rotationOfSingleProj.Item2.ElementAt(i) < 0) posNum[i] = Math.Abs(rotationOfSingleProj.Item2.ElementAt(i));
        //        }
        //        double sumAbs = posNum.Sum();
        //        if (Math.Abs(sum) == sumAbs)
        //            sumSingleProj.Add(1);
        //    }
        //    double Allsum = sumSingleProj.ToArray().Sum();
        //    if (Allsum == 3) convex = true;
        //    else convex = false;
        //    //проверяем на вогнутость/выпуклость всего полигона по трём проекциям
        //    //List<int> conditionCombinations = new List<int>();

        //    //for (int i = 0; i < listAllConv.Count; i++)
        //    //{
        //    //    if (listAllConv[i] == true) conditionCombinations.Add(i);
        //    //}

        //    //if (conditionCombinations.Count == listAllConv.Count) convex = true;//если каждая true - выпуклая, то весь вупуклый и весь полигон
        //    //else convex = false;//если хотябы одна проекция вогнутая, то и вогнутый весь полигон 

        //    return convex;//возвращаем вогнутость/выпуклость всего полигона
        //}
    }
}
