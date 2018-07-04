using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TheJourneyGame.Model
{
    class Bat : Enemy
    {
        private const int _batMoveInterval = 5;
        private const long _batMovingTimeSpan = 1000000;
        private const string _batImagePath = @"\image\kwadrat.png";


        public Bat(Point point, Canvas playArea, int hp) 
            : base(point, _batMoveInterval, _batMovingTimeSpan, playArea, hp, _batImagePath)
        {

        }
    }
}
