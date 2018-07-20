using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TheJourneyGame.Model
{
    [Serializable]
    abstract class Enemy : Position, IFightable, INotifyPropertyChanged
    {
        protected int _attackRange { get; set; }
        protected int _maxAttackPower { get; set; }
        public int ExpToGain { get;}
        public int HitPoints { get; protected set; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }
        public static DispatcherTimer Timer = new DispatcherTimer();
        protected Canvas _playArea { get; }
        protected Point _playerPosition { get => GameController.PlayerLocation; }
        protected Image _enemyAppearance { get; private set; }
        public StackPanel EnemyStackPanel { get; protected set; }
        private int calculationInterval = 0;
        private Direction playerDirection = Direction.Right;
        
        public Enemy(Point point, int moveInterval, long movingTimeSpan, Canvas playArea, int hp, 
            string imagePath, int maxAttackPower, int expToGain) 
            : base(point, moveInterval)
        {
            HitPoints = hp;
            _enemyAppearance = new Image();
            _enemyAppearance.Height = _enemyAppearance.Width = 25;
            _enemyAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            _playArea = playArea;
            _maxAttackPower = maxAttackPower;
            ExpToGain = expToGain;
            InitializeEnemy(movingTimeSpan);
        }

        #region Initialization and help methods
        private void InitializeEnemy(long movingTimeSpan)
        {
            ProgressBar enemyHpBar = new ProgressBar();
            enemyHpBar.Maximum = HitPoints;
            EnemyStackPanel = new StackPanel();
            EnemyStackPanel.Children.Add(enemyHpBar);
            EnemyStackPanel.Children.Add(_enemyAppearance);
            Binding bindingX = new Binding();
            bindingX.Source = this;
            bindingX.Path = new PropertyPath("HitPoints");
            bindingX.Mode = BindingMode.OneWay;
            enemyHpBar.SetBinding(ProgressBar.ValueProperty, bindingX);
            Timer.Interval = new TimeSpan(movingTimeSpan);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (calculationInterval == 0)
            {
                playerDirection = CalculatePlayerDirection(GameController.PlayerLocation);
            }
            calculationInterval++;
            if (calculationInterval >= 3)
                calculationInterval = 0;
            Move(playerDirection, _playArea);
           // Move((Direction)random.Next(23, 27), _playArea);
        }
        public Direction CalculatePlayerDirection(Point playerLocation)
        {
            Direction playerDirection;
            double x = Location.X - playerLocation.X;
            double y = Location.Y - playerLocation.Y;
            double c = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double angle45st = 0.7071D;
            double angle = (Math.Sin(Math.Abs(x) / Math.Abs(y)));
            if (x<=0 && y<=0)
            {
                //Quarter nr I
                if (angle < angle45st)
                    playerDirection = Direction.Up;
                else
                    playerDirection = Direction.Right;
            }
            else if(x>=0 && y<=0)
            {
                //Quarter nr II
                if (angle < angle45st)
                    playerDirection = Direction.Up;
                else
                    playerDirection = Direction.Left;

            }
            else if(x>=0 && y>=0)
            {
                //Quarter nr III
                if (angle < angle45st)
                    playerDirection = Direction.Down;
                else
                    playerDirection = Direction.Left;
            }
            else
            {
                //Quarter nr IV
                if (angle < angle45st)
                    playerDirection = Direction.Down;
                else
                    playerDirection = Direction.Right;
            }
            return playerDirection;
        }
        #endregion

        #region Enemy mechanics
        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            Canvas.SetLeft(EnemyStackPanel, Location.X);
            Canvas.SetBottom(EnemyStackPanel, Location.Y);

            // DODAC:
            // ########### WARUNKI ATAKU ########
            // ########### NIE WCHODZENIE NA GRACZa ########
        }
        /// <summary>
        /// Returns true if enemy is dead after taking a hit.
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public virtual bool TakeAHit(int hp)
        {
            HitPoints -= hp;
            OnPropertyChanged("HitPoints");
            if (IsDead)
            {
                //_playArea.Children.Remove(this.EnemyAppearance);
                _playArea.Children.Remove(EnemyStackPanel);
                return true;
            }
            
            return false;
        }

        public abstract void Attack(IFightable atackDestination);
        #endregion
    }
}
