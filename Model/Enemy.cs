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
    abstract class Enemy : Position, IFightable, INotifyPropertyChanged
    {
        protected int _attackRange { get; set; }
        protected int _maxAttackPower { get; set; }
        public int HitPoints { get; protected set; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }
        public DispatcherTimer timer = new DispatcherTimer();
        protected Canvas _playArea { get; }
        protected Point _playerPosition { get => GameController.PlayerLocation; }
        protected Image _enemyAppearance { get; private set; }
        public StackPanel EnemyStackPanel { get; protected set; }
        
        public Enemy(Point point, int moveInterval, long movingTimeSpan, Canvas playArea, int hp, 
            string imagePath, int maxAttackPower) 
            : base(point, moveInterval)
        {
            HitPoints = hp;
            _enemyAppearance = new Image();
            _enemyAppearance.Height = _enemyAppearance.Width = 25;
            _enemyAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            _playArea = playArea;
            _maxAttackPower = maxAttackPower;
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
            timer.Interval = new TimeSpan(movingTimeSpan);
            timer.Tick += Timer_Tick;
            timer.Start();
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
            Move((Direction)random.Next(23, 27), _playArea);
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
