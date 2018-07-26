namespace TheJourneyGame
{
    public enum Direction
    {
        Left = 23,
        Up = 24,
        Right = 25,
        Down = 26,
    }
    public enum EquipmentType
    {
        Sword = 0,
        Bow,
        Mace,
        BluePotion,
        RedPotion,
    }
    public enum EnemyType
    {
        Bat = 0,
        Ghoul,
        Ghost,
    }
    public interface IFightable
    {
        void Attack(IFightable atackDestination);
        bool TakeAHit(int hpToTake);
    }
    public interface IDeserializable
    {
        void ReloadImagesAfterDeserialization();
    }


}