using MyDartsManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyDartsManager.DataStructure;

namespace MyDartsManager.process
{
    public class GameProcess
    {
        //database context to save and load data.
        private DartsDbContext db = new DartsDbContext();

        //Simulated match.
        private Match _match;

        //Used to keep track of players in the game and scores they need to score to finnish the game.
        public ObservableDictionary<Player, int> PlayerScores { get; private set; }

        //Used to keep track of the current player.
        public int PlayerIterator { get; private set; }
        //Used to keep track of the current round number.
        public int RoundIterator { get; private set; }

        //Used to save throws in single round.
        private int _throwIterator;
        public ThrowCombination Throw1 { get; private set; }
        public ThrowCombination Throw2 { get; private set; }
        public ThrowCombination Throw3 { get; private set; }

        //Temporary data storage, before player finishes the game or the game has ended.
        private List<Round> _rounds;

        private int _playerCount;
        private bool _doubleOut;



        public event EventHandler<string> PlayerChanged;
        public event EventHandler<Tuple<int, string>> ThrowChanged;
        public event EventHandler GameEnded;



        public GameProcess(List<Player> players, int scoreGoal, bool doubleOut)
        {
            PlayerScores = new ObservableDictionary<Player, int>();
            foreach (Player p in players)
            {
                PlayerScores.Add(p, scoreGoal);
            }

            _playerCount = players.Count;

            _match = new Match(scoreGoal, doubleOut);
            _doubleOut = doubleOut;

            db.Matches.Load();
            db.Matches.Add(_match);
            db.SaveChanges();


            _rounds = new List<Round>();

            PlayerIterator = 0;
            RoundIterator = 0;
            _throwIterator = 0;

            db.ThrowCombinations.Load();
            setThrowsToInvalid();
        }

