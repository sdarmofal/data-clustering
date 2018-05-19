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

        static List<Point> ReadFromFile(string file)
        {
            var list = new List<Point>();

            foreach (var line in File.ReadAllLines(file))
            {
                var values = line.Split(' ');
                list.Add(new Point(StringToDouble(values[0]), StringToDouble(values[1])));
            }

            return list;
        }

        static void Main(string[] args)
        {
            string DATA_ROOT_PATH = @"D:\Programowanie\C#\Sztuczna inteligencja\Data clustering\Data clustering\data\";
            string filename = @"spiral.txt";

            List<Point> points = ReadFromFile(DATA_ROOT_PATH + filename);
        }
    }
}
