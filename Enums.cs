namespace TheJourneyGame
{
    public enum Direction
    {
        Left = 23,
        Up = 24,
        Right = 25,
        Down = 26,
    }
    public interface IFightable
    {
        void Attack(IFightable atackDestination);
        bool TakeAHit(int hpToTake);
    }


}