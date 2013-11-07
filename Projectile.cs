using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PyroJumper
{
    class Projectile
    {
        private float _x;
        private float _y;
        private float _xVelocity;
        private float _yVelocity;
        private int _attackPower;
        private Bitmap _bmp;

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
            get { return _xVelocity; }
            set { _xVelocity = value; }
        }
        public float YVelocity
        {
            get { return _yVelocity; }
            set { _yVelocity = value; }
        }
        public int AttackPower
        {
            get { return _attackPower; }
            set { _attackPower = value; }
        }
        public Bitmap Bmp
        {
            get { return _bmp; }
            set { _bmp = value; }
        }

        public static Projectile CreateProjectile(float x, float y, float xDestination, float yDestination, int attackPower, float speed)
        {
            Projectile p = new Projectile();
            p.X = x;
            p.Y = y;
            float dist = (float)Math.Sqrt(((xDestination - x) * (xDestination - x)) + ((yDestination - y) * (yDestination - y)));
            p.XVelocity = ((xDestination - x) * speed) / dist;
            p.YVelocity = ((yDestination - y) * speed) / dist;
            p.AttackPower = attackPower;
            return p;
        }
    }
}
