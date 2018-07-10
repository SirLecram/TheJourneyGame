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
        private const int _attackDirections = 1;


        public Bow(Point point, string name, int damage) 
            : base(point, name, damage, 150, EquipmentType.Bow)
        {
            WeaponAppearance.ToolTip = "Attack Directions: " + _attackDirections.ToString() + ";\n" +
                "Attack Range: " + Range.ToString() + ";\n" + "Damage: " + Damage.ToString() + ";";
        }

        public override bool UseWeapon(Point enemyLocation, Direction sightDirection)
        {
            switch (sightDirection)
            {
                case Direction.Right:
                    if (enemyLocation.X < GameController.PlayerLocation.X
                        || enemyLocation.Y > GameController.PlayerLocation.Y + 25
                        || enemyLocation.Y < GameController.PlayerLocation.Y - 25)
                        return false;
                    break;
                case Direction.Up:
                    if (enemyLocation.Y <= GameController.PlayerLocation.Y 
                        || enemyLocation.X > GameController.PlayerLocation.X + 25
                        || enemyLocation.X < GameController.PlayerLocation.X - 25)
                        return false;
                    break;
                case Direction.Left:
                    if (enemyLocation.X > GameController.PlayerLocation.X
                        || enemyLocation.Y > GameController.PlayerLocation.Y + 25
                        || enemyLocation.Y < GameController.PlayerLocation.Y - 25)
                        return false;
                    break;
                case Direction.Down:
                    if (enemyLocation.Y > GameController.PlayerLocation.Y
                        || enemyLocation.X > GameController.PlayerLocation.X + 25
                        || enemyLocation.X < GameController.PlayerLocation.X - 25)
                        return false;
                    break;
                default: break;
            }
            return true;
        }
    }
}
