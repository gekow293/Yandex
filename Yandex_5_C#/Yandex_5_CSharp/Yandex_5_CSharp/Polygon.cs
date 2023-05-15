//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Yandex_5_CSharp
//{
//    public class Polygon
//    {
//        public static Dictionary<int, string> coordNode;//хранилище координат для каждого узла

//        public static int size_polygon;

//        public static List<Dictionary<int, string>> evenCombinations = new List<Dictionary<int, string>>(); //хранилище комбинаций

//        public Point3D[] point3Ds { get; set; }

//        private static List<int> Even = new List<int>();//лист для четных перестановок(ведь надо типа взять из всех перестановок только четные, то есть в исходном положении узлов узлы будут поменяны местами четное количество раз(0,2,4,...))
//        private int count = 0;
//        public static int currentCombination = -1;//счетчик полученных полигонов. Каждый раз увеличивается при создании нового полигона, если предыдущий не прошел проверку в методе Main() на невыпуклость(вогнутость). Нам нужны вогнутые (хотя бы по одной проекции)

//        public Polygon()
//        {
//            if (currentCombination == -1) GetDataFromFileForCombinations();//cчитывем входной файл один раз

//            currentCombination = currentCombination + 1;//увеличиваем счетчик при создании нового полигона

//            point3Ds = new Point3D[size_polygon];//создаем массив узлов полигона

//            for (int i = 0; i <= size_polygon - 1; i++)//инициализируем каждый раз при создании нового полигона его узлы тремя координатами
//            {
//                if (size_polygon < 3) continue;

//                String[] tokens = evenCombinations[currentCombination].ElementAt(i).Value.Split(' ');//берем текущую(следующую) комбинацию и инициализируем узлы своими координатами в том же порядке

//                point3Ds[i].X = int.Parse(tokens[0]);
//                point3Ds[i].Y = int.Parse(tokens[1]);
//                point3Ds[i].Z = int.Parse(tokens[2]);
//            }
//            //получается 4 узла с 3-мя координатами
//        }

//        private void GetDataFromFileForCombinations()
//        {
//            using (StreamReader sr = new StreamReader("input.txt"))
//            {
//                size_polygon = int.Parse(sr.ReadLine());//считываем количество узлов
//                coordNode = new Dictionary<int, string>();
//                Dictionary<int, string> current = new Dictionary<int, string>(size_polygon);

//                for (int i = 0; i <= size_polygon - 1; i++)//считываем координаты узлов
//                {
//                    String line = sr.ReadLine();
//                    coordNode.Add(i + 1, line);
//                }
//                GetCombinationsNewPoints(coordNode, current);//рекурсивная функция создания комбинаций перестановок узлов в исходном полигоне и записываем их в лист evenCombinations, который будет использоваться в классе Program
//            }
//        }

//        private void GetCombinationsNewPoints(Dictionary<int, string> coordNode, Dictionary<int, string> current)
//        {
//            if (coordNode.Values.Count == 0) //если все координаты использованы, получаем новую комбинацию
//            {
//                count++;//просто счетчик
//                if (count % 2 == 0) Even.Add(count);   //добавляем только четное число
//                int j = Even.Count % 2;//берем четную запись в Even

//                if (j == 0) evenCombinations.Add(current);//добавляем четную перестановку в коллекцию. (если j != 0 то нечетная)
//                return;
//            }

//            foreach (var node in coordNode) //в цикле для каждого элемента прибавляем его к итоговой строке, создаем новый список из оставшихся элементов, и вызываем эту же функцию рекурсивно с новыми параметрами.
//            {
//                IDictionary<int, string> dict = new Dictionary<int, string>(coordNode);
//                dict.Remove(node);
//                GetCombinationsNewPoints(dict.ToDictionary(k => k.Key, v => v.Value), 
//                    current.ToArray().Append(node).ToDictionary(k => k.Key, v => v.Value));
//            }
//        }


//        public override string ToString()
//        {
//            StringBuilder sb = new StringBuilder();

//            //sb.AppendFormat("[[{0}], [{1}], [{2}], [{3}]]",
//            //    arrCombinations.ToArray()[currentCombination].ElementAt(0).Value,
//            //    arrCombinations.ToArray()[currentCombination].ElementAt(1).Value,
//            //    arrCombinations.ToArray()[currentCombination].ElementAt(2).Value,
//            //    arrCombinations.ToArray()[currentCombination].ElementAt(3).Value).

//            sb.AppendFormat("{0} {1} {2} {3}",
//              evenCombinations.ToArray()[currentCombination].ElementAt(0).Key,
//              evenCombinations.ToArray()[currentCombination].ElementAt(1).Key,
//              evenCombinations.ToArray()[currentCombination].ElementAt(2).Key,
//              evenCombinations.ToArray()[currentCombination].ElementAt(3).Key);

//            return sb.ToString();
//        }


//    }

//    public struct Point3D
//    {
//        public int X { get; set; }
//        public int Y { get; set; }
//        public int Z { get; set; }

//        //public int Number { get; set; }
//    }
//}