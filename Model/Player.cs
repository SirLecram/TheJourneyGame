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
        private Equipment _equippedItem;
        private List<Equipment> _equipmentList { get; set; }
        public IEnumerable<Equipment> EquipmentList { get => _equipmentList; }
        private Direction _sightDirection { get; set; }
        private BitmapImage _playerImage = new BitmapImage(new Uri(@"\image\Player.png", UriKind.Relative));
        private BitmapImage _playerHitImage = new BitmapImage(new Uri(@"\image\PlayerHit.png", 
            UriKind.Relative));
        private DispatcherTimer animationTimer = new DispatcherTimer();

        public int HitPoints { get; private set; }
        public Image PlayersAppearance { get; private set; }
        public bool IsDead { get { if (HitPoints <= 0) return true; else return false; } }

        public Player(Point point, int amountOfHP) : base(point, 10)
        {
            InitializePlayer(amountOfHP);
            _equippedItem = new Sword(new Point(0, 0), "Miecz", @"\image\sword100x100.png", 3);
            _equipmentList.Add(_equippedItem);
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
            animationTimer.Interval = new TimeSpan(TimeSpan.FromMilliseconds(750).Ticks);
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
        /// <summary>
        /// A method which provide a way to attack selected enemy
        /// </summary>
        /// <param name="enemyToAttack">Enemy to attack</param>
        public void Attack(IFightable enemyToAttack)
        {
            Enemy enemy = (Enemy)enemyToAttack;
            if (Nearby(enemy.Location, 50))
            {
                if (((Weapon)_equippedItem).UseWeapon(enemy.Location, _sightDirection))
                    enemyToAttack.TakeAHit(5);
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
            // W TYM MIEJSCU DOPISAC:
            // ################# WARUNKI PODNOSZENIA PRZEDMIOTOW  #####################
        }
        #endregion
    }
}
