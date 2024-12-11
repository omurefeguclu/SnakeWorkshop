using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeWorkshop.Scripts.Data
{
    public class SnakeGameData
    {
        // Consistent data
        public int GameGridSize { get; private set; }
        
        // Gameplay data
        public Snake Snake { get; private set; }
        public List<Collectible> Collectibles { get; private set; }
        public MovementDirection CurrentMovementDirection { get; private set; }
        
        
        public SnakeGameData(int gameGridSize)
        {
            GameGridSize = gameGridSize;
            
            var initialSnakeSquareCoords = new GridCoords(gameGridSize / 2, gameGridSize / 2);
            
            Snake = new Snake(initialSnakeSquareCoords);
            Collectibles = new List<Collectible>();
            CurrentMovementDirection = MovementDirection.Right;
            
            // Add the first collectible
            Collectibles.Add(new Collectible(GetRandomEmptyCoords()));
        }
        
        public void SetMovementDirection(MovementDirection direction)
        {
            CurrentMovementDirection = direction;
        }
        public GridCoords GetRandomEmptyCoords()
        {
            // Deadlock handle is added to prevent infinite loop
            var deadlockCounter = 0;
            
            while (true)
            {
                // Generate a random position within the game grid
                var randomX = UnityEngine.Random.Range(0, GameGridSize);
                var randomY = UnityEngine.Random.Range(0, GameGridSize);
                var randomCoords = new GridCoords(randomX, randomY);
                
                // Check if the random position is not occupied by the snake
                if (!Snake.OccupiedSquares.Contains(randomCoords) &&
                    // this is called a LINQ query, Any() is used to check if there is any collectible at the found random position
                    !Collectibles.Any(c => c.Square == randomCoords))
                {
                    return randomCoords;
                }
                
                if (deadlockCounter++ > 100)
                {
                    Debug.LogError("Deadlock detected while generating a random collectible position");
                    return randomCoords;
                }
            }
        }
    }

    

    
    
    

    
}
