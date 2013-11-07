using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PyroJumper
{
    class Explosion
    {
        private Point _origin;
        private int _maxRadius;
        private int _currentRadius;
        private int _accelerationPower;
        private int _killingPower;
        private int _radiusGrowth;
        private bool _hasHitPyro;
        private Pellet.ExplosionType _type;

        public Point Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        public int MaxRadius
        {
            get { return _maxRadius; }
            set { _maxRadius = value; }
        }
        public int CurrentRadius
        {
            get { return _currentRadius; }
            set { _currentRadius = value; }
        }
        public int AccelerationPower
        {
            get { return _accelerationPower; }
            set { _accelerationPower = value; }
        }
        public int KillingPower
        {
            get { return _killingPower; }
            set { _killingPower = value; }
        }
        public int RadiusGrowth
        {
            get { return _radiusGrowth; }
            set { _radiusGrowth = value; }
        }
        public Pellet.ExplosionType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public bool HasHitPyro
        {
            get { return _hasHitPyro; }
            set { _hasHitPyro = value; }
        }
        
        
        public static Explosion CreateExplosion(int x, int y, Pellet.ExplosionType type, float radiusbonus, float attackpowerbonus, float accelerationbonus)
        {
            Explosion explosion = new Explosion();
            explosion.Origin = new Point(x, y);
            explosion.CurrentRadius = 0;
            explosion.Type = type;
            switch (type)
            {
                case Pellet.ExplosionType.Normal:
                    explosion.MaxRadius = 200;
                    explosion.KillingPower = 10;
                    explosion.AccelerationPower = 12;
                    explosion.RadiusGrowth = 20;
                    break;
                case Pellet.ExplosionType.Flash:
                    explosion.MaxRadius = 300;
                    explosion.KillingPower = 50;
                    explosion.AccelerationPower = 6;
                    explosion.RadiusGrowth = 50;
                    break;
                case Pellet.ExplosionType.Gravitron:
                    explosion.MaxRadius = 150;
                    explosion.KillingPower = 3;
                    explosion.AccelerationPower = 8;
                    explosion.RadiusGrowth = 15;
                    break;
                case Pellet.ExplosionType.Missile:
                    explosion.MaxRadius = 180;
                    explosion.KillingPower = 40;
                    explosion.AccelerationPower = 8;
                    explosion.RadiusGrowth = 20;
                    break;
                case Pellet.ExplosionType.Boquet:
                    explosion.MaxRadius = 200;
                    explosion.KillingPower = 60;
                    explosion.AccelerationPower = 8;
                    explosion.RadiusGrowth = 25;
                    break;
                case Pellet.ExplosionType.Flower:
                    explosion.MaxRadius = 200;
                    explosion.KillingPower = 60;
                    explosion.AccelerationPower = 5;
                    explosion.RadiusGrowth = 25;
                    break;
                default:
                    break;
            }
            explosion.MaxRadius = (int)(radiusbonus * explosion.MaxRadius);
            explosion.RadiusGrowth = (int)(radiusbonus * explosion.RadiusGrowth);
            explosion.KillingPower = (int)(attackpowerbonus * explosion.KillingPower);
            explosion.AccelerationPower = (int)(accelerationbonus * explosion.AccelerationPower);

            explosion.HasHitPyro = false;
            
            return explosion;
        }
        
    }
}
