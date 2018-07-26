using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TheJourneyGame.Model
{
    [Serializable]
    abstract class Weapon : Equipment, IDeserializable
    {
        [NonSerialized] public Image WeaponAppearance;// { get; private set; }
        public override string Name { get; protected set; }
        public int Damage { get; }
        public int Range { get; }
        public Weapon(Point point, string name, int damage, int range,
            EquipmentType eqType) : base(point, eqType, name)
        {
            string imagePath = GameController.EqImagePathDictionary[EqType];
            WeaponAppearance = new Image();
            WeaponAppearance.Height = WeaponAppearance.Width = 30;
            WeaponAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            Damage = damage;
            Range = range;
        }

        public virtual void ReloadImagesAfterDeserialization()
        {
            string imagePath = GameController.EqImagePathDictionary[EqType];
            WeaponAppearance = new Image();
            WeaponAppearance.Height = WeaponAppearance.Width = 30;
            WeaponAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }
        /// <summary>
        /// Checks if the enemy is in the right direction (diffrent weapons has diffrent atack
        /// directions).
        /// </summary>
        /// <param name="point">Position of enemy</param>
        /// <param name="sightDirection">Direction the player is facing</param>
        /// <returns></returns>
        public abstract bool UseWeapon(Point point, Direction sightDirection);

        public override void Move(Direction direction, Canvas playArea)
        {
            throw new NotImplementedException();
        }
    }
}
