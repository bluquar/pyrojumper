using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PyroJumper
{
    class Damage
    {
        private bool _friendly;
        private bool _forScore;
        private int _x;
        private int _y;
        private int _timer;
        private int _amount;

        public bool Friendly
        {
            get { return _friendly; }
            set { _friendly = value; }
        }
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public int Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public bool ForScore
        {
            get { return _forScore; }
            set { _forScore = value; }
        }

        public static Damage CreateDamage(int x, int y, int amount, bool friendly, bool forScore)
        {
            Damage d = new Damage();
            d.Timer = 20;
            d.X = x;
            if (amount > 9)
                x -= (int)(Math.Log10(amount) * 10);
            d.Y = y;
            d.Friendly = friendly;
            d.ForScore = forScore;
            d.Amount = amount;

            return d;
        }

    }
}
