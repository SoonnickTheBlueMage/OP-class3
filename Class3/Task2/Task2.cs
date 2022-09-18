using System.Text;
using Microsoft.VisualBasic;
using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;

namespace Task2
{
    public class Task2
    {

/*
 * В этом задании необходимо написать программу, способную табулировать сразу несколько
 * функций одной вещественной переменной на одном заданном отрезке.
 */


// Сформируйте набор как минимум из десяти вещественных функций одной переменной
internal static Dictionary<FunctionName, OneVariableFunction> AvailableFunctions =
            new Dictionary<FunctionName, OneVariableFunction>
            {
                { "square", x => x * x },
                { "sin", Math.Sin },
                { "cos", Math.Cos },
                { "double", x => 2 * x},
                { "pow2", x => Math.Pow(2, x)},
                { "half", x => x/2 },
                { "ans", Math.Abs},
                { "divPI", x => x / Math.PI},
                { "log2", Math.Log2},
                { "logE", Math.Log}
            };

// Тип данных для представления входных данных
internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

// Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("incorrect input data");
                return null;
            }
            double fromX = Convert.ToDouble(args[0]);
            double toX = Convert.ToDouble(args[1]);

            if (fromX > toX)
                (fromX, toX) = (toX, fromX);
            
            int numberOfPoints = Convert.ToInt32(args[2]);
            List<string> funNames = new List<string>();

            foreach (var item in args)
            {
                if (AvailableFunctions.Keys.Contains(item))
                    funNames.Add(item);
            }

            if (funNames.Count == 0)
            {
                Console.WriteLine("incorrect input data");
                return null;
            }
            
            return new InputData(fromX, toX, numberOfPoints, funNames);
        }

// Тип данных для представления таблицы значений функций
// с заголовками столбцов и строками (первый столбец --- значение x,
// остальные столбцы --- значения функций). Одно из полей --- количество знаков
// после десятичной точки.
        internal record FunctionTable(List<double> Table, int NumberOfpoints, List<string> FunctionNames)
        {
            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                string value = "X-value";
                sb.Append($"{value,-10}");

                foreach (var funName in FunctionNames)
                    sb.Append($" {funName,10}");

                for (int i = 0; i < Table.Count; i++)
                {
                    if (i % (FunctionNames.Count + 1) == 0)
                    {
                        sb.Append('\n');
                        sb.Append($"{Table[i],-10:F3}");
                    }
                    else
                    {
                        sb.Append(" ");
                        sb.Append($"{Table[i],10:F3}");
                    }
                }

                return sb.ToString();
            }
        }
    

/*
 * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
 * с заданным количеством точек.
 */
        internal static FunctionTable tabulate(InputData input)
        {
            var step = (input.ToX - input.FromX) / (input.NumberOfPoints - 1);
            var table = new List<double>();

            for (var x = input.FromX; x <= input.ToX; x += step)
            {
                table.Add(x);
                foreach (var function in input.FunctionNames) 
                    table.Add(AvailableFunctions[function](x));
            }

            return new FunctionTable(table, input.NumberOfPoints, input.FunctionNames);
        }
        
        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = prepareData(args);

            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            if (input != null) Console.WriteLine(tabulate(input));
        }
    }
}