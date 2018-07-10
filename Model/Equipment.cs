using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace TheJourneyGame.Model
{
    abstract class Equipment : Position
    {
        public bool IsPickedUp { get; protected set; }
        public abstract string Name { get; protected set; }
        public EquipmentType EqType { get; protected set; }

        public Equipment(Point point, EquipmentType eqType, string name) : base(point, 0)
        {
            IsPickedUp = false;
            EqType = eqType;
            Name = name;

        }

        public void PickUp()
        {
            IsPickedUp = true;
        }
    }
}
