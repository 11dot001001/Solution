using System;
using System.Collections;
using System.Collections.Generic;

namespace GameCore.Tools
{
    public class Road : IEnumerable<WayPosition>
    {
        public List<WayPosition> Points { get; }
        public float EaseFactor { get; set; } 

        public Road()
        {
            Points = new List<WayPosition>();
            EaseFactor = 1f;
        }
        public Road(Road road)
        {
            Points = new List<WayPosition>(road.Points);
            EaseFactor = road.EaseFactor;
        }

        public IEnumerator<WayPosition> GetEnumerator() => Points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
    }
}