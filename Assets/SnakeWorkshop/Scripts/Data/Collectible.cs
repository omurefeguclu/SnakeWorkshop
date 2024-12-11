namespace SnakeWorkshop.Scripts.Data
{
    public class Collectible
    {
        public Collectible(GridCoords square)
        {
            Square = square;
        }

        public GridCoords Square { get; private set; }
        
    }
}