using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PyroJumper
{



    class Pellet
    {
        private Bitmap _bmp;
        private float _x;
        private float _y;
        private float _xV;
        private float _yV;
        private float _xDestination;
        private float _yDestination;
        private int _timeLeft;
        private float _speed;

        public enum ExplosionType
        {
            Normal,
            Flash,
            Gravitron,
            Missile,
            Boquet,
            Flower,
        }

        private ExplosionType _explosionType;

        public Bitmap Bmp
        {
            get { return _bmp; }
            set { _bmp = value; }
        }
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
        public float XVelocity
        {
            get { return _xV; }
            set { _xV = value; }
        }
        public float YVelocity
        {
            get { return _yV; }
            set { _yV = value; }
        }
        public float XDestination
        {
            get { return _xDestination; }
            set { _xDestination = value; }
        }
        public float YDestination
        {
            get { return _yDestination; }
            set { _yDestination = value; }
        }
        public int TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; }
        }
        public ExplosionType Type
        {
            get { return _explosionType; }
            set { _explosionType = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public static Pellet CreatePellet(float x, float y, float destinationX, float destinationY, ExplosionType type, float speedbonus)
        {
            Pellet p = new Pellet();
            p.X = x;
            p.Y = y;
            p.XDestination = destinationX;
            p.YDestination = destinationY;
            p.Type = type;

            switch (type)
            {
                case ExplosionType.Normal:
                    p.Speed = 30;
                    break;
                case ExplosionType.Flash:
                    p.Speed = 50;
                    break;
                case ExplosionType.Gravitron:
                    p.Speed = 10;
                    break;
                case ExplosionType.Missile:
                    p.Speed = 30;
                    break;
                case ExplosionType.Boquet:
                    p.Speed = 35;
                    break;
                case ExplosionType.Flower:
                    p.Speed = 45;
                    break;
                default:
                    break;
            }

            p.Speed *= speedbonus;

            float dist = (float)Math.Sqrt(((p.XDestination - p.X) * (p.XDestination - p.X)) + ((p.YDestination - p.Y) * (p.YDestination - p.Y)));
            p.XVelocity = p.Speed * (p.XDestination - p.X) / dist;
            p.YVelocity = p.Speed * (p.YDestination - p.Y) / dist;
            p.TimeLeft = (int)(dist / p.Speed);

            return p;
        }
    }
}
