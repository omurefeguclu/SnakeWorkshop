using System;
using SnakeWorkshop.Scripts.Data;
using UnityEngine;

namespace SnakeWorkshop.Scripts.Gameplay
{
    [Serializable]
    public class SnakeGameInputModule : ISnakeGameModule
    {
        private SnakeGameData _gameData;
        
        public void Initialize(SnakeGameData gameData)
        {
            _gameData = gameData;
        }

        public void OnUpdate()
        {
            // Catch the input from the player and update the game data accordingly
            if (Input.GetKeyDown(KeyCode.W))
            {
                _gameData.SetMovementDirection(MovementDirection.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _gameData.SetMovementDirection(MovementDirection.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _gameData.SetMovementDirection(MovementDirection.Left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _gameData.SetMovementDirection(MovementDirection.Right);
            }
        }
    }
}