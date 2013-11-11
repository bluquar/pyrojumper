using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using System.Xml.Serialization;

namespace PyroJumper
{
    public partial class PyroJumper : Form
    {
        #region MemberData

        private bool _paused = true; //bool to determine whether we are in the menu or in the gameplay

        #region clicking stuff
        //information about where the mouse is and where clicks are, and whether the mouse is being held down
        private Point _canvasClickDown; //putting the click button down
        private Point _canvasClickUp; //releasing the click button
        private Point _canvasMove; //whenever the mouse moves...basically where the cursor is
        private bool _clicking = false; //is the click button currently down?
        private bool _clicked = false; //was the click button down last time?
        private int _hoveringUpgrade = 0;
        #endregion clicking stuff
        #region Canvas
        private Bitmap _canvasBmp; //_canvasBmp is where we will draw everything. 
        //midx and midy are to reduce "Canvas.Width / 2" calculations because those can get excessive. they are assigned in the initialization
        private float _canvasMidX;
        private float _canvasMidY;

        private Bitmap[] _flashBmps;
        private Bitmap _flashBmp;
        #endregion
        #region Menu
        private bool _StartButtonHovering = false; //bool to tell us whether the mouse is hovering over the start button
        private bool _startingNewGame = false; //a flag to start a new game
        //info about which level the user can and wants to start on
        private int _levelSelected = 1;
        private int _levelsUnlocked = 2;
        private int _levelSelectButtonHovering = 0;
        #endregion
        #region GameOVER
        private bool _ending;
        private int _endCounter;
        private Bitmap _drowning;
        private Bitmap _drowning2;
        private Bitmap _drowning3;
        private Bitmap _drowningArm;
        private Bitmap _bubble;
        private Bitmap _fastForwardBmp;
        private Bitmap _fastForwardingBmp;
        #endregion //game over
        #region GameCOMPLETE
        private bool _completing;
        private int _completeCounter;
        private TimeSpan _initialTime;
        private TimeSpan _completionTime;
        private TimeSpan _totalTime;
        private TimeSpan _currentTime;
        private TimeSpan _previousTime;
        private int _initialMoney;
        private int _moneyGained;
        private int _initialKills;
        private int _totalKills;
        private int _killGained;
        private int _healthLost;
        private int _score;
        private int _timeBonus;
        private int _healthBonus;
        private int _coinBonus;
        private int _killBonus;
        private bool _newHighScore;
        private Bitmap _scoreBoardBmp;
        private Bitmap _levelCompleteBmp;
        #endregion //Game Complete
        #region High Scores
        private int[][] _highScoreTimes;
        private int[][] _highScores;
        private bool[] _highScoreSet;
        private int[] _upgrades;
        #endregion //High scores
        #region Saving
        private GameSave _gameSave;
        private PlayerPersist _currentPlayer = null;
        #endregion //saving
        #region Button bitmaps
        //bitmaps for all the images on the menu. they are assigned in initialization
        private Bitmap _startButtonBmp;
        private Bitmap _startButtonHoveringBmp;
        private Bitmap _levelSelectButtonBmp;
        private Bitmap _levelSelectButtonHoveringBmp;
        private Bitmap _levelSelectedButtonBmp;
        private Bitmap _levelSelectedButtonHoveringBmp;
        private Bitmap _lockBmp;
        private Bitmap _titleBmp;
        private Bitmap _upgradeButtonGreenBmp;
        private Bitmap _upgradeButtonRedBmp;
        private Bitmap _upgradeButtonGreenHoveringBmp;
        private Bitmap _coinIcon;
        #endregion Buttons
        #region Brushes
        //brushes
        private Brush _blackBrush = new SolidBrush(Color.Black);
        private Brush _magentaBrush = new SolidBrush(Color.Magenta);
        private Brush _greenBrush = new SolidBrush(Color.Green);
        private Brush _orangeBrush = new SolidBrush(Color.Orange);
        private Brush _transparentBrush = new SolidBrush(Color.Transparent);
        private Brush _expBrush = new SolidBrush(Color.Orange);
        private Brush _waterBrush = new SolidBrush(Color.Navy);
        private Brush _whiteBrush = new SolidBrush(Color.White);
        private Brush _damageBrush = new SolidBrush(Color.DarkBlue);
        private Brush _redBrush = new SolidBrush(Color.Red);
        private Brush _purpleBrush = new SolidBrush(Color.Purple);
        private Brush _grayBrush = new SolidBrush(Color.LightGray);
        private Brush _arsenalBrush = new SolidBrush(Color.White);
        private Brush _forestBrush = new SolidBrush(Color.ForestGreen);
        private Brush _missileBrush = new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.PaleVioletRed, Color.LightSteelBlue);
        private Brush _skullBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.Transparent);
        #endregion //Brushes
        #region Pens
        //pens
        private Pen _greenPen = new Pen(Color.Green);
        private Pen _redPen = new Pen(Color.Red);
        private Pen _whitePen = new Pen(Color.White);
        private Pen _blackPen = new Pen(Color.Black);
        #endregion
        #region Fonts
        //fonts
        private Font _levelFont = new Font("Bradley Hand ITC", 27);
        private Font _levelNumberFont = new Font("Courier New", 15);
        private Font _gameOverFont = new Font("Arial", 50);
        private Font _damageFont = new Font("Arial", 12);
        private Font _statsFont = new Font("Arial", 20);
        private Font _unitsFont = new Font("Courier New", 8);
        private Font _exponentFont = new Font("Courier New", 5);
        #endregion //Fonts
        #region Pyro
        //data for pyro: since there is only ever one, we don't need a Pyro class
        private Bitmap _pyroBmp;
        private Bitmap _pyroArmBmp;
        private Bitmap _pyroArmBasicBmp;
        private double _pyroXs;
        private double _pyroYs;
        private double _pyroXv;
        private double _pyroYv;
        private double _pyroXa;
        private double _pyroYa;
        private double _pyroXd;
        private double _pyroYd;
        private int _pyroArmAnchorX;
        private int _pyroArmAnchorY;
        private enum PyroState
        {
            rising,
            drifting,
            falling,
            rolling
        }
        private PyroState _pyroState;
        private int _pyroRollCount;

        private int _pyroHealthMax = 100;
        private int _pyroHealth;
        private Bitmap _healthBarEmpty;
        private Bitmap _healthBar;
        private int _invincibilityTimer;

        private int _money;
        #endregion //pyro
        #region goals
        private int _heightGoal;
        #endregion //goals
        #region Charge
        // Charge info
        private int _charge; //charge out of 100
        private int _maxCharge = 100;
        private int _chargeRate = 5; //how fast do we recharge?
        private Bitmap _chargeBarEmpty;
        private Bitmap _chargeBarFull;
        private Bitmap _chargeBarFilling;
        #endregion Charge
        #region Selection Wheel

        private Bitmap _selectionWheelBmp;
        private Bitmap _selectionWheelOutlineBmp;
        private Bitmap _selectionWheelHoverBmp;
        private float _selectionWheelAngleCurrent;
        private float _selectionWheelAngleDestination;
        private int _numberOfUnlockedExplosionTypes;

        #endregion //Selection Wheel
        #region Water
        //water rising
        private int _waterLevel = 1000;
        private int _waterRisingRate = 2;
        private int _waterAccelerationCountdown = 1000;
        private int _waterAccelerationCountdownInit = 1000;
        private Bitmap _waves;
        private int _waveNumber = 1;
        private int _waveSwitchTimer = 10;
        private int _waveSwitchTimerInitial = 10;
        private Bitmap _downArrowBmp;
        #endregion //water
        #region Wall
        private int _wallAnchor = 0;
        private Bitmap _wallBmp;
        private Bitmap _wallRightBmp;
        #endregion //Wall
        #region Explosions
        private Pellet.ExplosionType _activeExplosionType;

        private int _flashTimer;
        #endregion //Explosions
        #region screen anchor
        //Screen Anchor
        private int _screenAnchorY = 0;
        #endregion //screen anchor
        #region object lists
        //Lists
        // Pellets!
        private List<Pellet> _activePellets = new List<Pellet>();

        //Explosions =O
        private List<Explosion> _activeExplosions = new List<Explosion>();

        //Animations
        private List<Animation> _activeAnimations = new List<Animation>();

        //Bubbles
        private List<Bubble> _activeBubbles = new List<Bubble>();

        //Coins
        private List<Coin> _activeCoins = new List<Coin>();
        private Point[] _coinLocations;
        private int[] _coinValues;

        //Monsters
        private List<Monster> _inactiveMonsters = new List<Monster>();
        private List<Monster> _activeMonsters = new List<Monster>();
        private List<Monster> _dyingMonsters = new List<Monster>();
        private Point[] _monsterSpawnPoints;
        private int[] _monsterSpawnTriggers;
        private int[] _monsterSpawnIDs;      

        //Damage
        private List<Damage> _activeDamages = new List<Damage>();

        //Projectiles
        private List<Projectile> _activeProjectiles = new List<Projectile>();

        #endregion //object lists
        #region Monsters
        private Bitmap _birdLeft;
        private Bitmap _birdRight;
        private Bitmap _beanLeft;
        private Bitmap _beanRight;

        #endregion //Monsters
        #region Pellets
        private Bitmap _pelletNormalBmp;
        private Bitmap _pelletFlashBmp;
        private Bitmap _pelletGravitronBmp;
        private Bitmap _pelletMissileBmp;
        private Bitmap _pelletSkullBmp;
        #endregion //Pellets
        #region Magnet
        private float _magnetRange;
        private float _magnetStrength;
        #endregion //Magnet
        #region upgrades
        private float _bulletSpeedBonus;
        private float _explosionRadiusBonus;
        private float _attackPowerBonus;
        private float _accelerationPowerBonus;
        private float _recoilBonus;
        #endregion //upgrades

        Random _rand = new Random();
        #endregion //member data
        #region Initialization

        public PyroJumper()
        {
            //initializers
            InitializeComponent();
            InitializeCanvas();
            InitializeButtons();
            LoadGame();
            //InitializeHighScores();

            //adds key press events and a timer
            this.KeyDown += new KeyEventHandler(GotKeyDown);
            this.KeyUp += new KeyEventHandler(GotKeyUp);
            GameTick.Enabled = true;
        }
        private void InitializeCanvas()
        {
            //makes the canvas bitmap and assigns midx and midy
            _canvasBmp = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = _canvasBmp;
            _canvasMidX = Canvas.Width / 2;
            _canvasMidY = Canvas.Height / 2;
        }
        private void InitializeButtons()
        {
            //sets each button bmp to its respective bmp file
            _startButtonBmp = new Bitmap(GetType(), "StartButton.bmp");
            _startButtonHoveringBmp = new Bitmap(GetType(), "StartButtonHovering.bmp");
            _levelSelectButtonBmp = new Bitmap(GetType(), "LevelSelectButton.bmp");
            _levelSelectButtonHoveringBmp = new Bitmap(GetType(), "LevelSelectButtonHovering.bmp");
            _lockBmp = new Bitmap(GetType(), "Lock.bmp");
            _titleBmp = new Bitmap(GetType(), "Title.bmp");
            _levelSelectedButtonBmp = new Bitmap(GetType(), "LevelSelectedButton.bmp");
            _levelSelectedButtonHoveringBmp = new Bitmap(GetType(), "LevelSelectedButtonHovering.bmp");
            _upgradeButtonGreenBmp = new Bitmap(GetType(), "UpgradeButtonGreen.bmp");
            _upgradeButtonRedBmp = new Bitmap(GetType(), "UpgradeButtonRed.bmp");
            _upgradeButtonGreenHoveringBmp = new Bitmap(GetType(), "UpgradeButtonGreenHovering.bmp");
            _coinIcon = new Bitmap(GetType(), "CoinIcon.bmp");

            //makes the color magenta transparent for each bitmap
            _startButtonBmp.MakeTransparent(Color.Magenta);
            _startButtonHoveringBmp.MakeTransparent(Color.Magenta);
            _levelSelectButtonBmp.MakeTransparent(Color.Magenta);
            _levelSelectButtonHoveringBmp.MakeTransparent(Color.Magenta);
            _lockBmp.MakeTransparent(Color.Magenta);
            _titleBmp.MakeTransparent(Color.Magenta);
            _levelSelectedButtonBmp.MakeTransparent(Color.Magenta);
            _levelSelectedButtonHoveringBmp.MakeTransparent(Color.Magenta);
            _upgradeButtonGreenBmp.MakeTransparent(Color.Magenta);
            _upgradeButtonRedBmp.MakeTransparent(Color.Magenta);
            _upgradeButtonGreenHoveringBmp.MakeTransparent(Color.Magenta);
            _coinIcon.MakeTransparent(Color.Magenta);

            _pelletNormalBmp = new Bitmap(GetType(), "PelletNormal.bmp");
            _pelletFlashBmp = new Bitmap(GetType(), "FlashPellet.bmp");
            _pelletGravitronBmp = new Bitmap(GetType(), "PelletGravitron.bmp");
            _pelletMissileBmp = new Bitmap(GetType(), "PelletMissile.bmp");
            _pelletSkullBmp = new Bitmap(GetType(), "Skull.bmp");
            _pelletNormalBmp.MakeTransparent(Color.Magenta);
            _pelletFlashBmp.MakeTransparent(Color.Magenta);
            _pelletGravitronBmp.MakeTransparent(Color.Magenta);
            _pelletMissileBmp.MakeTransparent(Color.Magenta);
            _pelletSkullBmp.MakeTransparent(Color.Magenta);
        }
        private void InitializeHighScores()
        {
            _highScores = new int[][] { new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 },
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, 
                                        new int[] { 0, 0, 0, 0 }, };
            _highScoreTimes = new int[][] { new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},};
                                            
            _highScoreSet = new bool[] { false, false, false, false, false, false, false, false, false, false };

            _upgrades = new int[] { 0, 0, 0, 0, 0 };

            _gameSave = new GameSave();
            PlayerPersist player = new PlayerPersist(_highScores, _highScoreTimes, _highScoreSet, _upgrades, _money);
            _gameSave.Player = player;
            _currentPlayer = player;
            //SaveGame();
        }

        #endregion //Init
        private void GameTimer_Tick(object sender, EventArgs e) //every time the timer ticks this function is called
        {

            if (_startingNewGame)
                InitializeNewGame(); //sets everything up based on what level is selected.
            if (!_paused)
            {
                DoAI(); //this is the gameplay AI for when we aren't in the menu
            }
            else if (_paused)
            {
                DoMenuAI(); //this is the AI for when we are in the menu... obviously
            }
            DoRender(); //draws everything to Canvas
        }

        private void DoAI()
        {
            ManageCharge();
            ManageAnimations();
            ManageFlash();
            ManageSelectionWheel();
            ManageDamage();
            ManageCoins();

            SpawnMonsters();
            ManageProjectiles();
            ManageMonsters();
            ManagePellets();
            MoveGamePieces();
            ManageExplosions();
            ManageWater();
            ManageDeath();
            ManageCompletion();
        }

        private void InitializeNewGame()
        {
            _paused = false; //puts us in gameplay mode

            #region Pyro Rendering
            _pyroBmp = new Bitmap(GetType(), "PyroRisingRightAR.bmp"); //sets pyro's bmp
            _pyroArmBmp = new Bitmap(GetType(), "Arm.bmp");
            _pyroArmBasicBmp = new Bitmap(GetType(), "Arm.bmp");
            _pyroArmBasicBmp.MakeTransparent(Color.Magenta);
            _pyroBmp.MakeTransparent(Color.Magenta);
            _pyroArmAnchorX = 4;
            _pyroArmAnchorY = -3;
            #endregion //Pyro Rendering
            #region Pyro
            //set all of pyro's position vars
            _pyroXs = _canvasMidX; //x spatium
            _pyroXv = 10; //x velocity
            _pyroXa = 0; //x acceleration
            _pyroYs = _canvasMidY; //y spatium
            _pyroYv = -10; //y velocity
            _pyroYa = 0.2; //y acceleration
            _pyroXd = 0.995; //x drag
            _pyroYd = 0.995; //y drag
            #endregion Pyro
            #region finishing and misc
            _invincibilityTimer = 0;
            _startingNewGame = false; //takes the starting flag down
            _canvasClickUp.X = 0; //resets the click position
            _canvasClickUp.Y = 0;
            _initialTime = DateTime.Now.TimeOfDay;
            _currentTime = DateTime.Now.TimeOfDay;
            _previousTime = _currentTime;
            _initialMoney = _money;
            _initialKills = _totalKills;
            _newHighScore = false;
            _score = 0;
            _timeBonus = 0;
            _healthBonus = 0;
            _healthLost = 0;
            _coinBonus = 0;
            _killBonus = 0;
            _killGained = 0;
            _completing = false;
            _wallAnchor = 1000;
            #endregion //miscelaneous
            #region magnet
            _magnetRange = 100;
            _magnetStrength = 8;
            #endregion //magnet
            #region bonuses
            _recoilBonus = (float)Math.Pow(1.1f, _upgrades[0]);
            _bulletSpeedBonus = (float)Math.Pow(1.1f, _upgrades[1]);
            _explosionRadiusBonus = (float)Math.Pow(1.1f, _upgrades[2]);
            _accelerationPowerBonus = (float)Math.Pow(1.1f, _upgrades[3]);
            _attackPowerBonus = (float)Math.Pow(1.1f, _upgrades[4]);
            #endregion //bonuses
            #region height goal
            switch (_levelSelected)
            {
                case 1:
                    _heightGoal = -15000;
                    break;
                case 2:
                    _heightGoal = -30000;
                    break;
                default:
                    break;
            }
            #endregion //height goal
            #region Explosion Selection
            _activeExplosionType = Pellet.ExplosionType.Normal;
            _selectionWheelAngleCurrent = 0;
            _selectionWheelAngleDestination = 0;
            _numberOfUnlockedExplosionTypes = 5;
            #endregion //Explosion Selection
            #region health
            _pyroHealthMax = 100;
            _pyroHealth = _pyroHealthMax;
            #endregion //health
            #region Water
            _waterLevel = 1000;
            _waterRisingRate = 2;
            _waterAccelerationCountdown = 1000;
            #endregion //Water
            #region Charge
            _maxCharge = 100;
            _charge = _maxCharge;
            _chargeRate = 10;
            #endregion //Charge
            #region Clear Lists
            _activeBubbles.Clear();
            _activeAnimations.Clear();
            _activeExplosions.Clear();
            _activePellets.Clear();
            _activeCoins.Clear();
            _activeDamages.Clear();
            _activeProjectiles.Clear();
            _activeMonsters.Clear();
            _inactiveMonsters.Clear();
            _dyingMonsters.Clear();
            #endregion
            #region Coins
            switch (_levelSelected)
            {
                case 1:
                    _coinLocations = new Point[] { new Point(500, 500), new Point(600, 600), new Point(100, -50), new Point(150, -50), new Point (200, -50),
                                                   new Point(400, -400), new Point(450, -450), new Point(500, -500),
                                                   new Point(10, -3000), new Point(50, -3000), new Point(90, -3000),
                                                   new Point(10, -3040), new Point(50, -3040), new Point(90, -3040)};
                    _coinValues = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                    break;
                case 2:
                    _coinLocations = new Point[] { new Point(800, 800), new Point(600, 600) };
                    _coinValues = new int[] { 1, 1 };
                    break;
                default:
                    break;
            }
            for (int i = 0; i < _coinLocations.Length; i++)
            {
                Coin c = Coin.CreateCoin(_coinLocations[i].X, _coinLocations[i].Y, _coinValues[i], 0, 0);
                c.Anim.Bmp = new Bitmap(GetType(), "Coin.bmp");
                c.Anim.Bmp.MakeTransparent(Color.Magenta);
                _activeAnimations.Add(c.Anim);
                _activeCoins.Add(c);
            }
            #endregion //Coins
            #region Monsters
            switch (_levelSelected)
            {
                case 1:
                    _monsterSpawnPoints = new Point[] { new Point(500, -2000), new Point(800, -2200), new Point(200, -2200), new Point(500, -2300),
                                                        new Point(100, -6000), new Point(200, -6000), new Point(300, -6000), new Point(900, -6500),
                                                        new Point(600, -10000), new Point(500, -10000), new Point(900, -10000), new Point(200, -10000) };
                    _monsterSpawnTriggers = new int[] { -1600, -1800, -1800, -1800, -5000, -5000, -5000, -5000, -8000, -8000, -8000, -8000 };
                    _monsterSpawnIDs = new int[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2 };
                    break;
                case 2:
                    _monsterSpawnPoints = new Point[] { new Point(500, -2000), new Point(500, -3500), new Point(300, -3500), new Point(400, -3500),
                                                        new Point(700, -3500), new Point(600, -3500)};
                    _monsterSpawnTriggers = new int[] { -2000, -2100, -2200, -2300, -2400, -2500 };
                    _monsterSpawnIDs = new int[] { 1, 1, 1, 1, 1, 1 };
                    break;
                default:
                    break;
            }
            MonsterType type = MonsterType.Bird;
            for (int i = 0; i < _monsterSpawnPoints.Length; i++)
            {
                switch (_monsterSpawnIDs[i])
                {
                    case 1:
                        type = MonsterType.Bird;
                        break;
                    case 2:
                        type = MonsterType.Bean;
                        break;
                    default:
                        break;
                }
                Monster m = Monster.CreateMonster(_monsterSpawnPoints[i].X, _monsterSpawnPoints[i].Y, type, _monsterSpawnTriggers[i]);
                _inactiveMonsters.Add(m);
            }
            #endregion Monsters
            #region Bitmaps
            #region Monster Bitmaps
            //Monster bitmaps
            _birdLeft = new Bitmap(GetType(), "Bird.bmp");
            _birdRight = new Bitmap(GetType(), "Bird2.bmp");
            _birdLeft.MakeTransparent(Color.Magenta);
            _birdRight.MakeTransparent(Color.Magenta);        

            _beanLeft = new Bitmap(GetType(), "Bean.bmp");
            _beanRight = new Bitmap(GetType(), "Bean2.bmp");
            _beanLeft.MakeTransparent(Color.Magenta);
            _beanRight.MakeTransparent(Color.Magenta);
            #endregion
            #region Charge Bitmaps
            //charge images
            _chargeBarEmpty = new Bitmap(GetType(), "EmptyChargeBar.bmp");
            _chargeBarFull = new Bitmap(GetType(), "FullChargeBar.bmp");
            _chargeBarFilling = new Bitmap(GetType(), "ChargeBarFilling.bmp");

            //charge image transparencies
            _chargeBarEmpty.MakeTransparent(Color.Magenta);
            _chargeBarFull.MakeTransparent(Color.Magenta);
            _chargeBarFilling.MakeTransparent(Color.Magenta);
            #endregion //charge
            #region Wave Bitmaps
            //waves
            _waves = new Bitmap(GetType(), "Waves.bmp");
            _waves.MakeTransparent(Color.LightGray);
            _downArrowBmp = new Bitmap(GetType(), "DownArrow.bmp");
            _downArrowBmp.MakeTransparent(Color.Magenta);
            #endregion //waves
            #region Drowning
            //drowning
            _drowning = new Bitmap(GetType(), "Drowning.bmp");
            _drowning2 = new Bitmap(GetType(), "Drowning2.bmp");
            _drowning3 = new Bitmap(GetType(), "Drowning3.bmp");
            _drowningArm = new Bitmap(GetType(), "DrowningArm.bmp");
            _drowningArm.MakeTransparent(Color.Magenta);
            _bubble = new Bitmap(GetType(), "Bubble.bmp");
            _bubble.MakeTransparent(Color.Magenta);
            #endregion //Drowning
            #region Fast-Forward Bitmaps
            //fast forward
            _fastForwardBmp = new Bitmap(GetType(), "FastForward.bmp");
            _fastForwardingBmp = new Bitmap(GetType(), "FastForwarding.bmp");
            _fastForwardBmp.MakeTransparent(Color.Magenta);
            _fastForwardingBmp.MakeTransparent(Color.Magenta);
            #endregion //fastforward
            #region Wall Bitmaps
            //walls
            _wallBmp = new Bitmap(GetType(), "Wall.bmp");
            _wallRightBmp = new Bitmap(GetType(), "WallReverse.bmp");
            _wallBmp.MakeTransparent(Color.Magenta);
            _wallRightBmp.MakeTransparent(Color.Magenta);
            #endregion //wall bitmaps
            #region Health Bar Bitmaps
            //hp bar
            _healthBar = new Bitmap(GetType(), "HealthBar.bmp");
            _healthBarEmpty = new Bitmap(GetType(), "HealthBarEmpty.bmp");
            _healthBar.MakeTransparent(Color.Magenta);
            _healthBarEmpty.MakeTransparent(Color.Magenta);
            #endregion //Health Bar Bitmaps
            #region Selection Wheel
            //selection wheel
            _selectionWheelBmp = new Bitmap(GetType(), "SelectionWheel.bmp");
            _selectionWheelOutlineBmp = new Bitmap(GetType(), "SelectionWheelOutline.bmp");
            _selectionWheelHoverBmp = new Bitmap(GetType(), "SelectionWheelHover.bmp");
            _selectionWheelBmp.MakeTransparent(Color.Magenta);
            _selectionWheelOutlineBmp.MakeTransparent(Color.Magenta);
            _selectionWheelHoverBmp.MakeTransparent(Color.Magenta);
            #endregion //Selection Wheel
            #region Pellets
            //pellets

            #endregion //Pellets
            #region Flash
            //flash



            _flashBmps = new Bitmap[] { _waves, _waves, _waves, _waves, _waves};

            for (int i = 1; i < 6; i++)
            {
                _flashBmp = new Bitmap(Canvas.Width, Canvas.Height);

                Bitmap flashbmp = new Bitmap(Canvas.Width, Canvas.Height);

                ColorMatrix cm4 = new ColorMatrix();
                cm4.Matrix00 = cm4.Matrix11 = cm4.Matrix22 = cm4.Matrix44 = 1;
                cm4.Matrix33 = (0.8f - (0.1f * (float)i));

                //create an image attributes object and set the color matrix to cm
                ImageAttributes ia4 = new ImageAttributes();
                ia4.SetColorMatrix(cm4);

                using (Graphics g = Graphics.FromImage(flashbmp))
                {
                    g.FillRectangle(_whiteBrush, 0, 0, Canvas.Width, Canvas.Height);
                }

                using (Graphics g = Graphics.FromImage(_flashBmp))
                {
                    g.DrawImage(flashbmp, new Rectangle(0, 0, Canvas.Width, Canvas.Height), 0, 0, Canvas.Width, Canvas.Height, GraphicsUnit.Pixel, ia4);
                }

                _flashBmps[i - 1] = _flashBmp;
            }


            _flashBmp = new Bitmap(Canvas.Width, Canvas.Height);
            using (Graphics g = Graphics.FromImage(_flashBmp))
            {
                g.FillRectangle(_whiteBrush, 0, 0, Canvas.Width, Canvas.Height);
            }
            #endregion //Flash
            #region ScoreBoard
            //score board
            _scoreBoardBmp = new Bitmap(GetType(), "ScoreBoard.bmp");
            _scoreBoardBmp.MakeTransparent(Color.Magenta);
            #endregion //Score board
            #region Level Complete
            //level complete
            _levelCompleteBmp = new Bitmap(622, 60);
            using (Graphics g = Graphics.FromImage(_levelCompleteBmp))
            {
                g.DrawString("LEVEL COMPLETE!", _gameOverFont, _greenBrush, 0, 0);
            }
            #endregion //level complete
            #endregion //Bitmaps
        }

        private void ManageCharge()
        {
            if (_pyroHealth < 0)
                _pyroHealth = 0;

            _charge += _chargeRate;
            if (_charge > _maxCharge)
                _charge = _maxCharge;
            if (_charge < 0)
                _charge = 0;
            if (_pyroHealth == 0)
                _charge -= _chargeRate + 1;
        }
        private void ManagePellets()
        {
            //new pellets? sure! why not.
            if (_clicked != _clicking && _clicked == true && _ending == false) //if you were clicking but now you're not, then we're gonna create a pellet
            {
                if (_charge == _maxCharge) //...but only if you're all charged up
                {
                    Pellet p = Pellet.CreatePellet((float)_pyroXs, (float)_pyroYs, _canvasClickUp.X, _canvasClickDown.Y + _screenAnchorY, _activeExplosionType, _bulletSpeedBonus);

                    float kick = 0;

                    switch (_activeExplosionType)
                    {
                        case Pellet.ExplosionType.Normal:
                            p.Bmp = _pelletNormalBmp;
                            _charge -= 100;
                            kick = 1.5f;
                            break;
                        case Pellet.ExplosionType.Flash:
                            p.Bmp = _pelletFlashBmp;
                            _charge -= 50;
                            kick = 0.2f;
                            break;
                        case Pellet.ExplosionType.Gravitron:
                            p.Bmp = _pelletGravitronBmp;
                            _charge -= 100;
                            kick = 2.6f;
                            break;
                        case Pellet.ExplosionType.Missile:
                            p.Bmp = _pelletMissileBmp;
                            #region rotate
                            int l = (Math.Max(p.Bmp.Width, p.Bmp.Height)) * (142 / 100); //increase matrix size to allow space for rotation

                            float angle = 0;
                            if (p.XVelocity != 0) //fancy rotation math
                            {
                                if (p.YVelocity <= 0 && p.XVelocity > 0) //going up and right
                                    angle = 180 * (float)(Math.Atan((p.XVelocity) / ((-1) * (p.YVelocity))) / Math.PI);
                                if (p.YVelocity > 0 && p.XVelocity > 0) //going down and right
                                    angle = 180 - (float)((180 * Math.Atan((p.XVelocity) / (p.YVelocity))) / Math.PI);
                                if (p.YVelocity <= 0 && p.XVelocity < 0) //going up and left
                                    angle = 360 - (float)((180 * Math.Atan((p.XVelocity) / (p.YVelocity))) / Math.PI);
                                if (p.YVelocity > 0 && p.XVelocity < 0) //going down and left
                                    angle = 180 + (float)(180 * (Math.Atan((p.XVelocity) / ((-1) * (p.YVelocity)))) / Math.PI);
                            }

                            Bitmap bmp2 = new Bitmap(l, l); //make a new bmp for the rotated image
                            using (Graphics g = Graphics.FromImage(bmp2))
                            {
                                g.SmoothingMode = SmoothingMode.HighQuality; //not really sure what this does
                                g.Clear(Color.Transparent); //sets the bmp to transparent
                                Matrix b = new Matrix(); //makes a matrix
                                b.Translate((p.Bmp.Width / 2) * -1,
                                    (p.Bmp.Height / 2) * -1, MatrixOrder.Append); //translates the matrix so the center is in the top left corner
                                b.Rotate((float)(angle), MatrixOrder.Append); //rotates clockwise based on the angle determined above ^
                                b.Translate((bmp2.Width / 2), (bmp2.Height / 2), MatrixOrder.Append); //translates the center back to the center
                                g.Transform = b; //rotates the bitmap based on the matrix
                                g.DrawImage(p.Bmp, 0, 0); //draws the bitmap to bmp2

                                p.Bmp = bmp2; //sets the temporary bitmap to the usable bitmap
                            }
                            #endregion //rotate
                            _charge -= 100;
                            kick = 2f;
                            break;
                        case Pellet.ExplosionType.Boquet:
                            p.Bmp = _pelletSkullBmp;
                            _charge -= 100;
                            kick = 0.6f;
                            break;
                        default:
                            break;
                    }
                    _activePellets.Add(p);

                    Animation a = Animation.CreateAnimation(_canvasClickUp.X - 10, _canvasClickDown.Y - 20 + _screenAnchorY, AnimationType.Flag, p.TimeLeft / 12);
                    a.Bmp = new Bitmap(GetType(), "Flag.bmp");
                    a.Bmp.MakeTransparent(Color.Magenta);
                    _activeAnimations.Add(a);

                    kick *= _recoilBonus;

                    _pyroXv -= kick * (p.XVelocity / p.Speed);
                    _pyroYv -= kick * (p.YVelocity / p.Speed);

                }
            }
            List<Pellet> toDelete = new List<Pellet>();
            List<Pellet> toAdd = new List<Pellet>();
            //let's move the ones that are already there now
            foreach (Pellet pellet in _activePellets)
            {
                pellet.X += pellet.XVelocity; //self explanitory
                pellet.Y += pellet.YVelocity; //mhm

                if (pellet.Type == Pellet.ExplosionType.Boquet || pellet.Type == Pellet.ExplosionType.Flower)
                {
                    #region rotate
                    int l = (Math.Max(pellet.Bmp.Width, pellet.Bmp.Height)) * (142 / 100); //increase matrix size to allow space for rotation

                    Bitmap bmp2 = new Bitmap(l, l); //make a new bmp for the rotated image
                    using (Graphics g = Graphics.FromImage(bmp2))
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality; //not really sure what this does
                        g.Clear(Color.Transparent); //sets the bmp to transparent
                        Matrix b = new Matrix(); //makes a matrix
                        b.Translate((pellet.Bmp.Width / 2) * -1,
                            (pellet.Bmp.Height / 2) * -1, MatrixOrder.Append); //translates the matrix so the center is in the top left corner
                        b.Rotate((float)(10), MatrixOrder.Append); //rotates clockwise based on the angle determined above ^
                        b.Translate((bmp2.Width / 2), (bmp2.Height / 2), MatrixOrder.Append); //translates the center back to the center
                        g.Transform = b; //rotates the bitmap based on the matrix
                        g.DrawImage(pellet.Bmp, 0, 0); //draws the bitmap to bmp2

                        pellet.Bmp = bmp2; //sets the temporary bitmap to the usable bitmap
                    }
                    #endregion //rotate
                }

                pellet.TimeLeft--;            //countdown timer
                if (pellet.TimeLeft <= 0)     //when it reaches the end of the timer
                {
                    toDelete.Add(pellet);     //then it's gonna get deleted

                    Explosion e = Explosion.CreateExplosion((int)pellet.XDestination,(int)pellet.YDestination, pellet.Type, _explosionRadiusBonus, _attackPowerBonus, _accelerationPowerBonus);
                    _activeExplosions.Add(e);

                    AnimationType at = AnimationType.NormalExplosion;
                    switch (pellet.Type)
                    {
                        case Pellet.ExplosionType.Normal:
                            at = AnimationType.NormalExplosion;
                            break;
                        case Pellet.ExplosionType.Flash:
                            at = AnimationType.Flash;
                            break;
                        case Pellet.ExplosionType.Gravitron:
                            at = AnimationType.Gravitron;
                            break;
                        case Pellet.ExplosionType.Missile:
                            at = AnimationType.Missile;
                            break;
                        case Pellet.ExplosionType.Boquet:
                            at = AnimationType.Boquet;
                            #region new flowers
                            float magnitude = pellet.Speed;
                            float angle = 0;
                            if (pellet.XVelocity != 0) //fancy rotation math
                            {
                                if (pellet.YVelocity <= 0 && pellet.XVelocity > 0) //going up and right
                                    angle = 180 * (float)(Math.Atan((pellet.XVelocity) / ((-1) * (pellet.YVelocity))) / Math.PI);
                                if (pellet.YVelocity > 0 && pellet.XVelocity > 0) //going down and right
                                    angle = 180 - (float)((180 * Math.Atan((pellet.XVelocity) / (pellet.YVelocity))) / Math.PI);
                                if (pellet.YVelocity <= 0 && pellet.XVelocity < 0) //going up and left
                                    angle = 360 - (float)((180 * Math.Atan((pellet.XVelocity) / (pellet.YVelocity))) / Math.PI);
                                if (pellet.YVelocity > 0 && pellet.XVelocity < 0) //going down and left
                                    angle = 180 + (float)(180 * (Math.Atan((pellet.XVelocity) / ((-1) * (pellet.YVelocity)))) / Math.PI);
                            }
                            for (int i = 1; i < 12; i += 2)
                            {
                                Pellet p = Pellet.CreatePellet(pellet.XDestination, pellet.YDestination, pellet.XDestination + 300 * (float)(Math.Pow(1.1f, _explosionRadiusBonus)) * (float)Math.Cos((angle + 30 * i) * Math.PI / 180),
                                                                                                         pellet.YDestination + 300 * (float)(Math.Pow(1.1f, _explosionRadiusBonus)) * (float)Math.Sin((angle + 30 * i) * Math.PI / 180), 
                                                                                                         Pellet.ExplosionType.Flower, _bulletSpeedBonus);
                                p.Bmp = _pelletSkullBmp;
                                toAdd.Add(p);
                            }
                            #endregion //new flowers
                            break;
                        case Pellet.ExplosionType.Flower:
                            at = AnimationType.Boquet;
                            break;
                        default:
                            break;
                    }
                    Animation a = Animation.CreateAnimation((int)pellet.XDestination, (int)pellet.YDestination, at, 1);

                    switch (pellet.Type)
                    {
                        case Pellet.ExplosionType.Normal:
                            a.Bmp = new Bitmap(GetType(), "NormalExplosion.bmp");
                            break;
                        case Pellet.ExplosionType.Flash:
                            a.Bmp = new Bitmap(GetType(), "FlashExplosion.bmp");
                            _flashTimer = 5;
                            break;
                        case Pellet.ExplosionType.Gravitron:
                            a.Bmp = new Bitmap(GetType(), "Gravitron.bmp");
                            break;
                        case Pellet.ExplosionType.Missile:
                            a.Bmp = new Bitmap(GetType(), "MissileExplosion.bmp");
                            break;
                        case Pellet.ExplosionType.Boquet:
                            a.Bmp = new Bitmap(GetType(), "BoquetExplosion.bmp");
                            break;
                        case Pellet.ExplosionType.Flower:
                            a.Bmp = new Bitmap(GetType(), "BoquetExplosion.bmp");
                            break;
                        default:
                            break;
                    }

                    a.Bmp.MakeTransparent(Color.Magenta);
                    a.X -= a.FrameWidth / 2;
                    a.Y -= a.Bmp.Height / 2;
                    _activeAnimations.Add(a);
                }

                if ((pellet.TimeLeft == 3 || pellet.TimeLeft == 6 || pellet.TimeLeft == 9) && pellet.Type == Pellet.ExplosionType.Missile)
                {
                    Explosion e = Explosion.CreateExplosion((int)pellet.X, (int)pellet.Y, pellet.Type, _explosionRadiusBonus, _attackPowerBonus, _accelerationPowerBonus);
                    _activeExplosions.Add(e);
                    Animation a = Animation.CreateAnimation((int)pellet.X, (int)pellet.Y, AnimationType.Missile, 1);
                    a.Bmp = new Bitmap(GetType(), "MissileExplosion.bmp");
                    a.Bmp.MakeTransparent(Color.Magenta);
                    a.X -= a.FrameWidth / 2;
                    a.Y -= a.Bmp.Height / 2;
                    _activeAnimations.Add(a);
                }
            }
            foreach (Pellet p in toDelete)    //deleting and stuff now
                _activePellets.Remove(p);     //oh god they're all getting destroyed
            toDelete.Clear();                 //and now even the list of pellets to destroy is gone!!

            foreach (Pellet p in toAdd)       //but don't worry. we can always bring new life into the world.
                _activePellets.Add(p);
            toAdd.Clear();

            _clicked = _clicking;
        }
        private void ManageProjectiles()
        {
            List<Projectile> toDelete = new List<Projectile>();
            foreach (Projectile p in _activeProjectiles)
            {
                p.X += p.XVelocity;
                p.Y += p.YVelocity;

                if (p.X < -30 || p.X > Canvas.Width + 30)
                    toDelete.Add(p);
                if (p.Y > _waterLevel || p.Y + 5000 < _pyroYs)
                    toDelete.Add(p);

                if (_invincibilityTimer == 0)
                {
                    if ((_pyroXs - (_pyroBmp.Width / 2) <= (p.X + (p.Bmp.Width / 2))) && (_pyroXs + (_pyroBmp.Width / 2) >= (p.X - (p.Bmp.Width / 2)))) //in range of x?
                    {
                        if ((_pyroYs - (_pyroBmp.Height / 2) <= (p.Y + (p.Bmp.Height / 2))) && (_pyroYs + (_pyroBmp.Height / 2) >= (p.Y - (p.Bmp.Height / 2)))) //and y?
                        {
                            toDelete.Add(p);
                            _activeDamages.Add(Damage.CreateDamage((int)_pyroXs, (int)_pyroYs - (_pyroBmp.Height / 3), p.AttackPower, true, false));
                            _pyroHealth -= p.AttackPower;
                            _healthLost += p.AttackPower;
                            _invincibilityTimer = 30;
                        }
                    }
                }
            }
            foreach (Projectile p in toDelete)
                _activeProjectiles.Remove(p);
            toDelete.Clear();
        }
        private void ManageExplosions()
        {
            List<Explosion> toDelete = new List<Explosion>();
            foreach (Explosion e in _activeExplosions)
            {
                bool activated = false;

                if (!e.HasHitPyro)
                {
                    double dist = Math.Sqrt(((e.Origin.X - _pyroXs) * (e.Origin.X - _pyroXs)) + ((e.Origin.Y - _pyroYs) * (e.Origin.Y - _pyroYs)));
                    double adjustment = 0.5f + ((e.MaxRadius - dist) / (2 * e.MaxRadius));
                    if (adjustment > 1)
                        adjustment = 1;

                    if ((dist >= e.CurrentRadius && dist <= e.CurrentRadius + e.RadiusGrowth) || (dist <= e.CurrentRadius && e.CurrentRadius == e.RadiusGrowth))
                    {
                        if ((e.CurrentRadius != e.MaxRadius) || (dist - e.CurrentRadius <= (float)e.RadiusGrowth / 3)) //if it's already at its max radius, 1/3 growth more (I'm generous)
                        {
                            if (dist != 0) //if your location is right on top of the explosion epicenter then you'd be pushed in all directions, resulting in a net zero force
                            {              // (also, I don't really want to divide by zero)
                                _pyroXv -= ((e.Origin.X - _pyroXs) * (e.AccelerationPower) / (dist)) * adjustment; //the portion of the acceleration power of the explosion given to each component
                                _pyroYv -= ((e.Origin.Y - _pyroYs) * (e.AccelerationPower) / (dist)) * adjustment;

                                activated = true;
                                e.HasHitPyro = true;
                            }
                        }
                    }

                    dist -= _pyroBmp.Height / 2;

                    if (!activated && dist > 0)
                    {
                        if ((dist >= e.CurrentRadius && dist <= e.CurrentRadius + e.RadiusGrowth) || (dist <= e.CurrentRadius && e.CurrentRadius == e.RadiusGrowth))
                        {
                            if ((e.CurrentRadius != e.MaxRadius) || (dist - e.CurrentRadius <= (float)e.RadiusGrowth / 3)) //if it's already at its max radius, 1/3 growth more (I'm generous)
                            {
                                if (dist != 0) //if your location is right on top of the explosion epicenter then you'd be pushed in all directions, resulting in a net zero force
                                {              // (also, I don't really want to divide by zero)
                                    dist += _pyroBmp.Height / 2;
                                    _pyroXv -= ((e.Origin.X - _pyroXs) * (e.AccelerationPower) / (dist)) * adjustment; ; //the portion of the acceleration power of the explosion given to each component
                                    _pyroYv -= ((e.Origin.Y - _pyroYs) * (e.AccelerationPower) / (dist)) * adjustment;
                                    activated = true;
                                    e.HasHitPyro = true;
                                }
                            }
                        }
                    }
                }
                

                foreach (Monster m in _activeMonsters) //now to do explosion + monster collisions
                {
                    if (Math.Abs(m.Y - e.Origin.Y) <= e.MaxRadius + e.RadiusGrowth) //first let's just make sure it's in the vicinity...
                    {
                        float distance = (float)Math.Sqrt(((e.Origin.X - m.X) * (e.Origin.X - m.X)) + ((e.Origin.Y - m.Y) * (e.Origin.Y - m.Y))); //little bit o' pythagorean action

                        if ((distance >= e.CurrentRadius && distance <= e.CurrentRadius + e.RadiusGrowth) || (distance <= e.CurrentRadius && e.CurrentRadius == e.RadiusGrowth))
                        {
                            if ((e.CurrentRadius != e.MaxRadius) || (distance - e.CurrentRadius <= (float)e.RadiusGrowth / 3)) //same as above with the 1/3 thing
                            {
                                _activeDamages.Add(Damage.CreateDamage((int)m.X, (int)m.Y - (int)(m.Anim.Bmp.Height / 4), e.KillingPower, false, false));

                                if (!_completing)
                                {
                                    m.Health -= e.KillingPower;
                                }

                                m.X -= (float)((e.Origin.X - m.X) * (e.AccelerationPower) / (distance));
                                m.Y -= (float)((e.Origin.Y - m.Y) * (e.AccelerationPower) / (distance));

                                m.Chasing = true;
                            }
                        }
                    }
                }
                if (e.Type == Pellet.ExplosionType.Gravitron)
                {
                    foreach (Coin c in _activeCoins)
                    {
                        if (e.CurrentRadius * e.CurrentRadius >= ((c.X - e.Origin.X) * (c.X - e.Origin.X)) + ((c.Y - e.Origin.Y) * (c.Y - e.Origin.Y)))
                            c.Gravitating = true;
                    }
                }

                e.CurrentRadius += e.RadiusGrowth; //expand the shockwave
                if (e.CurrentRadius >= e.MaxRadius) //grown too much
                    toDelete.Add(e);
            }
            foreach (Explosion e in toDelete) //clean up finished explosions
                _activeExplosions.Remove(e);
            toDelete.Clear();
        }
        private void ManageAnimations()
        {
            List<Animation> toDelete = new List<Animation>();
            foreach (Animation a in _activeAnimations)
            {
                a.TimeUntilNextFrame--;

                if (a.TimeUntilNextFrame == 0)
                {
                    if (!a.Reversing)
                        a.CurrentFrame++;
                    else
                        a.CurrentFrame--;
                    a.TimeUntilNextFrame = a.TimeBetweenFrames;
                }
                if (a.CurrentFrame > a.NumberOfFrames)
                {
                    if (!a.Reversable)
                        a.CurrentFrame = 1;
                    else
                    {
                        a.CurrentFrame -= 2;
                        a.Reversing = true;
                    }
                }
                if (a.CurrentFrame < 1)
                {
                    a.CurrentFrame = 1;
                    a.Reversing = false;
                }

                a.TimeLeft--;
                if (a.TimeLeft <= 0 && a.Terminates)
                    toDelete.Add(a);
            }
            foreach (Animation a in toDelete)
                _activeAnimations.Remove(a);
            toDelete.Clear();
        }
        private void ManageFlash()
        {
            if (_flashTimer > 0)
            {
                _flashTimer--;
            }
            if (_flashTimer < 0)
                _flashTimer = 0;
        }
        private void ManageSelectionWheel()
        {
            switch (_activeExplosionType)
            {
                case Pellet.ExplosionType.Normal:
                    _selectionWheelAngleDestination = 360;
                    break;
                case Pellet.ExplosionType.Flash:
                    _selectionWheelAngleDestination = (360 / _numberOfUnlockedExplosionTypes);
                    break;
                case Pellet.ExplosionType.Gravitron:
                    _selectionWheelAngleDestination = (720 / _numberOfUnlockedExplosionTypes);
                    break;
                case Pellet.ExplosionType.Missile:
                    _selectionWheelAngleDestination = (1080 / _numberOfUnlockedExplosionTypes);
                    break;
                case Pellet.ExplosionType.Boquet:
                    _selectionWheelAngleDestination = (1440 / _numberOfUnlockedExplosionTypes);
                    break;
                default:
                    break;
            }

            while (_selectionWheelAngleCurrent <= 0)
                _selectionWheelAngleCurrent += 360;
            while (_selectionWheelAngleCurrent > 360)
                _selectionWheelAngleCurrent -= 360;

            while (_selectionWheelAngleDestination <= 0)
                _selectionWheelAngleDestination += 360;
            while (_selectionWheelAngleDestination > 360)
                _selectionWheelAngleDestination -= 360;

            float dist = Math.Abs(_selectionWheelAngleCurrent - _selectionWheelAngleDestination);
            if (dist > 40)
                _selectionWheelAngleCurrent += 15;
            else if (dist > 15)
                _selectionWheelAngleCurrent += 5;
            else if (dist > 3)
                _selectionWheelAngleCurrent += 2;
            else
                _selectionWheelAngleCurrent = _selectionWheelAngleDestination;
        }
        private void ManageDamage()
        {
            List<Damage> toDelete = new List<Damage>();
            foreach (Damage d in _activeDamages)
            {
                d.Timer--;
                d.Y--;

                if (d.Timer <= 0)
                    toDelete.Add(d);
            }
            foreach (Damage d in toDelete)
                _activeDamages.Remove(d);
            toDelete.Clear();
        }
        private void ManageCoins()
        {
            List<Coin> toDelete = new List<Coin>();
            foreach (Coin c in _activeCoins)
            {
                if (c.Y > _waterLevel)
                {
                    toDelete.Add(c);
                    _activeAnimations.Remove(c.Anim);
                }

                if (c.MoveTimer > 0)
                {
                    c.X += c.xV * ((float)c.MoveTimer / 30);
                    c.Y += c.yV * ((float)c.MoveTimer / 30);
                }

                if (c.Gravitating)
                {
                    float dist = (float)Math.Sqrt(((_pyroXs - c.X) * (_pyroXs - c.X)) + ((_pyroYs - c.Y) * (_pyroYs - c.Y)));
                    c.xV += ((_magnetStrength * ((float)_pyroXs - (c.X + c.xV))) / (dist * Math.Abs(dist)));
                    c.yV += ((_magnetStrength * ((float)_pyroYs - (c.Y + c.yV))) / (dist * Math.Abs(dist)));

                    c.X += (((_magnetStrength / 100) * ((float)_pyroXs - c.X)));
                    c.Y += (((_magnetStrength / 100) * ((float)_pyroYs - c.Y)));
                }

                if (c.MoveTimer > 0)
                    c.MoveTimer--;

                c.Anim.X = (int)c.X - (c.Anim.FrameWidth / 2);
                c.Anim.Y = (int)c.Y - (c.Anim.Bmp.Height / 2);
            }
            foreach (Coin c in toDelete)
                _activeCoins.Remove(c);
            toDelete.Clear();
        }

        private void SpawnMonsters()
        {
            List<Monster> toDelete = new List<Monster>();
            foreach (Monster m in _inactiveMonsters)
            {
                if (_pyroYs < m.SpawnTrigger)
                {
                    Monster am = Monster.SpawnMonster(m);
                    
                    switch (am.Type)
                    {
                        case MonsterType.Bird:
                            am.Anim.Bmp = _birdLeft;
                            break;    
                        case MonsterType.Bean:
                            am.Anim.Bmp = _beanLeft;
                            break;
                        default:
                            break;
                    }

                    _activeAnimations.Add(am.Anim);
                    toDelete.Add(am);
                    _activeMonsters.Add(am);
                }
            }
            foreach (Monster m in toDelete)
                _inactiveMonsters.Remove(m);
            toDelete.Clear();
        }
        private void ManageMonsters()
        {
            #region dying
            List<Monster> toDeleteDead = new List<Monster>();
            foreach (Monster m in _dyingMonsters)
            {
                m.DyingCounter--;
                m.X += m.XMoving;
                m.Y += m.YMoving;

                if (m.DyingCounter <= 0)
                    toDeleteDead.Add(m);
            }
            foreach (Monster m in toDeleteDead)
                _dyingMonsters.Remove(m);
            toDeleteDead.Clear();
            #endregion //dying
            #region death
            List<Monster> toDelete = new List<Monster>();
            foreach (Monster m in _activeMonsters)
            {
                if (m.Health <= 0)
                {
                    toDelete.Add(m);
                    KillMonster(m);
                    _activeAnimations.Remove(m.Anim);                    
                }
            }
            foreach (Monster m in toDelete)
                _activeMonsters.Remove(m);
            toDelete.Clear();
            #endregion //death
            #region movement
            foreach (Monster m in _activeMonsters)
            {
                m.X += m.Speed * m.XMoving;
                m.Y += m.Speed * m.YMoving;

                float dist = (float)Math.Sqrt(((_pyroXs - m.X) * (_pyroXs - m.X)) + ((_pyroYs - m.Y) * (_pyroYs - m.Y)));
                if (dist < Canvas.Width / 2)
                    m.Chasing = true;

                if (!m.Chasing)
                {
                    if (_rand.Next(80) == 4)
                    {
                        m.XMoving = _rand.Next(-2, 2);
                    }
                    if (_rand.Next(80) == 10)
                    {
                        m.YMoving = _rand.Next(-2, 2);
                    }
                }
                else
                {
                    m.XMoving = 0;
                    if (_pyroXs > (m.X + 5))
                        m.XMoving = 1;
                    else if (_pyroXs < (m.X - 5))
                        m.XMoving = -1;

                    m.YMoving = 0;
                    if (_pyroYs > (m.Y + 200))
                        m.YMoving = 1;
                    else if (_pyroYs < (m.Y - 5))
                        m.YMoving = -1;

                    if (Math.Abs(_pyroXs - m.X) < 200 && (_pyroYs - m.Y > 50) && (_pyroYs - m.Y < 400) && m.AttackCooldown == 0 && !m.AttackWarmingUp)
                    {
                        m.AttackWarmingUp = true;
                        m.AttackWarmup = m.AttackWarmupInitial;

                        m.AimX = (float)_pyroXs;
                        m.AimY = (float)_pyroYs;
                    }
                }
                if (m.AttackWarmingUp)
                {
                    m.XMoving = 0;
                    m.YMoving = 0;
                    m.AttackWarmup--;
                }

                if (m.AttackWarmup == 0 && m.AttackWarmingUp)
                {
                    m.AttackWarmingUp = false;
                    m.AttackCooldown = m.AttackCooldownInitial;
                    SpawnProjectile(m.X, m.Y + m.Anim.Bmp.Height / 2, m.ProjectileSpeed, m.AimX, m.AimY, m.ProjectilePower, m.Type);
                }

                if (m.AttackCooldown > 0)
                    m.AttackCooldown--;

                if (m.X <= m.Speed)
                    m.XMoving = 1;
                else if (m.X >= Canvas.Width - m.Speed)
                    m.XMoving = -1;

                if (m.X - (m.Anim.FrameWidth / 2) <= 2)
                    m.X = (2 + m.Anim.FrameWidth / 2);
                else if (m.X + (m.Anim.FrameWidth / 2) >= (Canvas.Width - 2))
                    m.X = (Canvas.Width - 2 - (m.Anim.FrameWidth / 2));

                if (m.XMoving > 0)
                    m.FacingLeft = false;
                else if (m.XMoving < 0)
                    m.FacingLeft = true;

                if (m.FacingLeft != m.AnimFacingLeft)
                {
                    switch (m.Type)
                    {
                        case MonsterType.Bird:
                            if (m.FacingLeft == true)
                                m.Anim.Bmp = _birdLeft;
                            else
                                m.Anim.Bmp = _birdRight;
                            break;
                        case MonsterType.Bean:
                            if (m.FacingLeft)
                                m.Anim.Bmp = _beanLeft;
                            else
                                m.Anim.Bmp = _beanRight;
                            break;
                        default:
                            break;
                    }
                    m.AnimFacingLeft = m.FacingLeft;
                }

                m.Anim.X = (int)(m.X - (m.Anim.FrameWidth / 2));
                m.Anim.Y = (int)(m.Y - (m.Anim.Bmp.Height / 2));
            }
            #endregion //movement
        }
        private void ManageWater()
        {
            if (_waterLevel > _heightGoal)
                _waterLevel -= _waterRisingRate;
            _waterAccelerationCountdown--;
            if (_waterAccelerationCountdown == 0)
            {
                _waterRisingRate++;
                _waterAccelerationCountdown = _waterAccelerationCountdownInit;
            }

            _waveSwitchTimer--;
            if (_waveSwitchTimer == 0)
            {
                _waveNumber++;
                _waveSwitchTimer = _waveSwitchTimerInitial;
            }
            if (_waveNumber == 5)
                _waveNumber = 1;
        }
        private void ManageDeath()
        {
            if (_ending)
            {
                if (_canvasClickUp.X >= Canvas.Width - 95 - _fastForwardBmp.Width &&
                    _canvasClickUp.X <= Canvas.Width - 95 &&
                    _canvasClickUp.Y <= _fastForwardBmp.Height)
                {
                    _endCounter -= 10;
                }


                _endCounter--;

                if (_endCounter < 0)
                    _endCounter = 0;

                if (_endCounter == 0)
                {
                    _ending = false;
                    _paused = true;
                }

                if (_endCounter == 470 || _endCounter == 370 || _endCounter == 365 || _endCounter == 330 || _endCounter == 250 || _endCounter == 170 || _endCounter == 45)
                {
                    Bubble b = new Bubble();
                    b.X = (int)(_canvasMidX + 250 - (int)((Math.Abs(_endCounter - 250)) / 4));

                    int y = 100;
                    if (_waterLevel - _screenAnchorY > 0)
                        y = _waterLevel - _screenAnchorY + 100;

                    b.Y = y + ((500 - _endCounter) / 2) + 30;

                    _activeBubbles.Add(b);
                }
                List<Bubble> toDelete = new List<Bubble>();
                foreach (Bubble b in _activeBubbles)
                {
                    b.Y -= (1 + _waterRisingRate);
                    if (b.Y < _waterLevel)
                        toDelete.Add(b);
                }
                foreach (Bubble b in toDelete)
                    _activeBubbles.Remove(b);
                toDelete.Clear();
            }
        }
        private void ManageCompletion()
        {
            if (_screenAnchorY <= _heightGoal && !_completing)
            {
                _completing = true;
                _completeCounter = 200;
                _completionTime = DateTime.Now.TimeOfDay;
                _totalTime = _completionTime - _initialTime;
                _moneyGained = _money - _initialMoney;
                _killGained = _totalKills - _initialKills;

                _killBonus = _killGained * 5;
                _coinBonus = _moneyGained * 4;
                _healthBonus = (100 - _healthLost);
                if (_healthBonus < 0)
                    _healthBonus = 0;
                _timeBonus = 180 - (int)_totalTime.TotalSeconds;
                if (_timeBonus < 0)
                    _timeBonus = 0;

                _score += _healthBonus + _coinBonus + _timeBonus + _killBonus;

                if (!_highScoreSet[_levelSelected - 1])
                {
                    _highScoreSet[_levelSelected - 1] = true;
                    _highScores[_levelSelected - 1] = new int[] { _moneyGained, _killGained, _healthLost, _score };
                    _highScoreTimes[_levelSelected - 1][0] = _totalTime.Minutes;
                    _highScoreTimes[_levelSelected - 1][1] = _totalTime.Seconds;
                    _newHighScore = true;
                }
                else
                {
                    if (_highScores[_levelSelected - 1][3] < _score)
                    {
                            _highScores[_levelSelected - 1][0] = _moneyGained;
                            _highScores[_levelSelected - 1] = new int[] { _moneyGained, _killGained, _healthLost, _score };
                            _highScoreTimes[_levelSelected - 1][0] = _totalTime.Minutes;
                            _highScoreTimes[_levelSelected - 1][1] = _totalTime.Seconds;
                            _newHighScore = true;
                    }
                }
                SaveGame();
            }
            if (_completing)
            {
                if (_completeCounter > 0)
                    _completeCounter--;
                if (_pyroYv > -1)
                    _pyroYv = -1;
                if (_canvasClickUp.X >= _canvasMidX - 70 && _canvasClickUp.X <= _canvasMidX + 70 && _canvasClickUp.Y >= _canvasMidY + 55 && _canvasClickUp.Y <= _canvasMidY + 85)
                    _paused = true;
            }
        }
        private void MoveGamePieces()
        {
            MovePyro(); //moves Pyro
            WallCollisions(); //looks at Pyro running into walls... possibly will do more things later
        }
        private void PyroIsDead()
        {
            _ending = true;
            _endCounter = 500;
            _pyroHealth = 0;
            SaveGame();
        }
        private void MovePyro()
        {
            _pyroXv *= _pyroXd; //drag
            _pyroYv *= _pyroYd;

            _pyroXv += _pyroXa; //add acceleration to velocity
            _pyroXs += _pyroXv; //add velocity to position

            foreach (Coin c in _activeCoins)
            {
                if (c.Gravitating)
                {
                    c.X += (float)_pyroXv;
                    c.Y += (float)_pyroYv;
                }
            }

            _pyroYv += _pyroYa;
            _pyroYs += _pyroYv;

            if (_invincibilityTimer > 0)
                _invincibilityTimer--;

            #region collisions
            List<Coin> toDelete = new List<Coin>();
            foreach (Coin c in _activeCoins)
            {
                float dist = (float)Math.Sqrt(((_pyroXs - c.X) * (_pyroXs - c.X)) + ((_pyroYs - c.Y) * (_pyroYs - c.Y)));
                if (dist <= (_pyroBmp.Height / 2) + (c.Anim.Bmp.Height / 2))
                {
                    if (!_completing)
                    {
                        toDelete.Add(c);
                        _money += c.Value;
                    }
                }

                else if (dist <= _magnetRange)
                {
                    c.MoveTimer = 60;
                    c.Gravitating = true;
                }
            }
            foreach (Coin c in toDelete)
            {
                _activeAnimations.Remove(c.Anim);
                _activeCoins.Remove(c);
            }
            toDelete.Clear();

            foreach (Monster m in _activeMonsters)
            {
                float dist = (float)(Math.Sqrt(((_pyroXs - m.X) * (_pyroXs - m.X)) + ((_pyroYs - m.Y) * (_pyroYs - m.Y))));
                if (((dist <= (_pyroBmp.Height / 2) + (m.Anim.FrameWidth)) && (_pyroHealth != 0)) && (_invincibilityTimer == 0))
                {
                    _pyroXv *= 0.66f;
                    _pyroYv *= 0.66f;

                    _pyroHealth -= m.Attack;
                    _healthLost += m.Attack;

                    _invincibilityTimer = 30;

                    Damage d = Damage.CreateDamage((int)_pyroXs, (int)(_pyroYs - (_pyroBmp.Height / 3)), m.Attack, true, false);
                    _activeDamages.Add(d);
                }
            }
            #endregion /collisions
            #region anchor
            if (!_ending)
            {
                if (_pyroYs < _screenAnchorY + (Canvas.Height / 4))
                    _screenAnchorY = (int)(_pyroYs - (Canvas.Height / 4));
                if (_pyroYs > _screenAnchorY + (Canvas.Height / 3) * 2)
                    _screenAnchorY = (int)(_pyroYs - (Canvas.Height / 3) * 2);
            }
            #endregion //anchor
            #region death
            if (_pyroYs > _waterLevel && !_ending)
                PyroIsDead();
            if (_pyroHealth == 0)
                _pyroYa = 0.5;
            #endregion //death


            #region angle
            float angle = 0;
            if (_pyroXv != 0) //fancy rotation math
            {
                if (_pyroYv <= 0 && _pyroXv > 0) //going up and right
                    angle = 180 * (float)(Math.Atan((_pyroXv) / ((-1) * (_pyroYv))) / Math.PI);
                if (_pyroYv > 0 && _pyroXv > 0) //going down and right
                    angle = 180 - (float)((180 * Math.Atan((_pyroXv) / (_pyroYv))) / Math.PI);
                if (_pyroYv <= 0 && _pyroXv < 0) //going up and left
                    angle = 360 - (float)((180 * Math.Atan((_pyroXv) / (_pyroYv))) / Math.PI);
                if (_pyroYv > 0 && _pyroXv < 0) //going down and left
                    angle = 180 + (float)(180 * (Math.Atan((_pyroXv) / ((-1) * (_pyroYv)))) / Math.PI);

                if (_pyroState == PyroState.rolling)
                    angle += 25 * _pyroRollCount;
            }

            float xPortion = (float)Math.Cos((Math.PI / 180) * angle);
            float yPortion = (float)Math.Sin((Math.PI / 180) * angle);

            #endregion //angle
            #region bitmap
            //figure out what state pyro is in... this determines which bitmap we use
            if (Math.Abs(_pyroXv / _pyroYv) > 1.6f)
            {
                _pyroState = PyroState.drifting;
            }
            else
            {
                if (_pyroYv < 0)
                    _pyroState = PyroState.rising;
                else
                    _pyroState = PyroState.drifting; //as opposed to falling
            }
            if ((Math.Sqrt(((_pyroXv) * (_pyroXv)) + ((_pyroYv) * (_pyroYv))) > 26) || Math.Sqrt(((_pyroXv) * (_pyroXv)) + ((_pyroYv) * (_pyroYv))) < 2)
                _pyroState = PyroState.rolling;
            #region determine state

            float standardX = 0;
            float standardY = 0;

            switch (_pyroState) //based on state, xvelocity and orientation, set a bmp
            {
                case PyroState.rising:
                    if (_pyroXv >= 0)
                    {
                        if (_canvasMove.X > _pyroXs + ((_pyroXv / _pyroYv) * (_canvasMove.Y - (_pyroYs - _screenAnchorY))))
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroRisingRightAR.bmp");
                            standardX = 6;
                            standardY = 4;
                        }
                        else
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroRisingRightAL.bmp");
                            standardX = -6;
                            standardY = 2;
                        }
                    }
                    else
                    {
                        if (_canvasMove.X > _pyroXs + ((_pyroXv / _pyroYv) * (_canvasMove.Y - (_pyroYs - _screenAnchorY))))
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroRisingLeftAR.bmp");
                            standardX = 6;
                            standardY = 2;
                        }
                        else
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroRisingLeftAL.bmp");
                            standardX = -6;
                            standardY = 3;
                        }
                    }
                    break;
                /*case PyroState.falling:
                    if (_canvasMove.X > _pyroXs)
                        _pyroBmp = new Bitmap(GetType(), "PyroFallingAimRight.bmp");       
                    else                                                                  
                        _pyroBmp = new Bitmap(GetType(), "PyroFallingAimLeft.bmp");        
                    break;                */                                                 
                case PyroState.rolling:                                                    
                    _pyroBmp = new Bitmap(GetType(), "PyroRolling.bmp");
                    break;                                                                 
                case PyroState.drifting:
                    if (_pyroXv >= 0)
                    {
                        if (_canvasMove.Y > (_pyroYs - _screenAnchorY) + ((_pyroYv / _pyroXv) * (_canvasMove.X - _pyroXs)))
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroDriftingRaD.bmp");
                            standardX = 4;
                            standardY = 4;
                        }
                        else
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroDriftingRaU.bmp");
                            standardX = -6;
                            standardY = 4;
                        }
                    }
                    else
                    {
                        if (_canvasMove.Y > (_pyroYs - _screenAnchorY) + ((_pyroYv / _pyroXv) * (_canvasMove.X - _pyroXs)))
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroDriftingLaD.bmp");
                            standardX = -6;
                            standardY = 3;
                        }
                        else
                        {
                            _pyroBmp = new Bitmap(GetType(), "PyroDriftingLaU.bmp");
                            standardX = 5;
                            standardY = 2;
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion //state
            _pyroBmp.MakeTransparent(Color.Magenta); //makes magenta transparent
            #endregion //bitmap
            #region rotation

            #region rotate pyro
            if (_pyroState != PyroState.rolling)
                _pyroRollCount = 0;
            else
                _pyroRollCount++;

                if (_pyroXv != 0)
                {

                int l = (Math.Max(_pyroBmp.Width, _pyroBmp.Height)) * (142 / 100); //increase matrix size to allow space for rotation
                Bitmap bmp2 = new Bitmap(l, l); //make a new bmp for the rotated image
                using (Graphics g = Graphics.FromImage(bmp2)) 
                {
                    g.SmoothingMode = SmoothingMode.HighQuality; //not really sure what this does
                    g.Clear(Color.Transparent); //sets the bmp to transparent
                    Matrix b = new Matrix(); //makes a matrix
                    b.Translate((_pyroBmp.Width / 2) * -1, 
                        (_pyroBmp.Height / 2) * -1, MatrixOrder.Append); //translates the matrix so the center is in the top left corner
                    b.Rotate((float)(angle), MatrixOrder.Append); //rotates clockwise based on the angle determined above ^
                    b.Translate((bmp2.Width / 2), (bmp2.Height / 2), MatrixOrder.Append); //translates the center back to the center
                    g.Transform = b; //rotates the bitmap based on the matrix
                    g.DrawImage(_pyroBmp, 0, 0); //draws the bitmap to bmp2

                   /* double xarmOffset = 0;
                    double yarmOffset = 0;

                    if (_pyroState == PyroState.drifting)
                    {
                        if (_pyroXv > 0 && _pyroYv > 0) //need to distinguish between aiming left/right and up/down somehow... should be same distinciton as drifting/floating                           
                        {
                            xarmOffset = (4 * Math.Cos((angle * Math.PI) / 180)) + (3 * Math.Cos(((90 - angle) * Math.PI) / 180));
                            yarmOffset = ((-4) * Math.Sin((angle * Math.PI) / 180)) + ((-3) * Math.Sin(((90 - angle) * Math.PI) / 180));
                        }
                    }*/


                    _pyroBmp = bmp2; //sets the temporary bitmap to the usable bitmap

                }
                }
            #endregion //rotate pyro
            #region rotate arm
                //rotate the arm

            double armCenterX = _pyroXs + (standardX * xPortion) + (standardY * yPortion);
            double armCenterY = _pyroYs + (standardX * yPortion) - (standardY * xPortion);

            double armLength = _pyroArmBasicBmp.Height;

            double armtoMouseDistance = Math.Sqrt(((_canvasMove.X - armCenterX) * (_canvasMove.X - armCenterX)) + ((_canvasMove.Y + _screenAnchorY - armCenterY) 
                                                                                                                 * (_canvasMove.Y + _screenAnchorY - armCenterY)));
            double scale = armLength / armtoMouseDistance;
            double armEndX = armCenterX + ((scale) * (_canvasMove.X - armCenterX));
            double armEndY = armCenterY + ((scale) * (_canvasMove.Y + _screenAnchorY - armCenterY));

            double xLength = armEndX - armCenterX;
            double yLength = armEndY - armCenterY;

            double armAngle = 0;
            if (xLength == 0 && yLength > 0)
                armAngle = 180;
            else if (xLength != 0)
            {
                if (xLength > 0)
                {
                    if (yLength == 0)
                        armAngle = 90;
                    else if (yLength > 0)
                        armAngle = (90 + (180 / Math.PI) * Math.Atan(yLength / xLength));
                    else
                        armAngle = (180 / Math.PI) * Math.Atan(xLength / ((-1) * yLength));
                }
                else
                {
                    if (yLength == 0)
                        armAngle = 270;
                    else if (yLength > 0)
                        armAngle = 180 + (180 / Math.PI) * Math.Atan(((-1) * xLength) / yLength);
                    else
                        armAngle = 270 + (180 / Math.PI) * Math.Atan(yLength / xLength);

                }
            }

            

            Bitmap bmp3 = new Bitmap(_pyroArmBasicBmp.Height, _pyroArmBasicBmp.Height); //make a new bmp for the rotated image
            using (Graphics g = Graphics.FromImage(bmp3))
            {
                g.SmoothingMode = SmoothingMode.HighQuality; //not really sure what this does
                g.Clear(Color.Transparent); //sets the bmp to transparent
                Matrix b = new Matrix(); //makes a matrix
                b.Translate((_pyroArmBasicBmp.Width / 2) * -1,
                    (_pyroArmBasicBmp.Height / 2) * -1, MatrixOrder.Append); //translates the matrix so the center is in the top left corner
                b.Rotate((float)(armAngle), MatrixOrder.Append); //rotates clockwise based on the angle determined above ^
                b.Translate((bmp3.Width / 2), (bmp3.Height / 2), MatrixOrder.Append); //translates the center back to the center
                g.Transform = b; //rotates the bitmap based on the matrix
                g.DrawImage(_pyroArmBasicBmp, 0, 0); //draws the bitmap to bmp2

                _pyroArmBmp = bmp3; //sets the temporary bitmap to the usable bitmap
            }
            #endregion //rotate

            _pyroArmAnchorX = (int)(armCenterX - (_pyroArmBmp.Width / 2) + (xLength / 2.5));
            _pyroArmAnchorY = (int)(armCenterY - (_pyroArmBmp.Height / 2) + (yLength / 2.5));

            #endregion //rotate
        }
        private void WallCollisions()
        {
            if ((((_pyroXs - (_pyroBmp.Width / 2)) <= 0) && (_pyroXv <= 0)) || //past the left wall and going left
                (((_pyroXs + (_pyroBmp.Width / 2)) >= Canvas.Width) && (_pyroXv >= 0))) //or past the right wall and going right
            {
                _pyroXv *= -0.2; //turn around and reduce speed to 20%
            }
        }
        private void SpawnCoins(float x, float y, int value, int quantity, int radius)
        {
            for (int i = quantity; i != 0; i--)
            {
                Coin c = Coin.CreateCoin((int)(x), (int)(y), value, (float)(((float)radius / 50) * Math.Cos((Math.PI * 2 * i) / ((float)quantity))), (float)(((float)radius / 50) * Math.Sin((Math.PI * 2 * i) / ((float)quantity))));
                c.Anim.Bmp = new Bitmap(GetType(), "Coin.bmp");
                c.Anim.Bmp.MakeTransparent(Color.Magenta);
                _activeAnimations.Add(c.Anim);
                _activeCoins.Add(c);
            }
        }
        private void SpawnProjectile(float x, float y, float speed, float xDestination, float yDestination, int power, MonsterType type)
        {
            Projectile p = Projectile.CreateProjectile(x, y, xDestination, yDestination, power, speed);

            switch (type)
            {
                case MonsterType.Bird:
                    p.Bmp = new Bitmap(GetType(), "IceShard.bmp");
                    p.Bmp.MakeTransparent(Color.Magenta);
                    break;
                case MonsterType.Bean:
                    p.Bmp = new Bitmap(GetType(), "IceArrow.bmp");
                    p.Bmp.MakeTransparent(Color.Magenta);
                    break;
                default:
                    break;
            }

            int l = (Math.Max(p.Bmp.Width, p.Bmp.Height)) * (142 / 100); //increase matrix size to allow space for rotation

            float angle = 0;
            if (p.XVelocity != 0) //fancy rotation math
            {
                if (p.YVelocity <= 0 && p.XVelocity > 0) //going up and right
                    angle = 180 * (float)(Math.Atan((p.XVelocity) / ((-1) * (p.YVelocity))) / Math.PI);
                if (p.YVelocity > 0 && p.XVelocity > 0) //going down and right
                    angle = 180 - (float)((180 * Math.Atan((p.XVelocity) / (p.YVelocity))) / Math.PI);
                if (p.YVelocity <= 0 && p.XVelocity < 0) //going up and left
                    angle = 360 - (float)((180 * Math.Atan((p.XVelocity) / (p.YVelocity))) / Math.PI);
                if (p.YVelocity > 0 && p.XVelocity < 0) //going down and left
                    angle = 180 + (float)(180 * (Math.Atan((p.XVelocity) / ((-1) * (p.YVelocity)))) / Math.PI);
            }

            Bitmap bmp2 = new Bitmap(l, l); //make a new bmp for the rotated image
            using (Graphics g = Graphics.FromImage(bmp2))
            {
                g.SmoothingMode = SmoothingMode.HighQuality; //not really sure what this does
                g.Clear(Color.Transparent); //sets the bmp to transparent
                Matrix b = new Matrix(); //makes a matrix
                b.Translate((p.Bmp.Width / 2) * -1,
                    (p.Bmp.Height / 2) * -1, MatrixOrder.Append); //translates the matrix so the center is in the top left corner
                b.Rotate((float)(angle), MatrixOrder.Append); //rotates clockwise based on the angle determined above ^
                b.Translate((bmp2.Width / 2), (bmp2.Height / 2), MatrixOrder.Append); //translates the center back to the center
                g.Transform = b; //rotates the bitmap based on the matrix
                g.DrawImage(p.Bmp, 0, 0); //draws the bitmap to bmp2

                p.Bmp = bmp2; //sets the temporary bitmap to the usable bitmap
            }

            _activeProjectiles.Add(p);
        }
        private void KillMonster(Monster monster)
        {
            switch (monster.Type)
            {
                case MonsterType.Bird:
                    SpawnCoins(monster.X, monster.Y, 1, 6, 80);                    
                    break;
                case MonsterType.Bean:
                    SpawnCoins(monster.X, monster.Y, 1, 15, 140);
                    break;
                default:
                    break;
            }
            Bitmap dyingBmp = new Bitmap(monster.Anim.FrameWidth, monster.Anim.Bmp.Height);
            using (Graphics gr = Graphics.FromImage(dyingBmp))
                gr.DrawImage(monster.Anim.Bmp, 0, 0, new Rectangle(monster.Anim.FrameWidth * (monster.Anim.CurrentFrame - 1), 0, monster.Anim.FrameWidth, monster.Anim.Bmp.Height), GraphicsUnit.Pixel);
            monster.DyingBmp = dyingBmp;

            _score += monster.ScoreBonus;

            Damage d = Damage.CreateDamage(500, Canvas.Height - 50, monster.ScoreBonus, false, true);
            _activeDamages.Add(d);


            monster.Y -= monster.Anim.Bmp.Height / 2;
            monster.X -= monster.Anim.FrameWidth / 2;
            _dyingMonsters.Add(monster);

            _totalKills++;
        }

        private void DoMenuAI()
        {
            #region StartButton
            if ((_canvasMove.X >= (_canvasMidX - 100)) && (_canvasMove.X <= (_canvasMidX + 100)) &&
                (_canvasMove.Y >= (Canvas.Height - 200)) && (_canvasMove.Y <= (Canvas.Height - 100))) //IF DA MOUSE IS OVER DA START BUTTON
                _StartButtonHovering = true;                                                        //THEN DA MOUSE IS OVER DA START BUTTON
            else
                _StartButtonHovering = false;                                                       //IF IT ISN'T THEN IT ISN'T
            if ((_canvasClickUp.X >= (_canvasMidX - 100)) && (_canvasClickUp.X <= (_canvasMidX + 100)) && //AND IF YOU CLICK ON THE START BUTTON
                (_canvasClickUp.Y >= (Canvas.Height - 200)) && (_canvasMove.Y <= (Canvas.Height - 100)))
                _startingNewGame = true;                                                                     //THEN UR STARTING A GAME
            #endregion //Start Button
            #region LevelSelectButtons
            _levelSelectButtonHovering = 0; //assume we're not hovering over any level-select buttons
            if ((_canvasMove.Y >= Canvas.Height - 70) && (_canvasMove.Y <= Canvas.Height - 25)) //are you within the y-range of the buttons?
            {
                for (int i = 1; i < 11; i++) //check each of the ten buttons in their respective x domains
                {
                    if ((_canvasMove.X >= (_canvasMidX - 475 + (80 * i))) && (_canvasMove.X <= (_canvasMidX - 425 + (80 * i))))
                    {
                        if (_levelsUnlocked >= i) //assuming we have it unlocked,
                            _levelSelectButtonHovering = i; //then we are now hovering over it
                        break;
                    }
                }
            }
            if ((_canvasClickUp.Y >= Canvas.Height - 70) && (_canvasClickUp.Y <= Canvas.Height - 25)) //same business but clicking
            {                                                                                         //instead of hovering
                for (int i = 1; i < 11; i++)
                {
                    if ((_canvasClickUp.X >= (_canvasMidX - 475 + (80 * i))) && (_canvasClickUp.X <= (_canvasMidX - 425 + (80 * i))))
                    {
                        if (_levelsUnlocked >= i)
                            _levelSelected = i;
                        break;
                    }
                }
            }

            #region upgrades
            if ((_canvasClickUp.Y >= 600) && (_canvasClickUp.Y <= 640))
            {
                for (int i = 1; i < 6; i++)
                {
                    if (_canvasClickUp.X >= 405 + (i * 100) && _canvasClickUp.X <= 405 + (i * 100) + _upgradeButtonGreenBmp.Width)
                    {
                        if (_money >= ((int)((Math.Pow(3, _upgrades[i - 1])) + 2) * 40)) //reduced fee
                        {
                            _money -= ((int)((Math.Pow(3, _upgrades[i - 1])) + 2) * 40); //reduced fee

                            _upgrades[i - 1]++;

                            _canvasClickUp.Y = -10;

                            break;
                        }
                    }
                }
            }
            #endregion //upgrades
            #endregion //Level Select Buttons
        }

        private void DoRender()
        {
            using (Graphics gr = Graphics.FromImage(Canvas.Image))
            {
                #region Menu
                if (_paused)
                {
                    gr.Clear(Color.DarkGray);
                    #region StartButton
                    if (!_StartButtonHovering)
                        gr.DrawImage(_startButtonBmp, _canvasMidX - 100, Canvas.Height - 200); //draws the non-hovering image if not hovering
                    else if (_StartButtonHovering)
                        gr.DrawImage(_startButtonHoveringBmp, _canvasMidX - 100, Canvas.Height - 200); //draws hovering image if hovering
                    #endregion //Start Button
                    #region LevelSelectButtons

                    gr.DrawString("Level:", _levelFont, _blackBrush, 50, Canvas.Height - 60);
                    for (int i = 1; i < 11; i++)
                    {
                        //draw each of the 10 level buttons, making sure to check if they are hovering or selected
                        if (i != _levelSelectButtonHovering)
                        {
                            if (i != _levelSelected)
                                gr.DrawImage(_levelSelectButtonBmp, _canvasMidX - 480 + (80 * i), Canvas.Height - 80);
                            else
                                gr.DrawImage(_levelSelectedButtonBmp, _canvasMidX - 480 + (80 * i), Canvas.Height - 80);
                            gr.DrawString(i.ToString(), _levelNumberFont, _blackBrush, _canvasMidX - 463 + (80 * i), Canvas.Height - 58);

                        }
                        else if (i == _levelSelectButtonHovering)
                        {
                            if (i != _levelSelected)
                                gr.DrawImage(_levelSelectButtonHoveringBmp, _canvasMidX - 480 + (80 * i), Canvas.Height - 80);
                            else
                                gr.DrawImage(_levelSelectedButtonHoveringBmp, _canvasMidX - 480 + (80 * i), Canvas.Height - 80);
                            gr.DrawString(i.ToString(), _levelNumberFont, _blackBrush, _canvasMidX - 458 + (80 * i), Canvas.Height - 63);

                        }
                        if (i > _levelsUnlocked)
                            gr.DrawImage(_lockBmp, _canvasMidX - 482 + (80 * i), Canvas.Height - 35);
                    }
                    #endregion //Level Select Buttons
                    #region Title
                    gr.DrawImage(_titleBmp, 205, 10); //draw the title image
                    #endregion //Title
                    #region High Scores
                    gr.DrawString("Level " + _levelSelected.ToString() + " high scores:", _statsFont, _whiteBrush, 20, 660);
                    gr.DrawLine(_whitePen, 15, 690, 270, 690);
                    gr.DrawString("Coins: " + _highScores[_levelSelected - 1][0].ToString(), _statsFont, _whiteBrush, 20, 710);
                    gr.DrawString("Kills: " + _highScores[_levelSelected - 1][1].ToString(), _statsFont, _whiteBrush, 20, 740);
                    gr.DrawString("Health Lost: " + _highScores[_levelSelected - 1][2].ToString(), _statsFont, _whiteBrush, 20, 770);
                    gr.DrawString("Time: " + (_highScoreTimes[_levelSelected - 1][0]).ToString("00") + ":" +
                                             (_highScoreTimes[_levelSelected - 1][1]).ToString("00"), _statsFont, _whiteBrush, 20, 800);
                    gr.DrawString("Score: " + _highScores[_levelSelected - 1][3].ToString(), _statsFont, _blackBrush, 20, 830);
                    #endregion //High Scores
                    #region bank
                    gr.DrawString("Coins: " + _money.ToString(), _levelNumberFont, _blackBrush, 20, 300);
                    #endregion //bank
                    #region shop
                    _hoveringUpgrade = 0;

                    if (_canvasMove.Y >= 600 && _canvasMove.Y <= 640)
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            if (_canvasMove.X >= 405 + (i * 100) && _canvasMove.X <= 405 + (i * 100) + _upgradeButtonGreenBmp.Width)
                            {
                                _hoveringUpgrade = i;
                                break;
                            }
                        }
                    }

                    gr.DrawString("Arsenal", _levelFont, _blackBrush, 750, 210);

                    //Recoil label
                    gr.DrawString("Recoil", _levelNumberFont, _blackBrush, 490, 265);
                    gr.DrawString("(m/s )", _unitsFont, _blackBrush, 505, 285);
                    gr.DrawString("2", _exponentFont, _blackBrush, 533, 286);
                    //Recoils
                    if (_hoveringUpgrade == 1)
                    {
                        _arsenalBrush = _forestBrush;
                        _upgrades[0]++;
                    }
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[0])) * (45f))).ToString(), _levelNumberFont, _arsenalBrush, 510, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[0])) * (6f))).ToString(), _levelNumberFont, _arsenalBrush, 510, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[0])) * (78f))).ToString(), _levelNumberFont, _arsenalBrush, 510, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[0])) * (60f))).ToString(), _levelNumberFont, _arsenalBrush, 510, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[0])) * (18f))).ToString(), _levelNumberFont, _arsenalBrush, 510, 500);
                    if (_hoveringUpgrade == 1)
                    {
                        _arsenalBrush = _whiteBrush;
                        _upgrades[0]--;
                    }
                    

                    //bullet speed label
                    gr.DrawString("Bullet", _levelNumberFont, _blackBrush, 590, 250);
                    gr.DrawString("Speed", _levelNumberFont, _blackBrush, 590, 265);
                    gr.DrawString("(m/s)", _unitsFont, _blackBrush, 605, 285);
                    //bullet speeds
                    if (_hoveringUpgrade == 2)
                    {
                        _arsenalBrush = _forestBrush;
                        _upgrades[1]++;
                    }
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[1])) * ((30f)))).ToString(), _levelNumberFont, _arsenalBrush, 610, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[1])) * ((50f)))).ToString(), _levelNumberFont, _arsenalBrush, 610, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[1])) * ((10f)))).ToString(), _levelNumberFont, _arsenalBrush, 610, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[1])) * ((30f)))).ToString(), _levelNumberFont, _arsenalBrush, 610, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[1])) * ((35f)))).ToString(), _levelNumberFont, _arsenalBrush, 610, 500);
                    if (_hoveringUpgrade == 2)
                    {
                        _arsenalBrush = _whiteBrush;
                        _upgrades[1]--;
                    }

                    //blast radius label
                    gr.DrawString("Blast", _levelNumberFont, _blackBrush, 690, 250);
                    gr.DrawString("Radius", _levelNumberFont, _blackBrush, 690, 265);
                    gr.DrawString("(m)", _unitsFont, _blackBrush, 708, 285);
                    //blast radii
                    if (_hoveringUpgrade == 3)
                    {
                        _arsenalBrush = _forestBrush;
                        _upgrades[2]++;
                    }
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * ((20f / 3f)))).ToString(), _levelNumberFont, _arsenalBrush, 710, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * ((10f)))).ToString(), _levelNumberFont, _arsenalBrush, 710, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * ((5f)))).ToString(), _levelNumberFont, _arsenalBrush, 710, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * ((6f)))).ToString(), _levelNumberFont, _arsenalBrush, 710, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * ((20f / 3f)))).ToString(), _levelNumberFont, _arsenalBrush, 710, 500);

                    //growth rates
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * (20f))).ToString(), _levelNumberFont, _arsenalBrush, 1035, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * (50f))).ToString(), _levelNumberFont, _arsenalBrush, 1035, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * (15f))).ToString(), _levelNumberFont, _arsenalBrush, 1035, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * (20f))).ToString(), _levelNumberFont, _arsenalBrush, 1035, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[2])) * (20f))).ToString(), _levelNumberFont, _arsenalBrush, 1035, 500);
                    if (_hoveringUpgrade == 3)
                    {
                        _arsenalBrush = _whiteBrush;
                        _upgrades[2]--;
                    }

                    //movement force label
                    gr.DrawString("Movement", _levelNumberFont, _blackBrush, 780, 250);
                    gr.DrawString("Force", _levelNumberFont, _blackBrush, 800, 265);
                    gr.DrawString("(N/m)", _unitsFont, _blackBrush, 812, 285);
                    //acceleration powers (overpressure)
                    if (_hoveringUpgrade == 4)
                    {
                        _arsenalBrush = _forestBrush;
                        _upgrades[3]++;
                    }
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[3])) * ((180f)))).ToString(), _levelNumberFont, _arsenalBrush, 810, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[3])) * ((90f)))).ToString(), _levelNumberFont, _arsenalBrush, 810, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[3])) * ((120f)))).ToString(), _levelNumberFont, _arsenalBrush, 810, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[3])) * ((120f)))).ToString(), _levelNumberFont, _arsenalBrush, 810, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[3])) * ((75f)))).ToString(), _levelNumberFont, _arsenalBrush, 810, 500);
                    if (_hoveringUpgrade == 4)
                    {
                        _arsenalBrush = _whiteBrush;
                        _upgrades[3]--;
                    }

                    //attack power label
                    gr.DrawString("Attack", _levelNumberFont, _blackBrush, 900, 250);
                    gr.DrawString("Damage", _levelNumberFont, _blackBrush, 900, 265);
                    gr.DrawString("(HP)", _unitsFont, _blackBrush, 925, 285);
                    //attack powers
                    if (_hoveringUpgrade == 5)
                    {
                        _arsenalBrush = _forestBrush;
                        _upgrades[4]++;
                    }
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[4])) * ((10f)))).ToString(), _levelNumberFont, _arsenalBrush, 920, 300);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[4])) * ((50f)))).ToString(), _levelNumberFont, _arsenalBrush, 920, 350);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[4])) * ((3f)))).ToString(), _levelNumberFont, _arsenalBrush, 920, 400);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[4])) * ((40f)))).ToString(), _levelNumberFont, _arsenalBrush, 920, 450);
                    gr.DrawString(((int)((Math.Pow(1.1f, _upgrades[4])) * ((60f)))).ToString(), _levelNumberFont, _arsenalBrush, 920, 500);
                    if (_hoveringUpgrade == 5)
                    {
                        _arsenalBrush = _whiteBrush;
                        _upgrades[4]--;
                    }

                    //growth rate label
                    gr.DrawString("Explosion", _levelNumberFont, _blackBrush, 1000, 250);
                    gr.DrawString("Growth", _levelNumberFont, _blackBrush, 1015, 265);
                    gr.DrawString("(m/s)", _unitsFont, _blackBrush, 1030, 285);


                    //images and names
                    gr.DrawImage(_pelletNormalBmp, 325, 300);
                    gr.DrawString("H-Bomb", _levelNumberFont, _whiteBrush, 375, 300);

                    gr.DrawImage(_pelletFlashBmp, 325, 350);
                    gr.DrawString("Flash", _levelNumberFont, _whiteBrush, 375, 350);

                    gr.DrawImage(_pelletGravitronBmp, 325, 400);
                    gr.DrawString("Gravitron", _levelNumberFont, _whiteBrush, 375, 400);

                    gr.DrawImage(_pelletMissileBmp, 325, 450);
                    gr.DrawString("Missile", _levelNumberFont, _whiteBrush, 375, 450);

                    gr.DrawImage(_pelletSkullBmp, 325, 500);
                    gr.DrawString("Boquet", _levelNumberFont, _whiteBrush, 375, 500);

                    //upgrade buttons
                    for (int i = 1; i < 6; i++)
                    {
                        if (_money >= ((int)((Math.Pow(3, _upgrades[i - 1])) + 2) * 40)) //reduced fee
                        {
                            if (_hoveringUpgrade != i)
                                gr.DrawImage(_upgradeButtonGreenBmp, 405 + (i * 100), 600);
                            else
                                gr.DrawImage(_upgradeButtonGreenHoveringBmp, 405 + (i * 100), 600);
                        }
                        else
                            gr.DrawImage(_upgradeButtonRedBmp, 405 + (i * 100), 600);
                        gr.DrawImage(_coinIcon, 390 + (i * 100), 647);
                        gr.DrawString(((int)((Math.Pow(3, _upgrades[i - 1])) + 2) * 40).ToString(), _unitsFont, _blackBrush, 405 + (i * 100), 645); //reduced fee
                    }


                    #endregion //shop
                }
                #endregion //menu
                #region gameplay
                else if (!_paused)
                {
                    #region Background
                    gr.Clear(Color.LightGray);
                    #endregion //background
                    #region wall
                    int startY = _wallAnchor;

                    while (startY + _wallBmp.Height > _screenAnchorY)
                    {
                        if (startY < _screenAnchorY + Canvas.Height)
                        {
                            gr.DrawImage(_wallBmp, 0, startY - _screenAnchorY);
                        }
                        startY -= _wallBmp.Height;
                    }
                    startY = _wallAnchor + (_wallRightBmp.Height / 2);
                    while (startY + _wallRightBmp.Height > _screenAnchorY)
                    {
                        if (startY < _screenAnchorY + Canvas.Height)
                        {
                            gr.DrawImage(_wallRightBmp, Canvas.Width - 10, startY - _screenAnchorY);
                        }
                        startY -= _wallRightBmp.Height;
                    }
                    #endregion //wall
                    #region water
                    if (_waterLevel - _waves.Height - 2 < Canvas.Height + _screenAnchorY)
                    {
                        gr.FillRectangle(_waterBrush, 0, _waterLevel - _screenAnchorY, Canvas.Width, Canvas.Height - _waterLevel + _screenAnchorY);
                        int x = 0 - (100 * _waveNumber);

                        while (x < Canvas.Width)
                        {
                            gr.DrawImage(_waves, x, _waterLevel - _waves.Height - _screenAnchorY);
                            x += 400;
                        }
                    }
                    else
                    {
                        gr.DrawImage(_downArrowBmp, _selectionWheelBmp.Width + 16, Canvas.Height - 30);
                        gr.DrawString(((int)((_waterLevel - _screenAnchorY - Canvas.Height) / 30)).ToString() + "m", _levelNumberFont, _waterBrush, _selectionWheelBmp.Width + 30, Canvas.Height - 30);
                    }
                    gr.DrawString((((int)((-1 * _screenAnchorY) / 30)).ToString()) + "m / " + ((int)((-1 * _heightGoal) / 30)).ToString() + "m", _levelNumberFont, _blackBrush, _selectionWheelBmp.Width + 30, Canvas.Height - 50);

                    #endregion
                    #region dying monsters
                    //create a color matrix and set the alpha value to 0.5
                    ColorMatrix cm4 = new ColorMatrix();
                    cm4.Matrix00 = cm4.Matrix11 = cm4.Matrix22 = cm4.Matrix44 = 1;
                    cm4.Matrix33 = (0.3f);

                    //create an image attributes object and set the color matrix to cm
                    ImageAttributes ia4 = new ImageAttributes();
                    ia4.SetColorMatrix(cm4);
                    foreach (Monster m in _dyingMonsters)
                    {
                        //cm4.Matrix33 = ((float)m.DyingCounter / 15);
                        //ia4.SetColorMatrix(cm4);

                        //gr.DrawImage(m.DyingBmp, new Rectangle((int)m.X, (int)m.Y - _screenAnchorY, m.DyingBmp.Width, m.DyingBmp.Height), 0, 0, m.DyingBmp.Width, m.DyingBmp.Height, GraphicsUnit.Pixel, ia4);
                        gr.DrawImage(m.DyingBmp, (int)m.X, (int)m.Y - _screenAnchorY);
                    }
                    #endregion //dying monsters
                    #region Pyro
                    if (!_ending)
                    {
                        gr.DrawImage(_pyroBmp, (float)(_pyroXs - (_pyroBmp.Width / 2)), (float)(_pyroYs - (_pyroBmp.Height / 2)) - _screenAnchorY); //draw pyro
                        gr.DrawImage(_pyroArmBmp, _pyroArmAnchorX, _pyroArmAnchorY - _screenAnchorY);


                        //test tool
                        //gr.DrawEllipse(_greenPen, (float)(_pyroXs - (_pyroBmp.Width / 2)), (float)(_pyroYs - (_pyroBmp.Height / 2)) - _screenAnchorY, _pyroBmp.Width, _pyroBmp.Height);
                    }
                    else
                    {
                        #region death
                        //create a color matrix and set the alpha value to 0.5
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1;
                        cm.Matrix33 = ((float)_endCounter / 500);

                        //create an image attributes object and set the color matrix to cm
                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm);

                        int y = 100;
                        if (_waterLevel - _screenAnchorY > 0)
                            y = _waterLevel - _screenAnchorY + 100;
                        if (_endCounter > 400)
                        {
                            gr.DrawImage(_drowning, new Rectangle((int)_canvasMidX - 367, y + ((500 - _endCounter) / 2), 735, _drowning.Height), 0, 0, 735, _drowning.Height, GraphicsUnit.Pixel, ia);
                            //gr.DrawImage(_drowning, _canvasMidX - 367, y + ((500 - _endCounter) / 2), new Rectangle(0, 0, 735, _drowning.Height), GraphicsUnit.Pixel);
                        }
                        else if (_endCounter > 300)
                        {
                            gr.DrawImage(_drowning, new Rectangle((int)_canvasMidX - 367, y + ((500 - _endCounter) / 2), 735, _drowning.Height), 735, 0, 735, _drowning.Height, GraphicsUnit.Pixel, ia);
                            //gr.DrawImage(_drowning, _canvasMidX - 367, y + ((500 - _endCounter) / 2), new Rectangle(736, 0, 735, _drowning.Height), GraphicsUnit.Pixel);
                        }
                        else if (_endCounter > 200)
                        {
                            gr.DrawImage(_drowning2, new Rectangle((int)_canvasMidX - 367, y + ((500 - _endCounter) / 2), 735, _drowning2.Height), 0, 0, 735, _drowning2.Height, GraphicsUnit.Pixel, ia);
                            //gr.DrawImage(_drowning2, _canvasMidX - 367, y + ((500 - _endCounter) / 2));
                        }
                        else
                        {
                            gr.DrawImage(_drowning3, new Rectangle((int)_canvasMidX - 367, y + ((500 - _endCounter) / 2), 735, _drowning3.Height), 0, 0, 735, _drowning3.Height, GraphicsUnit.Pixel, ia);
                            //gr.DrawImage(_drowning3, _canvasMidX - 367, y + ((500 - _endCounter) / 2));
                            gr.DrawImage(_drowningArm, new Rectangle((int)_canvasMidX - 150, y + ((500 - _endCounter) / 5), _drowningArm.Width, _drowningArm.Height), 0, 0, _drowningArm.Width, _drowningArm.Height, GraphicsUnit.Pixel, ia);
                            //gr.DrawImage(_drowningArm, _canvasMidX - 150, y + ((500 - _endCounter) / 5));
                        }
                        foreach (Bubble b in _activeBubbles)
                        {
                            gr.DrawImage(_bubble, b.X, b.Y);
                        }

                        if (_endCounter < 450)
                            gr.DrawString("GAME OVER", _gameOverFont, _blackBrush, _endCounter, 40);
                        #endregion //death
                    }

                    #endregion Pyro
                    #region Pellets
                    foreach (Pellet pellet in _activePellets)
                    {
                        if (pellet.Y < Canvas.Height + _screenAnchorY && pellet.Y + pellet.Bmp.Height > _screenAnchorY)
                            gr.DrawImage(pellet.Bmp, pellet.X - (pellet.Bmp.Width / 2), pellet.Y - (pellet.Bmp.Height / 2) - _screenAnchorY);
                    }
                    #endregion //Pellets
                    #region Explosions
                    foreach (Explosion e in _activeExplosions)
                    {
                        if (e.CurrentRadius < Canvas.Width)
                        {
                            Bitmap expBmp = new Bitmap(2 * (e.CurrentRadius + e.RadiusGrowth), 2 * (e.CurrentRadius + e.RadiusGrowth));

                            switch (e.Type)
                            {
                                case Pellet.ExplosionType.Normal:
                                    _expBrush = _orangeBrush;
                                    break;
                                case Pellet.ExplosionType.Flash:
                                    _expBrush = _whiteBrush;
                                    break;
                                case Pellet.ExplosionType.Gravitron:
                                    _expBrush = _purpleBrush;
                                    break;
                                case Pellet.ExplosionType.Missile:
                                    _expBrush = _missileBrush;
                                    break;
                                case Pellet.ExplosionType.Boquet:
                                    _expBrush = _skullBrush;
                                    break;
                                case Pellet.ExplosionType.Flower:
                                    _expBrush = _skullBrush;
                                    break;
                                default:
                                    break;
                            }

                            using (Graphics g = Graphics.FromImage(expBmp))
                            {
                                g.FillEllipse(_expBrush, 0, 0, 2 * (e.CurrentRadius + e.RadiusGrowth), 2 * (e.RadiusGrowth + e.CurrentRadius));
                                g.FillEllipse(_magentaBrush, e.RadiusGrowth, e.RadiusGrowth, 2 * e.CurrentRadius, 2 * e.CurrentRadius);
                            }

                            expBmp.MakeTransparent(Color.Magenta);

                            cm4.Matrix33 = (0.5f);
                            ia4.SetColorMatrix(cm4);

                            gr.DrawImage(expBmp, new Rectangle(e.Origin.X - e.CurrentRadius - e.RadiusGrowth, e.Origin.Y - e.CurrentRadius - _screenAnchorY - e.RadiusGrowth, 2 * (e.CurrentRadius + e.RadiusGrowth), 2 * (e.CurrentRadius + e.RadiusGrowth)),
                                                 0, 0, 2 * (e.CurrentRadius + e.RadiusGrowth), 2 * (e.RadiusGrowth + e.CurrentRadius), GraphicsUnit.Pixel, ia4);
                        }

                        //gr.DrawEllipse(_redPen, e.Origin.X - e.CurrentRadius, e.Origin.Y - e.CurrentRadius - _screenAnchorY, 2 * e.CurrentRadius, 2 * e.CurrentRadius);
                    }
                    #endregion //explosions
                    #region Animations
                    foreach (Animation a in _activeAnimations)
                    {
                        if (a.Y < Canvas.Height + _screenAnchorY && a.Y + a.Bmp.Height > _screenAnchorY) 
                            gr.DrawImage(a.Bmp, a.X, a.Y - _screenAnchorY, new Rectangle((a.CurrentFrame- 1) * a.FrameWidth, 0,
                                         a.FrameWidth, a.Bmp.Height), GraphicsUnit.Pixel);
                    }
                    #endregion //animations
                    #region projectiles
                    foreach (Projectile p in _activeProjectiles)
                    {
                        gr.DrawImage(p.Bmp, p.X - (p.Bmp.Width / 2), p.Y - (p.Bmp.Height / 2) - _screenAnchorY);
                    }
                    #endregion //projectiles
                    #region Monster HP Bars

                    foreach (Monster m in _activeMonsters)
                    {
                        if (m.Health < m.MaxHealth)
                        {
                            gr.FillRectangle(_blackBrush, m.X - 20, m.Y + (m.Anim.Bmp.Height) + 2 - _screenAnchorY, 40, 6);
                            gr.FillRectangle(_greenBrush, m.X - 19, m.Y + (m.Anim.Bmp.Height) + 3 - _screenAnchorY, (38 * m.Health) / m.MaxHealth, 4);
                        }
                    }

                    #endregion //Monster hp bars
                    #region Damage
                    foreach (Damage d in _activeDamages)
                    {
                        /*
                        //create a color matrix and set the alpha value to 0.5
                        //ColorMatrix cm4 = new ColorMatrix();
                        //cm4.Matrix00 = cm4.Matrix11 = cm4.Matrix22 = cm4.Matrix44 = 1;
                        cm4.Matrix33 = ((float)d.Timer / 20);

                        //create an image attributes object and set the color matrix to cm
                        //ImageAttributes ia4 = new ImageAttributes();
                        ia4.SetColorMatrix(cm4);

                        Bitmap dmgBmp = new Bitmap(500, 500);

                        using (Graphics g = Graphics.FromImage(dmgBmp))
                        {
                            if (!d.ForScore)
                            {
                                if (d.Friendly)
                                {
                                    g.DrawString(d.Amount.ToString(), _damageFont, _redBrush, 0, 0);
                                }
                                else
                                {
                                    g.DrawString(d.Amount.ToString(), _damageFont, _damageBrush, 0, 0);
                                }
                            }
                            else
                                g.DrawString("+" + d.Amount.ToString(), _damageFont, _greenBrush, 0, 0);
                        }

                        if (d.ForScore)
                            d.Y += _screenAnchorY;
                        gr.DrawImage(dmgBmp, new Rectangle(d.X, d.Y - _screenAnchorY, dmgBmp.Width, dmgBmp.Height), 0, 0, dmgBmp.Width, dmgBmp.Height, GraphicsUnit.Pixel, ia4);
                        if (d.ForScore)
                            d.Y -= _screenAnchorY;
                        */

                        if (!d.ForScore)
                        {
                            if (d.Friendly)
                            {
                                gr.DrawString(d.Amount.ToString(), _damageFont, _redBrush, d.X, d.Y - _screenAnchorY);
                            }
                            else
                                gr.DrawString(d.Amount.ToString(), _damageFont, _damageBrush, d.X, d.Y - _screenAnchorY);
                        }
                        else
                            gr.DrawString("+" + d.Amount.ToString(), _damageFont, _greenBrush, d.X, d.Y);
                    }
                    #endregion //Damage
                    #region Flash
                    if (_flashTimer > 0)
                    {
                        //cm4.Matrix33 = ((float)_flashTimer / 7);
                        //ia4.SetColorMatrix(cm4);

                        //gr.DrawImage(_flashBmp, new Rectangle(0, 0, Canvas.Width, Canvas.Height), 0, 0, Canvas.Width, Canvas.Height, GraphicsUnit.Pixel, ia4);

                        gr.DrawImage(_flashBmps[5 - _flashTimer], 0, 0);
                    }
                    #endregion //Flash
                    #region Charge Bar
                    //charge bar

                    gr.DrawImage(_chargeBarFilling, Canvas.Width - _chargeBarFilling.Width - 5, Canvas.Height - _chargeBarFilling.Height - 5
                                  + (((_chargeBarFilling.Height * (_maxCharge - _charge)) / _maxCharge)), 
                                  new Rectangle(0, (((_chargeBarFilling.Height * (_maxCharge - _charge)) / _maxCharge)), 
                                               _chargeBarFilling.Width, (_chargeBarFilling.Height * _charge) / _maxCharge), GraphicsUnit.Pixel);
                    if (_charge < _maxCharge)
                        gr.DrawImage(_chargeBarEmpty, Canvas.Width - _chargeBarEmpty.Width - 5, Canvas.Height - _chargeBarEmpty.Height - 5);
                    else
                        gr.DrawImage(_chargeBarFull, Canvas.Width - _chargeBarFull.Width - 5, Canvas.Height - _chargeBarFull.Height - 5);
                    #endregion //charge bar
                    #region Health Bar
                    gr.DrawImage(_healthBar, Canvas.Width - _chargeBarFilling.Width - 15 - _healthBar.Width, Canvas.Height - _healthBar.Height - 5,
                                 new Rectangle(0, 0, (_healthBar.Width * _pyroHealth) / _pyroHealthMax, _healthBar.Height), GraphicsUnit.Pixel);
                    gr.DrawImage(_healthBarEmpty, Canvas.Width - _chargeBarFilling.Width - 15 - _healthBar.Width, Canvas.Height - _healthBar.Height - 5);
                    #endregion //hp bar
                    #region Selection Wheel

                    for (int i = 0; i < _numberOfUnlockedExplosionTypes; i++)
                    {
                        float angle = _selectionWheelAngleCurrent - ((360 / _numberOfUnlockedExplosionTypes) * i) + 90;
                        switch (i)
                        {
                            case 0:
                                gr.DrawImage(_pelletNormalBmp, 80 + 45 * (float)Math.Cos((Math.PI * angle) / 180) - (_pelletNormalBmp.Width / 2), 887 - 45 * (float)Math.Sin((Math.PI * angle) / 180) - (_pelletNormalBmp.Height / 2));
                                break;
                            case 1:
                                gr.DrawImage(_pelletFlashBmp, 80 + 45 * (float)Math.Cos((Math.PI * angle) / 180) - (_pelletFlashBmp.Width / 2), 887 - 45 * (float)Math.Sin((Math.PI * angle) / 180) - (_pelletFlashBmp.Height / 2));
                                break;
                            case 2:
                                gr.DrawImage(_pelletGravitronBmp, 80 + 45 * (float)Math.Cos((Math.PI * angle) / 180) - (_pelletGravitronBmp.Width / 2), 887 - 45 * (float)Math.Sin((Math.PI * angle) / 180) - (_pelletGravitronBmp.Height / 2));
                                break;
                            case 3:
                                gr.DrawImage(_pelletMissileBmp, 80 + 45 * (float)Math.Cos((Math.PI * angle) / 180) - (_pelletGravitronBmp.Width / 2), 887 - 45 * (float)Math.Sin((Math.PI * angle) / 180) - (_pelletGravitronBmp.Height / 2));
                                break;
                            case 4:
                                gr.DrawImage(_pelletSkullBmp, 80 + 45 * (float)Math.Cos((Math.PI * angle) / 180) - (_pelletSkullBmp.Width / 2), 887 - 45 * (float)Math.Sin((Math.PI * angle) / 180) - (_pelletSkullBmp.Height / 2));
                                break;
                            default:
                                break;
                        }
                    }

                    //create a color matrix and set the alpha value to 0.5
                    //ColorMatrix cm3 = new ColorMatrix();
                    //cm3.Matrix00 = cm3.Matrix11 = cm3.Matrix22 = cm3.Matrix44 = 1;
                    cm4.Matrix33 = (0.3f);

                    //create an image attributes object and set the color matrix to cm
                    //ImageAttributes ia3 = new ImageAttributes();
                    ia4.SetColorMatrix(cm4);

                    gr.DrawImage(_selectionWheelBmp, new Rectangle(15, Canvas.Height - _selectionWheelBmp.Height - 10, _selectionWheelBmp.Width, _selectionWheelBmp.Height),
                                    0, 0, _selectionWheelBmp.Width, _selectionWheelBmp.Height, GraphicsUnit.Pixel, ia4);

                    gr.DrawImage(_selectionWheelOutlineBmp, 15, Canvas.Height - _selectionWheelOutlineBmp.Height - 10);

                    if (((_canvasMove.X - 80) * (_canvasMove.X - 80)) + ((_canvasMove.Y - 887) * (_canvasMove.Y - 887)) <= 3800)
                        gr.DrawImage(_selectionWheelHoverBmp, 15, Canvas.Height - _selectionWheelHoverBmp.Height - 10);

                    #endregion //selection wheel
                    #region Fast forward button
                    if (_ending)
                    {
                        if ((_canvasMove.X < Canvas.Width - _fastForwardBmp.Width - 95 || _canvasMove.X > Canvas.Width - 95) || _canvasMove.Y > 5 + _fastForwardBmp.Height)
                            gr.DrawImage(_fastForwardBmp, Canvas.Width - _fastForwardBmp.Width - 95, 5);
                        else
                            gr.DrawImage(_fastForwardingBmp, Canvas.Width - _fastForwardingBmp.Width - 95, 5);
                    }
                    #endregion //fast forward
                    #region score and money
                    gr.DrawString("Score: " + _score.ToString(), _levelNumberFont, _blackBrush, 500, Canvas.Height - 30);
                    gr.DrawString("Coins: " + _money.ToString(), _levelNumberFont, _blackBrush, 330, Canvas.Height - 30);
                    #endregion
                    #region Completion
                    if (_completing)
                    {
                        gr.DrawImage(_scoreBoardBmp, _canvasMidX - _scoreBoardBmp.Width / 2, _canvasMidY - _scoreBoardBmp.Height / 2);
                        gr.DrawString("Time: " + ((int)_totalTime.Minutes).ToString("00") + ":" + ((int)_totalTime.Seconds).ToString("00"), _statsFont, _whiteBrush, _canvasMidX - 180, _canvasMidY - 90);
                        gr.DrawString("+" + _timeBonus.ToString(), _statsFont, _greenBrush, _canvasMidX + 70, _canvasMidY - 90);
                        gr.DrawString("Kills: " + _killGained.ToString(), _statsFont, _whiteBrush, _canvasMidX - 180, _canvasMidY - 64);
                        gr.DrawString("+" + _killBonus.ToString(), _statsFont, _greenBrush, _canvasMidX + 70, _canvasMidY - 64);
                        gr.DrawString("Coins: " + _moneyGained.ToString(), _statsFont, _whiteBrush, _canvasMidX - 180, _canvasMidY - 38);
                        gr.DrawString("+" + _coinBonus.ToString(), _statsFont, _greenBrush, _canvasMidX + 70, _canvasMidY - 38);
                        gr.DrawString("Health Lost: " + _healthLost.ToString(), _statsFont, _whiteBrush, _canvasMidX - 180, _canvasMidY - 12);
                        gr.DrawString("+" + _healthBonus.ToString(), _statsFont, _greenBrush, _canvasMidX + 70, _canvasMidY - 12);
                        gr.DrawString("Score: " + _score.ToString(), _statsFont, _grayBrush, _canvasMidX - 180, _canvasMidY + 14);

                        if (_newHighScore)
                            gr.DrawString("New High Score!", _statsFont, _greenBrush, _canvasMidX - 105, 300);

                        cm4.Matrix33 = (200 - (float)_completeCounter) / 200;
                        ia4.SetColorMatrix(cm4);
                        gr.DrawImage(_levelCompleteBmp, new Rectangle((int)_canvasMidX - _levelCompleteBmp.Width / 2, 200, _levelCompleteBmp.Width, _levelCompleteBmp.Height),
                                     0, 0, _levelCompleteBmp.Width, _levelCompleteBmp.Height, GraphicsUnit.Pixel, ia4);

                        if (_canvasMove.X >= _canvasMidX - 70 && _canvasMove.X <= _canvasMidX + 70 && _canvasMove.Y >= _canvasMidY + 55 && _canvasMove.Y <= _canvasMidY + 85)
                            gr.FillRectangle(_orangeBrush, _canvasMidX - 70, _canvasMidY + 55, 140, 30);
                        else
                            gr.FillRectangle(_whiteBrush, _canvasMidX - 70, _canvasMidY + 55, 140, 30);
                        gr.FillRectangle(_blackBrush, _canvasMidX - 68, _canvasMidY + 57, 136, 26);
                        gr.DrawString("Proceed", _damageFont, _purpleBrush, _canvasMidX - 35, _canvasMidY + 60);
                    }
                    #endregion //Completion
                    #region performance
                    _currentTime = DateTime.Now.TimeOfDay;
                    //gr.DrawString(((int)(1000 * (_currentTime.TotalSeconds - _previousTime.TotalSeconds))).ToString(), _levelFont, _blackBrush, 100, 100);
                    _previousTime = _currentTime;
                    #endregion //performance
                }
                #endregion //gampeply

            }
            Canvas.Invalidate();
        }

        #region Input

        private void GotKeyDown(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyDown(e);
        }
        private void GotKeyUp(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyUp(e);
        }

        public bool ProcessKeyDown(KeyEventArgs e)
        {
            bool handled = true;

            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Right:
                    //_pyroXv += 3;
                    break;
                case System.Windows.Forms.Keys.Left:
                    //_pyroXv -= 3;
                    break;
                case System.Windows.Forms.Keys.Up:
                    //_pyroYv -= 3;
                    break;
                case System.Windows.Forms.Keys.Down:
                    //_pyroYv += 3;
                    break;
                default:
                    handled = true;
                    break;
            }
            return handled;
        }
        public bool ProcessKeyUp(KeyEventArgs e)
        {
            bool handled = true;

            switch (e.KeyCode)
            {
                default:
                    handled = true;
                    break;
            }
            return handled;
        }

        void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _canvasClickDown.X = e.X;
                _canvasClickDown.Y = e.Y;
                _canvasMove.X = e.X;
                _canvasMove.Y = e.Y;
                _clicking = true;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                switch (_activeExplosionType)
                {
                    case Pellet.ExplosionType.Normal:
                        _activeExplosionType = Pellet.ExplosionType.Flash;
                        break;
                    case Pellet.ExplosionType.Flash:
                        _activeExplosionType = Pellet.ExplosionType.Gravitron;
                        break;
                    case Pellet.ExplosionType.Gravitron:
                        _activeExplosionType = Pellet.ExplosionType.Missile;
                        break;
                    case Pellet.ExplosionType.Missile:
                        _activeExplosionType = Pellet.ExplosionType.Boquet;
                        break;
                    case Pellet.ExplosionType.Boquet:
                        _activeExplosionType = Pellet.ExplosionType.Normal;
                        break;
                    default:
                        break;
                }
            }
        }
        void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _canvasClickUp.X = e.X;
                _canvasClickUp.Y = e.Y;
                _clicking = false;

            }
        }
        void Canvas_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _canvasMove.X = e.X;
            _canvasMove.Y = e.Y;

        }

        void Game_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            SaveGame();
        }
        

        private void SaveGame()
        {
            _currentPlayer.HighScores = _highScores;
            _currentPlayer.HighScoreSet = _highScoreSet;
            _currentPlayer.HighScoreTimes = _highScoreTimes;
            _currentPlayer.Upgrades = _upgrades;
            _currentPlayer.Money = _money;

            string fileName = GetSaveName();

            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                using (TextWriter writer = new StreamWriter(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
                    serializer.Serialize(writer, _gameSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail: " + ex.Message, "Unable to save", MessageBoxButtons.OK);
            }
        }
        private void LoadGame()
        {
            string fileName = GetSaveName();
            bool createNew = true;

            InitializeHighScores();

            if (File.Exists(fileName))
            {
                try
                {
                    using (TextReader rdr = new StreamReader(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
                        _gameSave = (GameSave)serializer.Deserialize(rdr);
                        createNew = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fail: " + ex.Message, "Unable to load", MessageBoxButtons.OK);
                }
            }
            if (createNew)
            {
                InitializeHighScores();
                SaveGame();
            }
            _currentPlayer = _gameSave.Player;

            _highScores = _gameSave.Player.HighScores;
            _highScoreSet = _gameSave.Player.HighScoreSet;
            _highScoreTimes = _gameSave.Player.HighScoreTimes;
            _upgrades = _gameSave.Player.Upgrades;
            _money = _gameSave.Player.Money;

            /*_highScoreTimes = new int[][] { new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},
                                            new int[] {0, 0},};
            */
            //_upgrades = new int[] { 0, 0, 0, 0, 0 };
        }
        private string GetSaveName()
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                              "PyroSave.xml");
        }

        #endregion //input
    }
}
