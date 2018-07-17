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
    class Ghost : Enemy
    {
        private const int _enemyMoveInterval = 5;
        private const long _movingTimeSpan = 700000;
        private const int _expToGain = 25;
        private const string _imagePath = @"\image\Ghost.png";
        private BitmapImage _image = new BitmapImage(new Uri(@"\image\Ghost.png",
            UriKind.Relative));
        private BitmapImage _hitImage = new BitmapImage(new Uri(@"\image\GhostHit.png",
            UriKind.Relative));

        public string Name { get; }

        public Ghost(Point point, Canvas playArea, int hp, int maxAttackPower)
            : base(point, _enemyMoveInterval, _movingTimeSpan, playArea, hp, _imagePath, maxAttackPower,
                  _expToGain)
        {
            _attackRange = 55;
            Name = "Ghost";
            _enemyAppearance.ToolTip = Name;
        }

        public override void Attack(IFightable atackDestination)
        {
            if (Nearby(_playerPosition, _attackRange))
                atackDestination.TakeAHit(random.Next(_maxAttackPower + 1));


        }

        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            if (_enemyAppearance.Source != _image)
            {
                _enemyAppearance.Source = _image;
            }

        }

        public override bool TakeAHit(int hp)
        {
            bool died = base.TakeAHit(hp);
            _enemyAppearance.Source = _hitImage;

            return died;
        }
    }
}
