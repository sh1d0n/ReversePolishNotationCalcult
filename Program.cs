using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcutSharp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class ReversePolishCalculator
    {
        Form1 form1 = new Form1();
        public string[] Start(string input)
        {
            string[] forReturn = { "", "", ""};

            if (input[0] == '-')
            {
                input = '0' + input;
            }

            try
            {
                string rpnExpression = ConvertToReversePolishNotation(input);
                forReturn[0] = rpnExpression;

                double result = EvaluateReversePolishNotation(rpnExpression);
                forReturn[1] = result.ToString("0.###");
            }
            catch (Exception ex)
            {
                forReturn[2] = "Ошибка: " + ex.Message;
            }

            return forReturn;
        }

        static string ConvertToReversePolishNotation(string input)
        {
            StringBuilder output = new StringBuilder();
            Stack<char> stack = new Stack<char>();

            foreach (char token in input)
            {
                if (char.IsDigit(token) || token == ',' || token == '.')
                {
                    if (token == '.')
                    {
                        output.Append(',');
                    }
                    else
                    {
                        output.Append(token);
                    }
                }
                else if (IsOperator(token))
                {
                    SetSpace(output);

                    while (stack.Count > 0 && IsOperator(stack.Peek()) && GetOperatorPrecedence(token) <= GetOperatorPrecedence(stack.Peek()))
                    {
                        output.Append(stack.Pop());
                        SetSpace(output);
                    }

                    stack.Push(token);
                }
                else if (token == '(')
                {
                    stack.Push(token);
                }
                else if (token == ')')
                {
                    SetSpace(output);
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output.Append(stack.Pop());
                        SetSpace(output);
                    }

                    if (stack.Count == 0 || stack.Peek() != '(')
                    {
                        throw new ArgumentException("Некорректное выражение.");
                    }

                    stack.Pop();
                }
            }

            while (stack.Count > 0)
            {
                if (stack.Peek() == '(' || stack.Peek() == ')')
                {
                    throw new ArgumentException("Некорректное выражение.");
                }

                SetSpace(output);
                output.Append(stack.Pop());
            }

            return output.ToString().Trim();
        }

        static double EvaluateReversePolishNotation(string input)
        {
            Stack<double> stack = new Stack<double>();
            string[] tokens = input.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (IsOperator(token))
                {
                    if (token == "√")
                    {
                        if (stack.Count < 1)
                        {
                            throw new ArgumentException("Некорректное выражение.");
                        }

                        double operand = stack.Pop();
                        double result = PerformOperation(token, operand);
                        stack.Push(result);
                    }
                    else
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Некорректное выражение.");
                        }

                        double operand2 = stack.Pop();
                        double operand1 = stack.Pop();
                        double result = PerformOperation(token, operand1, operand2);
                        stack.Push(result);
                    }
                }
                else
                {
                    throw new ArgumentException("Некорректное выражение.");
                }
            }

            if (stack.Count != 1)
            {
                throw new ArgumentException("Некорректное выражение.");
            }

            return stack.Pop();
        }

        static bool IsOperator(char token)
        {
            return token == '+' || token == '-' || token == '*' || token == '/' || token == '√' || token == '^';
        }

        static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "√" || token == "^";
        }

        static int GetOperatorPrecedence(char op)
        {
            if (op == '+' || op == '-')
            {
                return 1;
            }
            else if (op == '*' || op == '/')
            {
                return 2;
            }
            else if (op == '√' || op == '^')
            {
                return 3;
            }

            throw new ArgumentException("Некорректный оператор.");
        }

        static double PerformOperation(string token, double operand1, double operand2 = 0)
        {
            if (token == "+")
            {
                return operand1 + operand2;
            }
            else if (token == "-")
            {
                return operand1 - operand2;
            }
            else if (token == "*")
            {
                return operand1 * operand2;
            }
            else if (token == "/")
            {
                if (operand2 == 0)
                {
                    throw new DivideByZeroException("Деление на ноль.");
                }
                return operand1 / operand2;
            }
            else if (token == "^")
            {
                return Math.Pow(operand1, operand2);
            }
            else if (token == "√")
            {
                return Math.Sqrt(operand1);
            }

            throw new ArgumentException("Некорректный оператор.");
        }

        static void SetSpace(StringBuilder output)
        {
            if (output.Length == 0)
            {
                return;
            }
            if (output[output.Length - 1] != ' ')
            {
                output.Append(" ");
            }
        }
    }
}
