using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TheJourneyGame.Model
{
    class Player : Position, IFightable
    {
        private Equipment _equippedItem;
        public int HitPoints { get; private set; }
        private List<Equipment> _equipmentList { get; set; }
        public Image PlayersAppearance { get; private set; }
        private Direction _sightDirection { get; set; }

        public Player(Point point, int amountOfHP) : base(point, 10)
        {
            _equipmentList = new List<Equipment>();
            PlayersAppearance = new Image();
            PlayersAppearance.Height = PlayersAppearance.Width = 25;
            PlayersAppearance.Source = new BitmapImage(new Uri(@"\image\kwadrat.png", UriKind.Relative));
            HitPoints = amountOfHP;
            _sightDirection = Direction.Left;
            _equippedItem = new Sword(new Point(0, 0), "Miecz", @"\image\kwadrat.png", 3);
        }

        //OTrzymanie ciosu - w przyszlosci rowniez tarcza i obrona
        public bool TakeAHit(int numberOfHP)
        {
            return true;
        }
        public void IncreaseHp(int numberOfHP)
        {

        }
        public void Equip(Equipment itemToEquip)
        {

        }

        public void Attack(IFightable enemyToAttack)
        {
            Enemy enemy = (Enemy)enemyToAttack;
            if (Nearby(enemy.Location, 50))
            {
                if(((Weapon)_equippedItem).UseWeapon(enemy.Location, _sightDirection))
                    enemyToAttack.TakeAHit(5);
            }
        }
        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            _sightDirection = direction;
          //  Canvas.SetLeft(PlayersAppearance, location.X);
           // Canvas.SetBottom(PlayersAppearance, location.Y);
            // W TYM MIEJSCU DOPISAC:
            // ################# WARUNKI PODNOSZENIA PRZEDMIOTOW  #####################
        }

    }
}
