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
    class Ghoul : Enemy
    {
        private const int _enemyMoveInterval = 4;
        private const long _movingTimeSpan = 800000;
        private const int _expToGain = 15;
        private const string _imagePath = @"\image\Ghoul.png";
        private BitmapImage _image = new BitmapImage(new Uri(@"\image\Ghoul.png",
            UriKind.Relative));
        private BitmapImage _hitImage = new BitmapImage(new Uri(@"\image\GhoulHit.png",
            UriKind.Relative));

        public string Name { get; }

        public Ghoul(Point point, Canvas playArea, int hp, int maxAttackPower)
            : base(point, _enemyMoveInterval, _movingTimeSpan, playArea, hp, _imagePath, maxAttackPower,
                  _expToGain)
        {
            _attackRange = 70;
            Name = "Ghoul";
            _enemyAppearance.ToolTip = Name;
        }

        public override void Attack(IFightable atackDestination)
        {
            if (Nearby(_playerPosition, _attackRange))
                atackDestination.TakeAHit(random.Next(_maxAttackPower + 1));
        }
        public override bool TakeAHit(int hp)
        {
            bool died = base.TakeAHit(hp);
            _enemyAppearance.Source = _hitImage;

            return died;
        }
        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            if (_enemyAppearance.Source != _image)
            {
                _enemyAppearance.Source = _image;
            }

        }
    }
}
