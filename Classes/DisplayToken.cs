using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScientificCalculator.OperatorType;
using static ScientificCalculator.PrecessionRule;

namespace ScientificCalculator
{
    internal abstract class DisplayToken
    {
        // Sets boundaries on what kinds of tokens must precede this token
        public virtual PrecessionRule PrecessionRule => None;
        // A token that can be operated on
        public virtual bool IsExpression => false;
        public abstract string DisplayValue { get; }
    }

    // Used to indicate that a token has a left parameter on the end in the display
    internal interface IFunctionToken;
    internal interface IContainerToken
    {
        public DisplayTokenList InnerList { get; }
    }

    internal class DigitDisplayToken : DisplayToken
    {
        public int Value { get; set; }
        public override bool IsExpression => true;
        public override string DisplayValue
        {
            get
            {
                return Value.ToString();
            }
        }

        public DigitDisplayToken(int valueIn)
        {
            Value = valueIn;
        }
    }

    internal class DecimalPointToken : DisplayToken
    {
        public override PrecessionRule PrecessionRule => AfterDigit;
        public override string DisplayValue
        {
            get
            {
                return ".";
            }
        }
    }

    internal class OperatorDisplayToken : DisplayToken
    {
        public override PrecessionRule PrecessionRule => AfterExpression;
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

        public OperatorDisplayToken(OperatorType typeIn)
        {
            Type = typeIn;
        }
    }

    internal class ParenthesisDisplayToken : DisplayToken
    {
        public bool IsRightParenthesis { get; set; }
        // A right parenthesis completes the enclosement of an expression, therefore is treated as an expression
        public override bool IsExpression => IsRightParenthesis;
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

        public ParenthesisDisplayToken(bool isRightParenthesis)
        {
            IsRightParenthesis = isRightParenthesis;
        }
    }

    internal class RootDisplayToken : DisplayToken, IContainerToken, IFunctionToken
    {
        public override bool IsExpression => true;
        public int Root { get; set; }
        public DisplayTokenList InnerList { get; }
        public override string DisplayValue
        {
            get
            {
                return "sqrt(";
            }
        }

        public RootDisplayToken(int root = 2)
        {
            Root = root;
            InnerList = new DisplayTokenList();
        }

        /*public double GiveOutputValue()
        {
            double listOutput = InnerList.GiveOutputValue();
            return Math.Pow(listOutput, 1.0 / Root);
        }*/
    }

    
}
