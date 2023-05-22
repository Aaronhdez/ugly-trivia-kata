using System;

namespace Trivia {
    public class GameRunner {
        private static bool _notAWinner;
        private static Game _instance;

        public static void Main(string[] args) {
            _instance = new Game();
            AddPlayers(_instance);
            PlayRounds();
        }
        
        private static void AddPlayers(Game trivia) {
            trivia.Add("Chet");
            trivia.Add("Pat");
            trivia.Add("Sue");
        }

        private static void PlayRounds() {
            var rand = new Random();
            do {
                _instance.Roll(rand.Next(5) + 1);
                _notAWinner = rand.Next(9) == 7 ? _instance.WrongAnswer() : _instance.WasCorrectlyAnswered();
            } while (_notAWinner);
        }
    }
}