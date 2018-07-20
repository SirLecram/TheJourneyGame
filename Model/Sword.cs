using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheJourneyGame.Model
{
    [Serializable]
    class Sword : Weapon
    {
        private const int _attackDirections = 3;
        //public const int ReuseTime = 

        public Sword(Point point, string name, int damage) 
            : base(point, name, damage, 50, EquipmentType.Sword)
        {
            WeaponAppearance.ToolTip = "Attack Directions: " + _attackDirections.ToString() + ";\n" +
                "Attack Range: " + Range.ToString() + ";\n" + "Damage: " + Damage.ToString() + ";";
        }
        public override void ReloadImagesAfterDeserialization()
        {
            base.ReloadImagesAfterDeserialization();
            WeaponAppearance.ToolTip = "Attack Directions: " + _attackDirections.ToString() + ";\n" +
                "Attack Range: " + Range.ToString() + ";\n" + "Damage: " + Damage.ToString() + ";";
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
