using GameCore.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Tools
{
    public class Road
    {
        private List<BacteriumProximity> _bacteriumProximities;
        private float _length;

        public Road() { }
        public Road(BacteriumModel start, BacteriumModel end)
        {
            _bacteriumProximities = new List<BacteriumProximity>();
            DirectionFactor = 1f;
            _length = float.NaN;
            Start = start;
            End = end;
        }
        public Road(Road road)
        {
            _bacteriumProximities = new List<BacteriumProximity>(road.BacteriumProximities);
            DirectionFactor = road.DirectionFactor;
            _length = float.NaN;
            Start = road.Start;
            End = road.End;
        }

        public BacteriumModel Start { get; private set; }
        public BacteriumModel End { get; private set; }
        public int Id { get; set; }
        public float DirectionFactor { get; set; }
        public List<BacteriumProximity> BacteriumProximities => _bacteriumProximities;
        public float Length => !float.IsNaN(_length) ? _length : throw new Exception();

        public void SetLength()
        {
            if (_bacteriumProximities.Count != 0)
            {
                _length = Vector2.Distance(Start.Transform.Position, _bacteriumProximities[0].StartPosition);
                for (int i = 0; i < _bacteriumProximities.Count - 1; i++)
                {
                    _length += _bacteriumProximities[i].Distance;
                    _length += Vector2.Distance(_bacteriumProximities[i].EndPosition, _bacteriumProximities[i + 1].StartPosition);
                }
                _length += _bacteriumProximities[_bacteriumProximities.Count - 1].Distance;
                _length += Vector2.Distance(End.Transform.Position, _bacteriumProximities[_bacteriumProximities.Count - 1].EndPosition);
            }
            else
                _length = Vector2.Distance(Start.Transform.Position, End.Transform.Position);
        }
    }
}