using ScientificCalculator.Classes;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScientificCalculator.BinaryOperatorType;
using static ScientificCalculator.PrecessionRule;

namespace ScientificCalculator
{
    internal abstract class DisplayToken
    {
        public abstract string DisplayValue { get; }

        public abstract TokenBehaviour Behaviour { get; }

        public abstract void ConversionRoutine(DTConverter c);
    }

    internal readonly struct TokenBehaviour
    {
        // Struct is initialized with object initializer instead of constructor
        public TokenBehaviour() { }

        // Sets boundaries on what kinds of tokens must precede this token
        public PrecessionRule PrecessionRule { get; init; } = None;

        // Does this token represent a numerical value that can be operated on?
        public bool IsExpression { get; init; } = false;
    }

    internal interface IContainerDisplayToken
    {
        public DisplayTokenList InnerList { get; }
    }

    internal class DigitToken : DisplayToken
    {
        public int Value { get; set; }

        public override TokenBehaviour Behaviour { get; } = new()
        {
            IsExpression = true
        };

        public override string DisplayValue
        {
            get
            {
                return Value.ToString();
            }
        }

        public DigitToken(int valueIn)
        {
            Value = valueIn;
        }

        public override void ConversionRoutine(DTConverter c)
        {
            c.NumberBuilder.Append(DisplayValue);
            c.NumberBuilderMode = true;
        }
    }

    internal class DecimalPointToken : DisplayToken
    {
        public override TokenBehaviour Behaviour { get; } = new()
        {
            PrecessionRule = AfterDigit
        };

        public override string DisplayValue
        {
            get
            {
                return ".";
            }
        }

        public override void ConversionRoutine(DTConverter c)
        {
            c.NumberBuilder.Append("0.");
            c.NumberBuilderMode = true;
        }
    }

    internal class OperatorToken : DisplayToken
    {
        public override TokenBehaviour Behaviour { get; } = new()
        {
            PrecessionRule = AfterExpression
        };

        public BinaryOperatorType Type { get; set; }

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

        public OperatorToken(BinaryOperatorType typeIn)
        {
            Type = typeIn;
        }

        public override void ConversionRoutine(DTConverter c)
        {
            TypeModule typeModule = new BinaryOperatorModule(Type);
            LogicNode node = new SingleNode(typeModule);
            c.ListOut.AddNode(node);
        }
    }

    internal class ParenthesisToken : DisplayToken
    {
        public bool IsRightParenthesis { get; set; }

        // A right parenthesis completes the enclosement of an expression, therefore is treated as an expression
        public override TokenBehaviour Behaviour { get; } 

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
            Behaviour = new()
            {
                IsExpression = IsRightParenthesis
            };
            
        }

        public override void ConversionRoutine(DTConverter c)
        {
            if (IsRightParenthesis)
            {
                c.ListOut.OnInnerList = false;
            }
            else
            {
                LogicNode newList = new LogicNodeList();
                c.ListOut.AddNode(newList);
                c.ListOut.OnInnerList = true;
            }
                
        }
    }

    internal class RootToken : DisplayToken, IContainerDisplayToken
    {
        public override TokenBehaviour Behaviour { get; } = new()
        {
            IsExpression = true
        };

        public override string DisplayValue
        {
            get
            {
                return "√";
            }
        }

        public int Root { get; set; }

        public DisplayTokenList InnerList { get; set; }

        public RootToken(int root = 2)
        {
            Root = root;
            InnerList = new DisplayTokenList();
        }

        public override void ConversionRoutine(DTConverter c)
        {

        }
    }
}
