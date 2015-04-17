using System.Collections.Generic;

namespace BitwiseNumbersRoundCountdown.ViewModel
{
    public class Presenter : ObservableObject {
        private static readonly Model.NumbersRound _game = new Model.NumbersRound();

        public Model.NumbersRound Game => _game;
    }
}