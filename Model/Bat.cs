using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TheJourneyGame.Model
{
    class Bat : Enemy
    {
        private const int _batMoveInterval = 5;
        private const long _batMovingTimeSpan = 10000000;
        private const string _batImagePath = @"\image\BatEnemy.png";
        private BitmapImage _batImage = new BitmapImage(new Uri(@"\image\BatEnemy.png",
            UriKind.Relative));
        private BitmapImage _batHitImage = new BitmapImage(new Uri(@"\image\BatHited.png",
            UriKind.Relative));


        public Bat(Point point, Canvas playArea, int hp) 
            : base(point, _batMoveInterval, _batMovingTimeSpan, playArea, hp, _batImagePath)
        {
            _attackRange = 30;
        }

        public override void Attack(IFightable atackDestination)
        {
            if (Nearby(_playerPosition, _attackRange))
                atackDestination.TakeAHit(3);
        }

        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            if (EnemyAppearance.Source != _batImage)
            {
                EnemyAppearance.Source = _batImage;
            }
            
        }

        public override bool TakeAHit(int hp)
        {
            bool died = base.TakeAHit(hp);
            EnemyAppearance.Source = _batHitImage;
            return died;
        }
    }
}
