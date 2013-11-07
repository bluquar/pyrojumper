using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PyroJumper
{
    class Coin
    {
        private float _x;
        private float _y;
        private float _xV;
        private float _yV;
        private Animation _anim;
        private int _value;
        private int _moveTimer;
        private bool _gravitating;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public Animation Anim
        {
            get { return _anim; }
            set { _anim = value; }
        }
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public float xV
        {
            get { return _xV; }
            set { _xV = value; }
        }
        public float yV
        {
            get { return _yV; }
            set { _yV = value; }
        }
        public int MoveTimer
        {
            get { return _moveTimer; }
            set { _moveTimer = value; }
        }
        public bool Gravitating
        {
            get { return _gravitating; }
            set { _gravitating = value; }
        }

        public static Coin CreateCoin(int x, int y,  int value, float xV, float yV)
        {
            Coin c = new Coin();
            c.X = x;
            c.Y = y;
            c.xV = xV;
            c.yV = yV;
            c.Gravitating = false;
            switch (value)
            {
                case 1:
                    Animation a = Animation.CreateAnimation(x - 10, y - 10, AnimationType.BronzeCoin, 0);
                    c.Anim = a;
                    break;
                default:
                    break;
            }
            c.Value = value;
            if (c.xV != 0 || c.yV != 0)
                c.MoveTimer = 50;
            return c;
        }
    }
}
