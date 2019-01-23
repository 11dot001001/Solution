using Devdeb.Maths.Geometry2D;
using GameCore.Model;
using System;
using UnityEngine;

namespace GameCore.Tools
{
    public struct BacteriumProximity
    {
        public BacteriumModel BindedBacterium;
        public Vector2 StartDirection;
        public Vector2 EndDirection;
        public bool IsClockRotate;

        public BacteriumProximity(BacteriumModel bindedBacterium, Vector2 startDirection, Vector2 endDirection, bool isClockRotate)
        {
            BindedBacterium = bindedBacterium ?? throw new ArgumentNullException(nameof(bindedBacterium));
            StartDirection = startDirection;
            EndDirection = endDirection;
            IsClockRotate = isClockRotate;
        }

        public Vector2 StartPosition => BindedBacterium.Transform.Position + StartDirection;
        public Vector2 EndPosition => BindedBacterium.Transform.Position + EndDirection;
        public float Distance => (float)Math.PI * BindedBacterium.Transform.BacteriumRadius * Geometry2D.Angle(StartDirection, EndDirection, IsClockRotate) / 180;
    }
}