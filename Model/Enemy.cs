using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TheJourneyGame.Model
{
    abstract class Enemy : Position, IFightable
    {
        protected int _attackRange { get; set; }
        public int HitPoints { get; protected set; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }
        public DispatcherTimer timer = new DispatcherTimer();
        protected Canvas _playArea { get; }
        protected Point _playerPosition { get => GameController.PlayerLocation; }
        public Image EnemyAppearance { get; private set; }
        
        public Enemy(Point point, int moveInterval, long movingTimeSpan, Canvas playArea, int hp, 
            string imagePath) 
            : base(point, moveInterval)
        {
            HitPoints = hp;
            EnemyAppearance = new Image();
            EnemyAppearance.Height = EnemyAppearance.Width = 25;
            EnemyAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            _playArea = playArea;
            timer.Interval = new TimeSpan(movingTimeSpan);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            Move((Direction)random.Next(23, 27), _playArea);
        }

        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            Canvas.SetLeft(EnemyAppearance, Location.X);
            Canvas.SetBottom(EnemyAppearance, Location.Y);
            
            
            // DODAC:
            // ########### WARUNKI ATAKU ########
            // ########### NIE WCHODZENIE NA GRACZa ########
        }
        public virtual bool TakeAHit(int hp)
        {
            HitPoints -= hp;
            
            if (IsDead)
            {
                _playArea.Children.Remove(this.EnemyAppearance);
                return true;
            }
            return false;
        }

        public abstract void Attack(IFightable atackDestination);
    }
}
