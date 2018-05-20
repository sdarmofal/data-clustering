using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Data_clustering
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static double CalculateEuclideanMetric(Point point1, Point point2)
        {
            return Math.Sqrt(
                (point2.X - point1.X) * (point2.X - point1.X) +
                (point2.Y - point1.Y) * (point2.Y - point1.Y)
            );
        }
    }

    class Program
    {
        static double StringToDouble(string value)
        {
            double parsedValue;
            value = value.Trim();
            if (!double.TryParse(value.Replace(',', '.'), out parsedValue) && !double.TryParse(value.Replace('.', ','), out parsedValue))
                throw new Exception("Can't convert string to double");

            return parsedValue;
        }

        static List<List<Point>> ReadFromFile(string file)
        {
            var list = new List<List<Point>>();

            foreach (var line in File.ReadAllLines(file))
            {
                var values = line.Split(' ');
                Point point = new Point(StringToDouble(values[0]), StringToDouble(values[1]));
                List<Point> pointList = new List<Point>();
                pointList.Add(point);
                list.Add(pointList);
            }

            return list;
        }

        static Dictionary<string, List<Point>> FindNearestGroups(List<List<Point>> groups)
        {
            SortedDictionary<double, Dictionary<string, List<Point>>> groupsDistance = new SortedDictionary<double, Dictionary<string, List<Point>>>();

            foreach (List<Point> group in groups)
            {
                List<List<Point>> currentGroups = new List<List<Point>>(groups);
                currentGroups.Remove(group);
                var nearest = FindNearestGroup(currentGroups, group);
                Dictionary<string, List<Point>> dictBuilder = new Dictionary<string, List<Point>>();
                dictBuilder.Add("current", group);
                dictBuilder.Add("another", nearest.Value);
                if (!groupsDistance.Keys.Contains(nearest.Key))
                {
                    groupsDistance.Add(nearest.Key, dictBuilder);
                }
            }
            return groupsDistance.First().Value;
        }

        static KeyValuePair<double, List<Point>> FindNearestGroup(List<List<Point>> groups, List<Point> currentGroup)
        {
            SortedDictionary<double, List<Point>> nearestPointsInGroups = new SortedDictionary<double, List<Point>>();
            SortedDictionary<double, List<Point>> nearestGroups = new SortedDictionary<double, List<Point>>();

            foreach (List<Point> group in groups)
            {
                nearestPointsInGroups.Clear();
                foreach (Point currentGroupPoint in currentGroup)
                {                    
                    foreach (Point anotherGroupPoint in group)
                    {
                        double metric = Point.CalculateEuclideanMetric(currentGroupPoint, anotherGroupPoint);
                        if (! nearestPointsInGroups.Keys.Contains(metric))
                        {
                            nearestPointsInGroups.Add(metric, group);
                        }
                    }
                }

                // Average point
                int avgPoint = nearestPointsInGroups.Keys.Count / 2;
                KeyValuePair<double, List<Point>> element = nearestPointsInGroups.ElementAt(avgPoint);

                // Nearest point
                // KeyValuePair<double, List<Point>> element = nearestPointsInGroups.ElementAt(0);

                // Fartest point
                //KeyValuePair<double, List<Point>> element = nearestPointsInGroups.ElementAt(nearestPointsInGroups.Count - 1);


                nearestGroups.Add(element.Key, element.Value);
            }

            return nearestGroups.First();
        }

        static void PrintGroups(List<List<Point>> groups)
        {
            foreach (List<Point> group in groups)
            {
                Console.WriteLine("GROUP: ");
                foreach (Point point in group)
                {
                    Console.WriteLine("\t" + point.X + "\t" + point.Y);
                }
            }
        }

        static void Main(string[] args)
        {
            string DATA_ROOT_PATH = @"D:\Programowanie\C#\Sztuczna inteligencja\Data clustering\Data clustering\data\";
            string filename = @"spiral.txt";

            List<List<Point>> groups = ReadFromFile(DATA_ROOT_PATH + filename);

            do
            {
                var nearestGroups = FindNearestGroups(groups);
                groups.Remove(nearestGroups["current"]);
                groups.Remove(nearestGroups["another"]);
                nearestGroups["current"].AddRange(nearestGroups["another"]);
                groups.Add(nearestGroups["current"]);
            } while (groups.Count > 3);

            PrintGroups(groups);
            Console.ReadKey();
        }
    }
}
