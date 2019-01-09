using Noname.BitConversion;
using Noname.BitConversion.System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Tools
{
    public class Road : IEnumerable<WayPosition>
    {
        static public readonly VariableLengthBitConverter<Road> BitConverter;

        static Road()
        {
            VariableLengthBitConverterBuilder<Road> builder = new VariableLengthBitConverterBuilder<Road>();
            builder.AddField(a => a._points, (a, points) => a._points = points.ToList(), ListReliableBitConverter.GetInstance(WayPositionBitConverter.Instance));
            BitConverter = builder.Finalize();
        }

        private List<WayPosition> _points;
        private float _length;

        public Road()
        {
            _points = new List<WayPosition>();
            DirectionFactor = 1f;
            _length = float.NaN;
        }
        public Road(Road road)
        {
            _points = new List<WayPosition>(road.Points);
            DirectionFactor = road.DirectionFactor;
            _length = float.NaN;
        }

        public List<WayPosition> Points => _points;
        public float DirectionFactor { get; set; }
        public float Length => !float.IsNaN(_length) ? _length : throw new Exception();

        public void SetLength()
        {
            _length = 0;
            for (int i = 0; i < _points.Count-1; i++)
                _length += Vector2.Distance(_points[i].Position, _points[i + 1].Position);
        }

        public IEnumerator<WayPosition> GetEnumerator() => Points.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
    }
}