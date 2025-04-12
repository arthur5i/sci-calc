using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificCalculator
{
    internal enum OperatorType
    {
        Add, Subtract, Multiply, Divide, Error
    }

    internal enum PrecessionRule
    {
        None, AfterExpression, AfterDigit
    }
}
