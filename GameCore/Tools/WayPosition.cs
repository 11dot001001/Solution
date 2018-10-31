using ILibrary.Maths.Geometry2D;
using UnityEngine;

namespace GameCore.Tools
{
    public class WayPosition
    {
        public Vector2 position;

        public WayPosition(Vector2 position) => this.position = position;

        public WayPosition(Circle cirle, Vector2 tangencyDirection) => position = cirle.Position + tangencyDirection;

        public override string ToString() => position.ToString();
    }
}
