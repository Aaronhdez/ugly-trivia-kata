using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia {
    public class Game {
        private readonly List<string> _players = new List<string>();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> _rockQuestions = new LinkedList<string>();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;

        public Game() {
            for (var i = 0; i < 50; i++) {
                _popQuestions.AddLast(CreateQuestion("Pop", i));
                _scienceQuestions.AddLast(CreateQuestion("Science", i));
                _sportsQuestions.AddLast(CreateQuestion("Sports", i));
                _rockQuestions.AddLast(CreateQuestion("Rock", i));
            }
        }

        private string CreateQuestion(string category, int index) {
            return $"{category} Question {index}";
        }

        public void Add(string playerName) {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;
            Console.WriteLine($"{playerName} was added");
            Console.WriteLine($"They are player number {HowManyPlayers()}");
        }

        private int HowManyPlayers() {
            return _players.Count;
        }

        public void Roll(int roll) {
            Console.WriteLine($"{CurrentPlayer()} is the current player");
            Console.WriteLine($"They have rolled a {roll}");
            var place = CurrentPlayerPlace();

            if (PlayerInPenaltyBox()) {
                if (roll % 2 != 0) {
                    _isGettingOutOfPenaltyBox = true;
                    Console.WriteLine($"{CurrentPlayer()} is getting out of the penalty box");
                    RollTheDice(roll, place);
                    AskQuestion(place);
                }
                else {
                    Console.WriteLine($"{CurrentPlayer()} is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else {
                RollTheDice(roll, place);
                AskQuestion(place);
            }
        }

        private static int RollTheDice(int roll, int place) {
            place += roll;
            if (place > 11) place -= 12;
            return place;
        }

        private int CurrentPlayerPlace() {
            return _places[_currentPlayer];
        }

        private bool PlayerInPenaltyBox() {
            return _inPenaltyBox[_currentPlayer];
        }

        private string CurrentPlayer() {
            return _players[_currentPlayer];
        }

        private void AskQuestion(int place) {
            Console.WriteLine($"{CurrentPlayer()}'s new location is {place}");
            Console.WriteLine($"The category is {CurrentCategory()}");
            switch (CurrentCategory()) {
                case "Pop":
                    DisplayQuestion(_popQuestions);
                    break;
                case "Science":
                    DisplayQuestion(_scienceQuestions);
                    break;
                case "Sports":
                    DisplayQuestion(_sportsQuestions);
                    break;
                case "Rock":
                    DisplayQuestion(_rockQuestions);
                    break;
            }
        }

        private void DisplayQuestion(LinkedList<string> questionsList) {
            Console.WriteLine(questionsList.First());
            questionsList.RemoveFirst();
        }

        private string CurrentCategory() {
            var place = _places[_currentPlayer] % 4;
            return place switch {
                0 => "Pop",
                1 => "Science",
                2 => "Sports",
                _ => "Rock"
            };
        }

        public bool WasCorrectlyAnswered() {
            if (!_inPenaltyBox[_currentPlayer]) return PlayerAnswersCorrectly();
            if (_isGettingOutOfPenaltyBox) return PlayerAnswersCorrectly();
            SwitchToNextPLayer();
            return true;
        }

        private bool PlayerAnswersCorrectly() {
            Console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Console.WriteLine($"{CurrentPlayer()} now has {_purses[_currentPlayer]} Gold Coins.");

            var winner = DidPlayerWin();
            SwitchToNextPLayer();
            return winner;
        }
        
        private bool DidPlayerWin() {
            return _purses[_currentPlayer] != 6;
        }

        public bool WrongAnswer() {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine($"{CurrentPlayer()} was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;
            SwitchToNextPLayer();
            return true;
        }

        private void SwitchToNextPLayer() {
            _currentPlayer++;
            if (_currentPlayer == HowManyPlayers()) _currentPlayer = 0;
        }
    }
}