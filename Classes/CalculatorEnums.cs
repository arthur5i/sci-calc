using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificCalculator
{
    internal enum BinaryOperatorType
    {
        Add, Subtract, Multiply, Divide, Power, Error
    }

    internal enum UnaryOperatorType
    {
        Factorial, Sin, Cos, Tan, Error
    }

    internal enum PrecessionRule
    {
        None, AfterExpression, AfterDigit
    }
}
