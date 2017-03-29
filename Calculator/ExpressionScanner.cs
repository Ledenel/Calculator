using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class ExpressionScanner
    {
        string numString = "";
        bool hasDot = false;

        public string NumString
        {
            get
            {
                return numString;
            }
            private set
            {
                numString = value;
            }
        }

        public void Flush()
        {
            if (NumString != "" && NewNumDiscovered != null)
            {
                NewNumDiscovered(this, double.Parse(NumString));
            }
            NumString = "";
            hasDot = false;
        }

        public event EventHandler<double> NewNumDiscovered;
        public event EventHandler<string> NewOperatorDiscovered;
        public event EventHandler<string> NumInputed;

        public void ClearCurrent()
        {
            NumString = "";
            hasDot = false;
        }

        public void Back()
        {
            if(this.NumString != "")
            {
                this.NumString = this.NumString.Substring(0, NumString.Length - 1);
                if (NumInputed != null)
                {
                    NumInputed(this, "");
                }
            }
        }

        public void Write(string token)
        {
            int result = 0;
            if (int.TryParse(token, out result))
            {
                NumString += token;
                if (NumInputed != null)
                {
                    NumInputed(this, token);
                }
            }
            else if (token == ".")
            {
                if (!hasDot && NumString != "")
                {
                    NumString += token;
                    hasDot = true;
                }              
            }
            else
            {
                if (NumString != "" && NewNumDiscovered != null)
                {
                    NewNumDiscovered(this, double.Parse(NumString));
                }
                if (NewOperatorDiscovered != null)
                {
                    NewOperatorDiscovered(this, token);
                }
                ClearCurrent();
            }
        }
    }

    public class ExpressionDisplayer
    {
        ExpressionScanner es;
        public ExpressionDisplayer(ExpressionScanner es)
        {
            this.es = es;
            es.NewNumDiscovered += Es_NewNumDiscovered;
            es.NewOperatorDiscovered += Es_NewOperatorDiscovered;
        }

        public event EventHandler<string> ExpressionChanged;

        private void Es_NewNumDiscovered(object sender, double e)
        {
            this.num = e;
            neednum = false;
        }

        double num;
        string op;

        bool neednum = true;
        public Func<double> ResultProvider = () => 0;

        private void Es_NewOperatorDiscovered(object sender, string e)
        {
            if (Operator.FromName(e).VaribleCount > 1)
            {
                if (neednum)
                {
                    if (history == "")
                    {
                        history += ResultProvider().ToString() + " ";
                    }
                }
                else
                {
                    history += string.Format("{0} {1} ", op, num);
                }
                op = e;
                if (ExpressionChanged != null)
                {
                    ExpressionChanged(this, Current);
                }

            }
            neednum = true;
        }

        private string history = "";

        public string Current
        {
            get
            {
                return history + op;
            }
        }

        public void Clear()
        {
            num = 0;
            op = null;
            history = "";
            neednum = true;
            if (ExpressionChanged != null)
            {
                ExpressionChanged(this, Current);
            }
        }
    }
}
