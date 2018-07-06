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
    class Player : Position, IFightable
    {
        #region Private and protected attributes
        private Equipment _equippedItem;
        private List<Equipment> _equipmentList { get; set; }
        private Direction _sightDirection { get; set; }
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
                    return (_equippedItem as Weapon).Damage;
                else
                    return 4;
            }
        }
        private BitmapImage _playerImage = new BitmapImage(new Uri(@"\image\Player.png", UriKind.Relative));
        private BitmapImage _playerHitImage = new BitmapImage(new Uri(@"\image\PlayerHit.png", 
            UriKind.Relative));
        private DispatcherTimer animationTimer = new DispatcherTimer();
        #endregion

        public int HitPoints { get; private set; }
        public Image PlayersAppearance { get; private set; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }
        public IEnumerable<Equipment> EquipmentList { get => _equipmentList; }

        public Player(Point point, int amountOfHP) : base(point, 10)
        {
            InitializePlayer(amountOfHP);
        }
        #region Initialization and help methods
        private void InitializePlayer(int amountOfHP)
        {
            _equipmentList = new List<Equipment>();
            PlayersAppearance = new Image();
            PlayersAppearance.Height = PlayersAppearance.Width = 30;
            PlayersAppearance.Source = _playerImage;
            HitPoints = amountOfHP;
            _sightDirection = Direction.Left;
            animationTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(800).Ticks);
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

        }
        public void Equip(Equipment itemToEquip)
        {
            _equipmentList.Add(itemToEquip);

        }
        public void SelectToUse(EquipmentType eqToSelect)
        {
            Equipment selectedItem = _equipmentList.Find(Equipment => Equipment.EqType == eqToSelect);
            if (selectedItem != null)
            {
                _equippedItem = selectedItem;
            }
        }
        /// <summary>
        /// A method which provide a way to attack selected enemy
        /// </summary>
        /// <param name="enemyToAttack">Enemy to attack</param>
        public void Attack(IFightable enemyToAttack)
        {
            Enemy enemy = (Enemy)enemyToAttack;
            int minDamage = Math.Abs(_actualAttackPower - 5);
            int maxDamage = _actualAttackPower + 1;
            if (Nearby(enemy.Location, _actualAttackRange) && _equippedItem is Weapon)
            {
                if ((_equippedItem as Weapon).UseWeapon(enemy.Location, _sightDirection))
                    enemyToAttack.TakeAHit(random.Next(minDamage, maxDamage));
            }
            else if(Nearby(enemy.Location, _actualAttackRange))
            {
                enemyToAttack.TakeAHit(random.Next(maxDamage));
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
