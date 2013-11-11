using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

//using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace PyroJumper
{
    public enum MonsterType
    {
        Bird,
        Bean,
    }

    class Monster
    {
        private float _x;
        private float _y;
        private int _xMoving;
        private int _yMoving;
        private bool _facingLeft;
        private int _spawnX;
        private int _spawnY;
        private bool _spawned;
        private int _spawnTrigger;
        private bool _chasing;
        private int _health;
        private int _maxHealth;
        private Animation _anim;
        private MonsterType _type;
        private float _speed;
        private bool _animFacingLeft;
        private int _attack;
        private int _dyingCounter;
        private Bitmap _dyingBmp;
        private int _attackCooldown;
        private int _attackWarmup;
        private bool _attackWarmingUp;
        private int _attackWarmupInitial;
        private int _attackCooldownInitial;
        private float _projectileSpeed;
        private int _projectilePower;
        private int _scoreBonus;
        private float _aimX;
        private float _aimY;

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
        public int XMoving
        {
            get { return _xMoving; }
            set { _xMoving = value; }
        }
        public int YMoving
        {
            get { return _yMoving; }
            set { _yMoving = value; }
        }
        public bool FacingLeft
        {
            get { return _facingLeft; }
            set { _facingLeft = value; }
        }
        public int SpawnX
        {
            get { return _spawnX; }
            set { _spawnX = value; }
        }
        public int SpawnY
        {
            get { return _spawnY; }
            set { _spawnY = value; }
        }
        public bool Spawned
        {
            get { return _spawned; }
            set { _spawned = value; }
        }
        public int SpawnTrigger
        {
            get { return _spawnTrigger; }
            set { _spawnTrigger = value; }
        }
        public bool Chasing
        {
            get { return _chasing; }
            set { _chasing = value; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }
        public Animation Anim
        {
            get { return _anim; }
            set { _anim = value; }
        }
        public MonsterType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public bool AnimFacingLeft
        {
            get { return _animFacingLeft; }
            set { _animFacingLeft = value; }
        }
        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }
        public int DyingCounter
        {
            get { return _dyingCounter; }
            set { _dyingCounter = value; }
        }
        public Bitmap DyingBmp
        {
            get { return _dyingBmp; }
            set { _dyingBmp = value; }
        }
        public int AttackCooldown
        {
            get { return _attackCooldown; }
            set { _attackCooldown = value; }
        }
        public int AttackWarmup
        {
            get { return _attackWarmup; }
            set { _attackWarmup = value; }
        }
        public bool AttackWarmingUp
        {
            get { return _attackWarmingUp; }
            set { _attackWarmingUp = value; }
        }
        public int AttackWarmupInitial
        {
            get { return _attackWarmupInitial; }
            set { _attackWarmupInitial = value; }
        }
        public int AttackCooldownInitial
        {
            get { return _attackCooldownInitial; }
            set { _attackCooldownInitial = value; }
        }
        public float ProjectileSpeed
        {
            get { return _projectileSpeed; }
            set { _projectileSpeed = value; }
        }
        public int ProjectilePower
        {
            get { return _projectilePower; }
            set { _projectilePower = value; }
        }
        public int ScoreBonus
        {
            get { return _scoreBonus; }
            set { _scoreBonus = value; }
        }
        public float AimX
        {
            get { return _aimX; }
            set { _aimX = value; }
        }
        public float AimY
        {
            get { return _aimY; }
            set { _aimY = value; }
        }

        public static Monster CreateMonster(int spawnX, int spawnY, MonsterType type, int spawnTriggerY)
        {
            Monster m = new Monster();

            m.Type = type;

            m.Spawned = false;
            m.SpawnTrigger = spawnTriggerY;
            m.SpawnX = spawnX;
            m.SpawnY = spawnY;
            m.Chasing = false;
            m.FacingLeft = true;
            m.AnimFacingLeft = true;

            switch (type)
            {
                case MonsterType.Bird:
                    m.MaxHealth = 100;
                    m.Speed = 2;
                    m.Attack = 10;
                    m.AttackWarmupInitial = 15;
                    m.AttackCooldownInitial = 100;
                    m.ProjectileSpeed = 10;
                    m.ProjectilePower = 10;
                    m.ScoreBonus = 15;
                    break;
                case MonsterType.Bean:
                    m.MaxHealth = 300;
                    m.Speed = 3;
                    m.Attack = 5;
                    m.AttackWarmupInitial = 30;
                    m.AttackCooldownInitial = 50;
                    m.ProjectileSpeed = 12;
                    m.ProjectilePower = 30;
                    m.ScoreBonus = 40;
                    break;
                default:
                    break;
            }

            m.Health = m.MaxHealth;
            m.XMoving = 0;
            m.YMoving = 0;
            m.DyingCounter = 15;

            m.AttackWarmingUp = false;
            m.AttackWarmup = 0;
            m.AttackCooldown = 0;

            return m;
        }

        public static Monster SpawnMonster(Monster monster)
        {
            Monster m = monster;
            switch (m.Type)
            {
                case MonsterType.Bird:
                    m.Anim = Animation.CreateAnimation(m.SpawnX, m.SpawnY, AnimationType.Bird, 1);
                    break;
                case MonsterType.Bean:
                    m.Anim = Animation.CreateAnimation(m.SpawnX, m.SpawnY, AnimationType.Bean, 1);
                    break;
                default:
                    break;
            }

            m.Spawned = true;
            m.X = m.SpawnX;
            m.Y = m.SpawnY;
            m.XMoving = 0;
            m.YMoving = 0;

            return m;
        }
    }
}
