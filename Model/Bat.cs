using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TheJourneyGame.Model
{
    class Bat : Enemy
    {
        private const int _batMoveInterval = 5;
        private const long _batMovingTimeSpan = 800000;
        private const string _batImagePath = @"\image\BatEnemy.png";
        private BitmapImage _batImage = new BitmapImage(new Uri(@"\image\BatEnemy.png",
            UriKind.Relative));
        private BitmapImage _batHitImage = new BitmapImage(new Uri(@"\image\BatHited.png",
            UriKind.Relative));

        public string Name { get; }

        public Bat(Point point, Canvas playArea, int hp, int maxAttackPower) 
            : base(point, _batMoveInterval, _batMovingTimeSpan, playArea, hp, _batImagePath, maxAttackPower)
        {
            _attackRange = 50;
            Name = "Bat";
            _enemyAppearance.ToolTip = Name;
        }

        public override void Attack(IFightable atackDestination)
        {
            if (Nearby(_playerPosition, _attackRange))
                atackDestination.TakeAHit(random.Next(_maxAttackPower+1));
            
            
        }

        public override void Move(Direction direction, Canvas playArea)
        {
            base.Move(direction, playArea);
            if (_enemyAppearance.Source != _batImage)
            {
                _enemyAppearance.Source = _batImage;
            }
            
        }

        public override bool TakeAHit(int hp)
        {
            bool died = base.TakeAHit(hp);
            _enemyAppearance.Source = _batHitImage;
            
            return died;
        }
    }
}
