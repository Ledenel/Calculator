using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestInitialize()]
        public void init()
        {
            calc = new Calculator();
        }

        private Calculator calc;

        [TestMethod()]
        public void BasicTest()
        {
            calc.WriteNum(3);
            calc.WriteOperator(Operator.FromName("/"));
            calc.WriteNum(2);
            calc.Evaluate();
            Assert.AreEqual(1.5, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void DiscountTest()
        {
            calc.WriteNum(30);
            calc.WriteOperator(Operator.FromName("-"));
            calc.WriteNum(10);
            calc.WriteOperator(Operator.FromName("%"));
            calc.Evaluate();
            Assert.AreEqual(27, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void ContinousEvaluateTest()
        {
            calc.WriteNum(2);
            calc.WriteOperator(Operator.FromName("-"));
            calc.WriteNum(5);
            calc.Evaluate();
            calc.Evaluate();
            calc.Evaluate();
            Assert.AreEqual(2-5-5-5, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void ContinousOperationTest()
        {
            calc.WriteNum(2);
            calc.WriteOperator(Operator.FromName("+"));
            calc.WriteOperator(Operator.FromName("-"));
            calc.WriteOperator(Operator.FromName("*"));
            calc.WriteOperator(Operator.FromName("/"));
            calc.WriteNum(5);
            calc.Evaluate();
            calc.Evaluate();
            Assert.AreEqual(2.0/5/5, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void CalculationInOperationTest()
        {
            calc.WriteNum(4);
            calc.WriteOperator(Operator.FromName("*"));
            calc.WriteNum(3);
            calc.WriteOperator(Operator.FromName("+"));
            Assert.AreEqual(4*3, calc.CurrentNumbers[0], 1e-7);
            calc.Evaluate();
            Assert.AreEqual(12 + 12, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void ContinousNumTest()
        {
            calc.WriteNum(4);
            calc.WriteNum(3);
            calc.WriteOperator(Operator.FromName("*"));
            calc.WriteNum(5);
            calc.Evaluate();
            Assert.AreEqual(3 * 5, calc.CurrentNumbers[0], 1e-7);
            calc.WriteOperator(Operator.FromName("-"));
            calc.Evaluate();
            Assert.AreEqual(0, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void FirstOperatorTest()
        {
            calc.WriteOperator(Operator.FromName("+"));
            calc.Evaluate();
            Assert.AreEqual(0, calc.CurrentNumbers[0], 1e-7);
        }

        [TestMethod]
        public void DivideZeroTest()
        {
            calc.WriteNum(3);
            calc.WriteOperator(Operator.FromName("/"));
            calc.WriteNum(0);
            calc.Evaluate();
        }
    }
}