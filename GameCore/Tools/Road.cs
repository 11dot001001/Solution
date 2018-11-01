using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.Tools
{
    public class Road : IEnumerable<WayPosition>
    {
        static public readonly VariableLengthBitConverter<Road> BitConverter;

        static Road()
        {
            VariableLengthBitConverterBuilder<Road> builder = new VariableLengthBitConverterBuilder<Road>();
            builder.AddField(a => a._points, (a, points) => a._points = points.ToList(), IEnumerableReliableBitConverter.GetInstance(WayPositionBitConverter.Instance));
            BitConverter = builder.Finalize();
        }

        private IEnumerable<WayPosition> _points;

        public Road()
        {
            _points = new List<WayPosition>();
            EaseFactor = 1f;
        }
        public Road(Road road)
        {
            _points = new List<WayPosition>(road.Points);
            EaseFactor = road.EaseFactor;
        }

        public List<WayPosition> Points => _points.ToList();
        public float EaseFactor { get; set; }


        public IEnumerator<WayPosition> GetEnumerator() => Points.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
    }
}