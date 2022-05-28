
Console.WriteLine("\n===========================================\n");

Console.WriteLine("Hello from Expressions Evaluator .. !!!!");

Console.WriteLine("\n===========================================\n");

var data = "1+2*3/6";
var data2 = "2 + 4 * 6";

Console.WriteLine($"Data: \"{data}\"");
Console.WriteLine($"Data2: \"{data2}\"");

Console.WriteLine("\n===========================================\n");

Console.WriteLine(
    $"Result: " +
    $"{new System.Data.DataTable().Compute(data, default)} | " +
    $"{new NCalc.Expression(data).Evaluate()} | " +
    $"{Evaluate(data)} | " +
    $"{Evaluate2(data)} | " +
    $"{Evaluate3(data)} | " +
    $"{Evaluate4(data)} | " +
    $"{Evaluate5(data2)} | " 
);

Console.WriteLine("\n===========================================\n");

Console.WriteLine(
    $"Result => (26 == ({data2})):" +
    $" {26 == (int)new NCalc.Expression(data2).Evaluate()}"
);

Console.WriteLine("\n===========================================\n");

Console.ReadLine();

static object Evaluate(string expression)
{
    const string Eval = nameof(Eval);
    var loDataTable = new System.Data.DataTable();
    var loDataColumn = new System.Data.DataColumn(Eval, typeof(object), expression);
    loDataTable.Columns.Add(loDataColumn);
    loDataTable.Rows.Add(0);
    return loDataTable.Rows[0][Eval];
}

static object Evaluate2(string expression) =>
    new System.Xml.XPath.XPathDocument(new StringReader("<r/>"))
        .CreateNavigator().Evaluate(string.Format("number({0})",
            new System.Text.RegularExpressions.Regex(@"([\+\-\*])")
                .Replace(expression, " ${1} ")
                .Replace("/", " div ")
                .Replace("%", " mod "))
    )
;

static double Evaluate3(string input)
{
    string expr = "(" + input + ")";

    Stack<string> ops = new();
    Stack<double> vals = new();

    for (int i = 0; i < expr.Length; i++)
    {
        string s = expr.Substring(i, 1);

        if (s.Equals("(")) { }

        else if (s.Equals("+")) ops.Push(s);
        else if (s.Equals("-")) ops.Push(s);
        else if (s.Equals("*")) ops.Push(s);
        else if (s.Equals("/")) ops.Push(s);
        else if (s.Equals("sqrt")) ops.Push(s);

        else if (s.Equals(")"))
        {
            int count = ops.Count;

            while (count > 0)
            {
                string op = ops.Pop();
                double v = vals.Pop();

                if (op.Equals("+")) v = vals.Pop() + v;
                else if (op.Equals("-")) v = vals.Pop() - v;
                else if (op.Equals("*")) v = vals.Pop() * v;
                else if (op.Equals("/")) v = vals.Pop() / v;
                else if (op.Equals("sqrt")) v = Math.Sqrt(v);

                vals.Push(v);

                count--;
            }
        }
        else vals.Push(double.Parse(s));
    }

    return vals.Pop();
}

static double Evaluate4(string expr)
{
    Stack<string> stack = new();

    string value = "";

    for (int i = 0; i < expr.Length; i++)
    {
        string s = expr.Substring(i, 1);

        char chr = s.ToCharArray()[0];

        if (!char.IsDigit(chr) && chr != '.' && value != "")
        {
            stack.Push(value);
            value = "";
        }

        if (s.Equals("("))
        {
            string innerExp = "";

            i++; //Fetch Next Character

            int bracketCount = 0;

            for (; i < expr.Length; i++)
            {
                s = expr.Substring(i, 1);

                if (s.Equals("("))
                    bracketCount++;

                if (s.Equals(")"))
                    if (bracketCount == 0)
                        break;
                    else
                        bracketCount--;

                innerExp += s;
            }

            stack.Push(Evaluate4(innerExp).ToString());
        }

        else if (s.Equals("+")) stack.Push(s);
        else if (s.Equals("-")) stack.Push(s);
        else if (s.Equals("*")) stack.Push(s);
        else if (s.Equals("/")) stack.Push(s);
        else if (s.Equals("sqrt")) stack.Push(s);

        else if (s.Equals(")"))
        {
        }

        else if (char.IsDigit(chr) || chr == '.')
        {
            value += s;

            if (value.Split('.').Length > 2)
                throw new Exception("Invalid decimal.");

            if (i == (expr.Length - 1))
                stack.Push(value);
        }
        else
            throw new Exception("Invalid character.");
    }

    double result = 0;

    while (stack.Count >= 3)
    {
        double right = Convert.ToDouble(stack.Pop());
        string op = stack.Pop();
        double left = Convert.ToDouble(stack.Pop());

        if (op == "+") result = left + right;
        else if (op == "+") result = left + right;
        else if (op == "-") result = left - right;
        else if (op == "*") result = left * right;
        else if (op == "/") result = left / right;

        stack.Push(result.ToString());
    }

    return Convert.ToDouble(stack.Pop());
}

