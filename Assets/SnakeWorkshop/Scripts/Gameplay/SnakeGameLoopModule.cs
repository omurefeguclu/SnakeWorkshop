using System;
using System.Linq;
using SnakeWorkshop.Scripts.Data;
using SnakeWorkshop.Scripts.Helpers;
using UnityEngine;

namespace SnakeWorkshop.Scripts.Gameplay
{
    [Serializable]
    public class SnakeGameLoopModule : ISnakeGameModule
    {
        // Configurable fields
        [SerializeField] private float framePerSecond = 5f;
        
        
        // Events
        public event Action OnGameLoopIterated;
        public event Action OnGameOver;
        
        
        // Private fields
        private SnakeGameData _gameData;
        
        // This is an example of lazy initialization -> the UpdateTimerHelper is only created when it is needed
        private UpdateTimerHelper _updateTimer;

        private UpdateTimerHelper UpdateTimer =>
            _updateTimer ??= new UpdateTimerHelper(1f / framePerSecond, paused: true);

        public void Initialize(SnakeGameData gameData)
        {
            UpdateTimer.ResetTimer();
            
            _gameData = gameData;
            
            // Start the game loop
            UpdateTimer.TogglePause(false);
        }

        // Unity lifecycle methods such as Update() can not be used in classes that are not derived from MonoBehaviour
        // Instead, we need to call this method from the Update() method of the owner of this module
        public void OnUpdate()
        {
            // Check if the timer has reached the interval (called every loop frame)
            if (UpdateTimer.OnTimerInterval())
            {
                ProcessGameLoop();
            }
        }

        #region Game Events

        
        private void OnGameEnded()
        {
            // Notify the subscribers that the game has ended
            OnGameOver?.Invoke();
            
            // Pause the game loop
            UpdateTimer.TogglePause(true);
        }
        
        private void OnSnakeAteFood(Collectible eatenCollectible)
        {
            // Notify the subscribers that the snake has eaten the food
            // OnSnakeAteFood?.Invoke();
            
            // Add a new square to the snake body
            // NOTE: On my inspection of the classic snake game, the snake does grow after one frame, not instantly
            // So, adding a duplicate of the tail square to the end of the snake body will work as expected
            var tailSquare = _gameData.Snake.OccupiedSquares.Last.Value;
            _gameData.Snake.OccupiedSquares.AddLast(tailSquare);
            
            // Remove the collectible
            _gameData.Collectibles.Remove(eatenCollectible);
            
            // Create a new collectible
            var newCollectiblePosition = _gameData.GetRandomEmptyCoords();
            _gameData.Collectibles.Add(new Collectible(newCollectiblePosition));
        }
        
        #endregion
        
        #region Movement
        
        private void MoveSnake()
        {
            // Try to move the snake in the current direction
            var headSquare = _gameData.Snake.OccupiedSquares.First;
            
            // Calculate the new head position
            var newHeadCoords = headSquare.Value + _gameData.CurrentMovementDirection;
            
            // Remove the tail square and add new head square
            _gameData.Snake.OccupiedSquares.RemoveLast();
            _gameData.Snake.OccupiedSquares.AddFirst(newHeadCoords);
        }
        
        #endregion
        
        #region Collision Check
        
        private void SnakeCollisionCheck()
        {
            // Check if the new snake head position is colliding with the snake body or the game boundaries
            
            // Game boundaries check
            if (IsSnakeOutOfBounds())
            {
                OnGameEnded();
                
                return;
            }
            
            // Snake body check
            if (IsSnakeCollidingWithItself())
            {
                OnGameEnded();
                
                return;
            }
            
            // Food check
            if (IsSnakeCollidingWithCollectible(out var eatenCollectible))
            {
                OnSnakeAteFood(eatenCollectible);
            }
        }
        private bool IsSnakeOutOfBounds()
        {
            // Check if the new snake head position is colliding with the game boundaries
            var headSquare = _gameData.Snake.OccupiedSquares.First.Value;
            return headSquare.X < 0 || headSquare.X >= _gameData.GameGridSize || headSquare.Y < 0 || headSquare.Y >= _gameData.GameGridSize;
        }
        private bool IsSnakeCollidingWithItself()
        {
            // Check if the new snake head position is colliding with the snake body
            var headSquare = _gameData.Snake.OccupiedSquares.First.Value;
            var headSquareNode = _gameData.Snake.OccupiedSquares.First.Next;
            while (headSquareNode != null)
            {
                // Check if the head square is colliding with the current body square
                if (headSquareNode.Value == headSquare)
                {
                    return true;
                }
                
                headSquareNode = headSquareNode.Next;
            }

            return false;
        }
        private bool IsSnakeCollidingWithCollectible(out Collectible eatenCollectible)
        {
            // No need to check if there are no collectibles
            if (_gameData.Collectibles.Count == 0)
            {
                eatenCollectible = null;
                return false;
            }
                
            
            // Check if the new snake head position is colliding with the collectible
            var headSquare = _gameData.Snake.OccupiedSquares.First.Value;
            foreach (var collectible in _gameData.Collectibles)
            {
                if (headSquare == collectible.Square)
                {
                    eatenCollectible = collectible;
                    return true;
                }
            }

            eatenCollectible = null;
            return false;
        }
        
        #endregion
        
        // This is the initial game loop implementation
        private void ProcessGameLoop()
        {
            Debug.Log("Game loop iteration");
            
            // Move the snake
            MoveSnake();
            
            // Check if the new snake head position is colliding with the snake body or the game boundaries
            SnakeCollisionCheck();
            
            // Notify the subscribers that the game loop has been iterated
            OnGameLoopIterated?.Invoke();
        }
    }
}