

using System.Data;

Console.WriteLine("Hello from Expressions Evaluator .. !!!!\n");

var data = "1+2*3/6";

Console.WriteLine($"Data: \"{data}\"\n");

var result = new DataTable().Compute(data, default);

Console.WriteLine($"Result: {result}");

Console.ReadLine();