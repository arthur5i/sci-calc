using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScientificCalculator.BinaryOperatorType;
using static ScientificCalculator.UnaryOperatorType;

namespace ScientificCalculator
{
    internal interface IValueNode
    {
        public double GetResult();
    }

    internal abstract class CalcNode
    {
        public CalcNode? LeftNode { get; set; }
        public CalcNode? RightNode { get; set; }

        public CalcNode() { }

        public CalcNode(CalcNode? leftNode, CalcNode? rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }

    internal class NumberCalcNode : CalcNode, IValueNode
    {
        public double Value { get; set; }

        public NumberCalcNode(double value,
            CalcNode? leftNode = null, CalcNode? rightNode = null): base(leftNode, rightNode)
        {
            Value = value;
        }

        public double GetResult()
        {
            return Value;
        }
    }

    internal class BinaryOperatorCalcNode : CalcNode, IValueNode
    {
        public BinaryOperatorType Type { get; set; }

        public BinaryOperatorCalcNode(BinaryOperatorType type,
            CalcNode? leftNode = null, CalcNode? rightNode = null) : base(leftNode, rightNode)
        {
            Type = type;
        }

        public double GetResult()
        {
            double leftNumber = 0;
            double rightNumber = 0;
            int nullCheck = 0;

            if (LeftNode != null)
            {
                leftNumber = ((IValueNode)LeftNode).GetResult();
                nullCheck += 1;
            }

            if (RightNode != null)
            {
                rightNumber = ((IValueNode)RightNode).GetResult();
                nullCheck += 2;
            }

            switch (nullCheck)
            {
                case 1:
                    return leftNumber;
                case 2:
                    return rightNumber;
                case 3:
                    double resultNumber;

                    switch (Type)
                    {
                        case Add:
                            resultNumber = leftNumber + rightNumber;
                            break;
                        case Subtract:
                            resultNumber = leftNumber - rightNumber;
                            break;
                        case Multiply:
                            resultNumber = leftNumber * rightNumber;
                            break;
                        case Divide:
                            resultNumber = leftNumber / rightNumber;
                            break;
                        case Power:
                            resultNumber = Math.Pow(leftNumber, rightNumber);
                            break;
                        default:
                            throw new Exception();
                    }

                    return resultNumber;

                default:
                    throw new Exception();
            }
        }
    }
}