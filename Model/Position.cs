using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TheJourneyGame.Model
{

    abstract class Position
    {
        protected const int moveInterval = 5;
        protected Point location { get; set; }
        public Point Location { get => location; }
       /* private static Canvas _playArea { get; }
        protected static IEnumerable PlayAreaChildren { get => _playArea.Children}*/

        public Position(Point point/*, Canvas playArea*/)
        {
            location = point;

        }
        public Position(double x, double y)
        {
            location = new Point(x, y);
        }

        public bool Nearby(Point locationToCheck, int distance)
        {
            if (Math.Abs(location.X - locationToCheck.X) < distance
                && (Math.Abs(location.Y - locationToCheck.Y) < distance))
            {
                return true;
            }
            else
                return false;
                
        }
        public virtual void Move(Direction direction, Canvas playArea)
        {
            Point newLocation;
            switch (direction)
            {
                case Direction.Left:
                    if (location.X + 5 >= 0)
                    {
                        newLocation = new Point(location.X - 10, location.Y);
                        location = newLocation;
                    }
                    break;
                case Direction.Up:
                    if(location.Y + 20 <= playArea.ActualHeight)
                    {
                        newLocation = new Point(location.X, location.Y + 10);
                        location = newLocation;
                    }
                    break;
                case Direction.Right:
                    if (location.X + 20 <= playArea.ActualWidth)
                    {
                        newLocation = new Point(location.X + 10, location.Y);
                        location = newLocation;
                    }
                    break;
                case Direction.Down:
                    if (location.Y + 5 >= 0)
                    {
                        newLocation = new Point(location.X, location.Y - 10);
                        location = newLocation;
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
