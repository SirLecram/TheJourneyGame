using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TheJourneyGame.Model;

namespace TheJourneyGame
{
    class GameController : INotifyPropertyChanged
    {
        public List<Weapon> WeaponsInRoom { get; private set; }
        private static Player _player { get; set; }
        public static Point PlayerLocation { get => _player.Location; }
        public int PlayerHitPoints { get => _player.HitPoints; }
        private Canvas _playArea { get; }
        private List<Enemy> _enemiesList { get; set; }
        public IEnumerable<Enemy> EnemiesList { get => _enemiesList; }
        //public IEnumerable<Equipment> EquipmentList { get => _player.EquipmentList; }
        public Point playerLocation { get => location(); }
        private DispatcherTimer playerAttackTimer = new DispatcherTimer();
        private Dictionary<EquipmentType, Image> _equipmentImageDictionary = new Dictionary<EquipmentType, Image>();
        private Grid _equipmentGrid { get; }


        public double PlayerXPosition { get => _player.Location.X; }
        public double PlayerYPosition { get => _player.Location.Y; }

        public GameController(Canvas playArea, Grid eqGrid)
        {
            _playArea = playArea;
            _equipmentGrid = eqGrid;
            InitializeGameAndTimers();
            InitializePlayerPositionBinding();

            Weapon wp = new Sword(new Point(250, 200), "Miecz", @"\image\weapons\sword100x100.png", 10);
            WeaponsInRoom.Add(wp);
            _playArea.Children.Add(wp.WeaponAppearance);
            Canvas.SetLeft(wp.WeaponAppearance, wp.Location.X);
            Canvas.SetBottom(wp.WeaponAppearance, wp.Location.Y);
            Weapon wp2 = new Bow(new Point(400, 200), "Łuk", @"\image\weapons\Bow.png", 10);
            WeaponsInRoom.Add(wp2);
            _playArea.Children.Add(wp2.WeaponAppearance);
            Canvas.SetLeft(wp2.WeaponAppearance, wp2.Location.X);
            Canvas.SetBottom(wp2.WeaponAppearance, wp2.Location.Y);
            Weapon wp3 = new Mace(new Point(400, 300), "Buława", @"\image\weapons\Mace.png", 10);
            WeaponsInRoom.Add(wp3);
            _playArea.Children.Add(wp3.WeaponAppearance);
            Canvas.SetLeft(wp3.WeaponAppearance, wp3.Location.X);
            Canvas.SetBottom(wp3.WeaponAppearance, wp3.Location.Y);
            InitializeEquipmentImages();

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

        private void InitializeGameAndTimers()
        {
            WeaponsInRoom = new List<Weapon>();
            InitializeEnemies();
             /*_playArea.Children.Add(_enemiesList[0].EnemyAppearance);
             _playArea.Children.Add(_enemiesList[1].EnemyAppearance);
             Canvas.SetLeft(_enemiesList[0].EnemyAppearance, _enemiesList[0].Location.X);
             Canvas.SetBottom(_enemiesList[0].EnemyAppearance, _enemiesList[0].Location.Y);
             Canvas.SetLeft(_enemiesList[1].EnemyAppearance, _enemiesList[1].Location.X);
             Canvas.SetBottom(_enemiesList[1].EnemyAppearance, _enemiesList[1].Location.Y);*/
            playerAttackTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(2000).Ticks);
            playerAttackTimer.Tick += PlayerAttackTimer_Tick;
            playerAttackTimer.Start();
            _player = new Player(new Point(250, 20), 10);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);

        }
        private void InitializeEnemies()
        {
            _enemiesList = new List<Enemy>();
            _enemiesList.Add(new Bat(new Point(140, 140), _playArea, 20, 3));
            _enemiesList.Add(new Bat(new Point(300, 150), _playArea, 30, 3));
            foreach(Enemy enemy in _enemiesList)
            {
                _playArea.Children.Add(enemy.EnemyStackPanel);
                Canvas.SetLeft(enemy.EnemyStackPanel, enemy.Location.X);
                Canvas.SetBottom(enemy.EnemyStackPanel, enemy.Location.Y);
            }
        }

        public void InitializeEquipmentImages()
        {
            List<string> imageSource = new List<string>()
            {
                @"\image\weapons\sword100x100.png",
                @"\image\weapons\Bow.png",
                @"\image\weapons\Mace.png",
                @"\image\\weapons\Bow.png",
                @"\image\weapons\Bow.png",
            };
            for(int i = 0; i<5; i++)
            {
                Image newImage = new Image();
                newImage.Source = new BitmapImage(new Uri(imageSource[i], UriKind.Relative));
                _equipmentImageDictionary.Add((EquipmentType)i, newImage);
                _equipmentGrid.Children.Add(newImage);
                Grid.SetColumn(newImage, i);
                newImage.Visibility = Visibility.Hidden;
                newImage.MouseDown += NewImage_MouseDown;
                newImage.Cursor = Cursors.Hand;
                if(i<WeaponsInRoom.Count)
                {
                    newImage.ToolTip = WeaponsInRoom.
                    Find(weapon => weapon.EqType == (EquipmentType)i).WeaponAppearance.ToolTip;
                }
                
            }

        }

        private void NewImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image imgToFind = (Image)sender;
            
            int indexOfKey = _equipmentImageDictionary.Values.ToList().IndexOf(imgToFind);
            EquipmentType selectedEqType = _equipmentImageDictionary.Keys.ToList()[indexOfKey];
            _player.SelectToUse(selectedEqType);

        }

        private void PlayerAttackTimer_Tick(object sender, EventArgs e)
        {
            foreach (Enemy enemy in EnemiesList)
            {
                enemy.Attack(_player);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        #endregion

        #region Game mechanics
        /// <summary>
        /// A method which is called by event when player move to selected direction
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Direction direction)
        {
            _player.Move(direction, _playArea);
            OnPropertyChanged("playerLocation");
            OnPropertyChanged("PlayerXPosition");
            OnPropertyChanged("PlayerYPosition");
            PickUpEquipment();

        }
        /// <summary>
        /// A method which is called by event when player is trying to attack enemy
        /// </summary>
        public void AttackEnemy()
        {
            foreach (Enemy enemy in _enemiesList)
            {
                _player.Attack(enemy);
            }
            _enemiesList.RemoveAll(enemy => enemy.HitPoints <= 0);

            OnPropertyChanged("EnemiesList");

        }
        /// <summary>
        /// When a player stands on the Equipment, the item will be added to the player equipment.
        /// </summary>
        public void PickUpEquipment()
        {
            bool isLocationTheSame = false;
            foreach (Weapon weapon in WeaponsInRoom)
            {
                if (playerLocation == weapon.Location)
                {
                    isLocationTheSame = true;
                    _player.Equip(weapon);
                    _playArea.Children.Remove(weapon.WeaponAppearance);
                    _equipmentImageDictionary[weapon.EqType].Visibility = Visibility.Visible;
                }
            }
            if(isLocationTheSame)
                WeaponsInRoom.Remove((Weapon)_player.EquipmentList.ToList().Last());
            
        }
        #endregion


    }
}
