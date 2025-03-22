using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Ink;
using static ScientficCalculator.OperatorType;

namespace ScientficCalculator
{
    internal abstract class DisplayToken
    {
        public abstract string DisplayValue { get; }
    }

    // Used to indicate that a token has a left parameter on the end in the display
    internal interface IFunctionToken;

    internal class NumeralToken : DisplayToken
    {
        public int Value { get; set; }
        public override string DisplayValue
        {
            get
            {
                return Value.ToString();
            }
        }

        public NumeralToken(int valueIn)
        {
            Value = valueIn;
        }
    }

    internal class DecimalPointToken : DisplayToken
    {
        public override string DisplayValue
        {
            get
            {
                return ".";
            }
        }
    }

    internal class OperatorToken : DisplayToken
    {
        public OperatorType Type { get; set; }
        public override string DisplayValue
        {
            get
            {
                switch (Type)
                {
                    case Add:
                        return "+";
                    case Subtract:
                        return "-";
                    case Multiply:
                        return "×";
                    case Divide:
                        return "÷";
                    default:
                        return "[invalid operator]";
                }
            }
        }

        public OperatorToken(OperatorType typeIn)
        {
            Type = typeIn;
        }
    }

    internal class ParenthesisToken : DisplayToken
    {
        public bool IsRightParenthesis { get; set; }
        public override string DisplayValue
        {
            get
            {
                if (IsRightParenthesis)
                {
                    return ")";
                }
                else
                {
                    return "(";
                }
            }
        }

        public ParenthesisToken(bool isRightParenthesis)
        {
            IsRightParenthesis = isRightParenthesis;
        }
    }

    internal class RootToken : DisplayToken, IFunctionToken
    {
        public int Root { get; set; }
        public DisplayTokenList InputValue { get; }
        public override string DisplayValue
        {
            get
            {
                return "sqrt(";
            }
        }

        public RootToken(int root = 2)
        {
            Root = root;
            InputValue = new DisplayTokenList();
        }

        public double GiveOutputValue()
        {
            double listOutput = InputValue.GiveOutputValue();
            return Math.Pow(listOutput, 1.0 / Root);
        }
    }

    
}
