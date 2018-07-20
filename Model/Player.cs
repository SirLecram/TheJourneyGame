using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TheJourneyGame.Model
{
    [Serializable]
    class Player : Position, IFightable
    {
        #region Private and protected attributes
        private Equipment _equippedItem;
        private List<Equipment> _equipmentList { get; set; }
        private Direction _sightDirection { get; set; }
        private Dictionary<int, int> _experienceStages = new Dictionary<int, int>();
        private int _maxHp { get; set; }
        private int _basicAttacPower { get; set; }
        private int _actualAttackRange {
            get
            {
                if (_equippedItem is Weapon)
                    return (_equippedItem as Weapon).Range;
                else
                    return 30;
            } }
        private int _actualAttackPower
        {
            get
            {
                if (_equippedItem is Weapon)
                    return (_equippedItem as Weapon).Damage + _basicAttacPower;
                else
                    return _basicAttacPower;
            }
        }
        [NonSerialized] private BitmapImage _playerImage = new BitmapImage(new Uri(@"\image\Player.png", UriKind.Relative));
        [NonSerialized] private BitmapImage _playerHitImage = new BitmapImage(new Uri(@"\image\PlayerHit.png", 
            UriKind.Relative));
        [NonSerialized] private DispatcherTimer animationTimer = new DispatcherTimer();
        #endregion

        public int HitPoints { get; private set; }
        public int ExperiencePoints { get; private set; }
        public int PlayerLevel { get; private set; }
        public int CompletedLevels { get; private set; }
        [NonSerialized] public Image PlayersAppearance;//{ get; private set; }
        public IReadOnlyDictionary<int, int > ExperienceStages { get => _experienceStages; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }
        public IEnumerable<Equipment> EquipmentList { get => _equipmentList; }

        public Player(Point point, int amountOfHP) : base(point, 10)
        {
            InitializePlayer(amountOfHP);
            InitializeExperienceStages();
        }
        #region Initialization and help methods
        private void InitializePlayer(int amountOfHP)
        {
            _equipmentList = new List<Equipment>();
            PlayersAppearance = new Image();
            PlayersAppearance.Height = PlayersAppearance.Width = 30;
            PlayersAppearance.Source = _playerImage;
            HitPoints = amountOfHP;
            _maxHp = amountOfHP;
            _basicAttacPower = 20;
            PlayerLevel = 1;
            CompletedLevels = 0;
            _sightDirection = Direction.Left;
            animationTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(800).Ticks);
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }
        private void InitializeExperienceStages()
        {
            int initialExp = 100;
            for(int i = 1; i<=11; i++)
            {
                if (i == 11)
                    initialExp = 1000000;
                _experienceStages.Add(i, initialExp);
                initialExp *= 2;
            }

        }

        public void LevelUp()
        {
            if(PlayerLevel<11)
            {
                PlayerLevel++;
                _maxHp += 5;
                _basicAttacPower++;
                HitPoints = _maxHp;
            }
            
            
        }
        public void LevelCompleted(int levelNumber)
        {
            if(levelNumber>CompletedLevels+1)
                CompletedLevels = levelNumber-1;
        }
        public void ReloadImagesAfterDeserialization()
        {
            _playerImage = new BitmapImage(new Uri(@"\image\Player.png", UriKind.Relative));
            _playerHitImage = new BitmapImage(new Uri(@"\image\PlayerHit.png", UriKind.Relative));
            animationTimer = new DispatcherTimer();
            PlayersAppearance = new Image();
            PlayersAppearance.Height = PlayersAppearance.Width = 30;
            PlayersAppearance.Source = _playerImage;
            foreach(Equipment eq in _equipmentList)
            {
                if (eq is Weapon)
                    (eq as Weapon).ReloadImagesAfterDeserialization();
                else if (eq is Potion)
                    (eq as Potion).ReloadImagesAfterDeserialization();
            }
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (PlayersAppearance.Source != _playerImage)
                PlayersAppearance.Source = _playerImage;
        }
        #endregion

        #region Player mechanics
        /// <summary>
        /// A method which should be called to couse an injury to the player.
        /// </summary>
        /// <param name="numberOfHP">Damage deal by enemy</param>
        /// <returns></returns>
        public bool TakeAHit(int numberOfHP)
        {
            //OTrzymanie ciosu - w przyszlosci rowniez tarcza i obrona
            HitPoints -= numberOfHP;
            if (IsDead)
                MessageBox.Show("UMARLES, SORRY -> Do implementacji śmierć gracza;");
            PlayersAppearance.Source = _playerHitImage;
            return true;
        }
        public void IncreaseHp(int numberOfHP)
        {
            if (HitPoints + numberOfHP >= _maxHp)
                HitPoints = _maxHp;
            else
                HitPoints += numberOfHP;
        }
        public void Equip(Equipment itemToEquip)
        {
            _equipmentList.Add(itemToEquip);
            itemToEquip.PickUp();
        }
        public bool SelectToUse(EquipmentType eqToSelect)
        {
            Equipment selectedItem = _equipmentList.Find(Equipment => Equipment.EqType == eqToSelect);
            if (selectedItem != null)
            {
                _equippedItem = selectedItem;
                return true;
            }
            return false;
        }
        public void UsePotion(EquipmentType potionType)
        {
            Potion selectedPotion =(Potion) _equipmentList.Find(Equipment => Equipment.EqType == potionType);
            IncreaseHp(selectedPotion.UsePotion());
            _equipmentList.Remove(selectedPotion);
        }
        public void GainExp(int expToGain)
        {
            ExperiencePoints += expToGain;
            if (ExperiencePoints > _experienceStages[PlayerLevel])
                LevelUp();
        }
        public void RevivePlayer()
        {
            HitPoints = _maxHp;
            location = new Point(900, 120);
            
        }
        /// <summary>
        /// A method which provide a way to attack selected enemy
        /// </summary>
        /// <param name="enemyToAttack">Enemy to attack</param>
        public void Attack(IFightable enemyToAttack)
        {
            Enemy enemy = (Enemy)enemyToAttack;
            int minDamage = Math.Abs(_actualAttackPower - _basicAttacPower);
            int maxDamage = _actualAttackPower + 1;
            if (Nearby(enemy.Location, _actualAttackRange) && _equippedItem is Weapon)
            {
                if ((_equippedItem as Weapon).UseWeapon(enemy.Location, _sightDirection))
                    if (enemyToAttack.TakeAHit(random.Next(minDamage, maxDamage)))
                        GainExp(enemy.ExpToGain);
            }
            else if(Nearby(enemy.Location, _actualAttackRange))
            {
                if (enemyToAttack.TakeAHit(random.Next(maxDamage)))
                    GainExp(enemy.ExpToGain);
            }
        }
        /// <summary>
        /// A method wchich provide a way to moving player on the play area.
        /// </summary>
        /// <param name="direction">Move direction</param>
        /// <param name="playArea">Game area</param>
        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            _sightDirection = direction;
        }

        #endregion
    }
}
