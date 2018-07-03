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
        private Player _player { get; set; }
        public Point PlayerLocation { get => _player.Location; }
        public int PlayerHitPoints { get => _player.HitPoints; }
        private Canvas _playArea { get; }


        public double PlayerXPosition { get => _player.Location.X; }
        public double PlayerYPosition { get => _player.Location.Y; }

        public GameController(Canvas playArea)
        {
            _playArea = playArea;
            /*Weapon wp = new Weapon(new Point(18, 200), "Miecz", @"\image\weapon.png");
            _playArea.Children.Add(wp.WeaponAppearance);
            Canvas.SetLeft(wp.WeaponAppearance, wp.Location.X);
            Canvas.SetBottom(wp.WeaponAppearance, wp.Location.Y);*/
            _player = new Player(new Point(250, 20), 10);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);
            InitializePlayerPositionBinding();
            
            
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
            OnPropertyChanged("PlayerLocation");
            OnPropertyChanged("PlayerXPosition");
            OnPropertyChanged("PlayerYPosition");
        }

        
        

    }
}
