using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BitwiseNumbersRoundCountdown.ViewModel
{
    public class Presenter : ObservableObject {
        private static readonly Model.NumbersRound _game = new Model.NumbersRound();
        private int _playerSolution;
        private string _playerSolutionBinary;

        public Model.NumbersRound Game => _game;
        public int PlayerSolution {
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

        public ICommand SubmitCommand
        {
            get { return new DelegateCommand(Submit); }
        }

        public void Submit() {
            PlayerSolution = Game.CalcuateSolution();
        }
    }
}