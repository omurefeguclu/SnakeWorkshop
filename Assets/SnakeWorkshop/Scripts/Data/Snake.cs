using System.Collections.Generic;

namespace SnakeWorkshop.Scripts.Data
{
    public class Snake
    {
        public Snake(GridCoords initialSquareCoords)
        {
            OccupiedSquares = new LinkedList<GridCoords>();
            OccupiedSquares.AddFirst(initialSquareCoords);
        }
        
        // This is a list of square ids that the snake occupies
        // LinkedList is used because it is more efficient to add and remove elements from the beginning and end of the list
        // The snake movement will be implemented by removing the last element and adding a new element to the front of the list
        public LinkedList<GridCoords> OccupiedSquares { get; private set; }
    }
}