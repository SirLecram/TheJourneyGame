﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheJourneyGame.Model
{
    class Mace : Weapon
    {
        private const int _attackDirections = 4;

        public Mace(Point point, string name, string imagePath, int damage) 
            : base(point, name, imagePath, damage, 50)
        {
            EqType = EquipmentType.Mace;
            WeaponAppearance.ToolTip = "Attack Directions: " + _attackDirections.ToString() + ";\n" +
                "Attack Range: " + Range.ToString() + ";\n" + "Damage: " + Damage.ToString() + ";";
        }

        public override bool UseWeapon(Point point, Direction sightDirection)
        {
            return true;
        }
    }
}
