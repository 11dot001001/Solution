using System;
using GameCore.Tools;
using UnityEngine;

namespace GameCore.Model
{
    public abstract class BacteriumBase
    {
        private readonly RoadManager[] _roads;
        private BacteriumData _bacteriumData;

        protected BacteriumBase(int roadsCount, BacteriumData bacteriumData)
        {
            _roads = new RoadManager[roadsCount];
            _bacteriumData = bacteriumData;
        }

        public Vector2 Position { get => _bacteriumData._transform.Position; set { _bacteriumData._transform.Position = value; PositionChanged?.Invoke(this, EventArgs.Empty); } }
        public int VirusCount { get => _bacteriumData._virusCount; set { _bacteriumData._virusCount = value; RadiusChanged?.Invoke(this, EventArgs.Empty); } }
        public float TransportRadius => _bacteriumData._transform.TransportRadius;
        public float BacteriumRadius => _bacteriumData._transform.BacteriumRadius;
        public Transform Transform => _bacteriumData._transform;
        public RoadManager[] Roads => _roads;
        public BacteriumData BacteriumData => _bacteriumData;

        public event EventHandler PositionChanged;
        public event EventHandler RadiusChanged;
    }
}