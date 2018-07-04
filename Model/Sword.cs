using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheJourneyGame.Model
{
    class Sword : Weapon
    {
        public const int Range = 20;
        private const int _attackDirections = 3;
        //public const int ReuseTime = 

        public Sword(Point point, string name, string imagePath, int damage) 
            : base(point, name, imagePath, damage)
        {

        }

        public override bool UseWeapon(Point enemyLocation, Direction sightDirection)
        {
            switch(sightDirection)
            {
                case Direction.Right:
                    if (enemyLocation.X + 5 < GameController.PlayerLocation.X)
                        return false;
                    break;
                case Direction.Up:
                    if (enemyLocation.Y + 5 < GameController.PlayerLocation.Y)
                        return false;
                    break;
                case Direction.Left:
                    if (enemyLocation.X - 5 > GameController.PlayerLocation.X)
                        return false;
                    break;
                case Direction.Down:
                    if (enemyLocation.Y - 5 > GameController.PlayerLocation.Y)
                        return false;
                    break;
                default: break;
            }
            return true;
            
        }
    }
}
