using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PyroJumper
{
    public enum AnimationType
    {
        Flag,
        NormalExplosion,
        BronzeCoin,
        Bird,
        Bean,
        Flash,
        Gravitron,
        Missile,
        Boquet,
    }
    class Animation
    {
        private int _x;
        private int _y;
        private int _numberOfFrames;
        private int _currentFrame;
        private int _frameWidth;
        private int _timeBetweenFrames;
        private int _timeUntilNextFrame;
        private float _numberOfCycles;
        private Bitmap _bmp;
        private int _timeLeft;
        private bool _reversable;
        private bool _reversing;
        private bool _terminates;

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
        public int NumberOfFrames
        {
            get { return _numberOfFrames; }
            set { _numberOfFrames = value; }
        }
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value; }
        }
        public int FrameWidth
        {
            get { return _frameWidth; }
            set { _frameWidth = value; }
        }
        public int TimeBetweenFrames
        {
            get { return _timeBetweenFrames; }
            set { _timeBetweenFrames = value; }
        }
        public int TimeUntilNextFrame
        {
            get { return _timeUntilNextFrame; }
            set { _timeUntilNextFrame = value; }
        }
        public float NumberOfCycles
        {
            get { return _numberOfCycles; }
            set { _numberOfCycles = value; }
        }
        public Bitmap Bmp
        {
            get { return _bmp; }
            set { _bmp = value; }
        }
        public int TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; }
        }
        public bool Reversable
        {
            get { return _reversable; }
            set { _reversable = value; }
        }
        public bool Reversing
        {
            get { return _reversing; }
            set { _reversing = value; }
        }
        public bool Terminates
        {
            get { return _terminates; }
            set { _terminates = value; }
        }


        public static Animation CreateAnimation(int x, int y, AnimationType type, float cycles)
        {
            Animation a = new Animation();
            a.X = x;
            a.Y = y;
            a.NumberOfCycles = cycles;

            switch (type)
            {
                case AnimationType.NormalExplosion:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 200;
                    a.NumberOfFrames = 5;
                    a.TimeBetweenFrames = 1;
                    a.Reversable = true;
                    a.Terminates = true;
                    break;
                case AnimationType.Flag:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 20;
                    a.NumberOfFrames = 4;
                    a.TimeBetweenFrames = 3;
                    a.Reversable = false;
                    a.Terminates = true;
                    break;
                case AnimationType.BronzeCoin:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 20;
                    a.NumberOfFrames = 14;
                    a.TimeBetweenFrames = 4;
                    a.Reversable = false;
                    a.Terminates = false;
                    break;
                case AnimationType.Bird:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 100;
                    a.NumberOfFrames = 3;
                    a.TimeBetweenFrames = 3;
                    a.Reversable = true;
                    a.Terminates = false;
                    break;
                case AnimationType.Bean:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 123;
                    a.NumberOfFrames = 3;
                    a.TimeBetweenFrames = 3;
                    a.Reversable = true;
                    a.Terminates = false;
                    break;
                case AnimationType.Flash:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 200;
                    a.NumberOfFrames = 4;
                    a.TimeBetweenFrames = 1;
                    a.Reversable = false;
                    a.Terminates = true;
                    break;
                case AnimationType.Gravitron:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 300;
                    a.NumberOfFrames = 4;
                    a.TimeBetweenFrames = 1;
                    a.Reversable = false;
                    a.Terminates = true;
                    break;
                case AnimationType.Missile:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 200;
                    a.NumberOfFrames = 5;
                    a.TimeBetweenFrames = 1;
                    a.Reversable = true;
                    a.Terminates = true;
                    break;
                case AnimationType.Boquet:
                    a.CurrentFrame = 1;
                    a.FrameWidth = 200;
                    a.NumberOfFrames = 5;
                    a.TimeBetweenFrames = 1;
                    a.Reversable = true;
                    a.Terminates = true;
                    break;
                default:
                    break;
            }
            a.TimeLeft = (int)(a.NumberOfCycles * a.NumberOfFrames * a.TimeBetweenFrames);
            if (a.Reversable)
                a.TimeLeft *= 2;
            a.TimeUntilNextFrame = a.TimeBetweenFrames;
            a.Reversing = false;

            return a;
        }

    }
}
