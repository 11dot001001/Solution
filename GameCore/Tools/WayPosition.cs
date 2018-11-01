using ILibrary.Maths.Geometry2D;
using Noname.BitConversion;
using UnityEngine;

namespace GameCore.Tools
{
    public struct WayPosition
    {
        public Vector2 Position;

        public WayPosition(Vector2 position) => Position = position;

        public WayPosition(Circle cirle, Vector2 tangencyDirection) => Position = cirle.Position + tangencyDirection;

        public override string ToString() => Position.ToString();
    }
}
