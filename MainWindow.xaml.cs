using ScientificCalculator.Classes;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ScientificCalculator.BinaryOperatorType;

namespace ScientificCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal CalculatorMain Calculator { get; } = new CalculatorMain();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = Calculator;
            Calculator.InputDisplay.PropertyChanged += OnInputDisplayChange;
        }

        public void OnClickNumeral(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int digit = button.Name switch
            {
                "Key0" => 0,
                "Key1" => 1,
                "Key2" => 2,
                "Key3" => 3,
                "Key4" => 4,
                "Key5" => 5,
                "Key6" => 6,
                "Key7" => 7,
                "Key8" => 8,
                "Key9" => 9,
                _ => 0
            };
            DisplayToken token = new DigitToken(digit);
            Calculator.InputDisplay.AddToken(token);
        }

        public void OnClickOperator(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            BinaryOperatorType type = button.Name switch
            {
                "KeyAdd" => Add,
                "KeySub" => Subtract,
                "KeyMult" => Multiply,
                "KeyDiv" => Divide,
                _ => Error
            };
            DisplayToken token = new OperatorToken(type);
            Calculator.InputDisplay.AddToken(token);
        }

        public void OnClickPoint(object sender, RoutedEventArgs e)
        {
            DisplayToken token = new DecimalPointToken();
            Calculator.InputDisplay.AddToken(token);
        }

        public void OnClickDel(object sender, RoutedEventArgs e)
        {
            Calculator.InputDisplay.RemoveCurrentToken();
        }

        public void OnClickAc(object sender, RoutedEventArgs e)
        {
            Calculator.InputDisplay.ClearTokens();
        }

        public void OnClickEquals(object sender, RoutedEventArgs e)
        {
            Calculator.Calculate();
        }

        public void OnClickRoot(object sender, RoutedEventArgs e)
        {
            DisplayToken token = new RootToken();
            Calculator.InputDisplay.AddToken(token);
        }

        public void OnClickLeft(object sender, RoutedEventArgs e)
        {
            Calculator.InputDisplay.PreviousToken();
        }

        public void OnClickRight(object sender, RoutedEventArgs e)
        {
            Calculator.InputDisplay.NextToken();
        }

        public void OnInputDisplayChange(object sender, PropertyChangedEventArgs e)
        {
            DisplayTokenList displayTokenList = (DisplayTokenList)sender;

            StringBuilder strOut = new StringBuilder();
            int index = 0;

            InputDisplayTextBlock.Inlines.Clear();

            foreach (DisplayToken token in displayTokenList)
            {
                if (token is IContainerDisplayToken)
                {
                    strOut.Append(token.DisplayValue);
                    IContainerDisplayToken container = (IContainerDisplayToken)token;
                    strOut.Append("[");
                    strOut.Append(container.InnerList.ToString());
                    strOut.Append("]");
                }
                else
                {
                    if (displayTokenList.isCurrentPosition(index))
                    {
                        InputDisplayTextBlock.Inlines.Add(strOut.ToString());
                        strOut.Clear();
                        strOut.Append(token.DisplayValue);
                        InputDisplayTextBlock.Inlines.Add(
                            new Run(strOut.ToString())
                            {
                                TextDecorations = TextDecorations.Underline
                            }
                        );
                        strOut.Clear();
                    }
                    else
                    {
                        strOut.Append(token.DisplayValue);
                    }
                }

                index++;
            }

            InputDisplayTextBlock.Inlines.Add(strOut.ToString());
        }
    }
}