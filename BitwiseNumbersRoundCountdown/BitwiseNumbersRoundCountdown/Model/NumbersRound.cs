using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Microsoft.CodeAnalysis.Scripting.CSharp;
using BitwiseNumbersRoundCountdown.ViewModel;

namespace BitwiseNumbersRoundCountdown.Model
{
    public class NumbersRound : ObservableObject
    {
        private readonly List<int> _numbersList;
        private readonly List<string> _operatorsList;
        private readonly int _target;
        private string _playerSolutionString;
        private int _playerSolution;
        private string _playerSolutionBinary;
        private Timer _gameTimer;
        private int _timeRemaining;

        public List<int> NumbersList
        {
            get { return _numbersList; }
        }

        public List<string> OperatorsList
        {
            get { return _operatorsList; }
        }

        public int Target
        {
            get { return _target; }
        }

        public string PlayerSolutionString
        {
            get { return _playerSolutionString; }
            set
            { _playerSolutionString = value;
                RaisePropertyChangedEvent("PlayerSolutionString");
            }
        }

        public int PlayerSolution
        {
            get { return _playerSolution; }
            set
            {
                _playerSolution = value;
                PlayerSolutionBinary = Convert.ToString(_playerSolution, 2);
                RaisePropertyChangedEvent("PlayerSolution");
            }
        }

        public string PlayerSolutionBinary
        {
            get { return _playerSolutionBinary; }
            set
            {
                _playerSolutionBinary = value;
                RaisePropertyChangedEvent("PlayerSolutionBinary");
            }
        }

        public Timer GameTimer
        {
            get { return _gameTimer; }
            set { _gameTimer = value; }
        }

        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                _timeRemaining = value;
                RaisePropertyChangedEvent("TimeRemaining");
            }
        }

        public NumbersRound()
        {
            _numbersList = _generateNumbers();
            _operatorsList = new List<string>{ "~", "&", "|", "^", "<<", ">>" };
            _target = _generateTarget();
            PlayerSolutionString = "";
            TimeRemaining = 30;
        }

        private List<int> _generateNumbers()
        {
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

            while (numbers.Count > 0)
            {
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

        public void StartTimer() {
            TimeRemaining = 30;
            GameTimer = new Timer(1000);
            GameTimer.Elapsed += GameTimerElapsed;
            GameTimer.Enabled = true;
        }

        public void GameTimerElapsed(object sender, ElapsedEventArgs e)
        {
            --TimeRemaining;
            if (TimeRemaining == 0)
            {
                GameTimer.Enabled = false;
                CalculateSolution();
            }
        }

        public bool CheckResult()
        {
            return Target == PlayerSolution;
        }

        public void CalculateSolution() {
            PlayerSolution = (int)CSharpScript.Eval(PlayerSolutionString);
        }

    }
}