using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PyroJumper
{
    public class GameSave
    {
        PlayerPersist _player;

        public PlayerPersist Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public GameSave()
        {

        }
    }

    public class PlayerPersist
    {
        private int[][] _highScores;
        private int[][] _highScoreTimes;
        private bool[] _highScoreSet;
        private int[] _upgrades;
        private int _money;

        public int[][] HighScores
        {
            get { return _highScores; }
            set { _highScores = value; }
        }
        public int[][] HighScoreTimes
        {
            get { return _highScoreTimes; }
            set { _highScoreTimes = value; }
        }
        public bool[] HighScoreSet
        {
            get { return _highScoreSet; }
            set { _highScoreSet = value; }
        }
        public int[] Upgrades
        {
            get { return _upgrades; }
            set { _upgrades = value; }
        }
        public int Money
        {
            get { return _money; }
            set { _money = value; }
        }

        public PlayerPersist(int[][] highScores, int[][] highScoreTimes, bool[] highScoreSet, int[] upgrades, int money)
        {
            _highScores = highScores;
            _highScoreSet = highScoreSet;
            _highScoreTimes = highScoreTimes;
            _upgrades = upgrades;
            _money = money;
        }
        public PlayerPersist()
        {

        }
    }
}
