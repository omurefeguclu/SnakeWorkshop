using System;

namespace SnakeWorkshop.Scripts.Data
{
    public readonly struct GridCoords : IEquatable<GridCoords>
    {
        public readonly int X;
        public readonly int Y;
        
        // Operator overloading is used to simplify the code
        public static GridCoords operator +(GridCoords a, MovementDirection b)
        {
            return b switch
            {
                MovementDirection.Up => new GridCoords(a.X, a.Y + 1),
                MovementDirection.Down => new GridCoords(a.X, a.Y - 1),
                MovementDirection.Left => new GridCoords(a.X - 1, a.Y),
                MovementDirection.Right => new GridCoords(a.X + 1, a.Y),
                _ => a
            };
        }
        
        


        public GridCoords(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Equality Check
        
        public static bool operator ==(GridCoords a, GridCoords b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(GridCoords a, GridCoords b)
        {
            return !(a == b);
        }
        public bool Equals(GridCoords other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCoords other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        #endregion
    }
}