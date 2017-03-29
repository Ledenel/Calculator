using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
        }

        ExpressionScanner es;
        Calculator calc;

        Dictionary<Keys, string> keyMap = new Dictionary<Keys, string>{
            { Keys.Enter,"eval"},
            { Keys.Back, "back" },
            { Keys.Delete, "clear" },
            { Keys.Escape, "clearall" },
            { Keys.D0, "0" },
            { Keys.D1, "1" },
            { Keys.D2, "2" },
            { Keys.D3, "3" },
            { Keys.D4, "4" },
            { Keys.D5, "5" },
            { Keys.D6, "6" },
            { Keys.D7, "7" },
            { Keys.D8, "8" },
            { Keys.D9, "9" },
            { Keys.NumPad0, "0" },
            { Keys.NumPad1, "1" },
            { Keys.NumPad2, "2" },
            { Keys.NumPad3, "3" },
            { Keys.NumPad4, "4" },
            { Keys.NumPad5, "5" },
            { Keys.NumPad6, "6" },
            { Keys.NumPad7, "7" },
            { Keys.NumPad8, "8" },
            { Keys.NumPad9, "9" },
            { Keys.Add, "+" },
            { Keys.Subtract, "-" },
            { Keys.Multiply, "*" },
            { Keys.Divide, "/" },
            { Keys.Decimal, "." },
        };

        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            es = new ExpressionScanner();
            ed = new ExpressionDisplayer(es);
            es.NumInputed += Es_NumInputed;
            es.NewNumDiscovered += Es_NewNumDiscovered;
            es.NewOperatorDiscovered += Es_NewOperatorDiscovered;
            calc = new Calculator();
            calc.AfterCalculation += Calc_AfterCalculation;
            ed.ExpressionChanged += Ed_ExpressionChanged;
            ed.ResultProvider = () => calc.CurrentNumbers[0];
        }

        private void Ed_ExpressionChanged(object sender, string e)
        {
            this.exprLabel.Text = ed.Current;
        }

        private ExpressionDisplayer ed;
        private void Calc_AfterCalculation(object sender, EventArgs e)
        {
            this.resultLabel.Text = calc.CurrentNumbers[0].ToString();
        }

        private void Es_NewOperatorDiscovered(object sender, string e)
        {
            Operator op = Operator.FromName(e);
            calc.WriteOperator(op);
        }

        private void Es_NewNumDiscovered(object sender, double e)
        {
            calc.WriteNum(e);
        }

        private void Es_NumInputed(object sender, string e)
        {
            this.resultLabel.Text = es.NumString;
        }

        private void ForEachControl(Control parent, Action<Control> action)
        {
            foreach (Control ctr in parent.Controls)
            {
                action(ctr);
                if (ctr.HasChildren)
                {
                    ForEachControl(ctr, action);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string key;
            if (keyMap.ContainsKey(keyData))
            {
                key = keyMap[keyData];
                ForEachControl(this, btn =>
{
    if (key.Equals(btn.Tag) && btn is Button)
    {
        Button bt = btn as Button;
        bt.PerformClick();
        bt.Focus();
    }
}
);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void operation_button_Click(object sender, EventArgs e)
        {
            var bt = sender as Control;
            es.Write(bt.Tag as string);
        }

        private void evalButton_Click(object sender, EventArgs e)
        {
            es.Flush();
            calc.Evaluate();
            ed.Clear();
        }

        private void clearcurrentbutton_Click(object sender, EventArgs e)
        {
            es.ClearCurrent();
            Calc_AfterCalculation(this, e);
        }

        private void clearallbutton_Click(object sender, EventArgs e)
        {
            calc.ClearAll();
            es.ClearCurrent();
            Calc_AfterCalculation(this, e);
            ed.Clear();
        }

        private void backbutton_Click(object sender, EventArgs e)
        {
            es.Back();
        }
    }
}
