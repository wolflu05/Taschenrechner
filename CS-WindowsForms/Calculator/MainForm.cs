using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(Keypress_Handler);
        }

        private string input = string.Empty;
        private bool hasError = false;

        // Operators
        private Dictionary<char, Tuple<Int16, Func<double, double, double>>> operators = new Dictionary<char, Tuple<Int16, Func<double, double, double>>>
        {
            { '+', new Tuple<Int16, Func<double, double, double>> ( 2, (a, b) => a + b ) },
            { '-', new Tuple<Int16, Func<double, double, double>> ( 2, (a, b) => a - b ) },
            { '*', new Tuple<Int16, Func<double, double, double>> ( 3, (a, b) => a * b ) },
            { '/', new Tuple<Int16, Func<double, double, double>> ( 3, (a, b) => a / b ) },
        };

        // Functions
        private Dictionary<string, Tuple<Int16, Func<double[], double>>> functions = new Dictionary<string, Tuple<Int16, Func<double[], double>>>
        {
            // func name -> [arg count, funcResolver(double[] params) -> double]
            { "sin", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Sin(param[0])) },
            { "asin", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Asin(param[0])) },
            { "cos", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Cos(param[0])) },
            { "acos", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Acos(param[0])) },
            { "tan", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Tan(param[0])) },
            { "atan", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Atan(param[0])) },
            { "ceil", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Ceiling(param[0])) },
            { "floor", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Floor(param[0])) },
            { "sqrt", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Sqrt(param[0])) },
            { "min", new Tuple<Int16, Func<double[], double>> (2, (double[] param) => Math.Min(param[0], param[1])) },
            { "max", new Tuple<Int16, Func<double[], double>> (2, (double[] param) => Math.Max(param[0], param[1])) },
            { "abs", new Tuple<Int16, Func<double[], double>> (1, (double[] param) => Math.Abs(param[0])) },
        };

        // Calculator functions
        private void addToken(string token)
        {
            // auto add bracket when adding a function
            if(this.functions.Keys.Contains(token))
            {
                token += "(";
            }

            this.input += token;
            this.updateText();
        }

        private void removeLastToken()
        {
            if (this.hasError)
            {
                this.result.Text = this.input;
                this.hasError = false;
                this.updateText();
                return;
            }

            if (this.input.Length <= 0) return;

            this.input = this.input.Substring(0, this.input.Length - 1);
            this.updateText();
        }

        private void clearTokens()
        {
            this.input = string.Empty;
            this.hasError = false;
            this.updateText();
        }

        private void evaluateEquation()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Infix: " + this.result.Text);

                var rpn = this.infixToRpn(this.input);

                System.Diagnostics.Debug.WriteLine("Rpn: " + String.Join(' ', rpn));

                var res = this.evaluateRpn(rpn);

                System.Diagnostics.Debug.WriteLine("Result: " + res.ToString());

                this.input = res.ToString();
                this.updateText();
            }
            catch (Exception e)
            {
                this.result.Text = e.Message;
                this.hasError = true;
                this.resizeText();
            }
        }

        private List<string> infixToRpn(string infixTokens)
        {
            var outputQueue = new List<string>();
            var operatorStack = new Stack<string>();

            var splitChars = new List<String> { " ", ";", "(", ")" }
                .Concat(this.operators.Keys.Select(x => x.ToString()))
                .Concat(this.functions.Keys);
            var splitRegex = "(" + String.Join("|", splitChars.Select(x => Regex.Escape(x))) + ")";

            string? prevToken = null;
            foreach (var token in Regex.Split(infixTokens, splitRegex))
            {
                if (token == String.Empty) continue;

                double val = 0;
                if (Double.TryParse(token, out val)) // --- number
                {
                    outputQueue.Add(token);
                }
                else if (token == "(") // --- left (
                {
                    operatorStack.Push("(");
                }
                else if (token == ")") // --- right )
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Add(operatorStack.Pop());
                    }
                    
                    if (operatorStack.Count == 0 || operatorStack.Peek() != "(") throw new Exception("Missing (");
                    operatorStack.Pop(); // remove (

                    if (operatorStack.Count > 0 && this.functions.Keys.Contains(operatorStack.Peek()))
                    {
                        outputQueue.Add(operatorStack.Pop());
                    }
                }
                else if (token == "-" && (prevToken == null || prevToken == "(" || this.operators.Keys.Contains(prevToken.First())))
                {
                    outputQueue.Add("u");
                }
                else if (token == ";")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Add(operatorStack.Pop());
                    }
                }
                else if (this.functions.Keys.Contains(token)) // --- functions
                {
                    operatorStack.Push(token);
                }
                else if (this.operators.Keys.Contains(token.First())) // --- operators
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(" && this.operators[operatorStack.Peek().First()].Item1 >= this.operators[token.First()].Item1)
                    {
                        outputQueue.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }

                prevToken = token;
            }

            while (operatorStack.Count > 0)
            {
                var last = operatorStack.Pop();
                if (last == "(") throw new Exception("Missing )");

                outputQueue.Add(last);
            }

            return outputQueue;
        }

        private double evaluateRpn(List<String> tokens)
        {
            if (tokens.Count == 0) return 0.0;

            var stack = new Stack<double>();

            while (tokens.Count > 0)
            {
                var token = tokens.First();
                tokens.RemoveAt(0);

                double val = 0;
                if (token == "u")
                {
                    double.TryParse(tokens.First(), out val);
                    tokens.RemoveAt(0);
                    stack.Push(-val);
                }
                else if (double.TryParse(token, out val))
                {
                    stack.Push(val);
                }
                else if (this.operators.Keys.Contains(token.First()))
                {
                    if (stack.Count < 2) throw new Exception($"Error: {token} expects two operands");

                    var a = stack.Pop();
                    var b = stack.Pop();

                    stack.Push(this.operators[token.First()].Item2(b, a));
                }
                else if (this.functions.Keys.Contains(token))
                {
                    var func = this.functions[token];

                    if (stack.Count < func.Item1) throw new Exception($"Error: {token} expects {func.Item1} arguments");

                    // create a list of arguments passed to the function resolver
                    var args = new List<double>(func.Item1);
                    for (int i = 0; i < func.Item1; i++) {
                        args.Add(stack.Pop());
                    }
                    args.Reverse();

                    stack.Push(func.Item2(args.ToArray()));
                }
            }

            if (stack.Count != 1)
            {
                throw new Exception("Error");
            }

            return stack.Peek();
        }

        private void updateText()
        {
            this.result.Text = this.input;
            this.resizeText();
        }

        private void resizeText()
        {
            // Auto resize output text label font size
            // See https://stackoverflow.com/a/25448687
            if (this.result.Text.Length > 0)
            {
                Image fakeImage = new Bitmap(1, 1);
                Graphics graphics = Graphics.FromImage(fakeImage);

                SizeF extent = graphics.MeasureString(this.result.Text, this.result.Font);

                float hRatio = this.result.Height / extent.Height;
                float wRatio = this.result.Width / extent.Width;
                float ratio = (hRatio < wRatio) ? hRatio : wRatio;
                float newSize = this.result.Font.Size * ratio;

                this.result.Font = new Font(this.result.Font.FontFamily, newSize, this.result.Font.Style);
            }
        }

        // Eventhandlers
        private void Handle_Click(System.Object sender, EventArgs e)
        {
            var buttonText = ((System.Windows.Forms.Button)sender).Text;

            var textMap = new Dictionary<string, string>
            {
                { "÷", "/" },
                { "×", "*" },
                { "√", "sqrt" }
            };

            foreach (var old in textMap.Keys)
            {
                buttonText = buttonText.Replace(old, textMap[old]);
            }

            this.addToken(buttonText);
        }

        private void HandleClear_Click(System.Object sender, EventArgs e)
        {
            this.clearTokens();
        }

        private void HandleDelete_Click(System.Object sender, EventArgs e)
        {
            this.removeLastToken();
        }

        private void HandleEqual_Click(System.Object sender, EventArgs e)
        {
            this.evaluateEquation();
        }

        private void Keypress_Handler(object? sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\b') // delete one char
            {
               this.removeLastToken();
            }
            else if(e.KeyChar == '\u007f') // clear input
            {
                this.clearTokens();
            }
            else
            {
                this.addToken(e.KeyChar.ToString());
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.evaluateEquation();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
