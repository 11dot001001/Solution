using GameCore.Model;
using GameCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ServerModel.GameMechanics
{
    public class Map
    {
        private Bacterium[] _bacteriums;

        public Map(float[] bacteriumsData) => Conversion(bacteriumsData ?? throw new ArgumentNullException(nameof(bacteriumsData)));
        public Map(Map map, EventHandler positionChanged, EventHandler radiusChanged)
        {
            _bacteriums = map.Bacteriums ?? throw new ArgumentNullException(nameof(map));
            foreach (Bacterium item in _bacteriums)
            {
                item.PositionChanged += positionChanged;
                item.RadiusChanged += radiusChanged;
            }
        }

        public Bacterium[] Bacteriums => _bacteriums;

        private void Conversion(float[] bacteriumsData)
        {
            int bacteriumCount = bacteriumsData.Length / 4;
            _bacteriums = new Bacterium[bacteriumCount];

            int offset = 0;
            for (int i = 0; i < bacteriumCount; i++)
                _bacteriums[i] = new Bacterium(i, bacteriumCount, new Vector2(bacteriumsData[offset++], bacteriumsData[offset++]), bacteriumsData[offset++], bacteriumsData[offset++]);

            for (int i = 0; i < _bacteriums.Length; i++)
                for (int j = 0; j < _bacteriums.Length; j++)
                    if (j != i)
                        _bacteriums[i].Roads.Add(_bacteriums[j].Id, new Path(_bacteriums[j], new List<Road>(new RoadManager(_bacteriums[i], _bacteriums[j], _bacteriums).Roads)));
        }
    }
}