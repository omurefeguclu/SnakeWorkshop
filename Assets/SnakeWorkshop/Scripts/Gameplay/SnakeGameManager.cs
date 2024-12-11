using System;
using SnakeWorkshop.Scripts.Data;
using UnityEngine;

namespace SnakeWorkshop.Scripts.Gameplay
{
    public class SnakeGameManager : MonoBehaviour
    {
        [SerializeField] private SnakeGameLoopModule gameLoopModule;
        [SerializeField] private SnakeGameRendererModule gameRendererModule;
        [SerializeField] private SnakeGameInputModule gameInputModule;
        
        private void Awake()
        {
            // Create the game data
            var gameData = new SnakeGameData(10);
            
            // Initialize the game modules
            gameLoopModule.Initialize(gameData);
            gameRendererModule.Initialize(gameData);
            gameInputModule.Initialize(gameData);
            
            // Subscribe to the game loop events
            gameLoopModule.OnGameLoopIterated += gameRendererModule.MarkAsDirty;
            gameLoopModule.OnGameOver += OnGameOver;
        }

        private void Update()
        {
            // Call the update methods of the game modules
            gameLoopModule.OnUpdate();
            gameRendererModule.OnUpdate();
            gameInputModule.OnUpdate();
        }
        
        private void OnGameOver()
        {
            Debug.Log("Game Over!");
            
            // Unsubscribe from the game loop events
            gameLoopModule.OnGameLoopIterated -= gameRendererModule.MarkAsDirty;
            gameLoopModule.OnGameOver -= OnGameOver;
        }
    }
}