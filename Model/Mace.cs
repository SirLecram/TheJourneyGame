using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheJourneyGame.Model
{
    class Mace : Weapon
    {
        public const int Range = 15;
        private const int _attackDirections = 4;

        public Mace(Point point, string name, string imagePath, int damage) 
            : base(point, name, imagePath, damage)
        {
        }

        public override bool UseWeapon(Point point, Direction sightDirection)
        {
            throw new NotImplementedException();
        }
    }
}
