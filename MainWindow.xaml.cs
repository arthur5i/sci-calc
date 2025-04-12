using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ScientificCalculator.OperatorType;

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
            Calculator.Display.AddToken(token);
        }

        public void OnClickOperator(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            OperatorType type = button.Name switch
            {
                "KeyAdd" => Add,
                "KeySub" => Subtract,
                "KeyMult" => Multiply,
                "KeyDiv" => Divide,
                _ => Error
            };
            DisplayToken token = new OperatorToken(type);
            Calculator.Display.AddToken(token);
        }
    }
}