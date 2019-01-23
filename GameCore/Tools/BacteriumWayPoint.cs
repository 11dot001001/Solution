using System;
using GameCore.Model;
using UnityEngine;

namespace GameCore.Tools
{
    public struct BacteriumWayPoint : IPosition
    {
        public BacteriumModel BindedBacterium;
        public Vector2 Direction;

        public BacteriumWayPoint(BacteriumModel bindedBacterium, Vector2 direction)
        {
            BindedBacterium = bindedBacterium ?? throw new ArgumentNullException(nameof(bindedBacterium));
            Direction = direction;
        }

        Vector2 IPosition.Position { get => BindedBacterium.Transform.Position + Direction; set => throw new System.NotImplementedException(); }
    }
}