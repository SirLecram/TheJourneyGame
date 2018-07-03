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
    class Player : Position
    {
        private Equipment _equippedItem;
        public int HitPoints { get; private set; }
        private List<Equipment> _equipmentList { get; set; }
        public Image PlayersAppearance { get; private set; }  

        public Player(Point point, int amountOfHP ) : base(point)
        {
            _equipmentList = new List<Equipment>();
            PlayersAppearance = new Image();
            PlayersAppearance.Height = PlayersAppearance.Width = 25;
            PlayersAppearance.Source = new BitmapImage(new Uri(@"\image\kwadrat.png", UriKind.Relative));
            HitPoints = amountOfHP;
            
        }

        //OTrzymanie ciosu - w przyszlosci rowniez tarcza i obrona
        public void TakeAHit(int numberOfHP)
        {

        }
        public void IncreaseHp(int numberOfHP)
        {

        }
        public void Equip(Equipment itemToEquip)
        {

        }

        public void Attack(Random random)
        {

        }
        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
          //  Canvas.SetLeft(PlayersAppearance, location.X);
           // Canvas.SetBottom(PlayersAppearance, location.Y);
            // W TYM MIEJSCU DOPISAC:
            // ################# WARUNKI PODNOSZENIA PRZEDMIOTOW  #####################
        }
    }
}
