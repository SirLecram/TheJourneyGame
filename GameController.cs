using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TheJourneyGame.Model;

namespace TheJourneyGame
{
    class GameController : INotifyPropertyChanged
    {
        public  IEnumerable<Weapon> WeaponsInRoom { get; private set; }
        private static Player _player { get; set; }
        public static Point PlayerLocation { get => _player.Location; }
        public int PlayerHitPoints { get => _player.HitPoints; }
        private Canvas _playArea { get; }
        private List<Enemy> _enemiesList { get; set; }
        public IEnumerable<Enemy> EnemiesList { get => _enemiesList; }
        public Point playerLocation { get => location(); }


        public double PlayerXPosition { get => _player.Location.X; }
        public double PlayerYPosition { get => _player.Location.Y; }

        public GameController(Canvas playArea)
        {
            _playArea = playArea;
            _enemiesList = new List<Enemy>();
             Weapon wp = new Sword(new Point(250, 200), "Miecz", @"\image\sword30x30.png", 10);
             _playArea.Children.Add(wp.WeaponAppearance);
             Canvas.SetLeft(wp.WeaponAppearance, wp.Location.X);
             Canvas.SetBottom(wp.WeaponAppearance, wp.Location.Y);
            _enemiesList.Add(new Bat(new Point(140, 140), playArea, 20));
            _enemiesList.Add(new Bat(new Point(300, 150), playArea, 30));
            _playArea.Children.Add(_enemiesList[0].EnemyAppearance);
            _playArea.Children.Add(_enemiesList[1].EnemyAppearance);
            Canvas.SetLeft(_enemiesList[0].EnemyAppearance, _enemiesList[0].Location.X);
            Canvas.SetBottom(_enemiesList[0].EnemyAppearance, _enemiesList[0].Location.Y);
            Canvas.SetLeft(_enemiesList[1].EnemyAppearance, _enemiesList[1].Location.X);
            Canvas.SetBottom(_enemiesList[1].EnemyAppearance, _enemiesList[1].Location.Y);
            _player = new Player(new Point(250, 20), 10);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);
            InitializePlayerPositionBinding();
            
            
        }
        public Point location()
        {
            return _player.Location;
        }
        #region Initialization & help methods
        private void InitializePlayerPositionBinding()
        {
            Binding bindingX = new Binding();
            Binding bindingY = new Binding();
            bindingX.Source = this;
            bindingY.Source = this;
            bindingX.Path = new PropertyPath("PlayerXPosition");
            bindingY.Path = new PropertyPath("PlayerYPosition");
            _player.PlayersAppearance.SetBinding(Canvas.LeftProperty, bindingX);
            _player.PlayersAppearance.SetBinding(Canvas.BottomProperty, bindingY);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void Move(Direction direction)
        {
            _player.Move(direction, _playArea);
            OnPropertyChanged("playerLocation");
            OnPropertyChanged("PlayerXPosition");
            OnPropertyChanged("PlayerYPosition");
        }
        public void AttackEnemy()
        {
            
            _player.Attack(_enemiesList[0]);
            _player.Attack(_enemiesList[1]);
        }
        
        

    }
}
