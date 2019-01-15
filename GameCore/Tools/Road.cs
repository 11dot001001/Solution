using GameCore.Model;
using Noname.BitConversion;
using Noname.BitConversion.System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Tools
{
    public class Road : IEnumerable<Vector2>
    {
        private List<Vector2> _points;
        private float _length;

        public Road() { }
        public Road(BacteriumModel start, BacteriumModel end)
        {
            _points = new List<Vector2>();
            DirectionFactor = 1f;
            _length = float.NaN;
            Start = start;
            End = end;
        }
        public Road(Road road)
        {
            _points = new List<Vector2>(road.Points);
            DirectionFactor = road.DirectionFactor;
            _length = float.NaN;
            Start = road.Start;
            End = road.End;
        }

        public List<Vector2> Points => _points;
        public int Id { get; set; }
        public float DirectionFactor { get; set; }
        public float Length => !float.IsNaN(_length) ? _length : throw new Exception();
        public BacteriumModel Start { get; private set; }
        public BacteriumModel End { get; private set; }

        public void SetLength()
        {
            _length = 0;
            for (int i = 0; i < _points.Count-1; i++)
                _length += Vector2.Distance(_points[i], _points[i + 1]);
        }

        public IEnumerator<Vector2> GetEnumerator() => Points.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
    }
}