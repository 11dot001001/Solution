using UnityEngine;

namespace GameCore.Tools
{
    public struct ExtremeWayPoint : IPosition
    {
        public ExtremeWayPoint(Vector2 position) => Position = position;

        public Vector2 Position { get; set; }
    }
}