        public void TriggerEvents()
        {
            PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator].Name);
            allThrowsChanged();
        }

        private void setThrowsToInvalid()
        {
            Throw1 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
            Throw2 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
            Throw3 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();

            allThrowsChanged();
        }

        private void allThrowsChanged()
        {
            ThrowChanged?.Invoke(this, new Tuple<int, string>(1, Throw1.ToString()));
            ThrowChanged?.Invoke(this, new Tuple<int, string>(2, Throw2.ToString()));
            ThrowChanged?.Invoke(this, new Tuple<int, string>(3, Throw3.ToString()));
        }


        private void nextPlayer()
        {
            if (PlayerIterator == PlayerScores.Count - 1)
            {
                PlayerIterator = 0;
                PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator].Name);
                RoundIterator++;
                return;
            }
            PlayerIterator++;
            PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator].Name);
        }


        private void saveRound()
        {
            Round round = new Round(RoundIterator, _match, db.Players.Find(PlayerScores.Keys.ToList()[PlayerIterator].PlayerId), Throw1, Throw2, Throw3);
            _rounds.Add(round);
        }




        public bool DartThrow(int value, int multiplier)
        {
            //new player throw
            if (_throwIterator > 2)
            {
                _throwIterator = 0;
                saveRound();
                nextPlayer();
                setThrowsToInvalid();
                return DartThrow(value, multiplier);
            }

            //player reaches 0 according to rules
            if (_doubleOut && multiplier == 2 || !_doubleOut)
            {

                if (PlayerScores.Values.ToList()[PlayerIterator] - value * multiplier == 0)
                {
                    setThrow(value, multiplier);
                    saveRound();
                    playerFinished();
                    return true;
                }
            }

            //player overthwors this round
            if (PlayerScores.Values.ToList()[PlayerIterator] - value * multiplier < 0 || 
                _doubleOut && PlayerScores.Values.ToList()[PlayerIterator] - value * multiplier <= 1)
            {
                playerOverthrow();
                setThrowsToInvalid();
                _throwIterator = 0;
                saveRound();
                nextPlayer();
                return false;
            }

            setThrow(value, multiplier);
            return false;
        }


        private void matchFinished()
        {
            GameEnded?.Invoke(this, null);
        }

        private void playerFinished()
        {
            List<Round> playerRounds = _rounds.Where(r => r.Player.PlayerId == PlayerScores.Keys.ToList()[PlayerIterator].PlayerId).ToList();

            MatchStatistic matchStat = new MatchStatistic();
            matchStat.PlayerID = PlayerScores.Keys.ToList()[PlayerIterator].PlayerId;
            matchStat.MatchID = _match.MatchId;
            matchStat.AverageScorePerRound = playerRounds.Average(r => r.RoundValue());
            matchStat.TotalScore = playerRounds.Sum(r => r.RoundValue());
            matchStat.RoundsPlayed = playerRounds.Count;
            matchStat.HighestScore = playerRounds.OrderBy(r => r.RoundValue()).First().RoundValue();
            matchStat.Placement = _playerCount - PlayerScores.Count + 1;
            db.MatchStatistics.Add(matchStat);

            db.Rounds.AddRange(_rounds.Where(p => p.Player == PlayerScores.Keys.ToList()[PlayerIterator]).ToList());
            db.SaveChanges();

            if (PlayerScores.Count == 1)
            {
                matchFinished();
                return;
            }

            if (PlayerScores.Count - 1 > PlayerIterator)
            {
                PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator + 1].Name);

                PlayerScores.Remove(PlayerScores.Keys.ToList()[PlayerIterator]);
                _throwIterator = 0;
                setThrowsToInvalid();
            }
            else
            {
                PlayerScores.Remove(PlayerScores.Keys.ToList()[PlayerIterator]);
                _throwIterator = 0;

                PlayerIterator = 0;
                PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator].Name);
                setThrowsToInvalid();
            }



        }

        private void playerOverthrow()
        {
            if (Throw1.Value != -1)
            {
                Player player = PlayerScores.Keys.ToList()[PlayerIterator];
                PlayerScores[player] = PlayerScores[player] + Throw1.Value * Throw1.Multiplier;
            }
            if (Throw2.Value != -1)
            {
                Player player = PlayerScores.Keys.ToList()[PlayerIterator];
                PlayerScores[player] = PlayerScores[player] + Throw2.Value * Throw2.Multiplier;
            }
            if (Throw3.Value != -1)
            {
                Player player = PlayerScores.Keys.ToList()[PlayerIterator];
                PlayerScores[player] = PlayerScores[player] + Throw3.Value * Throw3.Multiplier;
            }
        }

        private void setThrow(int value, int multiplier)
        {
            Player player = PlayerScores.Keys.ToList()[PlayerIterator];
            PlayerScores[player] = PlayerScores[player] - value * multiplier;


            switch (_throwIterator)
            {
                case 0:

                    Throw1 = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == multiplier).ToList().First();
                    ThrowChanged?.Invoke(this, new Tuple<int, string>(1, Throw1.ToString()));
                    _throwIterator++;
                    break;
                case 1:
                    Throw2 = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == multiplier).ToList().First();
                    ThrowChanged?.Invoke(this, new Tuple<int, string>(2, Throw2.ToString()));
                    _throwIterator++;
                    break;
                case 2:
                    Throw3 = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == multiplier).ToList().First();
                    ThrowChanged?.Invoke(this, new Tuple<int, string>(3, Throw3.ToString()));
                    _throwIterator++;
                    break;
            }

        }

        public void CancelGame()
        {
            db.SaveChanges();
            db.Matches.Remove(_match);
            db.SaveChanges();
        }

        public void RevertThrow()
        {

            if (_rounds.Count == 0 && Throw1.Value == -1)
            {
                return;
            }

            switch (this._throwIterator)
            {
                case 0:
                    Throw1 = _rounds.Last().Throw1;
                    Throw2 = _rounds.Last().Throw2;
                    PlayerScores[PlayerScores.Keys.Select(p => p).Where(p => p.PlayerId == _rounds.Last().Player.PlayerId).FirstOrDefault()] += _rounds.Last().Throw3.TrueValue();
                    Throw3 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
                    _rounds.Remove(_rounds.Last());
                    _throwIterator = 2;
                    if (PlayerIterator == 0)
                    {
                        PlayerIterator = PlayerScores.Count - 1;
                    }
                    else
                    {
                        PlayerIterator--;
                    }
                    PlayerChanged?.Invoke(this, PlayerScores.Keys.ToList()[PlayerIterator].Name);
                    allThrowsChanged();
                    break;
                case 1:
                    PlayerScores[PlayerScores.ElementAt(PlayerIterator).Key] += Throw1.TrueValue();
                    Throw1 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
                    _throwIterator = 0;
                    allThrowsChanged();
                    break;
                case 2:
                    PlayerScores[PlayerScores.ElementAt(PlayerIterator).Key] += Throw2.TrueValue();

                    Throw2 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
                    _throwIterator = 1;
                    allThrowsChanged();


                    break;
                case 3:
                    PlayerScores[PlayerScores.ElementAt(PlayerIterator).Key] += Throw3.TrueValue();

                    Throw3 = db.ThrowCombinations.Where(x => x.Value == -1 && x.Multiplier == 1).ToList().First();
                    _throwIterator = 2;
                    allThrowsChanged();

                    break;
            }
        }



    }
}
