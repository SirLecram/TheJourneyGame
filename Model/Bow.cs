using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheJourneyGame.Model
{
    class Bow : Weapon
    {
        public const int Range = 100;
        private const int _attackDirections = 1;


        public Bow(Point point, string name, string imagePath, int damage) 
            : base(point, name, imagePath, damage)
        {
        }

        public override bool UseWeapon(Point point, Direction sightDirection)
        {
            throw new NotImplementedException();
        }
    }
}
