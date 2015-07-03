using System;
using System.Timers;
using System.Collections.Generic;
using System.Windows.Input;

namespace BitwiseNumbersRoundCountdown.ViewModel
{
    public class Presenter : ObservableObject {
        private static Model.NumbersRound _game = new Model.NumbersRound();

        public Model.NumbersRound Game
        {
            get { return _game; }
            set { _game = value; }
        }

        public ICommand SubmitCommand
        {
            get { return new DelegateCommand(Submit); }
        }

        public void Submit()
        {
            Game.CalculateSolution();
            Game.StopTimer();
        }

        public ICommand StartCommand
        {
            get { return new DelegateCommand(Start); }
        }

        public void Start()
        {
            Game.StartTimer();
        }

    }
}