using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public delegate double Calculate(IReadOnlyList<double> x);

    public class InfinityList<T> : List<T>, IReadOnlyList<T>
    {
        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                try
                {
                    return base[index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return Count > 0 ? base[Count - 1] : Default;
                }
            }
        }

        public T Default;
    }

    public class Operator
    {
        private static Dictionary<string, Operator> ops;
        static Operator()
        {
            ops = new Dictionary<string, Operator>();
            ops.Add("+", new Operator(nums => nums[1] + nums[0], 2));
            ops.Add("-", new Operator(nums => nums[1] - nums[0], 2));
            ops.Add("*", new Operator(nums => nums[1] * nums[0], 2));
            ops.Add("/", new Operator(nums => nums[1] / nums[0], 2));
            ops.Add("%", new Operator(nums => nums[0] / 100.0 * nums[1], 1));
            ops.Add("mod", new Operator(nums => nums[1] % nums[0], 2));
            ops.Add("sqrt", new Operator(nums => Math.Sqrt(nums[0]), 1));
            ops.Add("negete", new Operator(nums => -nums[0], 1));
            ops.Add("inverse", new Operator(nums => 1 / nums[0], 1));
            ops.Add("square", new Operator(nums => nums[0] * nums[0], 1));
        }

        private Calculate calc;
        private int varibleCount;

        public Calculate Calc
        {
            get
            {
                return calc;
            }
        }

        public double Calculate(Calculator calculator)
        {
            double result = 0;
            if (calc != null)
            {
                result = calc(calculator.CurrentNumbers);
            }
            return result;
        }

        public int VaribleCount
        {
            get
            {
                return varibleCount;
            }
        }

        public Operator(Calculate calc, int count)
        {
            this.calc = calc;
            this.varibleCount = count;
        }

        public static Operator FromName(string name)
        {
            return ops[name];
        }
    }

    public class Calculator
    {
        public event EventHandler DivideByZeroError = (sender, args) => { };
        public event EventHandler MathError = (sender, args) => { };


        bool inputedNum = false;
        public void WriteOperator(Operator op)
        {
            switch (op.VaribleCount)
            {
                case 1:
                    this.ops.Insert(0, op);
                    Evaluate();
                    break;
                case 2:
                    if (inputedNum)
                    {
                        while (ops.Count > 0)
                        {
                            Evaluate();
                        }
                        this.ops.Insert(0, op);
                    }
                    else
                    {
                        if (this.ops.Count > 0)
                        {
                            this.ops[0] = op;
                        }
                        else
                        {
                            this.ops.Add(op);
                        }
                    }
                    break;
            }
            inputedNum = false;
            lastOp = op;
        }

        public void WriteNum(double num)
        {
            if (inputedNum)
            {
                this.nums[0] = num;
            }
            else
            {
                this.nums.Insert(0, num);
                lastNum = num;
                inputedNum = true;
            }
        }

        public void ClearAll()
        {
            this.nums.Clear();
            this.ops.Clear();
            this.lastOp = null;
            this.lastNum = 0;
            inputedNum = false;
        }

        private Operator lastOp = null;
        private double lastNum = 0;

        public void Evaluate()
        {
            if (ops.Count > 0)
            {
                var op = ops[0];
                ops.RemoveAt(0);
                double result = op.Calculate(this);
                for (int i = 0; i < op.VaribleCount && nums.Count > 0; i++)
                {
                    nums.RemoveAt(0);
                }
                nums.Insert(0, result);
            }
            else if (lastOp != null)
            {
                nums.Insert(0, lastNum);
                ops.Insert(0, lastOp);
                Evaluate();
            }
            if (AfterCalculation != null)
            {
                AfterCalculation(this, new EventArgs());
            }
        }

        public event EventHandler AfterCalculation;

        public InfinityList<double> nums = new InfinityList<double>();
        public List<Operator> ops = new List<Operator>();

        public IReadOnlyList<double> CurrentNumbers
        {
            get
            {
                return nums;
            }
        }

        public IReadOnlyList<Operator> CurrentOperators
        {
            get
            {
                return ops;
            }
        }
    }
}
