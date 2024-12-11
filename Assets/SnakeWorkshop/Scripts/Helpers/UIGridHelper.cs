using SnakeWorkshop.Scripts.Data;
using UnityEngine;

namespace SnakeWorkshop.Scripts.Helpers
{
    // This class will serve as a helper for implementing the grid-based UI
    // i.e. the grid-based UI will be implemented for rendering the snake game
    public class UIGridHelper
    {
        private readonly RectTransform _parent;
        private readonly int _gridSize;
        private readonly float _cellSpacing;

        // Data
        private float _cellSize;
        private Vector2 _bottomLeftPosition;
        
        
        private UIGridHelper(RectTransform parent, int gridSize, float cellSpacing)
        {
            _parent = parent;
            _gridSize = gridSize;
            _cellSpacing = cellSpacing;
            
            // Compute the cell size
            PreComputeUIData();
        }

        // Pre-compute the UI data such as cell size, cell spacing, etc.
        private void PreComputeUIData()
        {
            // Compute the cell size (consider the spacing)
            var squareSize = _parent.sizeDelta.x;

            _cellSize = (squareSize - (_gridSize - 1) * _cellSpacing) / _gridSize;
            
            // Compute the bottom-left position of the grid
            var offset = new Vector2(_cellSize / 2, _cellSize / 2);
            _bottomLeftPosition = new Vector2(-squareSize / 2, -squareSize / 2) + offset;
        }

        // This method returns the size delta of the cell
        // Use it like RectTransform.sizeDelta = GetCellSize();
        public Vector2 GetCellSize()
        {
            return new Vector2(_cellSize, _cellSize);
        }
        
        // This method returns local position of the cell based on the grid coordinates
        // Use it like RectTransform.localPosition = GetCellPosition(x, y);
        public Vector2 GetCellPosition(int x, int y)
        {
            // Compute the cell position based on the grid coordinates
            var step = _cellSize + _cellSpacing;
            var cellPosition = new Vector2(x * step, y * step);
            
            return _bottomLeftPosition + cellPosition;
        }
        public Vector2 GetCellPosition(GridCoords coords)
        {
            return GetCellPosition(coords.X, coords.Y);
        }
        
        // Static factory method for creating the UIGridHelper
        public static UIGridHelper Create(RectTransform parent, int gridSize, float cellSpacing)
        {
            return new UIGridHelper(parent, gridSize, cellSpacing);
        }
    }
}