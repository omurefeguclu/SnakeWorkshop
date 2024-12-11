using System;
using System.Collections.Generic;
using SnakeWorkshop.Scripts.Data;
using SnakeWorkshop.Scripts.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorkshop.Scripts.Gameplay
{
    // This is a traditional-like renderer for snake game, there is no smooth animations or fancy graphics
    [Serializable]
    public class SnakeGameRendererModule : ISnakeGameModule
    {
        [Header("References: ")]
        [SerializeField] private RectTransform rendererCanvas;
        [SerializeField] private Image cellObject;
        [Header("Settings: ")]
        [SerializeField] private float cellSpacing = 4f;
        [SerializeField] private Color snakeColor = Color.green;
        [SerializeField] private Color collectibleColor = Color.red;
        
        // Reference to the game data
        private SnakeGameData _gameData;
        private UIGridHelper _gridHelper;

        private RuntimeCloningHelper<Image> _cellCloner;
        
        // Is dirty flag is used to determine if the game state has changed and the game needs to be re-rendered
        private bool _isDirty;
        
        public void Initialize(SnakeGameData gameData)
        {
            // This is a placeholder for the game renderer initialization
            // In the final game, this method will be responsible for initializing the game renderer
            Debug.Log("Game renderer initialized");
            
            _gameData = gameData;
            _gridHelper = UIGridHelper.Create(rendererCanvas, gameData.GameGridSize, cellSpacing);
            
            _cellCloner = new RuntimeCloningHelper<Image>(cellObject, setupAction: SetupCell);
            
            _isDirty = true;
        }

        // Unity lifecycle methods such as Update() can not be used in classes that are not derived from MonoBehaviour
        // Instead, we need to call this method from the Update() method of the owner of this module
        public void OnUpdate()
        {
            if (!_isDirty) return;
            
            RenderGame();
            _isDirty = false;
        }
        
        public void MarkAsDirty()
        {
            _isDirty = true;
        }
        
        
        private void SetupCell(Image obj)
        {
            obj.rectTransform.sizeDelta = _gridHelper.GetCellSize();
        }
        private void RenderGame()
        {
            Debug.Log("Game rendering");
            
            // Calculate the number of cells to be rendered
            var cellCount = _gameData.Snake.OccupiedSquares.Count + _gameData.Collectibles.Count;
            
            // Populate the cells, unused cells will be returned to the pool, and new cells will be created if needed
            var cells = _cellCloner.Populate(cellCount);
            
            // Create a ref int to keep track of the current cell index
            var cellIndex = 0;
            
            // Render the snake
            RenderSnake(cells, ref cellIndex);
            
            // Render the collectibles
            RenderCollectibles(cells, ref cellIndex);
        }


        // This method is responsible for rendering the snake
        // A ref int is used to keep track of the current cell index, ref keyword is used to pass the variable by reference
        // Which means the original variable will be modified in this method
        private void RenderSnake(List<Image> cells, ref int cellIndex)
        {
            foreach (var square in _gameData.Snake.OccupiedSquares)
            {
                var cell = cells[cellIndex];
                cell.transform.localPosition = _gridHelper.GetCellPosition(square);
                cell.color = snakeColor;
                
                cellIndex++;
            }
        }
        
        // This method is responsible for rendering the collectibles
        private void RenderCollectibles(List<Image> cells, ref int cellIndex)
        {
            foreach (var collectible in _gameData.Collectibles)
            {
                var cell = cells[cellIndex];
                cell.transform.localPosition = _gridHelper.GetCellPosition(collectible.Square);
                cell.color = collectibleColor;

                cellIndex++;
            }
        }
    }
}