static double Evaluate5(string expr)
{
    expr = expr.ToLower();
    expr = expr.Replace(" ", "");
    expr = expr.Replace("true", "1");
    expr = expr.Replace("false", "0");

    Stack<string> stack = new();

    string value = "";

    for (int i = 0; i < expr.Length; i++)
    {
        string s = expr.Substring(i, 1);

        // pick up any doublelogical operators first.
        if (i < expr.Length - 1)
        {
            string op = expr.Substring(i, 2);
            if (op == "<=" || op == ">=" || op == "==")
            {
                stack.Push(value);
                value = "";
                stack.Push(op);
                i++;
                continue;
            }
        }

        char chr = s.ToCharArray()[0];

        if (!char.IsDigit(chr) && chr != '.' && value != "")
        {
            stack.Push(value);
            value = "";
        }

        if (s.Equals("("))
        {
            string innerExp = "";
            i++; //Fetch Next Character
            int bracketCount = 0;

            for (; i < expr.Length; i++)
            {
                s = expr.Substring(i, 1);

                if (s.Equals("(")) bracketCount++;

                if (s.Equals(")"))
                {
                    if (bracketCount == 0) break;
                    bracketCount--;
                }
                innerExp += s;
            }

            stack.Push(Evaluate5(innerExp).ToString());
        }

        else if (s.Equals("+") ||
                 s.Equals("-") ||
                 s.Equals("*") ||
                 s.Equals("/") ||
                 s.Equals("<") ||
                 s.Equals(">"))

            stack.Push(s);

        else if (char.IsDigit(chr) || chr == '.')
        {
            value += s;

            if (value.Split('.').Length > 2)
                throw new Exception("Invalid decimal.");

            if (i == (expr.Length - 1))
                stack.Push(value);
        }

        else throw new Exception("Invalid character.");
    }

    double result = 0;
    List<string> list = stack.ToList<string>();

    for (int i = list.Count - 2; i >= 0; i--)
    {
        if (list[i] == "/")
        {
            list[i] = (Convert.ToDouble(list[i - 1]) / Convert.ToDouble(list[i + 1])).ToString();
            list.RemoveAt(i + 1);
            list.RemoveAt(i - 1);
            i -= 2;
        }
    }

    for (int i = list.Count - 2; i >= 0; i--)
    {
        if (list[i] == "*")
        {
            list[i] = (Convert.ToDouble(list[i - 1]) * Convert.ToDouble(list[i + 1])).ToString();
            list.RemoveAt(i + 1);
            list.RemoveAt(i - 1);
            i -= 2;
        }
    }

    for (int i = list.Count - 2; i >= 0; i--)
    {
        if (list[i] == "+")
        {
            list[i] = (Convert.ToDouble(list[i - 1]) + Convert.ToDouble(list[i + 1])).ToString();
            list.RemoveAt(i + 1);
            list.RemoveAt(i - 1);
            i -= 2;
        }
    }

    for (int i = list.Count - 2; i >= 0; i--)
    {
        if (list[i] == "-")
        {
            list[i] = (Convert.ToDouble(list[i - 1]) - Convert.ToDouble(list[i + 1])).ToString();
            list.RemoveAt(i + 1);
            list.RemoveAt(i - 1);
            i -= 2;
        }
    }

    stack.Clear();

    for (int i = 0; i < list.Count; i++) stack.Push(list[i]);

    while (stack.Count >= 3)
    {
        double right = Convert.ToDouble(stack.Pop());
        string op = stack.Pop();
        double left = Convert.ToDouble(stack.Pop());

        if (op == "<") result = (left < right) ? 1 : 0;
        else if (op == ">") result = (left > right) ? 1 : 0;
        else if (op == "<=") result = (left <= right) ? 1 : 0;
        else if (op == ">=") result = (left >= right) ? 1 : 0;
        else if (op == "==") result = (left == right) ? 1 : 0;

        stack.Push(result.ToString());
    }

    return Convert.ToDouble(stack.Pop());
}