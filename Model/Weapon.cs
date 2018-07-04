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
    abstract class Weapon : Equipment
    {
        public Image WeaponAppearance { get; private set; }
        public override string Name { get; protected set; }
        public int Damage { get; }
        public Weapon(Point point, string name, string imagePath, int damage) : base(point)
        {
            Name = name;
            WeaponAppearance = new Image();
            WeaponAppearance.Height = WeaponAppearance.Width = 30;
            WeaponAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            Damage = damage;
        }

        public abstract bool UseWeapon(Point point, Direction sightDirection);

        public override void Move(Direction direction, Canvas playArea)
        {
            throw new NotImplementedException();
        }
    }
}
