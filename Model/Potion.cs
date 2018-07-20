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
    class Potion : Equipment
    {
        public override string Name { get; protected set; }
        private int _restoredHp { get; }
        [NonSerialized] public Image EquipmentAppearance;// { get; protected set; }
        //public bool IsUsed { get; private set; }

        public Potion(Point point, string name, int hpRestored, EquipmentType eqType)
            : base(point, eqType, name)
        {
            _restoredHp = hpRestored;
            //IsUsed = false;
            string imagePath = GameController.EqImagePathDictionary[EqType];
            EquipmentAppearance = new Image();
            EquipmentAppearance.Height = EquipmentAppearance.Width = 30;
            EquipmentAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            EquipmentAppearance.ToolTip = "Name: " + Name.ToString() + ";\n" +
                "Restored HP: " + _restoredHp.ToString() + ";";
        }

        public int UsePotion()
        {
            if(IsPickedUp)
            {
                IsPickedUp = false;
                return _restoredHp;
            }
            return 0;
        }

        public void ReloadImagesAfterDeserialization()
        {
            string imagePath = GameController.EqImagePathDictionary[EqType];
            EquipmentAppearance = new Image();
            EquipmentAppearance.Height = EquipmentAppearance.Width = 30;
            EquipmentAppearance.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            EquipmentAppearance.ToolTip = "Name: " + Name.ToString() + ";\n" +
                "Restored HP: " + _restoredHp.ToString() + ";";
        }

    }
}
