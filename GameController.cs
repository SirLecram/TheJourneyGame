using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Serializable]
    class GameController : INotifyPropertyChanged
    {
        public List<Weapon> WeaponsInRoom { get; private set; }
        public List<Equipment> EquipmentInRoom { get; private set; }
        public static IReadOnlyDictionary<EquipmentType, string> EqImagePathDictionary
        {
            get => _equipmentImagePathDictionary;
        }

        #region Binding Attributes
        public int PlayerHitPoints
        { get { if (_player != null) return _player.HitPoints; else return 0; } }
        public int PlayerLevel
        { get { if (_player != null) return _player.PlayerLevel; else return 0; } }
        public string PlayerExperience
        { get {
                if (_player != null)
                {
                    string stringToReturn = _player.ExperiencePoints + "/" 
                        + _player.ExperienceStages[PlayerLevel];
                    return stringToReturn;
                } 
                else return String.Empty; } }
        public int PlayerCompletedLevels
        { get { if (_player != null) return _player.CompletedLevels; else return 0; } }
        public int ActualGameLevel { get => _level; }
        public static Point PlayerLocation { get => _player.Location; }
        public double PlayerXPosition { get => _player.Location.X; }
        public double PlayerYPosition { get => _player.Location.Y; }
        #endregion

        private static Player _player { get; set; }
        private bool _isLevelFinished = false;
        private int _completedLevels { get; set; }


        private Canvas _playArea;//{ get; private set; }
        private List<Enemy> _enemiesList { get; set; }
        private int _level { get; set; }
        private Point _finishLocation { get; set; }
        public Point playerLocation { get => location(); }
        private DispatcherTimer attacksOnPlayerTimer = new DispatcherTimer();
        private DispatcherTimer levelTimer = new DispatcherTimer();
        private Dictionary<EquipmentType, Image> _equipmentImageDictionary = 
            new Dictionary<EquipmentType, Image>();
        private static Dictionary<EquipmentType, string> _equipmentImagePathDictionary { get; set; }
        private Grid _equipmentGrid;
        private int _levelBatAmount;
        private int _levelGhoulAmount;
        private List<EquipmentType> _levelWeapons;
        private int _levelPotionChancePer100;
        private TimeSpan _levelTimeIntervalStep;
        public Random random = new Random();


        

        public GameController(Canvas playArea, Grid eqGrid)
        {
            _playArea = playArea;
            _equipmentGrid = eqGrid;
            _level = 1;
            _completedLevels = 1;
            _levelWeapons = new List<EquipmentType>();
            InitializePlayer();
            InitializeGameAndTimers();
            InitializeEquipmentImages();
            InitializeEnemies();
           
        }
        public Point location()
        {
            if (_player != null)
                return _player.Location;
            else
                return new Point(0, 0);
        }
        #region Initialization
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
        private void InitializePlayer()
        {
            _player = new Player(new Point(250, 20), 50);
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);
        }
        private void InitializePlayer(Player player)
        {
            if (_playArea.Children.Contains(_player.PlayersAppearance))
                _playArea.Children.Remove(_player.PlayersAppearance);
            _player = player;
            _playArea.Children.Add(_player.PlayersAppearance);
            Canvas.SetLeft(_player.PlayersAppearance, _player.Location.X);
            Canvas.SetBottom(_player.PlayersAppearance, _player.Location.Y);
        }

        private void InitializeGameAndTimers()
        {
            WeaponsInRoom = new List<Weapon>();
            EquipmentInRoom = new List<Equipment>();
            
            attacksOnPlayerTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(2000).Ticks);
            attacksOnPlayerTimer.Tick += AttackOnPlayerTimer_Tick;
            levelTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(4000).Ticks);
            levelTimer.Tick += LevelTimer_Tick;

        }
        private void InitializeLevel(int levelNumber)
        {
            _isLevelFinished = false;
            InitializeEnemies();
            switch (levelNumber)
            {
                case 1:
                    InitializePlayerPositionBinding();
                    
                    SpawnEquipment(EquipmentType.BluePotion);
                    
                    _levelBatAmount = 10;
                    _levelPotionChancePer100 = 20;
                    _levelTimeIntervalStep = new TimeSpan(TimeSpan.FromMilliseconds(100).Ticks);
                    levelTimer.Interval = new TimeSpan(TimeSpan.FromSeconds(2).Ticks);
                    break;
                case 2:
                    
                    SpawnEquipment(EquipmentType.BluePotion);
                    SpawnEnemy(EnemyType.Bat, 3);
                    _levelBatAmount = 15;
                    _levelPotionChancePer100 = 15;
                    levelTimer.Interval = new TimeSpan(TimeSpan.FromSeconds(4).Ticks);
                    /*foreach (Enemy enemy in _enemiesList)
                    {
                        _playArea.Children.Add(enemy.EnemyStackPanel);
                        Canvas.SetLeft(enemy.EnemyStackPanel, enemy.Location.X);
                        Canvas.SetBottom(enemy.EnemyStackPanel, enemy.Location.Y);

                    }*/
                    _levelWeapons.Add(EquipmentType.Sword);
                    break;
                default:
                    SpawnEnemy(EnemyType.Ghost, 4);
                    SpawnEquipment(EquipmentType.Mace);
                    SpawnEquipment(EquipmentType.Bow);
                    break;

            }
            _finishLocation = new Point(5, _playArea.ActualHeight / 2);
        }
        public void InitializeLevel()
        {
            InitializeLevel(_level);

        }

        private void InitializeEnemies()
        {
            _enemiesList = new List<Enemy>();
           
            Enemy.Timer.Stop();
        }

        private void InitializeEquipmentImages()
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
                    newImage.Opacity = 0.7;
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
            if(_player.SelectToUse(selectedEqType))
            {
                int allWeapons = 3;
                for(int i = 0; i<allWeapons; i++)
                {
                    if (i == (int)selectedEqType)
                        _equipmentImageDictionary[selectedEqType].Opacity = 1.0;
                    else
                        _equipmentImageDictionary[(EquipmentType)i].Opacity = 0.7;
                }

            }
            SpawnEquipment(EquipmentType.RedPotion);

        }

        private void AttackOnPlayerTimer_Tick(object sender, EventArgs e)
        {
            if (_player.IsDead)
            {
                ResetGame();
                _player.RevivePlayer();
                MainWindow mw = Window.GetWindow(_playArea) as MainWindow;
                mw.GetMainMenu(true);
            }
            foreach (Enemy enemy in _enemiesList)
            {
                enemy.Attack(_player);
                
            }
            OnPropertyChanged("PlayerHitPoints");
            
            

        }

        private void LevelTimer_Tick(object sender, EventArgs e)
        {
            if (_levelBatAmount > 0)
            {
                int amountOfEnemies = random.Next(1, 3);
                SpawnEnemy(EnemyType.Bat, amountOfEnemies);
                _levelBatAmount-=amountOfEnemies;
            }
            else
            {
                levelTimer.Stop();
                
            }
            if (!CheckThatPlayerHaveEquipment(EquipmentType.BluePotion))
            {
                int potionSpawn = random.Next(100);
                if (potionSpawn <= _levelPotionChancePer100 / 2)
                    SpawnEquipment(EquipmentType.RedPotion);
                else if (potionSpawn <= _levelPotionChancePer100)
                    SpawnEquipment(EquipmentType.BluePotion);
            }
            levelTimer.Interval -= _levelTimeIntervalStep;
            if (levelTimer.Interval <= _levelTimeIntervalStep)
            {
                levelTimer.Stop();
                MessageBox.Show("Level Timer Stop");
            }
            if(_levelWeapons.Any())
            {
                //Spawnowanie broni z listy dostosowanej do poziomu
                int spawnChance = random.Next(100);
                if (spawnChance <= 10)
                {
                    SpawnEquipment(_levelWeapons.First());
                    _levelWeapons.Remove(_levelWeapons.First());
                }
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
                        newItem = new Sword(location, "Miecz", 20);
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
           /* else
                throw new Exception("Nie można utowrzyć przedmiotu, ponieważ znajduje się on już w Eq gracza.");*/
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
                        newItem = new Sword(GetRandomLocation(), "Miecz", 15);
                        break;
                    case EquipmentType.Bow:
                        newItem = new Bow(GetRandomLocation(), "Łuk", 5);
                        break;
                    case EquipmentType.Mace:
                        newItem = new Mace(GetRandomLocation(), "Buława", 18);
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
            /*else
                throw new Exception("Nie można utowrzyć przedmiotu, ponieważ znajduje się on już w Eq gracza.");*/
        }
        /// <summary>
        /// Spawns specified amount of enemies in specifed location
        /// </summary>
        /// <param name="enemyType">Type of enemy to spawn</param>
        /// <param name="numberOfEnemyToSpawn">Number of enemies to spawn</param>
        /// <param name="location">Location</param>
        public void SpawnEnemy(EnemyType enemyType, int numberOfEnemyToSpawn, Point location)
        {
            Enemy newEnemy = null;
            if (numberOfEnemyToSpawn <= 0)
                throw new Exception("numberOfEnemyToSpawn have to be > 0!");
            for(int i = 0; i<numberOfEnemyToSpawn; i++ )
            {
                switch (enemyType)
                {
                    case EnemyType.Bat:
                        newEnemy = new Bat(location, _playArea, 30, 5);
                        break;
                    case EnemyType.Ghoul:
                        newEnemy = new Ghoul(location, _playArea, 50, 8);
                        break;
                    case EnemyType.Ghost:
                        newEnemy = new Ghost(location, _playArea, 70, 10);
                        break;

                }

                _enemiesList.Add(newEnemy);
                _playArea.Children.Add(newEnemy.EnemyStackPanel);
                Canvas.SetLeft(newEnemy.EnemyStackPanel, newEnemy.Location.X);
                Canvas.SetBottom(newEnemy.EnemyStackPanel, newEnemy.Location.Y);
            }
            
        }
        /// <summary>
        /// Spawns specified amount of enemies in random location
        /// </summary>
        /// <param name="enemyType">Type of enemy to spawn</param>
        /// <param name="numberOfEnemyToSpawn">Number of enemies to spawn</param>
        /// <param name="location">Location</param>
        public void SpawnEnemy(EnemyType enemyType, int numberOfEnemyToSpawn)
        {
            Enemy newEnemy = null;
            if (numberOfEnemyToSpawn <= 0)
                throw new Exception("numberOfEnemyToSpawn have to be > 0!");
            for(int i = 0; i<numberOfEnemyToSpawn; i++)
            {
                switch (enemyType)
                {
                    case EnemyType.Bat:
                        newEnemy = new Bat(GetRandomLocation(), _playArea, 30, 5);
                        break;
                    case EnemyType.Ghoul:
                        newEnemy = new Ghoul(GetRandomLocation(), _playArea, 50, 8);
                        break;
                    case EnemyType.Ghost:
                        newEnemy = new Ghost(GetRandomLocation(), _playArea, 70, 10);
                        break;
                }

                _enemiesList.Add(newEnemy);
                _playArea.Children.Add(newEnemy.EnemyStackPanel);
                Canvas.SetLeft(newEnemy.EnemyStackPanel, newEnemy.Location.X);
                Canvas.SetBottom(newEnemy.EnemyStackPanel, newEnemy.Location.Y);
            }
            
        }
        private bool CheckThatPlayerHaveEquipment(EquipmentType eqType)
        {
            if (_player.EquipmentList.ToList().Exists(eq => eq.EqType == eqType)) return true;
            else return false;

        }
        public void LoadGame(string filePath)
        {
            using (Stream input = File.OpenRead(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                object data = formatter.Deserialize(input);
                if (data.GetType() == _player.GetType())
                {
                    Player playerToLoad = data as Player;
                    ResetGame();
                    playerToLoad.ReloadImagesAfterDeserialization();
                    InitializePlayer(playerToLoad);
                    InitializeEqAfterDeserialization();
                    InitializePlayerPositionBinding();
                    _level = PlayerCompletedLevels+1;
                    OnAllPropertyChanged();
                    //newGameController.ReloadDataAfterDeserialization(GameController);
                }
            }
        }
        public void SaveGame(string filePath)
        {
            using (Stream output = File.OpenWrite(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, _player);
            }
        }
        public void InitializeEqAfterDeserialization()
        {
            foreach(Equipment eq in _player.EquipmentList)
            {
                _equipmentImageDictionary[eq.EqType].Visibility = Visibility.Visible;
                AddToolTipsToGui(eq);
            }
        }
        private void OnAllPropertyChanged()
        {
            OnPropertyChanged("PlayerHitPoints");
            OnPropertyChanged("playerLocation");
            OnPropertyChanged("PlayerXPosition");
            OnPropertyChanged("PlayerYPosition");
            OnPropertyChanged("PlayerCompletedLevels");
            OnPropertyChanged("PlayerLevel");
            OnPropertyChanged("PlayerExperience");
            OnPropertyChanged("ActualGameLevel");
        }
        #endregion

        #region Game mechanics
        /// <summary>
        /// BEWARE! Before you will use StartGame() you have to use InitializeLevel();
        /// </summary>
        public void StartGame()
        {
            Enemy.Timer.Start();
            attacksOnPlayerTimer.Start();
            levelTimer.Start();
            if (!_playArea.Children.Contains(_player.PlayersAppearance))
                _playArea.Children.Add(_player.PlayersAppearance);
          
            //InitializeLevel(_level);
        }
        public void ResetGame()
        {
            _isLevelFinished = false;
            _playArea.Children.Clear();
            _enemiesList.Clear();
            attacksOnPlayerTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(2000).Ticks);
            levelTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(3000).Ticks);
            levelTimer.Stop();
            Enemy.Timer.Stop();
            
        }
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
            
            if(_isLevelFinished)
            {
                double maxPositionDiffrence = 10;
                if (maxPositionDiffrence > Math.Abs(_finishLocation.X - PlayerLocation.X)
                    && maxPositionDiffrence > Math.Abs(_finishLocation.Y - PlayerLocation.Y))
                {
                    _isLevelFinished = false;
                    MainWindow mw = MainWindow.GetWindow(_playArea) as MainWindow;
                    mw.GetMainMenu(true);
                    _level++;
                    _player.LevelCompleted(_level);
                    OnPropertyChanged("PlayerCompletedLevels");
                    //OnAllPropertyChanged();
                    /*if (_level > _completedLevels + 1)
                        _completedLevels = _level - 1;*/
                    /*if (_level > 2)
                        _level = 2;*/
                }
            }
            if (!_enemiesList.Any() && !levelTimer.IsEnabled)
                _isLevelFinished = true;
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
            OnAllPropertyChanged();
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
