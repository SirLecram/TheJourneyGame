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
        public List<Equipment> EquipmentInRoom { get; private set; }
        public static IReadOnlyDictionary<EquipmentType, string> EqImagePathDictionary
        {
            get => _equipmentImagePathDictionary;
        }
        private static Player _player { get; set; }
        public static Point PlayerLocation { get => _player.Location; }
        public int PlayerHitPoints { get => _player.HitPoints; }
        private Canvas _playArea { get; }
        private List<Enemy> _enemiesList { get; set; }
        private int _level { get; set; }
        public Point playerLocation { get => location(); }
        private DispatcherTimer playerAttackTimer = new DispatcherTimer();
        private DispatcherTimer levelTimer = new DispatcherTimer();
        private Dictionary<EquipmentType, Image> _equipmentImageDictionary = 
            new Dictionary<EquipmentType, Image>();
        private static Dictionary<EquipmentType, string> _equipmentImagePathDictionary { get; set; }
        private Grid _equipmentGrid { get; }
        private int _levelBatAmount;
        private List<EquipmentType> _levelWeapons;
        private int _levelPotionChancePer100;
        private TimeSpan _levelTimeIntervalStep;
        public Random random = new Random();


        public double PlayerXPosition { get => _player.Location.X; }
        public double PlayerYPosition { get => _player.Location.Y; }

        public GameController(Canvas playArea, Grid eqGrid)
        {
            _playArea = playArea;
            _equipmentGrid = eqGrid;
            _level = 1;
            InitializeGameAndTimers();
            InitializeEquipmentImages();
            InitializeLevel(_level);


        }
        public Point location()
        {
            return _player.Location;
        }
        #region Initialization
        private void InitializePlayerPositionBinding()
        {
            _player = new Player(new Point(250, 20), 50);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y); 
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
            EquipmentInRoom = new List<Equipment>();
            _equipmentImagePathDictionary = new Dictionary<EquipmentType, string>();
            playerAttackTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(2000).Ticks);
            playerAttackTimer.Tick += PlayerAttackTimer_Tick;
            levelTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(3000).Ticks);
            levelTimer.Tick += LevelTimer_Tick;
           // playerAttackTimer.Start();
           /* _player = new Player(new Point(250, 20), 50);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);*/

        }
        private void InitializeLevel(int levelNumber)
        {
            switch(levelNumber)
            {
                case 1:
                    InitializePlayerPositionBinding();
                    SpawnEquipment(EquipmentType.BluePotion, new Point(700, 200));
                    SpawnEquipment(EquipmentType.Sword, new Point(100, 50));
                    InitializeEnemies();
                    _levelBatAmount = 10;
                    _levelPotionChancePer100 = 15;
                    _levelTimeIntervalStep = new TimeSpan(TimeSpan.FromMilliseconds(20).Ticks);
                    playerAttackTimer.Start();
                    levelTimer.Start();
                    break;
            }
        }

        private void LevelTimer_Tick(object sender, EventArgs e)
        {
            if(_levelBatAmount > 0)
            {
                Bat enemyToAdd = new Bat(GetRandomLocation(), _playArea, 30, 4);
                _enemiesList.Add(enemyToAdd);
                _playArea.Children.Add(enemyToAdd.EnemyStackPanel);
                Canvas.SetLeft(enemyToAdd.EnemyStackPanel, enemyToAdd.Location.X);
                Canvas.SetBottom(enemyToAdd.EnemyStackPanel, enemyToAdd.Location.Y);
                _levelBatAmount--;
            }
            else
            {
                levelTimer.Stop();
            }
            if(!CheckThatPlayerHaveEquipment(EquipmentType.BluePotion))
            {
                int potionSpawn = random.Next(100);
                if (potionSpawn <= 5)
                    SpawnEquipment(EquipmentType.BluePotion);
            }
            levelTimer.Interval -= _levelTimeIntervalStep;
            if (levelTimer.Interval <= _levelTimeIntervalStep)
            {
                levelTimer.Stop();
                MessageBox.Show("Level Timer Stop");
            }
                

        }

        private void InitializeEnemies()
        {
            _enemiesList = new List<Enemy>();
            _enemiesList.Add(new Bat(new Point(140, 140), _playArea, 20, 3));
            _enemiesList.Add(new Bat(new Point(300, 150), _playArea, 30, 3));
            _enemiesList.Add(new Bat(new Point(150, 140), _playArea, 20, 3));
            _enemiesList.Add(new Bat(new Point(320, 150), _playArea, 30, 3));
            _enemiesList.Add(new Bat(new Point(200, 140), _playArea, 20, 3));
           /* _enemiesList.Add(new Bat(new Point(220, 150), _playArea, 30, 3));
            _enemiesList.Add(new Bat(new Point(352, 140), _playArea, 20, 3));
            _enemiesList.Add(new Bat(new Point(100, 150), _playArea, 30, 3));*/

            foreach (Enemy enemy in _enemiesList)
            {
                _playArea.Children.Add(enemy.EnemyStackPanel);
                Canvas.SetLeft(enemy.EnemyStackPanel, enemy.Location.X);
                Canvas.SetBottom(enemy.EnemyStackPanel, enemy.Location.Y);
            }
        }

        public void InitializeEquipmentImages()
        {
            _equipmentImagePathDictionary = new Dictionary<EquipmentType, string>();
            _equipmentImagePathDictionary.Add(EquipmentType.Sword, @"\image\weapons\sword100x100.png");
            _equipmentImagePathDictionary.Add(EquipmentType.Bow, @"\image\weapons\Bow.png");
            _equipmentImagePathDictionary.Add(EquipmentType.Mace, @"\image\weapons\Mace.png");
            _equipmentImagePathDictionary.Add(EquipmentType.BluePotion, @"\image\BluePotion.png");
            _equipmentImagePathDictionary.Add(EquipmentType.RedPotion, @"\image\RedPotion.png");

            for(int i = 0; i<5; i++)
            {
                string imagePath = _equipmentImagePathDictionary[(EquipmentType)i];
                Image newImage = new Image();
                newImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                _equipmentImageDictionary.Add((EquipmentType)i, newImage);
                _equipmentGrid.Children.Add(newImage);
                Grid.SetColumn(newImage, i);
                newImage.Visibility = Visibility.Hidden;
                newImage.Cursor = Cursors.Hand;
                if (i < 3)
                {
                    newImage.MouseDown += NewImage_MouseDown;
                }
            }
        }
        private void AddToolTipsToGui(Equipment equipmentCollected)
        {
            if(equipmentCollected is Weapon)
                _equipmentImageDictionary[equipmentCollected.EqType].ToolTip =
                (equipmentCollected as Weapon).WeaponAppearance.ToolTip;
            else if(equipmentCollected is Equipment)
                _equipmentImageDictionary[equipmentCollected.EqType].ToolTip =
                (equipmentCollected as Potion).EquipmentAppearance.ToolTip;
        }
        #endregion

        #region Events
        private void NewImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image imgToFind = (Image)sender;
            
            int indexOfKey = _equipmentImageDictionary.Values.ToList().IndexOf(imgToFind);
            EquipmentType selectedEqType = _equipmentImageDictionary.Keys.ToList()[indexOfKey];
            _player.SelectToUse(selectedEqType);
            SpawnEquipment(EquipmentType.RedPotion);

        }

        private void PlayerAttackTimer_Tick(object sender, EventArgs e)
        {
            foreach (Enemy enemy in _enemiesList)
            {
                enemy.Attack(_player);
            }
            OnPropertyChanged("PlayerHitPoints");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Help Methods
        private Point GetRandomLocation()
        {
            double x = random.Next((int)_playArea.ActualWidth - 10);
            double y = random.Next((int)_playArea.ActualHeight - 10);
            return new Point(x, y);
        }
        /// <summary>
        /// Spawns new Equipment on Game Area. Beware! You can't spawn equipment if it 
        /// already exist in the player inventory.
        /// </summary>
        /// <param name="eqType"></param>
        /// <param name="location"></param>
        private void SpawnEquipment(EquipmentType eqType, Point location)
        {
            Equipment newItem = null;
            if (!CheckThatPlayerHaveEquipment(eqType))
            {
                switch (eqType)
                {
                    case EquipmentType.Sword:
                        newItem = new Sword(location, "Miecz", 10);
                        break;
                    case EquipmentType.Bow:
                        newItem = new Bow(location, "Łuk", 5);
                        break;
                    case EquipmentType.Mace:
                        newItem = new Mace(location, "Buława", 12);
                        break;
                    case EquipmentType.BluePotion:
                        newItem = new Potion(location, "Niebieska mikstura", 12, eqType);
                        break;
                    case EquipmentType.RedPotion:
                        newItem = new Potion(location, "Czerwona mikstura", 20, eqType);
                        break;
                }

                if (newItem is Weapon)
                {
                    Weapon weapon = newItem as Weapon;
                    WeaponsInRoom.Add(weapon);
                    _playArea.Children.Add(weapon.WeaponAppearance);
                    Canvas.SetLeft(weapon.WeaponAppearance, weapon.Location.X);
                    Canvas.SetBottom(weapon.WeaponAppearance, weapon.Location.Y);
                }
                else
                {
                    Potion potion = newItem as Potion;
                    EquipmentInRoom.Add(newItem);
                    _playArea.Children.Add(potion.EquipmentAppearance);
                    Canvas.SetLeft(potion.EquipmentAppearance, potion.Location.X);
                    Canvas.SetBottom(potion.EquipmentAppearance, potion.Location.Y);
                }
            }
            else
                throw new Exception("Nie można utowrzyć przedmiotu, ponieważ znajduje się on już w Eq gracza.");
        }
        /// <summary>
        /// Spawns new Equipment on Game Area. You can use this version of SpawnEquipment() 
        /// only after initialization of Game Area. Beware! You can't spawn equipment if it 
        /// already exist in the player inventory.
        /// </summary>
        /// <param name="eqType"></param>
        private void SpawnEquipment(EquipmentType eqType)
        {
            Equipment newItem = null;
            if (!CheckThatPlayerHaveEquipment(eqType))
            {
                switch (eqType)
                {
                    case EquipmentType.Sword:
                        newItem = new Sword(GetRandomLocation(), "Miecz", 10);
                        break;
                    case EquipmentType.Bow:
                        newItem = new Bow(GetRandomLocation(), "Łuk", 5);
                        break;
                    case EquipmentType.Mace:
                        newItem = new Mace(GetRandomLocation(), "Buława", 12);
                        break;
                    case EquipmentType.BluePotion:
                        newItem = new Potion(GetRandomLocation(), "Niebieska mikstura", 12, eqType);
                        break;
                    case EquipmentType.RedPotion:
                        newItem = new Potion(GetRandomLocation(), "Czerwona mikstura", 20, eqType);
                        break;
                }
                if (newItem is Weapon)
                {
                    Weapon weapon = newItem as Weapon;
                    WeaponsInRoom.Add(weapon);
                    _playArea.Children.Add(weapon.WeaponAppearance);
                    Canvas.SetLeft(weapon.WeaponAppearance, weapon.Location.X);
                    Canvas.SetBottom(weapon.WeaponAppearance, weapon.Location.Y);
                }
                else
                {
                    Potion potion = newItem as Potion;
                    EquipmentInRoom.Add(newItem);
                    _playArea.Children.Add(potion.EquipmentAppearance);
                    Canvas.SetLeft(potion.EquipmentAppearance, potion.Location.X);
                    Canvas.SetBottom(potion.EquipmentAppearance, potion.Location.Y);
                }
            }
            else
                throw new Exception("Nie można utowrzyć przedmiotu, ponieważ znajduje się on już w Eq gracza.");
        }
        private bool CheckThatPlayerHaveEquipment(EquipmentType eqType)
        {
            if (_player.EquipmentList.ToList().Exists(eq => eq.EqType == eqType)) return true;
            else return false;

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
            List<Equipment> allEquipmentList = new List<Equipment>();
                allEquipmentList.AddRange(WeaponsInRoom);
                allEquipmentList.AddRange(EquipmentInRoom);
            foreach (Equipment equipment in allEquipmentList)
            {
                double maxPositionDiffrence = 7;
                if (maxPositionDiffrence > Math.Abs(equipment.Location.X - PlayerLocation.X)
                    && maxPositionDiffrence > Math.Abs(equipment.Location.Y - PlayerLocation.Y))
                {
                    _player.Equip(equipment);
                    _equipmentImageDictionary[equipment.EqType].Visibility = Visibility.Visible;
                    if(equipment is Weapon)
                    {
                        
                        _playArea.Children.Remove((equipment as Weapon).WeaponAppearance);
                        WeaponsInRoom.Remove(equipment as Weapon);
                    }
                    else
                    {
                        _playArea.Children.Remove((equipment as Potion).EquipmentAppearance);
                        EquipmentInRoom.Remove(equipment);
                    }
                    AddToolTipsToGui(equipment);
                       
                }
            }
        }
        public void UsePotion(EquipmentType potionType)
        {
            if(_equipmentImageDictionary[potionType].IsVisible)
            {
                _equipmentImageDictionary[potionType].Visibility = Visibility.Hidden;
                _player.UsePotion(potionType);
            }
            OnPropertyChanged("PlayerHitPoints");
        }
        #endregion


    }
}
