using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonDrawWpfApp
{
    class MyPolygon
    {
        public static Dictionary<int, string> coordNode;

        public static int size_curPoligon;

        //public static List<IDictionary<int, string>> evenCombinations = new List<IDictionary<int, string>>();
        //public List<List<Point3D.Axis>> point3Ds { get; set; }
        public static List<IDictionary<int, string>> evenCombinations = new List<IDictionary<int, string>>();

        public Point3D[] point3Ds { get; set; }

        private static List<int> Even = new List<int>();//чет
        private static int count = 0;
        public static int currentCombination = -1;

        public MyPolygon()
        {
            if (currentCombination == -1) GetDataFromFileForCombinations();//cчитывем входной файл один раз

            currentCombination = currentCombination + 1;//увеличиваем счетчик при создании нового полигона

            point3Ds = new Point3D[size_curPoligon];//создаем массив узлов полигона

            //List<Tuple<int, int, int>> listNodesWithKoord = new List<Tuple<int, int, int>>();

            for (int i = 0; i <= size_curPoligon - 1; i++)//инициализируем каждый раз при создании нового полигона его узлы тремя координатами
            {
                if (size_curPoligon < 3) continue;

                String[] tokens = evenCombinations[currentCombination].ElementAt(i).Value.Split(' ');//берем текущую(следующую) комбинацию и инициализируем узлы своими координатами в том же порядке

                point3Ds[i].X = int.Parse(tokens[0]);
                point3Ds[i].Y = int.Parse(tokens[1]);
                point3Ds[i].Z = int.Parse(tokens[2]);

                //for (int j = 0; j < 3; j++) { listNodesWithKoord.Add(new Tuple<int, int, int>(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]))); }
            }
            //получается 4 узла с 3-мя координатами
        }

        private void GetDataFromFileForCombinations()
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                size_curPoligon = int.Parse(sr.ReadLine());//считываем количество узлов
                coordNode = new Dictionary<int, string>();
                Dictionary<int, string> current = new Dictionary<int, string>(size_curPoligon);

                for (int i = 0; i <= size_curPoligon - 1; i++)//считываем координаты узлов
                {
                    String line = sr.ReadLine();
                    coordNode.Add(i + 1, line);
                }
                GetCombinationsNewPoints(coordNode, current);//рекурсивная функция создания комбинаций перестановок узлов в исходном полигоне и записываем их в лист evenCombinations, который будет использоваться в классе Program
            }
        }

        private void GetCombinationsNewPoints(Dictionary<int, string> coordNode, Dictionary<int, string> current)
        {
            if (coordNode.Values.Count == 0) //если все координаты использованы, получаем новую комбинацию
            {
                count++;//просто счетчик
                if (count % 2 == 0) Even.Add(count);   //добавляем только четное число
                int j = Even.Count % 2;//берем четную запись в Even

                if (j == 0) evenCombinations.Add(current);//добавляем четную перестановку в коллекцию. (если j != 0 то нечетная)
                return;
            }

            foreach (var node in coordNode) //в цикле для каждого элемента прибавляем его к итоговой строке, создаем новый список из оставшихся элементов, и вызываем эту же функцию рекурсивно с новыми параметрами.
            {
                IDictionary<int, string> dict = new Dictionary<int, string>(coordNode);
                dict.Remove(node);
                GetCombinationsNewPoints(dict.ToDictionary(k => k.Key, v => v.Value),
                    current.ToArray().Append(node).ToDictionary(k => k.Key, v => v.Value));
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} {2} {3}",
              evenCombinations.ToArray()[currentCombination].ElementAt(0).Key,
              evenCombinations.ToArray()[currentCombination].ElementAt(1).Key,
              evenCombinations.ToArray()[currentCombination].ElementAt(2).Key,
              evenCombinations.ToArray()[currentCombination].ElementAt(3).Key);

            return sb.ToString();
        }

        public static LinkedList<Node> initializationNodes()
        {
            LinkedList<Node> indexNode = new LinkedList<Node>();
            Node nodeFirst = new Node(0, new Node(1), new Node(MyPolygon.size_curPoligon - 1));
            indexNode.AddFirst(nodeFirst);
            for (int i = 1; i < MyPolygon.size_curPoligon - 1; i++)
            {
                indexNode.AddLast(new Node(i, new Node(i + 1), new Node(i - 1)));
            }
            Node nodeLast = new Node(MyPolygon.size_curPoligon - 1, nodeFirst, new Node(MyPolygon.size_curPoligon - 2));
            indexNode.AddLast(nodeLast);
            return indexNode;
        }

        //public static LinkedList<Point3D.Axis> initializePoint3D(String[] tokens)
        //{
        //    LinkedList<Point3D.Axis> indexPoint3D = new LinkedList<Point3D.Axis>();
        //    indexPoint3D.AddFirst(new Point3D.Axis(int.Parse(tokens[0]), new Point3D.Axis(int.Parse(tokens[1])), new Point3D.Axis(int.Parse(tokens[2]))));
        //    indexPoint3D.AddLast(new Point3D.Axis(int.Parse(tokens[1]), new Point3D.Axis(int.Parse(tokens[2])), new Point3D.Axis(int.Parse(tokens[0]))));
        //    indexPoint3D.AddLast(new Point3D.Axis(int.Parse(tokens[2]), new Point3D.Axis(int.Parse(tokens[0])), new Point3D.Axis(int.Parse(tokens[1]))));
        //    return indexPoint3D;
        //}
    }

    public struct Point3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    //class Point3D
    //{
    //    public int Value { get; set; }
    //    public Point3D Next { get; set; }
    //    public Point3D Previous { get; set; }
       

    //    public Point3D(int value, Point3D next, Point3D prev)
    //    {
    //        Value = value;
    //        Next = next;
    //        Previous = prev;
    //    }

    //    public Point3D NextPoint()
    //    {
    //        return Next;
    //    }

    //    public Point3D PreviousPoint()
    //    {
    //        return Previous;
    //    }

    //    public class Axis
    //    {
    //        public int Value { get; set; }
    //        public Axis Next { get; set; }
    //        public Axis Prev { get; set; }

    //        public Axis(int axis, Axis next, Axis prev)
    //        {
    //            Value = axis;
    //            Next = next;
    //            Prev = prev;
    //        } 

    //        public Axis(int value)
    //        {
    //            Value = value;
    //        }
            
    //        public Axis NextAxis()
    //        {
    //            return Next;
    //        }

    //        public Axis Previous()
    //        {
    //            return Prev;
    //        }
    //    } 
    //}

    class Node
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
}
