using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BitwiseNumbersRoundCountdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public NumbersRound Game;

        public MainWindow()
        {
            InitializeComponent();
            Game = new NumbersRound();
            this.DataContext = Game;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e) {
            Game = new NumbersRound();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            if (Game.CheckResult()) Console.WriteLine("You Won");
        }
    }

    public class NumbersRound {
        public List<int> NumbersList { get; }
        public int Target { get; }
        public int PlayerSolution;

        public NumbersRound() {
            NumbersList = _generateNumbers();
            Target = _generateTarget();
        }

        private List<int> _generateNumbers() {
            List<int> numbers = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 6; i++) numbers.Add(random.Next(0, 100));
            return numbers;
        }

        private int _generateTarget()
        {
            List<int> numbers = new List<int>(NumbersList);
            Random random = new Random();
            int target = _selectNumberFromList(numbers);
            int secondTarget = _selectNumberFromList(numbers);

            while (numbers.Count > 0) {
                int randomNum = random.Next(0, 5);
                if (randomNum != 0) secondTarget = _selectNumberFromList(numbers);

                //operator switch
                switch (randomNum)
                {
                    case 0: //NOT
                        target = ~target;
                        break;
                    case 1: //AND
                        target &= secondTarget;
                        break;
                    case 2: //OR
                        target |= secondTarget;
                        break;
                    case 3: //XOR
                        target ^= secondTarget;
                        break;
                    case 4: //LEFTSHIFT
                        target <<= secondTarget;
                        break;
                    case 5: //RIGHTSHIFT
                        target >>= secondTarget;
                        break;
                }
            }

            return target;
        }

        private int _selectNumberFromList(List<int> numbers)
        {
            Random random = new Random();
            int randomNum = random.Next(0, numbers.Count);
            int returnedNum = numbers.Where<int>((n, i) => i == randomNum).First();
            numbers.Remove(returnedNum);
            return returnedNum;
        }

        public bool CheckResult() {
            return Target == PlayerSolution;
        }
    }
}
